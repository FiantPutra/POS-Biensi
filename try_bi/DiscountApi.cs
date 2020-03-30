using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace try_bi
{
    class DiscountApi
    {
        public int id { get; set; }
        public String discountCode { set; get; }
        public int discountItem { get; set; }
        public decimal discountPercent { get; set; }
        public decimal discountAmount { get; set; }
        public int status { get; set; }
        public int discountType { set; get; }
        public String articleId { set; get; }
        public long articleIdFk { set; get; }
        public decimal totalDiscount { set; get; }
        public String discountDesc { set; get; }
        public List<DiscountApiItem> discountApiItems { set; get; }
    }

    public class DiscountApiItem
    {
        public String articleId { set; get; }
        public decimal amountDiscount { set; get; }
        public long articleIdFk { set; get; }
        public String discountDesc { set; get; }
        public String discountCode { set; get; }
        public decimal price { set; get; }
        public int discountType { set; get; }
        public int qty { set; get; }
        public int DiscountRetailLinesId { set; get; }
        public int DiscountRetailId { set; get; }

    }
}
