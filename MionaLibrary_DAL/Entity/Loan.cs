using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class Loan
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
