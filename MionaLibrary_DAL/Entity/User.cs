using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public string Role { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? Phone { get; set; } = null!;

    public virtual ICollection<BookRequest> BookRequests { get; set; } = new List<BookRequest>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();
}
