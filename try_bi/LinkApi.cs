using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml;
using System.Windows.Forms;

namespace try_bi
{
    class LinkApi
    {
        public string aLink, host_db, user_db, pass_db, name_db, msg_db, code_pc, print_default, printer_name;
        public string ftpFilePath, storeId;
        public LinkApi()
        {
            get_link();            
        }

        public void get_link()
        {
            //aLink = "http://mpos.biensicore.co.id";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:/Program Files (x86)/Pos Biensi/xmlConn.xml");

            string xpath = "Table/Product";
            var nodes = xmlDoc.SelectNodes(xpath);

            foreach (XmlNode childrenNode in nodes)
            {
                aLink = childrenNode.SelectSingleNode("link_api").InnerText;
                host_db = childrenNode.SelectSingleNode("host_db").InnerText;
                user_db = childrenNode.SelectSingleNode("user_db").InnerText;
                pass_db = childrenNode.SelectSingleNode("pass_db").InnerText;
                name_db = childrenNode.SelectSingleNode("name_db").InnerText;
                msg_db = childrenNode.SelectSingleNode("msg_db").InnerText;                
                ftpFilePath = childrenNode.SelectSingleNode("FilePath").InnerText;   
                storeId = childrenNode.SelectSingleNode("storeId").InnerText;
                //print_default = childrenNode.SelectSingleNode("printer_default").InnerText;
                //printer_name = childrenNode.SelectSingleNode("printer_name").InnerText;
            }
        }
    }
}
