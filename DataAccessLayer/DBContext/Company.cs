using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyCode { get; set; }

    public string? IndustryType { get; set; }

    public string? Headquarters { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? IsDefault { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<AssetStatus> AssetStatuses { get; set; } = new List<AssetStatus>();

    public virtual ICollection<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();

    public virtual ICollection<AttendanceStatus> AttendanceStatuses { get; set; } = new List<AttendanceStatus>();

    public virtual ICollection<BloodGroup> BloodGroups { get; set; } = new List<BloodGroup>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Designation> Designations { get; set; } = new List<Designation>();

    public virtual ICollection<ExpenseCategoryType> ExpenseCategoryTypes { get; set; } = new List<ExpenseCategoryType>();

    public virtual ICollection<ExpenseStatus> ExpenseStatuses { get; set; } = new List<ExpenseStatus>();

    public virtual ICollection<Gender> Genders { get; set; } = new List<Gender>();

    public virtual ICollection<HelpDeskCategory> HelpDeskCategories { get; set; } = new List<HelpDeskCategory>();

    public virtual ICollection<HolidayList> HolidayLists { get; set; } = new List<HolidayList>();

    public virtual ICollection<KpiCategory> KpiCategories { get; set; } = new List<KpiCategory>();

    public virtual ICollection<LeaveStatus> LeaveStatuses { get; set; } = new List<LeaveStatus>();

    public virtual ICollection<LeaveType> LeaveTypes { get; set; } = new List<LeaveType>();

    public virtual ICollection<MaritalStatus> MaritalStatuses { get; set; } = new List<MaritalStatus>();

    public virtual ICollection<PolicyCategory> PolicyCategories { get; set; } = new List<PolicyCategory>();

    public virtual ICollection<Priority> Priorities { get; set; } = new List<Priority>();

    public virtual ICollection<ProjectStatus> ProjectStatuses { get; set; } = new List<ProjectStatus>();

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    public virtual ICollection<Relationship> Relationships { get; set; } = new List<Relationship>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Weekoff> Weekoffs { get; set; } = new List<Weekoff>();
}
