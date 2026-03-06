using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Implementations
{
    public class DashboardService : IDashboardService
    {

        private readonly HRMSContext _context;

        public DashboardService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardEmployees(int companyId)
        {
            var employees = await _context.Users
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();

            var result = new DashboardDto
            {
                TotalEmployees = employees.Count(),
                
            };

            return result;
        }
    }
}
