using System;

namespace LoanCalc.Models
{
    public class LoanRepaymentPlan
    {
        /// <summary>
        /// дата платежа
        /// </summary>
        public DateTime PaymentsDate { get; set; }

        /// <summary>
        /// сумма платежа ds
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// проценты
        /// </summary>
        public decimal BalanceDebtPercent { get; set; }


        /// <summary>
        /// остаток долга
        /// </summary>
        public decimal BalanceDebt { get; set; }

        /// <summary>
        /// основной долг
        /// </summary>
        public decimal MainDebt { get; set; }


    }
}