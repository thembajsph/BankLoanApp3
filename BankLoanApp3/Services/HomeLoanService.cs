using BankLoanApp3.ViewModels;

namespace BankLoanApp3.Services
{
    public class HomeLoanService
    {
        // Business logic for calculating loans
        public HomeLoanApplicationViewModel CalculateLoan(HomeLoanApplicationViewModel model)
        {
            // Calculate MaxInstallment as 30% of Gross Salary
            model.MaxInstallment = model.GrossSalary * 0.30m;

            // Determine LoanPercentage based on CreditScore
            if (model.CreditScore >= 800)
            {
                model.LoanPercentage = 1.00m; // 100% loan
            }
            else if (model.CreditScore >= 750)
            {
                model.LoanPercentage = 0.975m; // 97.5% loan
            }
            else if (model.CreditScore >= 700)
            {
                model.LoanPercentage = 0.95m; // 95% loan
            }
            else if (model.CreditScore >= 650)
            {
                model.LoanPercentage = 0.90m; // 90% loan
            }
            else if (model.CreditScore >= 600)
            {
                model.LoanPercentage = 0.85m; // 85% loan
            }
            else if (model.CreditScore >= 550)
            {
                model.LoanPercentage = 0.80m; // 80% loan
            }
            else
            {
                model.LoanStatus = "Rejected"; // Loan is rejected for credit scores below 550
                model.LoanPercentage = 0.00m; // No loan given
                return model; // Exit early as no further calculations are needed
            }

            // Calculate DepositRequired as the difference between PurchasePrice and the loan amount
            model.DepositRequired = model.PurchasePrice * (1 - model.LoanPercentage);

            // Calculate NewInstallment based on PurchasePrice, DepositRequired, and the given rate of 0.00785
            decimal installmentRate = 0.00785m; // Use decimal for the rate
            model.NewInstallment = (model.PurchasePrice - model.DepositRequired) * installmentRate;

            // Compare NewInstallment with MaxInstallment (which is 30% of GrossSalary)
            if (model.NewInstallment <= model.MaxInstallment)
            {
                model.LoanStatus = "Granted"; // Loan is granted
            }
            else
            {
                model.LoanStatus = "Denied"; // Loan is denied if NewInstallment exceeds MaxInstallment
            }

            return model;
        }
    }
}




