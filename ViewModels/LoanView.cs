using System.ComponentModel.DataAnnotations;

namespace LoanCalc.ViewModels
{
    public class LoanView
    {
        [Required(ErrorMessage = "заполните поле")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "заполните поле")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "заполните поле")]
        public double Rate { get; set; }
        public bool IsNoValidLoanModel(LoanView loan)
        {
            return loan.Amount <= 0 || loan.Duration <= 0 || loan.Rate <= 0;
        }
    }
}