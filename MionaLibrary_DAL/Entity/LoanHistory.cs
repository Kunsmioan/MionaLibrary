using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class LoanHistory
{
    public int HistoryId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime ReturnDate { get; set; }

    public int? OverdueDays { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
