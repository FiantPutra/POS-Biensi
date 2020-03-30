using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Net.NetworkInformation;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;

namespace try_bi
{
    class HO_Print
    {
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        koneksi3 ckon3 = new koneksi3();

        private IList<Stream> m_streams;
        private int m_currentPageIndex;

        public void set_print(String idHead)
        {
            string command;
            DataTable dtHeader = null, dtLine = null, dtStore = null;

            String store_code = "";
            //ckon.con.Close();
            //string sql = "select * from ho_header where MUTASI_ORDER_ID='" + idHead + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //ckon.adapter.Fill(ckon.dt);
            //ckon.con.Close();
            //foreach (DataRow row in ckon.dt.Rows)
            //{
            //    store_code = row["MUTASI_TO_WAREHOUSE"].ToString();
            //}
            ////line
            //ckon2.con2.Close();
            ////string sql_line = "select * from ho_line where MUTASI_ORDER_ID='" + idHead + "'";
            //string sql_line = "SELECT _id, MUTASI_ORDER_ID, CONCAT(ARTICLE_ID, RIGHT(ARTICLE_NAME,2)) AS ARTICLE_ID, ARTICLE_NAME, COLOR, " +
            //    "PRICE, QUANTITY, UNIT, SUBTOTAL, DEPARTMENT FROM ho_line WHERE MUTASI_ORDER_ID='" + idHead + "'";
            //ckon2.cmd2 = new MySqlCommand(sql_line, ckon2.con2);
            //ckon2.con2.Open();
            //ckon2.adapter2 = new MySqlDataAdapter(ckon2.cmd2);
            //ckon2.adapter2.Fill(ckon2.dt2);
            //ckon2.con2.Close();
            ////store
            //ckon3.con3.Close();
            //string sql_store = "select * from store_relasi where CODE='" + store_code + "'";
            //ckon3.cmd3 = new MySqlCommand(sql_store, ckon3.con3);
            //ckon3.con3.Open();
            //ckon3.adapter3 = new MySqlDataAdapter(ckon3.cmd3);
            //ckon3.adapter3.Fill(ckon3.dt3);
            //ckon3.con3.Close();

            try
            {
                
                ckon.sqlCon().Open();
                command = "select * from ho_header where MUTASI_ORDER_ID='" + idHead + "'";
                CRUD sql_MutasiOrder = new CRUD();
                dtHeader = sql_MutasiOrder.ExecuteDataTable(command, ckon.sqlCon());

                foreach (DataRow row in dtHeader.Rows)
                {
                    store_code = row["MUTASI_TO_WAREHOUSE"].ToString();
                }

                command = "SELECT _id, MUTASI_ORDER_ID, CONCAT(ARTICLE_ID, RIGHT(ARTICLE_NAME,2)) AS ARTICLE_ID, ARTICLE_NAME, COLOR, "
                           + "PRICE, QUANTITY, UNIT, SUBTOTAL, DEPARTMENT FROM ho_line WHERE MUTASI_ORDER_ID='" + idHead + "'";
                CRUD sql_HOLine = new CRUD();
                dtLine = sql_HOLine.ExecuteDataTable(command, ckon.sqlCon());

                command = "select * from store_relasi where CODE='" + store_code + "'";
                CRUD sql_Store = new CRUD();
                dtStore = sql_Store.ExecuteDataTable(command, ckon.sqlCon());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {                
                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }

            //Report
            LocalReport report = new LocalReport();
            report.ReportPath = "Report.rdlc";
            report.DataSources.Add(new ReportDataSource("DataSetHead", dtHeader));
            report.DataSources.Add(new ReportDataSource("DataSetLine", dtLine));
            report.DataSources.Add(new ReportDataSource("DataSetStore", dtStore));
            Export(report);
            Print();
        }

        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = try_bi.Properties.Settings.Default.mPrinter;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.27in</PageWidth>
                <PageHeight>10.6in</PageHeight>
                <MarginTop>0.35in</MarginTop>
                <MarginLeft>0.35in</MarginLeft>
                <MarginRight>0.2in</MarginRight>
                <MarginBottom>0.2in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
        //end
    }
}
