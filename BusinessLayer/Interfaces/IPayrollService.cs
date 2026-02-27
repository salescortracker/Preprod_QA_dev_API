using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPayrollService
    {
        Task<List<PayrollTransactionDto>> PreviewPayrollAsync(ProcessPayrollRequestDto dto, int userId);
        Task<string> ProcessPayrollAsync(ProcessPayrollRequestDto dto, int userId);
        Task<List<PayrollTransactionDto>> GetPayrollByMonthAsync(int month, int year, int userId);

    }
}
