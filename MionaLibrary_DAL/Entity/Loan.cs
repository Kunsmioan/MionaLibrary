﻿using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class Loan
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public int RenewalCount { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Renewal> Renewals { get; set; } = new List<Renewal>();

    public virtual User User { get; set; } = null!;
}
