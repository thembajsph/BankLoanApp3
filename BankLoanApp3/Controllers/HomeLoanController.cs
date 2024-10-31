//using BankLoanApp3.Data;
//using BankLoanApp3.Models;
//using BankLoanApp3.Services;
//using BankLoanApp3.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;

//namespace BankLoanApp3.Controllers
//{
//    public class HomeLoanController : Controller
//    {
//        private readonly HomeLoanService _homeLoanService;
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<HomeLoanController> _logger;

//        // Constructor dependency injection
//        public HomeLoanController(HomeLoanService homeLoanService, ApplicationDbContext context, ILogger<HomeLoanController> logger)
//        {
//            _homeLoanService = homeLoanService;
//            _context = context;
//            _logger = logger;
//        }

//        // Display the form
//        [HttpGet]
//        public IActionResult Apply()
//        {
//            // Optionally return an empty model for the form
//            return View(new HomeLoanApplicationViewModel());
//        }

//        // Process form submission
//        // Process form submission
//        [HttpPost]
//        public IActionResult Apply(HomeLoanApplicationViewModel model)
//        {
//            _logger.LogInformation("Submit button clicked, starting form submission.");

//            // Set default LoanStatus before validation
//            if (string.IsNullOrEmpty(model.LoanStatus))
//            {
//                model.LoanStatus = "Pending"; // Set a default LoanStatus if it's not provided
//                _logger.LogWarning("LoanStatus was not set. Using default value: 'Pending'.");
//            }

//            if (ModelState.IsValid) // Validate the model after default is set
//            {
//                try
//                {
//                    // Perform loan calculation with submitted values
//                    model = _homeLoanService.CalculateLoan(model);

//                    // Save the application to the database
//                    var application = SaveToDatabase(model);

//                    // Store a success message in TempData
//                    TempData["SuccessMessage"] = "Loan application submitted successfully!";

//                    // Redirect to the result page with the application ID
//                    _logger.LogInformation("Loan application submitted successfully, redirecting to result page.");
//                    return RedirectToAction("Result", new { id = application.Id });
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error during loan application processing.");
//                    ModelState.AddModelError("", "An error occurred while processing your loan application.");
//                }
//            }
//            else
//            {
//                _logger.LogWarning("ModelState is invalid.");
//            }


//            // If we reach here, there was a failure
//            TempData["ErrorMessage"] = "Loan application submission failed. Please check the form and try again.";
//            return View(model); // Return form with validation messages
//        }





//        // Save the loan application to the database and return the saved application
//        private HomeLoanApplication SaveToDatabase(HomeLoanApplicationViewModel model)
//        {
//            var application = new HomeLoanApplication
//            {
//                GrossSalary = model.GrossSalary,
//                CreditScore = model.CreditScore,
//                PurchasePrice = model.PurchasePrice,
//                MaxInstallment = model.MaxInstallment,
//                LoanPercentage = model.LoanPercentage,
//                DepositRequired = model.DepositRequired,
//                NewInstallment = model.NewInstallment,
//                LoanStatus = model.LoanStatus,
//                ApplicationDate = DateTime.Now
//            };

//            _logger.LogInformation("Saving application: {Application}", application);

//            _context.HomeLoanApplications.Add(application);
//            _context.SaveChanges(); // Commit the transaction

//            return application; // Return the saved application
//        }

//        // Display the result page
//        [HttpGet]
//        public IActionResult Result(int id)
//        {
//            var application = _context.HomeLoanApplications
//                .Where(a => a.Id == id)
//                .Select(a => new HomeLoanApplicationViewModel
//                {
//                    Id = a.Id,
//                    GrossSalary = a.GrossSalary,
//                    CreditScore = a.CreditScore,
//                    PurchasePrice = a.PurchasePrice,
//                    MaxInstallment = a.MaxInstallment,
//                    LoanPercentage = a.LoanPercentage,
//                    DepositRequired = a.DepositRequired,
//                    NewInstallment = a.NewInstallment,
//                    LoanStatus = a.LoanStatus,
//                    ApplicationDate = a.ApplicationDate
//                })
//                .FirstOrDefault();

//            if (application == null)
//            {
//                return NotFound(); // Handle case where no application is found
//            }

