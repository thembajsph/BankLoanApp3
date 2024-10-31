//using BankLoanApp3.Models;
//using Microsoft.EntityFrameworkCore;

//namespace BankLoanApp3.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options)
//        {
//        }
//        // Add this line to include Loans in the DbContext
//        public DbSet<Loan> Loans { get; set; }

//    public DbSet<HomeLoanApplication> HomeLoanApplications { get; set; }


//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Seed initial data
//            modelBuilder.Entity<HomeLoanApplication>().HasData(
//                new HomeLoanApplication
//                {
//                    Id = 1,  // Ensure IDs are set manually if needed
//                    GrossSalary = 5000,
//                    CreditScore = 700,
//                    PurchasePrice = 200000,
//                    MaxInstallment = 1000,
//                    LoanPercentage = 80,
//                    DepositRequired = 40000,
//                    NewInstallment = 800,
//                    LoanStatus = "Approved",
//                    ApplicationDate = DateTime.Now
//                }
//            );
//        }
//    }
//}

using BankLoanApp3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankLoanApp3.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Loan> Loans { get; set; }
        public DbSet<HomeLoanApplication> HomeLoanApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed ApplicationUser data
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "sample-user-id",
                    UserName = "testuser",
                    NormalizedUserName = "TESTUSER",
                    Email = "testuser@example.com",
                    NormalizedEmail = "TESTUSER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAE...your-hashed-password...", // Use a hashed password here
                    SecurityStamp = string.Empty,
                    FirstName = "Test",
                    LastName = "User"
                }
            );

            // Seed HomeLoanApplication data
            modelBuilder.Entity<HomeLoanApplication>().HasData(
                new HomeLoanApplication
                {
                    Id = 1,
                    GrossSalary = 5000,
                    CreditScore = 700,
                    PurchasePrice = 200000,
                    MaxInstallment = 1000,
                    LoanPercentage = 80,
                    DepositRequired = 40000,
                    NewInstallment = 800,
                    LoanStatus = "Approved",
                    ApplicationDate = DateTime.Now,
                    ApplicationUserId = "sample-user-id" // This must match an existing user ID
                }
            );

            // Configure relationships
            modelBuilder.Entity<HomeLoanApplication>()
                .HasOne(h => h.ApplicationUser)
                .WithMany()
                .HasForeignKey(h => h.ApplicationUserId);
        }
    }
}
