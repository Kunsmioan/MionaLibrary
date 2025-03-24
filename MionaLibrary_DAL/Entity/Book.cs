using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int PublishYear { get; set; }

    public string Isbn { get; set; } = null!;

    public string? Genre { get; set; }

    public int Quantity { get; set; }

    public string Language { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public bool IsAvailable { get; set; }

    public int? Page { get; set; }

    public virtual ICollection<LoanHistory> LoanHistories { get; set; } = new List<LoanHistory>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
