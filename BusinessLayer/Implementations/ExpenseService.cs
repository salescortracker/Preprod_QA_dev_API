using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BusinessLayer.Implementations
{
    public class ExpenseService:IExpenseService
    {
        private readonly HRMSContext _context;
        private readonly IEmailService _emailService;

        public ExpenseService(HRMSContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        // -----------------------------------------------------
        // CREATE EXPENSE + SEND EMAIL TO REPORTING MANAGER
        // -----------------------------------------------------
        public async Task<int> CreateExpenseAsync(CreateExpenseDto dto)
        {
            // 1️⃣ Validate user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
                throw new Exception("User not found");

            int? departmentId = dto.DepartmentId > 0 ? dto.DepartmentId : null;

            // 2️⃣ Get expense limit
            //var limit = await _context.ExpenseLimitConfigs
            //    .Where(x =>
            //        x.CompanyId == dto.CompanyId &&
            //        x.RegionId == dto.RegionId &&
            //        x.ExpenseCategoryId == dto.ExpenseCategoryId &&
            //        (x.DepartmentId == departmentId || x.DepartmentId == null) &&
            //        x.IsActive &&
            //        !x.IsDeleted)
            //    .OrderByDescending(x => x.DepartmentId)
            //    .FirstOrDefaultAsync();

            //if (limit == null)
            //    throw new Exception("Expense limit not configured.");

            //if (dto.Amount > limit.PerTransactionLimit)
            //    throw new Exception($"Amount exceeds limit {limit.PerTransactionLimit}");

            // 3️⃣ Save expense
            var expense = new Expense
            {
                ProjectName = dto.ProjectName,
                Location = dto.Location,
                Country = dto.Country,
                ExpenseCategoryId = dto.ExpenseCategoryId,
                DepartmentId = departmentId,
                CurrencyCode = dto.CurrencyCode,
                Amount = dto.Amount,
                ExpenseDate = DateOnly.FromDateTime(dto.ExpenseDate),
                Reason = dto.Reason,
                HasReceipt = dto.Receipt != null,
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedBy = dto.UserId,
                CreatedDate = DateTime.Now,
                ReceiptPath = dto.ReceiptPath,
                Status = "Pending",
                DepartmentName=dto.departmentName
                
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            // -----------------------------------------------------
            // 4️⃣ SEND EMAIL TO REPORTING MANAGER
            // -----------------------------------------------------
            if (user.ReportingTo.HasValue)
            {
                var manager = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == user.ReportingTo.Value);

                if (manager != null && !string.IsNullOrEmpty(manager.Email))
                {
                    var emailBody = BuildManagerEmail(user.FullName, expense);

                    await _emailService.SendEmailAsync(
                        manager.Email,
                        "Expense Approval Required",
                        emailBody
                    );
                }
            }

            return expense.ExpenseId;
        }

        // -----------------------------------------------------
        // EMAIL BODY
        // -----------------------------------------------------
        private static string BuildManagerEmail(string employeeName, Expense expense)
        {
            var sb = new StringBuilder();

            sb.Append("<h3>Expense Approval Request</h3>");
            sb.Append("<p>An expense has been submitted and requires your approval.</p>");
            sb.Append("<table border='1' cellpadding='6' cellspacing='0'>");
            sb.Append($"<tr><td><b>Employee</b></td><td>{employeeName}</td></tr>");
            sb.Append($"<tr><td><b>Project</b></td><td>{expense.ProjectName}</td></tr>");
            sb.Append($"<tr><td><b>Amount</b></td><td>{expense.Amount} {expense.CurrencyCode}</td></tr>");
            sb.Append($"<tr><td><b>Date</b></td><td>{expense.ExpenseDate}</td></tr>");
            sb.Append($"<tr><td><b>Reason</b></td><td>{expense.Reason}</td></tr>");
            sb.Append("</table>");
            sb.Append("<p>Please login to HRMS to approve or reject.</p>");

            return sb.ToString();
        }

        // -----------------------------------------------------
        // GET EXPENSES BY USER
        // -----------------------------------------------------
        public async Task<List<CreateExpenseDto>> GetExpensesByUserAsync(int userId)
        {
            var expenses = await _context.Expenses
                .AsNoTracking()
                .Include(e => e.ExpenseCategory)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();

            return expenses.Select(e => new CreateExpenseDto
            {
                ProjectName = e.ProjectName,
                Location = e.Location,
                Country = e.Country,
                ExpenseCategoryId = e.ExpenseCategoryId ?? 0,
                ExpenseCategoryName = e.ExpenseCategory?.ExpenseCategoryName,
                DepartmentId = e.DepartmentId,   // ✅ NOW RETURNS CORRECT VALUE
                CurrencyCode = e.CurrencyCode,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate.HasValue
                    ? e.ExpenseDate.Value.ToDateTime(TimeOnly.MinValue)
                    : default,
                Reason = e.Reason,
                UserId = e.UserId,
                CompanyId = e.CompanyId,
                RegionId = e.RegionId,
                Status = e.Status,
                ReceiptPath = e.ReceiptPath
            }).ToList();
        }

        public async Task<List<ExpenseApprovalListDto>> GetExpensesForApprovalAsync(int managerId)
        {
            return await (
                from e in _context.Expenses
                join u in _context.Users on e.UserId equals u.UserId
                join c in _context.ExpenseCategories on e.ExpenseCategoryId equals c.ExpenseCategoryId
                where u.ReportingTo == managerId
                orderby e.Status == "Pending" ? 0 : 1, e.CreatedDate descending
                select new ExpenseApprovalListDto
                {
                    ExpenseId = e.ExpenseId,
                    ProjectName = e.ProjectName,
                    ExpenseCategoryId = e.ExpenseCategoryId ?? 0,   // ✅ 🔥 FIX
                    ExpenseCategoryName = c.ExpenseCategoryName,
                    Country = e.Country,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate.Value.ToDateTime(TimeOnly.MinValue),
                    UserFullName = u.FullName,
                    Status = e.Status
                }
            ).ToListAsync();
        }

        public async Task ApproveRejectExpensesAsync(ApproveRejectExpenseDto dto)
        {
            var expenseData = await (
                from e in _context.Expenses
                join u in _context.Users on e.UserId equals u.UserId
                where dto.ExpenseIds.Contains(e.ExpenseId)
                select new
                {
                    Expense = e,
                    EmployeeName = u.FullName,
                    EmployeeEmail = u.Email
                }
            ).ToListAsync();

            if (!expenseData.Any())
                throw new Exception("No expenses found");

            // -------------------------------
            // UPDATE STATUS
            // -------------------------------
            foreach (var item in expenseData)
            {
                item.Expense.Status = dto.Action;
                item.Expense.UpdatedBy = dto.ManagerId;
                item.Expense.UpdatedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // -------------------------------
            // SEND EMAIL TO EMPLOYEE
            // -------------------------------
            foreach (var item in expenseData)
            {
                if (!string.IsNullOrWhiteSpace(item.EmployeeEmail))
                {
                    var body = BuildEmployeeEmail(
                        item.EmployeeName,
                        item.Expense.ProjectName,
                        item.Expense.Amount,
                        item.Expense.CurrencyCode,
                        item.Expense.ExpenseDate,
                        dto.Action
                    );

                    await _emailService.SendEmailAsync(
                        item.EmployeeEmail,
                        $"Expense {dto.Action}",
                        body
                    );
                }
            }
        }

        private static string BuildEmployeeEmail(string employeeName, string projectName, decimal amount, string currency, DateOnly? expenseDate, string status)
        {
            var sb = new StringBuilder();

            sb.Append($"<p>Dear {employeeName},</p>");
            sb.Append("<p>Your expense request has been processed.</p>");
            sb.Append("<table border='1' cellpadding='6' cellspacing='0'>");
            sb.Append($"<tr><td><b>Project</b></td><td>{projectName}</td></tr>");
            sb.Append($"<tr><td><b>Amount</b></td><td>{amount} {currency}</td></tr>");
            sb.Append($"<tr><td><b>Date</b></td><td>{expenseDate}</td></tr>");
            sb.Append($"<tr><td><b>Status</b></td><td><b>{status}</b></td></tr>");
            sb.Append("</table>");
            sb.Append("<p>Please login to HRMS for more details.</p>");
            sb.Append("<p>Regards,<br/>HRMS Team</p>");

            return sb.ToString();
        }

        // -----------------------------------------------------
        // GET EXPENSE CATEGORIES
        // -----------------------------------------------------
        public async Task<List<ExpenseCategoryDto>> GetAllExpenseCategoriesAsync()
        {
            return await _context.ExpenseCategories
                .Where(x => x.IsActive)
                .Select(x => new ExpenseCategoryDto
                {
                    ExpenseCategoryID = x.ExpenseCategoryId,
                    ExpenseCategoryName = x.ExpenseCategoryName
                })
                .ToListAsync();
        }
        public async Task<List<CreateExpenseDto>> GetAllExpensesAsync()
        {
            var expenses = await _context.Expenses
                .AsNoTracking()
                .Include(e => e.ExpenseCategory) // Include category info
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();

            return expenses.Select(e => new CreateExpenseDto
            {
                ProjectName = e.ProjectName,
                Location = e.Location,
                Country = e.Country,
                ExpenseCategoryId = e.ExpenseCategoryId ?? 0,
                ExpenseCategoryName = e.ExpenseCategory?.ExpenseCategoryName,
                DepartmentId = e.DepartmentId,
                CurrencyCode = e.CurrencyCode,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate.HasValue ? e.ExpenseDate.Value.ToDateTime(TimeOnly.MinValue) : default,
                Reason = e.Reason,
                UserId = e.UserId,
                CompanyId = e.CompanyId,
                RegionId = e.RegionId,
                Status = e.Status,
                ReceiptPath = e.ReceiptPath
            }).ToList();
        }

    }
}
