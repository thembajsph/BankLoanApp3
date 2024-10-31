namespace BankLoanApp3.ViewModels
{
    public class DisplayLoanViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal CreditScore { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MaxInstallment { get; set; }
        public decimal LoanPercentage { get; set; }
        public decimal DepositRequired { get; set; }
        public decimal NewInstallment { get; set; }
        public string LoanStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
