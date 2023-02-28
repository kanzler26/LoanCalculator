using LoanCalc.Models;
using LoanCalc.utils;
using LoanCalc.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LoanCalc.Controllers
{
    public class CalculatorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ByDays()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CheckCalcForm(LoanView loan)
        {
            AddModelStateErrors(loan);
            if (!ModelState.IsValid)
            {
                return View("Index", loan);
            }
            return RedirectToAction("GetLoanRepaymentPlan", "Calculator", loan);
        }
        [HttpGet]
        public ActionResult CheckDayCalculatorForm(LoanByDaysView loanByDay)
        {
            AddModelStateErrors(loanByDay);
            if (loanByDay.PaymentStep <= 0)
            {
                ModelState.AddModelError(nameof(loanByDay.PaymentStep), "шаг платежа должен быть больше 0");
            }
            if (!ModelState.IsValid)
            {
                return View("bydays", loanByDay);
            }

            return RedirectToAction("GetLoanRepaymentPlanByDays", "Calculator", loanByDay);
        }

        [HttpGet]
        public ActionResult GetLoanRepaymentPlanByDays(LoanByDaysView loan)
        {
            if (loan.IsNoValidLoanModel(loan))
            {
                return new HttpStatusCodeResult(404);
            }
            List<LoanRepaymentPlan> repayments = AnnuityCalculations.PlanRepaymentByDaysInit(loan);
            return View("plan_repayment", repayments);
        }

        [HttpGet]
        public ActionResult GetLoanRepaymentPlan(LoanView loan)
        {
            if (loan.IsNoValidLoanModel(loan))
            {
                return new HttpStatusCodeResult(404);
            }
            List<LoanRepaymentPlan> repayments = AnnuityCalculations.PlanRepaymentsInit(loan);
            return View("plan_repayment", repayments);
        }
        public void AddModelStateErrors(LoanView loan)
        {
            if (loan.Amount <= 0)
            {
                ModelState.AddModelError(nameof(loan.Amount), "сумма должна быть больше 0");
            }
            if (loan.Duration <= 0)
            {
                ModelState.AddModelError(nameof(loan.Duration), "срок должен быть больше 0");
            }
            if (loan.Rate <= 0)
            {
                ModelState.AddModelError(nameof(loan.Rate), "ставка за год должна быть больше 0");
            }
        }
    }
}
