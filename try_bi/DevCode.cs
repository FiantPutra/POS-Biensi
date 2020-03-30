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
    class DevCode
    {
        public static String b = "";
        public string aDevCode;
        public DevCode()
        {
            get_link();
        }
        public void get_link()
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("xmlConn.xml");
            //xmlDoc.LoadXml("product.xml");

            string xpath = "Table/Product";
            var nodes = xmlDoc.SelectNodes(xpath);

            foreach (XmlNode childrenNode in nodes)
            {
                b = childrenNode.SelectSingleNode("code_pc").InnerText;
                //MessageBox.Show(a.ToString());
                //HttpContext.Current.Response.Write(childrenNode.SelectSingleNode("//Product_name").Value);
            }
            aDevCode = b;
        }
    }
}
