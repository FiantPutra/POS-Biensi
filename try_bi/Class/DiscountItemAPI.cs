using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace try_bi
{
    class DiscountItemAPI
    {
        public String articleId { set; get; }
        public decimal amountDiscount { set; get; }
        public long articleIdFk { set; get; }
        public String discountDesc { set; get; }
        public String discountCode { set; get; }
        public String discountType { set; get; }
    }
}
