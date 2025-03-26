using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class ReturnRequest
{
    public int ReturnRequestId { get; set; }

    public int LoanId { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public DateTime RequestDate { get; set; }

    public string? Announce { get; set; }

    public string Status { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual Loan Loan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
