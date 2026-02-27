using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IExpenseService
    {
        /// <summary>
        /// Creates a new expense record in the database.
        /// </summary>
        /// <param name="dto">The DTO containing expense details.</param>
        /// <returns>The newly created ExpenseID.</returns>
        Task<int> CreateExpenseAsync(CreateExpenseDto dto);

        /// <summary>
        /// Retrieves all expenses submitted by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose expenses are to be fetched.</param>
        /// <returns>List of Expense entities for that user.</returns>
        Task<List<CreateExpenseDto>> GetExpensesByUserAsync(int userId);
        /// <summary>
        /// Get all active expense categories for dropdown
        /// </summary>
        Task<List<ExpenseCategoryDto>> GetAllExpenseCategoriesAsync();

        Task<List<ExpenseApprovalListDto>> GetExpensesForApprovalAsync(int managerId);
        Task ApproveRejectExpensesAsync(ApproveRejectExpenseDto dto);

        /// <summary>
        /// Retrieves all expenses for all users (no user filter).
        /// </summary>
        Task<List<CreateExpenseDto>> GetAllExpensesAsync();
    }
}
