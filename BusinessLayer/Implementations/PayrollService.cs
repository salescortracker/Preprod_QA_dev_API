using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class PayrollService: IPayrollService
    {
        private readonly HRMSContext _context;

        public PayrollService(HRMSContext context)
        {
            _context = context;
        }

        /* ============================================================
            COMMON CALCULATION METHOD (USED BY PREVIEW & PROCESS)
         ============================================================ */

        private (decimal gross, decimal totalDeduction, List<PayrollDetail> details)
            CalculatePayroll(EmployeeSalary empSalary,
                             List<SalaryStructureComponent> structureComponents,
                             int userId)
        {
            decimal basic = 0;
            decimal gross = 0;
            decimal totalDeduction = 0;

            var payrollDetails = new List<PayrollDetail>();

            Console.WriteLine("========== PAYROLL DEBUG START ==========");
            Console.WriteLine($"EmployeeId: {empSalary.EmployeeId}");
            Console.WriteLine($"CTC: {empSalary.Ctc}");

            /* ================= BASIC ================= */

            var basicComponent = structureComponents
                .FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.Component.ComponentName) &&
                    x.Component.ComponentName.Trim().ToLower() == "basic");

            if (basicComponent != null)
            {
                Console.WriteLine("Basic Component Found");

                var calcType = basicComponent.CalculationType?.Trim().ToLower();

                if (calcType == "fixed")
                {
                    basic = basicComponent.Value;
                }
                else if (calcType == "percentage")
                {
                    basic = empSalary.Ctc * basicComponent.Value / 100;
                }

                basic = Math.Round(basic, 2);
                gross += basic;

                Console.WriteLine($"Basic Calculated: {basic}");

                payrollDetails.Add(CreatePayrollDetail(basicComponent.ComponentId, basic, userId));
            }
            else
            {
                Console.WriteLine("❌ Basic Component NOT FOUND");
            }

            /* ================= OTHER EARNINGS ================= */

            var earnings = structureComponents
                .Where(x =>
                    x.Component.Type == "Earning" &&
                    (x.Component.ComponentName == null ||
                     x.Component.ComponentName.Trim().ToLower() != "basic"));

            foreach (var item in earnings)
            {
                decimal amount = 0;
                var calcType = item.CalculationType?.Trim().ToLower();

                Console.WriteLine($"Processing Earning: {item.Component.ComponentName}");
                Console.WriteLine($"CalculationType: {item.CalculationType}");
                Console.WriteLine($"Value: {item.Value}");
                Console.WriteLine($"PercentageOf: {item.Component.PercentageOf}");

                if (calcType == "fixed")
                {
                    amount = item.Value;
                }
                else if (calcType == "percentage")
                {
                    if (!string.IsNullOrWhiteSpace(item.Component.PercentageOf) &&
                        item.Component.PercentageOf.Trim().ToLower() == "basic")
                    {
                        amount = basic * item.Value / 100;
                        Console.WriteLine($"Calculated From BASIC: {amount}");
                    }
                    else
                    {
                        amount = empSalary.Ctc * item.Value / 100;
                        Console.WriteLine($"Calculated From CTC: {amount}");
                    }
                }

                amount = Math.Round(amount, 2);
                gross += amount;

                payrollDetails.Add(CreatePayrollDetail(item.ComponentId, amount, userId));
            }

            /* ================= DEDUCTIONS ================= */

            var deductions = structureComponents
                .Where(x => x.Component.Type == "Deduction");

            foreach (var item in deductions)
            {
                decimal amount = 0;
                var calcType = item.CalculationType?.Trim().ToLower();

                Console.WriteLine("-------- DEDUCTION DEBUG --------");
                Console.WriteLine($"Component: {item.Component.ComponentName}");
                Console.WriteLine($"CalculationType: {item.CalculationType}");
                Console.WriteLine($"Value: {item.Value}");
                Console.WriteLine($"PercentageOf: {item.Component.PercentageOf}");
                Console.WriteLine($"Basic Used: {basic}");

                if (calcType == "fixed")
                {
                    amount = item.Value;
                }
                else if (calcType == "percentage")
                {
                    if (!string.IsNullOrWhiteSpace(item.Component.PercentageOf) &&
                        item.Component.PercentageOf.Trim().ToLower() == "basic")
                    {
                        amount = basic * item.Value / 100;
                        Console.WriteLine($"PF Calculated From BASIC: {amount}");
                    }
                    else
                    {
                        amount = empSalary.Ctc * item.Value / 100;
                        Console.WriteLine($"Deduction Calculated From CTC: {amount}");
                    }
                }

                amount = Math.Round(amount, 2);
                totalDeduction += amount;

                payrollDetails.Add(CreatePayrollDetail(item.ComponentId, amount, userId));

                Console.WriteLine($"Final Deduction Amount: {amount}");
                Console.WriteLine("----------------------------------");
            }

            Console.WriteLine($"Gross Salary: {gross}");
            Console.WriteLine($"Total Deduction: {totalDeduction}");
            Console.WriteLine($"Net Salary: {gross - totalDeduction}");
            Console.WriteLine("========== PAYROLL DEBUG END ==========\n");

            return (
                Math.Round(gross, 2),
                Math.Round(totalDeduction, 2),
                payrollDetails
            );
        }

        /* ============================================================
           PREVIEW PAYROLL
        ============================================================ */

        public async Task<List<PayrollTransactionDto>> PreviewPayrollAsync(ProcessPayrollRequestDto dto, int userId)
        {
            var resultList = new List<PayrollTransactionDto>();

            var activeSalaries = await _context.EmployeeSalaries
                .Where(x => x.IsActive && x.UserId == userId)
                .ToListAsync();

            foreach (var empSalary in activeSalaries)
            {
                var structureComponents = await _context.SalaryStructureComponents
                    .Include(x => x.Component)
                    .Where(x => x.StructureId == empSalary.StructureId)
                    .ToListAsync();

                var (gross, totalDeduction, payrollDetails) =
                    CalculatePayroll(empSalary, structureComponents, userId);

                resultList.Add(new PayrollTransactionDto
                {
                    EmployeeId = empSalary.EmployeeId,
                    Month = dto.Month,
                    Year = dto.Year,
                    GrossSalary = gross,
                    TotalDeductions = totalDeduction,
                    NetSalary = gross - totalDeduction,
                    Status = "Preview",
                    Details = payrollDetails.Select(d => new PayrollDetailDto
                    {
                        ComponentId = d.ComponentId,
                        Amount = d.Amount,
                        Type = structureComponents
                            .First(x => x.ComponentId == d.ComponentId)
                            .Component.Type,
                        ComponentName = structureComponents
                            .First(x => x.ComponentId == d.ComponentId)
                            .Component.ComponentName
                    }).ToList()
                });
            }

            return resultList;
        }

        /* ============================================================
           PROCESS PAYROLL
        ============================================================ */

        public async Task<string> ProcessPayrollAsync(ProcessPayrollRequestDto dto, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var activeSalaries = await _context.EmployeeSalaries
                    .Where(x => x.IsActive && x.UserId == userId)
                    .ToListAsync();

                foreach (var empSalary in activeSalaries)
                {
                    var alreadyProcessed = await _context.PayrollTransactions
                        .AnyAsync(x =>
                            x.EmployeeId == empSalary.EmployeeId &&
                            x.Month == dto.Month &&
                            x.Year == dto.Year &&
                            x.UserId == userId);

                    if (alreadyProcessed)
                        continue;

                    var structureComponents = await _context.SalaryStructureComponents
                        .Include(x => x.Component)
                        .Where(x => x.StructureId == empSalary.StructureId)
                        .ToListAsync();

                    var (gross, totalDeduction, payrollDetails) =
                        CalculatePayroll(empSalary, structureComponents, userId);

                    var payrollTransaction = new PayrollTransaction
                    {
                        EmployeeId = empSalary.EmployeeId,
                        Month = dto.Month,
                        Year = dto.Year,
                        GrossSalary = gross,
                        TotalDeductions = totalDeduction,
                        NetSalary = gross - totalDeduction,
                        Status = "Draft",
                        UserId = userId,
                        CompanyId = empSalary.CompanyId,
                        RegionId = empSalary.RegionId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.PayrollTransactions.Add(payrollTransaction);
                    await _context.SaveChangesAsync();

                    foreach (var detail in payrollDetails)
                    {
                        detail.PayrollId = payrollTransaction.PayrollId;
                        _context.PayrollDetails.Add(detail);
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return "Payroll Processed Successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        private PayrollDetail CreatePayrollDetail(int componentId, decimal amount, int userId)
        {
            return new PayrollDetail
            {
                ComponentId = componentId,
                Amount = Math.Round(amount, 2),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<List<PayrollTransactionDto>> GetPayrollByMonthAsync(int month, int year, int userId)
        {
            return await _context.PayrollTransactions
                .Where(x => x.Month == month && x.Year == year && x.UserId == userId)
                .Select(x => new PayrollTransactionDto
                {
                    PayrollId = x.PayrollId,
                    EmployeeId = x.EmployeeId,
                    Month = x.Month,
                    Year = x.Year,
                    GrossSalary = x.GrossSalary,
                    TotalDeductions = x.TotalDeductions,
                    NetSalary = x.NetSalary,
                    Status = x.Status,

                    Details = _context.PayrollDetails
                        .Where(d => d.PayrollId == x.PayrollId)
                        .Join(_context.SalaryComponents,
                              d => d.ComponentId,
                              c => c.ComponentId,
                              (d, c) => new PayrollDetailDto
                              {
                                  ComponentId = d.ComponentId,
                                  ComponentName = c.ComponentName,
                                  Amount = d.Amount,
                                  Type = c.Type
                              })
                        .ToList()
                })
                .ToListAsync();
        }
    
}
}
