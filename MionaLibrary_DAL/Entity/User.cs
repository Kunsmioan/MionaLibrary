using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public string Role { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();

    public virtual ICollection<LoanHistory> LoanHistories { get; set; } = new List<LoanHistory>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
