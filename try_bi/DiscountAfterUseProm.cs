using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;
using Tulpep.NotificationWindow;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace try_bi
{
    class DiscountAfterUseProm
    {
        public void retreive(String transaksi, String code_store, String id_cust)
        {
            koneksi ckon = new koneksi();
            koneksi2 ckon2 = new koneksi2();

            String art_id, art_name, spg_id, size, color, qty, disc_desc, sub_total2, discount_code, discount_code_get;
            int price, sub_total, disc, disc_type_new, status_diskon_api;

            
            Transaction transaction = new Transaction();
            transaction.storeCode = code_store;
            transaction.customerId = id_cust;
            List<TransactionLine> transLine = new List<TransactionLine>();

            
            String sql = "SELECT transaction_line._id ,transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID, transaction_line.DISCOUNT, transaction_line.DISCOUNT_DESC,transaction_line.DISCOUNT_TYPE,transaction_line.DISCOUNT_CODE, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line.ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + transaksi + "' ORDER BY transaction_line._id ASC";
            ckon.cmd = new MySqlCommand(sql, ckon.con);
            try
            {
                ckon.con.Open();
                //=====================================================================================
                ckon.myReader = ckon.cmd.ExecuteReader();
                while (ckon.myReader.Read())
                {
                    art_id = ckon.myReader.GetString("ARTICLE_ID");
                    art_name = ckon.myReader.GetString("ARTICLE_NAME");
                    spg_id = ckon.myReader.GetString("SPG_ID");
                    size = ckon.myReader.GetString("SIZE");
                    color = ckon.myReader.GetString("COLOR");
                    price = ckon.myReader.GetInt32("PRICE");
                    qty = ckon.myReader.GetString("QUANTITY");
                    disc_desc = ckon.myReader.GetString("DISCOUNT_DESC");
                    sub_total = ckon.myReader.GetInt32("SUBTOTAL");
                    disc = ckon.myReader.GetInt32("DISCOUNT");
                    disc_type_new = ckon.myReader.GetInt32("DISCOUNT_TYPE");
                    discount_code = ckon.myReader.GetString("DISCOUNT_CODE");

                    
                    //==============================================================================
                    string query = "SELECT * FROM ARTICLE WHERE ARTICLE_ID='" + art_id + "'";
                    ckon2.cmd2 = new MySqlCommand(query, ckon2.con2);
                    ckon2.con2.Open();
                    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
                    Article articleFromDb = new Article();
                    while (ckon2.myReader2.Read())
                    {

                        articleFromDb.articleId = ckon2.myReader2.GetString("ARTICLE_ID");
                        articleFromDb.articleName = ckon2.myReader2.GetString("ARTICLE_NAME");
                        articleFromDb.brand = ckon2.myReader2.GetString("BRAND");
                        articleFromDb.color = ckon2.myReader2.GetString("COLOR");
                        articleFromDb.department = ckon2.myReader2.GetString("DEPARTMENT");
                        articleFromDb.departmentType = ckon2.myReader2.GetString("DEPARTMENT_TYPE");
                        articleFromDb.gender = ckon2.myReader2.GetString("GENDER");
                        articleFromDb.id = ckon2.myReader2.GetInt32("_id");
                        articleFromDb.price = ckon2.myReader2.GetInt32("PRICE");
                        articleFromDb.size = ckon2.myReader2.GetString("SIZE");
                        articleFromDb.unit = ckon2.myReader2.GetString("UNIT");
                        articleFromDb.articleIdAlias = ckon2.myReader2.GetString("ARTICLE_ID_ALIAS");
                    }
                    ckon2.con2.Close();
                    //======================================================================
                    TransactionLine t = new TransactionLine();

                    t.discount = disc;
                    t.subtotal = sub_total;
                    t.quantity = Int32.Parse(qty);
                    t.price = price;
                    t.discountType = disc_type_new;
                    t.discountCode = discount_code;
                    t.article = articleFromDb;
                    transLine.Add(t);
                }
                //=====================================================================================
                
                transaction.transactionLines = transLine;
                ckon.con.Close();

                BiensiPosDbContext.BiensiPosDbDataContext contex = new BiensiPosDbContext.BiensiPosDbDataContext();
                DiscountCalculate dc = new DiscountCalculate(contex);
                DiscountMaster resultData = dc.Post(transaction);
                Console.WriteLine(JsonConvert.SerializeObject(transaction));
                //=================================================
                //for (int i = 0; i < resultData.discounts.Count; i++)
                //{
                //    discount_code_get = resultData.discounts[i].discountCode;
                //    data_diskon(discount_code_get);
                //}
                foreach (var c in resultData.discounts)
                {
                    var b = c.discountApiItems.ToList();

                    discount_code_get = c.discountCode;
                    status_diskon_api = c.status;
                    String art_diskon = c.articleId;
                    int disc_type = c.discountType;
                    //=================insert ke table disctype2 saat type diskon 2 dan status 1===========
                    if (c.status == 1 && c.discountType == 2)
                    {

                        String del = "delete from disctype2";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(del);
                        foreach (var a in b)
                        {
                            var hasil = a.price - a.amountDiscount;

                            String input = "Insert into disctype2 (TransId, articleid, Price, Discount, TotHarga, DiscountRetailId, DiscPersent) values ('" + transaksi + "','" + a.articleId + "','" + a.price + "','" + a.amountDiscount + "','" + hasil + "','" + a.discountCode + "','" + a.discountDesc + "')";
                            CRUD insert = new CRUD();
                            insert.ExecuteNonQuery(input);
                        }
                    }
                    if (c.status == 1 && c.discountType == 3)
                    {
                        if (resultData.discountItems != null)
                        {
                            foreach (var a in resultData.discountItems)
                            {
                                int price_real = 0, qty_real = 0, result_real = 0;
                                String search = "Select * from transaction_line where TRANSACTION_ID = '" + transaksi + "' AND ARTICLE_ID = '" + a.articleId + "'";
                                ckon.cmd = new MySqlCommand(search, ckon.con);
                                ckon.con.Open();
                                ckon.myReader = ckon.cmd.ExecuteReader();
                                while (ckon.myReader.Read())
                                {
                                    price_real = ckon.myReader.GetInt32("PRICE");
                                    qty_real = ckon.myReader.GetInt32("QUANTITY");
                                }
                                ckon.con.Close();
                                result_real = qty_real * price_real;

                                String update_transline = "UPDATE transaction_line SET SUBTOTAL = '" + result_real + "', Discount = '0', DISCOUNT_TYPE = '" + a.discountType + "', DISCOUNT_CODE = '" + a.discountCode + "', DISCOUNT_DESC = '" + a.discountCode + "' where TRANSACTION_ID = '" + transaksi + "' AND ARTICLE_ID = '" + a.articleId + "'";
                                CRUD new_query = new CRUD();
                                new_query.ExecuteNonQuery(update_transline);
                            }
                        }
                        if (c.discountApiItems != null)
                        {
                            foreach (var aa in c.discountApiItems)
                            {
                                int price_real = 0, qty_real = 0, result_real = 0;
                                String coodee = ""; String article_id_update = "";
                                String search = "Select * from transaction_line where TRANSACTION_ID = '" + transaksi + "' ";
                                ckon.cmd = new MySqlCommand(search, ckon.con);
                                ckon.con.Open();
                                ckon.myReader = ckon.cmd.ExecuteReader();
                                while (ckon.myReader.Read())
                                {
                                    //price_real = ckon.myReader.GetInt32("PRICE");
                                    //qty_real = ckon.myReader.GetInt32("QUANTITY");
                                    coodee = ckon.myReader.GetString("DISCOUNT_CODE");
                                    article_id_update = ckon.myReader.GetString("ARTICLE_ID");
                                    result_real = price_real * qty_real;
                                    if (coodee == aa.discountCode)
                                    {
                                        result_real = aa.qty * Convert.ToInt32(aa.price);

                                        String update_transline = "UPDATE transaction_line SET SUBTOTAL = '" + result_real + "', Discount = '0', DISCOUNT_TYPE = '" + aa.discountType + "', DISCOUNT_CODE = '" + aa.discountCode + "', DISCOUNT_DESC = '" + aa.discountCode + "' where TRANSACTION_ID = '" + transaksi + "' AND ARTICLE_ID = '" + aa.articleId + "'";
                                        CRUD new_query = new CRUD();
                                        new_query.ExecuteNonQuery(update_transline);
                                    }
                                }
                                ckon.con.Close();

                            }
                        }
                    }
                    //=================UPADTE STATUS DISKON KE DATABASE LOKAL(PROMOTION HEADER)=============
                    

                }
                ckon.con.Close();
            }

            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }

        }
    }
}
