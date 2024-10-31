//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using BankLoanApp3.Models;
//using BankLoanApp3.ViewModels;
//using System.Linq;
//using BankLoanApp3.Data;


//namespace BankLoanApp3.Controllers
//{
//    public class LoanController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public LoanController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return RedirectToAction("Login", "Account");
//            }

//            var loan = _context.Loans
//                .Where(l => l.ApplicationUserId == user.Id)
//                .OrderByDescending(l => l.ApplicationDate)
//                .FirstOrDefault(); // Fetch the most recent loan

//            var viewModel = new DisplayLoanViewModel
//            {
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                GrossSalary = loan?.GrossSalary ?? 0,
//                CreditScore = loan?.CreditScore ?? 0,
//                PurchasePrice = loan?.PurchasePrice ?? 0,
//                MaxInstallment = loan?.MaxInstallment ?? 0,
//                LoanPercentage = loan?.LoanPercentage ?? 0,
//                DepositRequired = loan?.DepositRequired ?? 0,
//                NewInstallment = loan?.NewInstallment ?? 0,
//                LoanStatus = loan?.LoanStatus ?? "No Current Loan",
//                ApplicationDate = loan?.ApplicationDate ?? DateTime.MinValue
//            };

//            return View(viewModel);
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankLoanApp3.Models;
using BankLoanApp3.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using BankLoanApp3.Data;
using System;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using BankLoanApp3.Models;
using BankLoanApp3.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankLoanApp3.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoanController> _logger;

        public LoanController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<LoanController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // Action method to create a new loan application
        public async Task<IActionResult> CreateLoanApplication()
        {
            var validUserId = "valid-user-id"; // Replace with a valid user ID

            var newLoanApplication = new HomeLoanApplication
            {
                Id = 2,
                ApplicationUserId = validUserId,
                GrossSalary = 70000,
                CreditScore = 750,
                PurchasePrice = 1000000,
                MaxInstallment = 21000,
                LoanPercentage = 95,
                DepositRequired = 50000,
                NewInstallment = 79166.67m,
                LoanStatus = "Denied",
                ApplicationDate = DateTime.Now
            };

            _context.HomeLoanApplications.Add(newLoanApplication);
            await _context.SaveChangesAsync();

            // Redirect to a success page or return a view
            return RedirectToAction("Index"); // Replace with appropriate action
        }

        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the most recent loan for the logged-in user, ordered by application date
            var loan = await _context.HomeLoanApplications
                .Where(l => l.ApplicationUserId == user.Id)
                .OrderByDescending(l => l.ApplicationDate)
                .FirstOrDefaultAsync();

            if (loan != null)
            {
                _logger.LogInformation($"Fetched loan: GrossSalary={loan.GrossSalary}, CreditScore={loan.CreditScore}, PurchasePrice={loan.PurchasePrice}");
            }
            else
            {
                _logger.LogInformation("No loan found for the user.");
            }

            // Prepare the ViewModel for displaying the loan information
            var viewModel = new HomeLoanApplicationViewModel
            {
                Id = loan?.Id ?? 0,
                GrossSalary = loan?.GrossSalary ?? 0,
                CreditScore = loan?.CreditScore ?? 0,
                PurchasePrice = loan?.PurchasePrice ?? 0,
                MaxInstallment = loan?.MaxInstallment ?? 0,
                LoanPercentage = loan?.LoanPercentage ?? 0,
                DepositRequired = loan?.DepositRequired ?? 0,
                NewInstallment = loan?.NewInstallment ?? 0,
                LoanStatus = loan?.LoanStatus ?? "No Current Loan",
                ApplicationDate = loan?.ApplicationDate ?? DateTime.MinValue,
                ApplicationUserId = loan?.ApplicationUserId ?? user.Id, // Ensure user ID is included
                UserFirstName = user.FirstName,
                UserLastName = user.LastName
            };

            return View(viewModel);
        }






        public async Task<IActionResult> AllLoans()
        {
            // Ensure you include the ApplicationUser navigation property
            var applications = await _context.HomeLoanApplications
                .Include(a => a.ApplicationUser) // Include the ApplicationUser data
                .Select(a => new
                {
                    a.Id,
                    a.GrossSalary,
                    a.CreditScore,
                    a.PurchasePrice,
                    a.MaxInstallment,
                    a.LoanPercentage,
                    a.DepositRequired,
                    a.NewInstallment,
                    a.LoanStatus,
                    a.ApplicationDate,
                    UserFirstName = a.ApplicationUser.FirstName,
                    UserLastName = a.ApplicationUser.LastName
                })
                .ToListAsync();

            // Prepare the ViewModel for the view
            var viewModel = new AllLoansViewModel
            {
                LoanApplications = applications.Select(app => new HomeLoanApplicationViewModel
                {
                    Id = app.Id,
                    GrossSalary = app.GrossSalary,
                    CreditScore = app.CreditScore,
                    PurchasePrice = app.PurchasePrice,
                    MaxInstallment = app.MaxInstallment,
                    LoanPercentage = app.LoanPercentage,
                    DepositRequired = app.DepositRequired,
                    NewInstallment = app.NewInstallment,
                    LoanStatus = app.LoanStatus,
                    ApplicationDate = app.ApplicationDate,
                    UserFirstName = app.UserFirstName,
                    UserLastName = app.UserLastName
                })
            };

            return View(viewModel);
        }




        //public IActionResult AllLoans()
        //{
        //    var applications = _context.HomeLoanApplications
        //        .Select(a => new HomeLoanApplicationViewModel
        //        {
        //            Id = a.Id,
        //            GrossSalary = a.GrossSalary,
        //            CreditScore = a.CreditScore,
        //            PurchasePrice = a.PurchasePrice,
        //            MaxInstallment = a.MaxInstallment,
        //            LoanPercentage = a.LoanPercentage,
        //            DepositRequired = a.DepositRequired,
        //            NewInstallment = a.NewInstallment,
        //            LoanStatus = a.LoanStatus,
        //            ApplicationDate = a.ApplicationDate
        //        })
        //        .ToList();

        //    return View(applications); // Make sure the view is strongly typed to IEnumerable<HomeLoanApplicationViewModel>
        //}


        // Add other actions like Apply, Details, etc. here if needed
    }
}
