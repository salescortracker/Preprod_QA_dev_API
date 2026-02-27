using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IWebHostEnvironment _env;
        private readonly HRMSContext _context;
        public ExpensesController(
           IExpenseService expenseService,
           IWebHostEnvironment env,
           HRMSContext context)
        {
            _expenseService = expenseService;
            _env = env;
            _context = context;
        }
        // -------------------------------------------------------------
        // CREATE EXPENSE
        // -------------------------------------------------------------
        [HttpPost("CreateExpense")]
        public async Task<IActionResult> CreateExpense([FromForm] CreateExpenseDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = ModelState
                    });

                string? receiptPath = null;

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    var root = _env.WebRootPath ??
                               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                    var uploadFolder = Path.Combine(root, "Uploads", "ExpenseReceipts");

                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var fullPath = Path.Combine(uploadFolder, fileName);

                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    receiptPath = Path.Combine("Uploads", "ExpenseReceipts", fileName)
                                    .Replace("\\", "/");

                    dto.ReceiptPath = receiptPath;
                }

                int expenseId = await _expenseService.CreateExpenseAsync(dto);

                return Ok(new
                {
                    Success = true,
                    ExpenseId = expenseId,
                    Message = "Expense submitted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // -------------------------------------------------------------
        // GET EXPENSES OF USER
        // -------------------------------------------------------------
        [HttpGet("GetByUserExpenses/{userId}")]
        public async Task<IActionResult> GetByUserExpenses(int userId)
        {
            try
            {
                var expenses = await _expenseService.GetExpensesByUserAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = expenses
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = ex.Message,
                    data = new List<object>()
                });
            }
        }

        [HttpGet("GetAllExpenseCategoriesAsync")]
        public async Task<IActionResult> GetAllExpenseCategoriesAsync()
        {
            try
            {
                var expenses = await _expenseService.GetAllExpenseCategoriesAsync();

                return Ok(new
                {
                    success = true,
                    data = expenses
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = ex.Message,
                    data = new List<object>()
                });
            }
        }

        // -------------------------------------------------------------
        // GET EXPENSE LIMIT
        // -------------------------------------------------------------
        [HttpGet("GetExpenseLimit")]
        public async Task<IActionResult> GetExpenseLimit(
            int companyId,
            int regionId,
            int? departmentId,
            int categoryId)
        {
            var limit = await _context.ExpenseLimitConfigs
                .Where(x => x.CompanyId == companyId &&
                            x.RegionId == regionId &&
                            x.ExpenseCategoryId == categoryId &&
                             (x.DepartmentId == departmentId || x.DepartmentId == null) &&
                            x.IsActive &&
                            !x.IsDeleted)
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefaultAsync();

            if (limit == null)
                return NotFound("Limit not configured");

            return Ok(new
            {
                limit.PerTransactionLimit,
                limit.DailyLimit,
                limit.MonthlyLimit,
                limit.CurrencyCode
            });
        }
        [HttpGet("GetexpensesForApproval/{managerId}")]
        public async Task<IActionResult> GetexpensesForApproval(int managerId)
        {
            var data = await _expenseService.GetExpensesForApprovalAsync(managerId);

            return Ok(new
            {
                success = true,
                data
            });
        }
        [HttpPost("expensesApproveReject")]
        public async Task<IActionResult> expensesApproveReject([FromBody] ApproveRejectExpenseDto dto)
        {
            try
            {
                await _expenseService.ApproveRejectExpensesAsync(dto);

                return Ok(new
                {
                    success = true,
                    message = $"Expenses {dto.Action} successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("GetExpensesAll")]
        public async Task<IActionResult> GetExpensesAll()
        {
            try
            {
                var expenses = await _expenseService.GetAllExpensesAsync();

                return Ok(new
                {
                    success = true,
                    data = expenses
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // -------------------------------------------------------------
        // GET ALL EXPENSE CATEGORIES
        // -------------------------------------------------------------
        [HttpGet("GetexpensesCategories")]
        public async Task<IActionResult> GetexpensesCategories()
        {
            try
            {
                var categories = await _expenseService.GetAllExpenseCategoriesAsync();

                return Ok(new
                {
                    success = true,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }

        }
    }
}
