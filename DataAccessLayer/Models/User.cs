using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class User
{
    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
