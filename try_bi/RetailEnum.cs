using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace try_bi
{
    class RetailEnum
    {
        public static int discountNormal = 0;
        public static int discountEmployee = 1;
        public static int discountMixAndMatch = 2;
        public static int discountBuyAndGet = 3;

        public static int requestTransaction = 0;
        public static int mutasiTransaction = 1;
        public static int returnTransaction = 2;
        public static int doTransaction = 3;

        public static int doStatusPending = 0;
        public static int doStatusConfirmed = 1;

        /*
        1=DeliveryOrder
        2=Mutasi Order
        3=Return Irder
        4=Sales Transaction
        9=Beginning Balance
        */
        public static int DeliveryOrder = 0;
        public static int MutasiOrder = 1;
        public static int ReturnOrder = 2;
        public static int SalesTransaction = 3;
        public static int BeginningBalance = 9;

        public static int transactionStore = 1;
        public static int transactionStoreinStore = 2;

        //for do type
        public static String RequestOrderEnum = "AD6";
        public static String ReturnOrderEnum = "AD9";
        public static String MutasiOrderEnum = "AD0";

        // for store in store
        public static String TransactionStoreInStore = "A02";


        public static int expenseBudget = 0;
        public static int expenseExpense = 1;


    }
}
