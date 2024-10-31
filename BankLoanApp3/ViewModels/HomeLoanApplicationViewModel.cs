using System;
using System.ComponentModel.DataAnnotations;

namespace BankLoanApp3.ViewModels
{
    public class HomeLoanApplicationViewModel
    {
        public int Id { get; set; } // Add this if you need to track the ID of the application

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Gross Salary must be a positive number.")]
        public decimal GrossSalary { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Credit Score must be between 0 and 1000.")]
        public decimal CreditScore { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Purchase Price must be a positive number.")]
        public decimal PurchasePrice { get; set; }

        public decimal MaxInstallment { get; set; }
        public decimal LoanPercentage { get; set; }
        public decimal DepositRequired { get; set; }
        public decimal NewInstallment { get; set; }

        // Remove [Required] attribute and initialize with a default value
        public string LoanStatus { get; set; } = "Pending";

        public DateTime ApplicationDate { get; set; }

        public string ApplicationUserId { get; set; }


        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}