//            return View(application); // Pass the model to the view
//        }

//        [HttpGet]
//        public IActionResult TestForm()
//        {
//            var model = new HomeLoanApplicationViewModel
//            {
//                GrossSalary = 5000,
//                CreditScore = 700,
//                PurchasePrice = 200000
//            };
//            return View(model); // Ensure this matches the path to your TestForm.cshtml view
//        }
//    }
//}
using BankLoanApp3.Data;
using BankLoanApp3.Models;
using BankLoanApp3.Services;
using BankLoanApp3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankLoanApp3.Controllers
{
    public class HomeLoanController : Controller
    {
        private readonly HomeLoanService _homeLoanService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeLoanController> _logger;
        private readonly UserManager<ApplicationUser> _userManager; // Add this field

        public HomeLoanController(
            HomeLoanService homeLoanService,
            ApplicationDbContext context,
            ILogger<HomeLoanController> logger,
            UserManager<ApplicationUser> userManager) // Add UserManager parameter
        {
            _homeLoanService = homeLoanService;
            _context = context;
            _logger = logger;
            _userManager = userManager; // Initialize UserManager
        }

        // List all loan applications
        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult Index()
        {
            var applications = _context.HomeLoanApplications
                .Select(a => new HomeLoanApplicationViewModel
                {
                    Id = a.Id,
                    GrossSalary = a.GrossSalary,
                    CreditScore = a.CreditScore,
                    PurchasePrice = a.PurchasePrice,
                    MaxInstallment = a.MaxInstallment,
                    LoanPercentage = a.LoanPercentage,
                    DepositRequired = a.DepositRequired,
                    NewInstallment = a.NewInstallment,
                    LoanStatus = a.LoanStatus,
                    ApplicationDate = a.ApplicationDate,
                    UserFirstName = a.ApplicationUser.FirstName,
                    UserLastName = a.ApplicationUser.LastName
                })
                .ToList();

            var model = new AllLoansViewModel
            {
                LoanApplications = applications
            };

            return View(model);
        }


        // Display the loan application form
        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var model = new HomeLoanApplicationViewModel
            {
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                ApplicationUserId = userId
            };

            return View(model);
        }


        // Process loan application form submission
        [HttpPost]
        public async Task<IActionResult> Apply(HomeLoanApplicationViewModel model)
        {
            _logger.LogInformation("Submit button clicked, starting form submission.");

            if (ModelState.IsValid)
            {
                try
                {
                    model = _homeLoanService.CalculateLoan(model);

                    var application = await SaveToDatabaseAsync(model);

                    TempData["SuccessMessage"] = "Loan application submitted successfully!";
                    _logger.LogInformation("Loan application submitted successfully, redirecting to result page.");
                    return RedirectToAction("Result", new { id = application.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during loan application processing.");
                    ModelState.AddModelError("", "An error occurred while processing your loan application. Error details: " + ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("ModelState Error: {Error}", error.ErrorMessage);
                }
            }

            TempData["ErrorMessage"] = "Loan application submission failed. Please check the form and try again.";
            return View(model);
        }


        private async Task<HomeLoanApplication> SaveToDatabaseAsync(HomeLoanApplicationViewModel model)
        {
            var application = new HomeLoanApplication
            {
                GrossSalary = model.GrossSalary,
                CreditScore = model.CreditScore,
                PurchasePrice = model.PurchasePrice,
                MaxInstallment = model.MaxInstallment,
                LoanPercentage = model.LoanPercentage,
                DepositRequired = model.DepositRequired,
                NewInstallment = model.NewInstallment,
                LoanStatus = model.LoanStatus,
                ApplicationDate = DateTime.Now,
                ApplicationUserId = model.ApplicationUserId
            };

            try
            {
                _logger.LogInformation("Attempting to save application: {Application}", application);

                _context.HomeLoanApplications.Add(application);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Application saved successfully with ID: {ApplicationId}", application.Id);

                return application;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while saving application. Inner Exception: {InnerExceptionMessage}, StackTrace: {StackTrace}", dbEx.InnerException?.Message, dbEx.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving application to the database. Inner Exception: {InnerExceptionMessage}, StackTrace: {StackTrace}", ex.InnerException?.Message, ex.StackTrace);
                throw;
            }
        }



        // Display the result of a loan application
        [HttpGet]
        public IActionResult Result(int id)
        {
            var application = _context.HomeLoanApplications
                .Where(a => a.Id == id)
                .Select(a => new HomeLoanApplicationViewModel
                {
                    Id = a.Id,
                    GrossSalary = a.GrossSalary,
                    CreditScore = a.CreditScore,
                    PurchasePrice = a.PurchasePrice,
                    MaxInstallment = a.MaxInstallment,
                    LoanPercentage = a.LoanPercentage,
                    DepositRequired = a.DepositRequired,
                    NewInstallment = a.NewInstallment,
                    LoanStatus = a.LoanStatus,
                    ApplicationDate = a.ApplicationDate
                })
                .FirstOrDefault();

            if (application == null)
            {
                _logger.LogWarning("Loan application with ID {Id} not found.", id);
                return NotFound();
            }

            return View(application);
        }



        // Display the edit form for a loan application
        [HttpGet]
        // [Authorize(Policy = "AdminOnly")] // Uncomment if authorization is required
        public async Task<IActionResult> Edit(int id)
        {
            // Find the loan application by ID
            var application = await _context.HomeLoanApplications.FindAsync(id);
            if (application == null)
            {
                _logger.LogWarning("Loan application with ID {Id} not found for editing.", id);
                return NotFound();
            }

            // Get the current user
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            // If user is null, handle accordingly
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                return NotFound();
            }

            // Create the view model with data
            var viewModel = new HomeLoanApplicationViewModel
            {
                Id = application.Id,
                GrossSalary = application.GrossSalary,
                CreditScore = application.CreditScore,
                PurchasePrice = application.PurchasePrice,
                MaxInstallment = application.MaxInstallment,
                LoanPercentage = application.LoanPercentage,
                DepositRequired = application.DepositRequired,
                NewInstallment = application.NewInstallment,
                LoanStatus = application.LoanStatus,
                ApplicationDate = application.ApplicationDate,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                ApplicationUserId = userId
            };

            return View(viewModel);
        }


        // Update a loan application
        [HttpPost]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(HomeLoanApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var application = await _context.HomeLoanApplications.FindAsync(model.Id);
                if (application == null)
                {
                    _logger.LogWarning("Loan application with ID {Id} not found for update.", model.Id);
                    return NotFound();
                }

                application.GrossSalary = model.GrossSalary;
                application.CreditScore = model.CreditScore;
                application.PurchasePrice = model.PurchasePrice;
                application.MaxInstallment = model.MaxInstallment;
                application.LoanPercentage = model.LoanPercentage;
                application.DepositRequired = model.DepositRequired;
                application.NewInstallment = model.NewInstallment;
                application.LoanStatus = model.LoanStatus;

                _context.HomeLoanApplications.Update(application);
                await _context.SaveChangesAsync();

                //TempData["SuccessMessage"] = "Loan application updated successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // Delete a loan application
        [HttpPost, ActionName("Delete")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.HomeLoanApplications.FindAsync(id);
            if (application == null)
            {
                _logger.LogWarning("Loan application with ID {Id} not found for deletion.", id);
                return NotFound();
            }

            _context.HomeLoanApplications.Remove(application);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Loan application deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}


//2 now now
//using BankLoanApp3.Data;
//using BankLoanApp3.Models;
//using BankLoanApp3.Services;
//using BankLoanApp3.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BankLoanApp3.Controllers
//{
//    public class HomeLoanController : Controller
//    {
//        private readonly HomeLoanService _homeLoanService;
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<HomeLoanController> _logger;

//        public HomeLoanController(HomeLoanService homeLoanService, ApplicationDbContext context, ILogger<HomeLoanController> logger)
//        {
//            _homeLoanService = homeLoanService;
//            _context = context;
//            _logger = logger;
//        }

//        // Display the loan application form
//        [HttpGet]
//        public IActionResult Apply()
//        {
//            return View(new HomeLoanApplicationViewModel());
//        }

//        // Process loan application form submission
//        [HttpPost]
//        public async Task<IActionResult> Apply(HomeLoanApplicationViewModel model)
//        {
//            _logger.LogInformation("Submit button clicked, starting form submission.");

//            if (string.IsNullOrEmpty(model.LoanStatus))
//            {
//                model.LoanStatus = "Pending"; // Set default LoanStatus if it's not provided
//                _logger.LogWarning("LoanStatus was not set. Using default value: 'Pending'.");
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    model = _homeLoanService.CalculateLoan(model);

//                    var application = SaveToDatabase(model);

//                    TempData["SuccessMessage"] = "Loan application submitted successfully!";
//                    _logger.LogInformation("Loan application submitted successfully, redirecting to result page.");
//                    return RedirectToAction("Result", new { id = application.Id });
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error during loan application processing.");
//                    ModelState.AddModelError("", "An error occurred while processing your loan application.");
//                }
//            }
//            else
//            {
//                _logger.LogWarning("ModelState is invalid.");
//            }

//            TempData["ErrorMessage"] = "Loan application submission failed. Please check the form and try again.";
//            return View(model);
//        }

//        // Save the loan application to the database
//        private HomeLoanApplication SaveToDatabase(HomeLoanApplicationViewModel model)
//        {
//            var application = new HomeLoanApplication
//            {
//                GrossSalary = model.GrossSalary,
//                CreditScore = model.CreditScore,
//                PurchasePrice = model.PurchasePrice,
//                MaxInstallment = model.MaxInstallment,
//                LoanPercentage = model.LoanPercentage,
//                DepositRequired = model.DepositRequired,
//                NewInstallment = model.NewInstallment,
//                LoanStatus = model.LoanStatus,
//                ApplicationDate = DateTime.Now
//            };

//            _logger.LogInformation("Saving application: {Application}", application);

//            _context.HomeLoanApplications.Add(application);
//            _context.SaveChanges();

//            return application;
//        }

//        // Display the result of a loan application
//        [HttpGet]
//        public IActionResult Result(int id)
//        {
//            var application = _context.HomeLoanApplications
//                .Where(a => a.Id == id)
//                .Select(a => new HomeLoanApplicationViewModel
//                {
//                    Id = a.Id,
//                    GrossSalary = a.GrossSalary,
//                    CreditScore = a.CreditScore,
//                    PurchasePrice = a.PurchasePrice,
//                    MaxInstallment = a.MaxInstallment,
//                    LoanPercentage = a.LoanPercentage,
//                    DepositRequired = a.DepositRequired,
//                    NewInstallment = a.NewInstallment,
//                    LoanStatus = a.LoanStatus,
//                    ApplicationDate = a.ApplicationDate
//                })
//                .FirstOrDefault();

//            if (application == null)
//            {
//                return NotFound();
//            }

//            return View(application);
//        }

//        // List all loan applications
//        [HttpGet]
//        //[Authorize(Policy = "AdminOnly")]
//        public IActionResult Index()
//        {
//            var applications = _context.HomeLoanApplications
//                .Select(a => new HomeLoanApplicationViewModel
//                {
//                    Id = a.Id,
//                    GrossSalary = a.GrossSalary,
//                    CreditScore = a.CreditScore,
//                    PurchasePrice = a.PurchasePrice,
//                    MaxInstallment = a.MaxInstallment,
//                    LoanPercentage = a.LoanPercentage,
//                    DepositRequired = a.DepositRequired,
//                    NewInstallment = a.NewInstallment,
//                    LoanStatus = a.LoanStatus,
//                    ApplicationDate = a.ApplicationDate
//                })
//                .ToList();

//            return View(applications);
//        }

//        // Display the edit form for a loan application
//        [HttpGet]
//        [Authorize(Policy = "AdminOnly")]
//        public IActionResult Edit(int id)
//        {
//            var application = _context.HomeLoanApplications.Find(id);
//            if (application == null)
//            {
//                return NotFound();
//            }

//            var viewModel = new HomeLoanApplicationViewModel
//            {
//                Id = application.Id,
//                GrossSalary = application.GrossSalary,
//                CreditScore = application.CreditScore,
//                PurchasePrice = application.PurchasePrice,
//                MaxInstallment = application.MaxInstallment,
//                LoanPercentage = application.LoanPercentage,
//                DepositRequired = application.DepositRequired,
//                NewInstallment = application.NewInstallment,
//                LoanStatus = application.LoanStatus,
//                ApplicationDate = application.ApplicationDate
//            };

//            return View(viewModel);
//        }

//        // Update a loan application
//        [HttpPost]
//        [Authorize(Policy = "AdminOnly")]
//        public async Task<IActionResult> Edit(HomeLoanApplicationViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var application = await _context.HomeLoanApplications.FindAsync(model.Id);
//                if (application == null)
//                {
//                    return NotFound();
//                }

//                application.GrossSalary = model.GrossSalary;
//                application.CreditScore = model.CreditScore;
//                application.PurchasePrice = model.PurchasePrice;
//                application.MaxInstallment = model.MaxInstallment;
//                application.LoanPercentage = model.LoanPercentage;
//                application.DepositRequired = model.DepositRequired;
//                application.NewInstallment = model.NewInstallment;
//                application.LoanStatus = model.LoanStatus;

//                _context.HomeLoanApplications.Update(application);
//                await _context.SaveChangesAsync();

//                return RedirectToAction("Index");
//            }

//            return View(model);
//        }

//        // Delete a loan application
//        [HttpPost, ActionName("Delete")]
//        [Authorize(Policy = "AdminOnly")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var application = await _context.HomeLoanApplications.FindAsync(id);
//            if (application == null)
//            {
//                return NotFound();
//            }

//            _context.HomeLoanApplications.Remove(application);
//            await _context.SaveChangesAsync();

//            return RedirectToAction("Index");
//        }
//    }
//}

//3
//using BankLoanApp3.Data;
//using BankLoanApp3.Models;
//using BankLoanApp3.Services;
//using BankLoanApp3.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BankLoanApp3.Controllers
//{
//    public class HomeLoanController : Controller
//    {
//        private readonly HomeLoanService _homeLoanService;
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<HomeLoanController> _logger;

//        public HomeLoanController(HomeLoanService homeLoanService, ApplicationDbContext context, ILogger<HomeLoanController> logger)
//        {
//            _homeLoanService = homeLoanService;
//            _context = context;
//            _logger = logger;
//        }

//        // Display the loan application form
//        [HttpGet]
//        public IActionResult Apply()
//        {
//            return View(new HomeLoanApplicationViewModel());
//        }

//        // Process loan application form submission
//        [HttpPost]
//        public async Task<IActionResult> Apply(HomeLoanApplicationViewModel model)
//        {
//            _logger.LogInformation("Submit button clicked, starting form submission.");

//            if (string.IsNullOrEmpty(model.LoanStatus))
//            {
//                model.LoanStatus = "Pending"; // Set default LoanStatus if it's not provided
//                _logger.LogWarning("LoanStatus was not set. Using default value: 'Pending'.");
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    model = _homeLoanService.CalculateLoan(model);

//                    var application = SaveToDatabase(model);

//                    TempData["SuccessMessage"] = "Loan application submitted successfully!";
//                    _logger.LogInformation("Loan application submitted successfully, redirecting to result page.");
//                    return RedirectToAction("Result", new { id = application.Id });
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error during loan application processing.");
//                    ModelState.AddModelError("", "An error occurred while processing your loan application.");
//                }
//            }
//            else
//            {
//                _logger.LogWarning("ModelState is invalid.");
//            }

//            TempData["ErrorMessage"] = "Loan application submission failed. Please check the form and try again.";
//            return View(model);
//        }

//        // Save the loan application to the database
//        private HomeLoanApplication SaveToDatabase(HomeLoanApplicationViewModel model)
//        {
//            var application = new HomeLoanApplication
//            {
//                GrossSalary = model.GrossSalary,
//                CreditScore = model.CreditScore,
//                PurchasePrice = model.PurchasePrice,
//                MaxInstallment = model.MaxInstallment,
//                LoanPercentage = model.LoanPercentage,
//                DepositRequired = model.DepositRequired,
//                NewInstallment = model.NewInstallment,
//                LoanStatus = model.LoanStatus,
//                ApplicationDate = DateTime.Now
//            };

//            _logger.LogInformation("Saving application: {Application}", application);

//            _context.HomeLoanApplications.Add(application);
//            _context.SaveChanges();

//            return application;
//        }

//        // Display the result of a loan application
//        [HttpGet]
//        public IActionResult Result(int id)
//        {
//            var application = _context.HomeLoanApplications
//                .Where(a => a.Id == id)
//                .Select(a => new HomeLoanApplicationViewModel
//                {
//                    Id = a.Id,
//                    GrossSalary = a.GrossSalary,
//                    CreditScore = a.CreditScore,
//                    PurchasePrice = a.PurchasePrice,
//                    MaxInstallment = a.MaxInstallment,
//                    LoanPercentage = a.LoanPercentage,
//                    DepositRequired = a.DepositRequired,
//                    NewInstallment = a.NewInstallment,
//                    LoanStatus = a.LoanStatus,
//                    ApplicationDate = a.ApplicationDate
//                })
//                .FirstOrDefault();

//            if (application == null)
//            {
//                return NotFound();
//            }

//            return View(application);
//        }

//        // List all loan applications
//        [HttpGet]
//        [Authorize(Policy = "AdminOnly")]
//        public IActionResult Index()
//        {
//            var applications = _context.HomeLoanApplications
//                .Select(a => new HomeLoanApplicationViewModel
//                {
//                    Id = a.Id,
//                    GrossSalary = a.GrossSalary,
//                    CreditScore = a.CreditScore,
//                    PurchasePrice = a.PurchasePrice,
//                    MaxInstallment = a.MaxInstallment,
//                    LoanPercentage = a.LoanPercentage,
//                    DepositRequired = a.DepositRequired,
//                    NewInstallment = a.NewInstallment,
//                    LoanStatus = a.LoanStatus,
//                    ApplicationDate = a.ApplicationDate
//                })
//                .ToList();

//            return View(applications);
//        }

//        // Display the edit form for a loan application
//        [HttpGet]
//        [Authorize(Policy = "AdminOnly")]
//        public IActionResult Edit(int id)
//        {
//            var application = _context.HomeLoanApplications.Find(id);
//            if (application == null)
//            {
//                return NotFound();
//            }

//            var viewModel = new HomeLoanApplicationViewModel
//            {
//                Id = application.Id,
//                GrossSalary = application.GrossSalary,
//                CreditScore = application.CreditScore,
//                PurchasePrice = application.PurchasePrice,
//                MaxInstallment = application.MaxInstallment,
//                LoanPercentage = application.LoanPercentage,
//                DepositRequired = application.DepositRequired,
//                NewInstallment = application.NewInstallment,
//                LoanStatus = application.LoanStatus,
//                ApplicationDate = application.ApplicationDate
//            };

//            return View(viewModel);
//        }

//        // Update a loan application
//        [HttpPost]
//        [Authorize(Policy = "AdminOnly")]
//        public async Task<IActionResult> Edit(HomeLoanApplicationViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var application = await _context.HomeLoanApplications.FindAsync(model.Id);
//                if (application == null)
//                {
//                    return NotFound();
//                }

//                application.GrossSalary = model.GrossSalary;
//                application.CreditScore = model.CreditScore;
//                application.PurchasePrice = model.PurchasePrice;
//                application.MaxInstallment = model.MaxInstallment;
//                application.LoanPercentage = model.LoanPercentage;
//                application.DepositRequired = model.DepositRequired;
//                application.NewInstallment = model.NewInstallment;
//                application.LoanStatus = model.LoanStatus;

//                _context.HomeLoanApplications.Update(application);
//                await _context.SaveChangesAsync();

//                return RedirectToAction("Index");
//            }

//            return View(model);
//        }

//        // Delete a loan application
//        [HttpPost, ActionName("Delete")]
//        [Authorize(Policy = "AdminOnly")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var application = await _context.HomeLoanApplications.FindAsync(id);
//            if (application == null)
//            {
//                return NotFound();
//            }

//            _context.HomeLoanApplications.Remove(application);
//            await _context.SaveChangesAsync();

//            return RedirectToAction("Index");
//        }
//    }
//}
