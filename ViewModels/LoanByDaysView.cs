using System.ComponentModel.DataAnnotations;

namespace LoanCalc.ViewModels
{
    public class LoanByDaysView : LoanView
    {
        [Required(ErrorMessage = "заполните поле")]
        public int PaymentStep { get; set; }

        public bool IsNoValidLoanModel(LoanByDaysView loan)
        {
            return loan.Amount <= 0 || loan.Duration <= 0 || loan.Rate <= 0 || loan.PaymentStep <= 0;
        }
    }
}