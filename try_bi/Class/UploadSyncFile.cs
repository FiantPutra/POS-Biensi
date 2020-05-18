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
    class UploadSyncFile
    {
        ftpServerList ftpServerList = new ftpServerList();
        String ftpServer, ftpUsername, ftpPassword;
        koneksi ckon = new koneksi();
        LinkApi xmlCon = new LinkApi();

        public void SyncUpload()
        {
            API_FTPServer fTPServer = new API_FTPServer();

            ftpServerList.ftpServers = fTPServer.FTPServerRequest();

            for (int i = 0; i < ftpServerList.ftpServers.Count; i++)
            {
                ftpServer = ftpServerList.ftpServers[i].ftpServerName;
                ftpUsername = ftpServerList.ftpServers[i].ftpUsername;
                ftpPassword = ftpServerList.ftpServers[i].ftpPassword;
            }

            runStoreProcedure();
        }

        public void runStoreProcedure()
        {
            CRUD sql = new CRUD();
            SqlConnection con = ckon.sqlConMsg();

            try
            {
                con.Open();
                String cmd_sp = "DataGenerated";
                SqlCommand cmd = new SqlCommand(cmd_sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

                UploaddFile();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void UploaddFile()
        {
            CRUD sql = new CRUD();
            String uploadPath = "";
            String fileName = "";
            String id = "";
            String jobId = "";
            String storeId = "";
            String rowFatch = "";            

            try
            {
                ckon.sqlConMsg().Open();
                String cmd = "SELECT JobID, StoreID, UploadPath, SynchDetail, RowFatch FROM JobTabletoSynchDetailUpload " +
                                "WHERE SynchDetail NOT IN(SELECT SynchDetail FROM JobSynchDetailUploadStatus)";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlConMsg());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {                        
                        uploadPath = Convert.ToString(ckon.sqlDataRd["UploadPath"]);
                        id = Convert.ToString(ckon.sqlDataRd["SynchDetail"]);
                        jobId = Convert.ToString(ckon.sqlDataRd["JobID"]);
                        storeId = Convert.ToString(ckon.sqlDataRd["StoreID"]);
                        rowFatch = Convert.ToString(ckon.sqlDataRd["RowFatch"]);

                        fileName = getFilename(uploadPath);
                        uploadToFTP(fileName, storeId, id, rowFatch);
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

        public void uploadToFTP(String fileName, String storeId, String syncDetail, String rowFatch)
        {
            CRUD sql = new CRUD();
            String ftpWebReq = "";
            String uploadFilePath = "";
            String fileToExtract = "";
            API_UploadSync uploadSyncDetail = new API_UploadSync();

            ftpWebReq = ftpServer + "Uploadfile" + "/" + storeId + "/" + fileName + ".bcp";            

            uploadFilePath = getFilePath(storeId);

            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpWebReq);

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                FileStream stream = File.OpenRead(uploadFilePath + @"\" + fileName + ".bcp");
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();               

                uploadSyncDetail.uploadSync(syncDetail);

                String cmd_insert = "IF NOT EXISTS (SELECT * FROM JobSynchDetailUploadStatus WHERE SynchDetail = '" + syncDetail + "') " +
                                           "BEGIN " +
                                           "INSERT INTO JobSynchDetailUploadStatus(SynchDetail, RowFatch, RowApplied, Status) " +
                                           "VALUES('" + syncDetail + "', '" + rowFatch + "', 0, 1) " +
                                           "END";
                sql.ExecuteNonQueryMsg(cmd_insert);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public String getFilePath(String storeId)
        {
            String filePath = "";

            filePath = xmlCon.ftpFilePath + @"Uploadfile\" + storeId;

            return filePath;
        }

        public String getFilename(String downloadPath)
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
    }
}
