 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//using Devart.Data.MySql;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
namespace try_bi
{
    class AddTransLine
    {
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();

        String code_store, customer, S_ID, disc_code, disc_type, disc_desc, t_id, id_spg;
        int subtotal, qty=1, disc, new_price, new_jumlah, new_harga, new_disc, new_discAmount, new_maxDisc, new_maxDiscAmount, isService;
        public void get_data(String code, String cust, String art_id, String spg_id)
        {
            code_store = code;
            customer = cust;
            S_ID = art_id;
            id_spg = spg_id;
        }
        //==method get data for trans_line
        public void get_data_trans_line(int price, String trans_id)
        {
            new_price = price;
            t_id = trans_id;
        }
        public async Task Post_Discount()
        {
            CRUD sql = new CRUD();
            Transaction transaction = new Transaction();
            transaction.storeCode = code_store;
            transaction.customerId = customer;
            List<TransactionLine> transLine = new List<TransactionLine>();
            Article articleFromDb = new Article();
            //==================================================================================
            try
            {
                ckon.sqlCon().Open();
                string cmd = "SELECT * FROM ARTICLE WHERE ARTICLE_ID='" + S_ID + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
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
                        subtotal = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        articleFromDb.articleIdAlias = ckon.sqlDataRd["ARTICLE_ID_ALIAS"].ToString();
                        isService = Convert.ToInt32(ckon.sqlDataRd["IS_SERVICE"].ToString());
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
            //====================================================================
            TransactionLine t = new TransactionLine();
            t.subtotal = subtotal;
            t.quantity = qty;
            t.discount = 0;
            t.price = subtotal;
            //wahyu
            //t.isService = isService;

            t.article = articleFromDb;
            transLine.Add(t);

            transaction.transactionLines = transLine;
            BiensiPOSContext.BiensiPOSDataContext contex = new BiensiPOSContext.BiensiPOSDataContext();
            DiscountCalculateNew dc = new DiscountCalculateNew(contex);
            try
            {
                DiscountMaster resultData = dc.Post(transaction);
                //=================================================
                try
                {

                    foreach (var c in resultData.discountItems)
                    {
                        disc = (Int32)c.amountDiscount;
                        disc_code = c.discountCode;
                        disc_type = c.discountType;
                        disc_desc = c.discountDesc;                        
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }            
        }
        //=======================END OF DISCOUNT=============================================================
        //================DATAGRIDVIEW TO DATABASE =========================================
        public void cek_article2()
        {
            CRUD sql = new CRUD();
            //new_price = int.Parse(S_price);
            string j = "1";
            //i = 0;
            try
            {
                ckon.sqlCon().Open();
                String cmd_tmp = "SELECT * FROM [" + t_id + "] WHERE ARTICLE_ID = '" + S_ID + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_tmp, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        new_jumlah = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"]);
                        new_harga = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"]);
                        new_disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"]);
                        new_price = Convert.ToInt32(ckon.sqlDataRd["PRICE"]);
                        new_discAmount = Convert.ToInt32(ckon.sqlDataRd["DISCOUNTAMOUNT"]);
                        id_spg = Convert.ToString(ckon.sqlDataRd["SPG_ID"]);
                        new_maxDisc = Convert.ToInt32(ckon.sqlDataRd["MAXDISCOUNTQUANTITY"]);
                        new_maxDiscAmount = Convert.ToInt32(ckon.sqlDataRd["MAXDISCOUNTAMOUNT"]);

                        String cmd = "SELECT * FROM transaction_line WHERE TRANSACTION_ID ='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                        ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                        if (ckon.sqlDataRd.HasRows)
                        {
                            String cmd_update = "UPDATE transaction_line SET QUANTITY='" + new_jumlah + "',DISCOUNT='" + new_disc + "' ,SUBTOTAL='" + new_harga + "', PRICE = '" + new_price + "', DISCOUNTAMOUNT = '" + new_discAmount + "', MAXDISCOUNTQUANTITY = '" + new_maxDisc + "', MAXDISCOUNTAMOUNT = '" + new_discAmount + "' " +
                                "WHERE TRANSACTION_ID='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                            sql.ExecuteNonQuery(cmd_update);
                        }
                        else
                        {
                            int convert_harga;//convert harga menjadi integer
                                              //                      //jika diskon tidak ada, maka subtotal dikurangi diskon

                            convert_harga = new_price;
                            new_harga = convert_harga - disc;

                            if (disc_type == null)
                            {
                                disc_type = "99";
                            }

                            string cmd_insert = "INSERT INTO transaction_line (TRANSACTION_ID,ARTICLE_ID,QUANTITY,PRICE,DISCOUNT,SUBTOTAL, SPG_ID, DISCOUNT_CODE,DISCOUNT_TYPE,DISCOUNT_DESC, IS_SERVICE,DISCOUNTAMOUNT,MAXDISCOUNTQUANTITY,MAXDISCOUNTAMOUNT) " +
                                "VALUES ('" + t_id + "','" + S_ID + "', '" + j + "', '" + new_price + "', '" + new_disc + "' ,'" + new_harga + "', '" + id_spg + "','" + disc_code + "','" + disc_type + "','" + disc_desc + "', '" + isService + "', '" + new_discAmount + "', '" + new_maxDisc + "', '" + new_maxDiscAmount + "')";
                            sql.ExecuteNonQuery(cmd_insert);
                        }
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
    }
}
