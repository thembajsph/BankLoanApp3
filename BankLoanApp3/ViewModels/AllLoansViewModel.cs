namespace BankLoanApp3.ViewModels
{
    public class AllLoansViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public IEnumerable<HomeLoanApplicationViewModel> LoanApplications { get; set; }
    }
}
