using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Diagnostics;
using System.Data.SqlClient;

namespace try_bi
{
    class DownloadSyncFile
    {
        String ftpServer, ftpUsername, ftpPassword;
        ftpServerList ftpServerList = new ftpServerList();
        koneksi ckon = new koneksi();
        LinkApi xmlCon = new LinkApi();

        public void SyncDownload()
        {
            API_UpdateSyncDetail updateSyncDetail = new API_UpdateSyncDetail();
            API_FTPServer fTPServer = new API_FTPServer();
            API_DownloadSyncDetail downloadSyncDetail = new API_DownloadSyncDetail();

            downloadSyncDetail.getDownloadSyncDetail();
            ftpServerList.ftpServers = fTPServer.FTPServerRequest();

            for (int i = 0; i < ftpServerList.ftpServers.Count; i++)
            {
                ftpServer = ftpServerList.ftpServers[i].ftpServerName;
                ftpUsername = ftpServerList.ftpServers[i].ftpUsername;
                ftpPassword = ftpServerList.ftpServers[i].ftpPassword;
            }

            downloadFile();

            insertRecordFormFile();
            deleteFile();
            updateSyncDetail.updateSync();
        }

        public void downloadFile()
        {
            CRUD sql = new CRUD();
            String downloadPath = "";
            String fileName = "";
            String id = "";
            String jobId = "";
            String storeId = "";
            String rowFatch = "";
            int syncType = 0;

            try
            {
                ckon.sqlConMsg().Open();
                String cmd = "SELECT JobID, StoreID, DownloadPath, SynchDetail, RowFatch, Synctype FROM JobTabletoSynchDetailDownload " +
                                "WHERE SynchDetail NOT IN(SELECT SynchDetail FROM JobSynchDetailDownloadStatus)  OR " +
                                "SynchDetail IN(SELECT SynchDetail FROM JobSynchDetailDownloadStatus a WHERE a.RowApplied = 0)";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlConMsg());

                if (ckon.sqlDataRd.HasRows)
                {
                    Guid downloadSession = Guid.NewGuid();

                    while (ckon.sqlDataRd.Read())
                    {
                        downloadPath = Convert.ToString(ckon.sqlDataRd["DownloadPath"]);
                        id = Convert.ToString(ckon.sqlDataRd["SynchDetail"]);
                        jobId = Convert.ToString(ckon.sqlDataRd["JobID"]);
                        storeId = Convert.ToString(ckon.sqlDataRd["StoreID"]);
                        rowFatch = Convert.ToString(ckon.sqlDataRd["RowFatch"]);
                        syncType = Convert.ToInt32(ckon.sqlDataRd["Synctype"]);

                        fileName = getFilename(downloadPath, syncType);
                        downloadFromFTP(downloadPath, fileName, id, jobId.Trim(), storeId.Trim(), rowFatch, downloadSession, syncType);

                        createFilePath(downloadPath);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }

        public void downloadFromFTP(String downloadPath, String fileName, String id, String jobId, String storeId, String rowFatch, Guid downloadSessionId, int syncType)
        {
            CRUD sql = new CRUD();
            String ftpWebReq = "";
            String downloadFilePath = "";
            String fileToExtract = "";
            String extractFilePath = "";            

            downloadFilePath = getDownloadFilePath(downloadPath, syncType);            

            ftpWebReq = ftpServer + downloadFilePath;

            createFilePath(downloadPath);

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpWebReq);

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                if (!File.Exists(downloadPath))
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    Stream responseStream = response.GetResponseStream();
                
                    FileStream file = File.Create(downloadPath);
                    byte[] buffer = new byte[32 * 1024];
                    int read;

                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }

                    file.Close();
                    responseStream.Close();
                    response.Close();
                }               

                extractFilePath = getExtactFilePath(downloadPath);

