﻿using System;

namespace BankLoanApp3.Models
{
    public class HomeLoanApplication
    {
        public int Id { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal CreditScore { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MaxInstallment { get; set; }
        public decimal LoanPercentage { get; set; }
        public decimal DepositRequired { get; set; }
        public decimal NewInstallment { get; set; }
        public string LoanStatus { get; set; }
        public DateTime ApplicationDate { get; set; }

        // Foreign key for ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } // Navigation property
    }
}

