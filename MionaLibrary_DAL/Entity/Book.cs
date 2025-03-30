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

    public int GenreId { get; set; }

    public int LanguageId { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public bool IsAvailable { get; set; }

    public int? Page { get; set; }

    public virtual ICollection<BookRequest> BookRequests { get; set; } = new List<BookRequest>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();
}
