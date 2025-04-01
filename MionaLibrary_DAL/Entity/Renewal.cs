using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class Renewal
{
    public int Id { get; set; }

    public int LoanId { get; set; }

    public DateTime RenewalDate { get; set; }

    public DateTime NewDueDate { get; set; }

    public int UserId { get; set; }

    public virtual Loan Loan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
