using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Enums
{
    public enum TransactionType
    {
        CashIn = 1,
        CashOut = 2
    }

    public enum TransactionSource
    {
        SalePayment = 1,
        SaleDueCollection = 2,
        PurchasePayment = 3,
        PurchaseDuePayment = 4,
        VendorPayment = 5,
        RentalIncome = 6,
        RentalSecurityDeposit = 7,
        Expense = 8,
        Salary = 9,
        ShopRent = 10,
        CashTransfer = 11,
        OpeningBalance = 12,
        Adjustment = 13,
        Other = 14
    }
}
