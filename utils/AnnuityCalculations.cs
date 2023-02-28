using LoanCalc.Models;
using LoanCalc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoanCalc.utils
{
    /// <summary>
    /// Вспомогательный класс для расчета кредита
    /// </summary>
    public static class AnnuityCalculations
    {
        /// <summary>
        /// расчет анн.платежа
        /// </summary>
        /// <param name="loan"></param>
        /// <returns> ежемесячный анн.платеж</returns>
        public static decimal CalcAnnuityPaymentPerMonth(LoanView loan)
        {
            decimal koeficent = GetAnnuityRatio(loan.Rate / 12 / 100, loan.Duration);
            return Math.Round(loan.Amount * koeficent, 2);
        }
        /// <summary>
        /// расчет коэффициента аннуитета
        /// </summary>
        /// <param name="monthRate">месячная процентная ставка</param>
        /// <param name="months">период выплаты кредита</param>
        /// <returns>коэффициент аннуитета</returns>
        public static decimal GetAnnuityRatio(double monthRate, int months)
        {
            decimal c = (decimal)Math.Pow(1 + monthRate, months);
            decimal a = (decimal)monthRate * c;
            decimal b = (decimal)Math.Pow(1 + monthRate, months) - 1;
            return a / b;

        }
        /// <summary>
        /// Высчитывает план выплаты кредита
        /// </summary>
        /// <param name="loan">данные по займу</param>
        /// <returns>список объектов LoanRepaymentPlan
        /// для отображения клиенту</returns>
        public static List<LoanRepaymentPlan> PlanRepaymentsInit(LoanView loan)
        {
            List<LoanRepaymentPlan> planRepayment = new List<LoanRepaymentPlan>();
            decimal percentInMonth = (decimal)(loan.Rate / 12 / 100);
            for (int i = 0; i < loan.Duration; i++)
            {
                LoanRepaymentPlan plan = new LoanRepaymentPlan();

                if (planRepayment.Count > 0)
                {
                    var lastItem = planRepayment.LastOrDefault();
                    plan.BalanceDebtPercent = Math.Round(lastItem.BalanceDebt * percentInMonth, 2);
                    plan.PaymentAmount = lastItem.PaymentAmount;
                    plan.MainDebt = lastItem.PaymentAmount - plan.BalanceDebtPercent;
                    plan.BalanceDebt = Math.Round(lastItem.BalanceDebt - plan.MainDebt, 2);
                    plan.PaymentsDate = lastItem.PaymentsDate.AddMonths(+1);
                }
                else
                {

                    plan.BalanceDebtPercent = (decimal)Math.Round(loan.Amount * percentInMonth, 2);
                    plan.PaymentAmount = CalcAnnuityPaymentPerMonth(loan);
                    plan.MainDebt = plan.PaymentAmount - plan.BalanceDebtPercent;
                    plan.BalanceDebt = Math.Round(loan.Amount - plan.MainDebt, 2);
                    plan.PaymentsDate = DateTime.Now.AddMonths(+1);
                }
                planRepayment.Add(plan);
            }
            return planRepayment;
        }

        /// <summary>
        /// Высчитывает план выплаты кредита по дням
        /// </summary>
        /// <param name="loan">данные по займу</param>
        /// <returns>список объектов LoanRepaymentPlan
        /// для отображения клиенту</returns>
        public static List<LoanRepaymentPlan> PlanRepaymentByDaysInit(LoanByDaysView loan)
        {
            List<LoanRepaymentPlan> planRepayment = new List<LoanRepaymentPlan>();
            for (int i = 0; i < loan.Duration / loan.PaymentStep; i++)
            {
                LoanRepaymentPlan plan = new LoanRepaymentPlan();

                if (planRepayment.Count > 0)
                {
                    var lastItem = planRepayment.LastOrDefault();
                    plan.BalanceDebtPercent = lastItem.BalanceDebtPercent;
                    plan.MainDebt = lastItem.MainDebt;
                    plan.BalanceDebt = Math.Round(lastItem.BalanceDebt - plan.MainDebt, 2);
                    plan.PaymentAmount = lastItem.PaymentAmount;
                    plan.PaymentsDate = lastItem.PaymentsDate.AddDays(+3);
                }
                else
                {
                    plan.BalanceDebtPercent = (decimal)Math.Round(loan.Amount * loan.Rate / 100 / loan.Duration / loan.PaymentStep, 2);
                    plan.MainDebt = loan.Amount / (loan.Duration / loan.PaymentStep);
                    plan.PaymentAmount = plan.MainDebt + plan.BalanceDebtPercent;
                    plan.BalanceDebt = Math.Round(loan.Amount - plan.MainDebt, 2);
                    plan.PaymentsDate = DateTime.Now.AddDays(+3);
                }
                planRepayment.Add(plan);
            }
            return planRepayment;
        }
    }
}