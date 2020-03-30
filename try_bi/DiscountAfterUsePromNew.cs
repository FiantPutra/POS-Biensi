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
    class DiscountAfterUsePromNew
    {
        public void retreive(String transaksi, String code_store, String id_cust)
        {
            koneksi ckon = new koneksi();            

            String art_id, art_name, spg_id, size, color, qty, disc_desc, sub_total2, discount_code, discount_code_get;
            int price, sub_total, disc, disc_type_new, status_diskon_api;

            CRUD sql = new CRUD();
            Transaction transaction = new Transaction();
            transaction.storeCode = code_store;
            transaction.customerId = id_cust;
            List<TransactionLine> transLine = new List<TransactionLine>();
            Article articleFromDb = new Article();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT article._id ,transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID, transaction_line.DISCOUNT, "
                                + "transaction_line.DISCOUNT_DESC,transaction_line.DISCOUNT_TYPE,transaction_line.DISCOUNT_CODE, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, "
                                + "article.BRAND, article.DEPARTMENT, article.DEPARTMENT_TYPE, article.GENDER, article.UNIT, article.ARTICLE_ID_ALIAS FROM transaction_line, article "
                                + "WHERE article.ARTICLE_ID = transaction_line.ARTICLE_ID AND transaction_line.TRANSACTION_ID = '" + transaksi + "' ORDER BY transaction_line._id ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        size = ckon.sqlDataRd["SIZE"].ToString();
                        color = ckon.sqlDataRd["COLOR"].ToString();
                        price =Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                        disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        disc_type_new = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_TYPE"].ToString());
                        discount_code = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                       
                        articleFromDb.articleId = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        articleFromDb.articleName = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        articleFromDb.brand = ckon.sqlDataRd["BRAND"].ToString();
                        articleFromDb.color = ckon.sqlDataRd["COLOR"].ToString();
                        articleFromDb.department = ckon.sqlDataRd["DEPARTMENT"].ToString();
                        articleFromDb.departmentType = ckon.sqlDataRd["DEPARTMENT_TYPE"].ToString();
                        articleFromDb.gender = ckon.sqlDataRd["GENDER"].ToString();
                        articleFromDb.id = Convert.ToInt32(ckon.sqlDataRd["_id"].ToString());
                        articleFromDb.price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        articleFromDb.size = ckon.sqlDataRd["SIZE"].ToString();
                        articleFromDb.unit = ckon.sqlDataRd["UNIT"].ToString();
                        articleFromDb.articleIdAlias = ckon.sqlDataRd["ARTICLE_ID_ALIAS"].ToString();
                        
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
                }                
                //=====================================================================================

                transaction.transactionLines = transLine;                

                BiensiPOSContext.BiensiPOSDataContext contex = new BiensiPOSContext.BiensiPOSDataContext();
                DiscountCalculateNew dc = new DiscountCalculateNew(contex);
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
                        String cmd_delete = "delete from disctype2";                        
                        sql.ExecuteNonQuery(cmd_delete);
                        foreach (var a in b)
                        {
                            var hasil = a.price - a.amountDiscount;

                            String cmd_insert = "Insert into disctype2 (TransId, articleid, Price, Discount, TotHarga, DiscountRetailId, DiscPersent) values ('" + transaksi + "','" + a.articleId + "','" + a.price + "','" + a.amountDiscount + "','" + hasil + "','" + a.discountCode + "','" + a.discountDesc + "')";                           
                            sql.ExecuteNonQuery(cmd_insert);
                        }
                    }
                    if (c.status == 1 && c.discountType == 3)
                    {
                        if (resultData.discountItems != null)
                        {
                            foreach (var a in resultData.discountItems)
                            {
                                int price_real = 0, qty_real = 0, result_real = 0;
                                String cmd_transLine = "Select * from transaction_line where TRANSACTION_ID = '" + transaksi + "' AND ARTICLE_ID = '" + a.articleId + "'";
                                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transLine, ckon.sqlCon());

                                if (ckon.sqlDataRd.HasRows)
                                {
                                    while (ckon.sqlDataRd.Read())
                                    {
                                        price_real = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                                        qty_real = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"].ToString());
                                    }
                                }                                
                                result_real = qty_real * price_real;

                                String cmd_update = "UPDATE transaction_line SET SUBTOTAL = '" + result_real + "', Discount = '0', DISCOUNT_TYPE = '" + a.discountType + "', DISCOUNT_CODE = '" + a.discountCode + "', DISCOUNT_DESC = '" + a.discountCode + "' where TRANSACTION_ID = '" + transaksi + "' AND ARTICLE_ID = '" + a.articleId + "'";                                
                                sql.ExecuteNonQuery(cmd_update);
                            }
                        }
                        if (c.discountApiItems != null)
                        {
                            foreach (var aa in c.discountApiItems)
                            {
                                int price_real = 0, qty_real = 0, result_real = 0;
                                String coodee = ""; String article_id_update = "";
                                String cmd_transLine = "Select * from transaction_line where TRANSACTION_ID = '" + transaksi + "' ";
                                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transLine, ckon.sqlCon());

                                if (ckon.sqlDataRd.HasRows)
                                {
                                    while (ckon.sqlDataRd.Read())
                                    {                                        
                                        coodee = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                                        article_id_update = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                                        result_real = price_real * qty_real;
                                        if (coodee == aa.discountCode)
                                        {
                                            result_real = aa.qty * Convert.ToInt32(aa.price);

                                            String cmd_update = "UPDATE transaction_line SET SUBTOTAL = '" + result_real + "', Discount = '0', DISCOUNT_TYPE = '" + aa.discountType + "', DISCOUNT_CODE = '" + aa.discountCode + "', DISCOUNT_DESC = '" + aa.discountCode + "' where TRANSACTION_ID = '" + transaksi + "' AND ARTICLE_ID = '" + aa.articleId + "'";                                            
                                            sql.ExecuteNonQuery(cmd_update);
                                        }
                                    }
                                }                                                                
                            }
                        }
                    }
                    //=================UPADTE STATUS DISKON KE DATABASE LOKAL(PROMOTION HEADER)=============
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }

        }
    }
}
