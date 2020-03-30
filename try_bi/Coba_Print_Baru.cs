using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace try_bi
{
    class Coba_Print_Baru
    {
        public void coba_print()
        {
            try
            {
                LinkApi ls = new LinkApi();
                PrintDialog printDialog = new PrintDialog();

                PrintDocument printDocument = new PrintDocument();

                printDialog.Document = printDocument; //add the document to the dialog box...        

                printDocument.PrinterSettings.PrinterName = ls.printer_name; //try_bi.Properties.Settings.Default.mPrinter;//seting nama printer
                printDocument.PrintPage += new PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing
                printDocument.Print();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Your Printer Name Is Unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Can't connect to the printer #{'" + try_bi.Properties.Settings.Default.mPrinter+"'}#"+ ex.ToString());
                var appLog = new System.Diagnostics.EventLog("Pos Biensi");
                appLog.Source = "Printer";
                appLog.WriteEntry(ex.Message);
            }

        }
        //

        //METHOD UNTUK MENCETAK STRUK
        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //this prints the reciept
            //int jrk_artc = 0;
            int qty2 = 0; int subtotal2 = 0;
            Graphics graphic = e.Graphics;

            Font font = new Font("Arial", 10); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 45;

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //graphic.DrawString(" STRUK PENJUALAN", new Font("Arial", 10), new SolidBrush(Color.Black), 120, startY + 25);
            graphic.DrawString(" RECEIPT TEST", new Font("Arial", 10), new SolidBrush(Color.Black), 150, startY);

            graphic.DrawString("3 SECOND PRINT TEST", new Font("Arial", 10), new SolidBrush(Color.Black), 70, startY + 20);

            graphic.DrawString("============================================", new Font("Arial", 10), new SolidBrush(Color.Black), startX, startY + 30);

            graphic.DrawString("Trans ID  : #123456789", new Font("Arial", 9), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            graphic.DrawString("Date        : 24/09/2020 11:00", new Font("Arial", 9), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            String judul = "Article ID".PadRight(26) + "Size".PadRight(10) + "Qty".PadRight(10) + "Disc".PadRight(12) + "Price".PadRight(12) + "Sub Total";
            graphic.DrawString(judul, new Font("Arial", 9), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            String LINE = "12345678".PadRight(26) + "L".PadRight(10) + "2".PadRight(10) + "-".PadRight(12) + "150.000".PadRight(12) + "300.000";
            graphic.DrawString(LINE, new Font("Arial", 7), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5;

            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0;

            String total_qty = "Sub Total".PadRight(45) + qty2.ToString().PadRight(37) + "Rp.".PadRight(7) + "300.000";
            graphic.DrawString(total_qty, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 3;

            String format_total_header = string.Format("{0:#,###}", 300000);
            graphic.DrawString("Total".PadRight(86) + "Rp".PadRight(7) + format_total_header, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 12;

            graphic.DrawString("Payment Method  : CASH", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 3;

            String paid = string.Format("{0:#,###}", 300000);
            graphic.DrawString("Paid".PadRight(86) + "Rp".PadRight(7) + paid, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 3;
            
            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0;

            graphic.DrawString("Return and Exchanges", new Font("Arial", 8), new SolidBrush(Color.Black), 130, startY + offset);
            offset = offset + (int)fontHeight + 3;

            graphic.DrawString("Within 7 Days With Receipt And Tags Attached", new Font("Arial", 8), new SolidBrush(Color.Black), 70, startY + offset);
            offset = offset + (int)fontHeight + 3;

            graphic.DrawString("Thank You For Coming", new Font("Arial", 8), new SolidBrush(Color.Black), 130, startY + offset);
            offset = offset + (int)fontHeight + 3;

            graphic.DrawString("WWW.3SECOND.CO.ID", new Font("Arial", 8), new SolidBrush(Color.Black), 90, startY + offset);
            offset = offset + (int)fontHeight + 3;
        }
        //
    }
}
