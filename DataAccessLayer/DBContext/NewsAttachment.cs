using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class NewsAttachment
{
    public int AttachmentId { get; set; }

    public int NewsId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? ContentType { get; set; }

    public long? FileSizeBytes { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public virtual CompanyNews News { get; set; } = null!;
}
