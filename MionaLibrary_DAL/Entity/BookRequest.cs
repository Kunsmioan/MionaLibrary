using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class BookRequest
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime RequestDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Announce { get; set; }

    public string? StatusColor { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