                if (!File.Exists(extractFilePath + @"\" + fileName + ".bcp") && syncType == 0)
                {
                    fileToExtract = extractFilePath + @"\" + fileName + ".zip";
                    ExtractFile(fileToExtract, extractFilePath + @"\");
                }

                String cmd_insert = "IF NOT EXISTS (SELECT * FROM JobSynchDetailDownloadStatus WHERE SynchDetail = '" + id + "') " +
                                            "BEGIN " +
                                            "INSERT INTO JobSynchDetailDownloadStatus(SynchDetail, RowFatch, RowApplied, Status, Downloadsessionid) " +
                                            "VALUES('" + id + "', '" + rowFatch + "', 0, 1, '" + downloadSessionId + "') " +
                                            "END";
                sql.ExecuteNonQueryMsg(cmd_insert);
            }
            catch (Exception e)
            {                
                if (e.Message.IndexOf("550") != -1)
                    MessageBox.Show("File "+ fileName + " not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public String getDownloadFilePath(String downloadPath, int syncType)
        {
            int charCount = 0;
            int charMatch = 0;
            int count = 0;
            int j = 0;
            int pathLength = 0;
            string rText = "", pattern = "\\", filePath = "";

            while ((j = downloadPath.IndexOf(pattern, j)) != -1)
            {
                pathLength = j;
                j += pattern.Length;
                count++;

                if (pathLength == 13)
                    break;
            }
            charCount = count;

            for (int i = 0; i < downloadPath.Length; i++)
            {
                if (downloadPath[i].ToString() == @"\" && charMatch < charCount)
                {
                    charMatch++;
                }

                if (charMatch == charCount)
                {
                    rText += downloadPath[i].ToString();
                }
            }

            if (syncType == 0)
                filePath = getBetween(rText, @"\", ".zip").Replace(@"\", "/") + ".zip";
            else
                filePath = getBetween(rText, @"\", ".bcp").Replace(@"\", "/") + ".bcp";            

            return filePath;
        }

        public String getExtactFilePath(String downloadPath)
        {
            int charCount = 0;
            int charMatch = 0;
            int count = 0;
            int j = 0;
            int pathLength = 0;
            string rText = "", pattern = "\\", filePath = "";

            while ((j = downloadPath.IndexOf(pattern, j)) != -1)
            {
                pathLength = j;
                j += pattern.Length;                               
            }

            filePath = downloadPath.Substring(0, pathLength);

            return filePath;
        }

        public void createFilePath(String downloadPath)
        {
            String filePath = "";

            filePath = getExtactFilePath(downloadPath);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
        }        

        public void ExtractFile(String source, String destination)
        {
            string zPath = @"C:\Program Files\7-Zip\7zG.exe";
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = "x \"" + source + "\" -o" + destination;
                Process x = Process.Start(pro);
            }
            catch (System.Exception Ex) { }
        }
        public String getFilename(String downloadPath, int syncType)
        {
            int charCount = 0;
            int charMatch = 0;
            int count = 0;
            int j = 0;
            string rText = "", pattern = "\\", fileName = "";

            while ((j = downloadPath.IndexOf(pattern, j)) != -1)
            {
                j += pattern.Length;
                count++;
            }
            charCount = count;

            for (int i = 0; i < downloadPath.Length; i++)
            {
                if (downloadPath[i].ToString() == @"\")
                {
                    charMatch++;
                }

                if (charMatch == charCount)
                {
                    rText += downloadPath[i].ToString();
                }
            }

            if (syncType == 0)         
                fileName = getBetween(rText, @"\", ".zip");
            else
                fileName = getBetween(rText, @"\", ".bcp");

            return fileName;
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public void insertRecordFormFile()
        {
            CRUD sql = new CRUD();
            String downloadSessionId = "";
            String downloadSessionIdPrev = "";
            int syncDetailId = 0;
            SqlConnection con = ckon.sqlConMsg();

            try
            {
                String cmd = "SELECT * FROM JobSynchDetailDownloadStatus WHERE RowApplied = 0 AND status = 1 ORDER BY SynchDetail ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, con);

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        downloadSessionId = Convert.ToString(ckon.sqlDataRd["Downloadsessionid"]);
                        syncDetailId = Convert.ToInt32(ckon.sqlDataRd["SynchDetail"]);

                        if (downloadSessionIdPrev != downloadSessionId)
                        {
                            runStoreProcedure(downloadSessionId, syncDetailId);

                            downloadSessionIdPrev = downloadSessionId;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public void runStoreProcedure(String downloadSessionId, int syncDetailId)
        {
            CRUD sql = new CRUD();
            SqlConnection con = ckon.sqlConMsg();

            try
            {
                con.Open();
                String cmd_sp = "DatadownloadtoApplied";
                SqlCommand cmd = new SqlCommand(cmd_sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add("@Downloadsessionid", DbType.Guid).Value = downloadSessionId;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                String delete = "DELETE FROM JobSynchDetailDownloadStatus WHERE SynchDetail = '" + syncDetailId + "'";
                sql.ExecuteNonQueryMsg(delete);

                MessageBox.Show(e.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void deleteFile()
        {
            String downloadPath = "";
            String jobId = "";
            String storeId = "";
            String path = "";
            String fileName = "";            
            int syncType = 0;
            CRUD sql = new CRUD();
            SqlConnection con = ckon.sqlConMsg();

            try
            {
                String cmd = "SELECT * FROM JobTabletoSynchDetailDownload a " +
                                "INNER JOIN JobSynchDetailDownloadStatus b ON b.SynchDetail = a.SynchDetail " +
                                "WHERE b.RowApplied != 0 AND b.status = 1 ORDER BY b.SynchDetail ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, con);

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        downloadPath = Convert.ToString(ckon.sqlDataRd["DownloadPath"]);
                        jobId = Convert.ToString(ckon.sqlDataRd["JobID"]);
                        storeId = Convert.ToString(ckon.sqlDataRd["StoreID"]);
                        syncType = Convert.ToInt32(ckon.sqlDataRd["Synctype"]);

                        fileName = getFilename(downloadPath, syncType);
                        path = getExtactFilePath(downloadPath);

                        if (File.Exists(path + @"\" + fileName + ".bcp"))
                            File.Delete(path + @"\" + fileName + ".bcp");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}
