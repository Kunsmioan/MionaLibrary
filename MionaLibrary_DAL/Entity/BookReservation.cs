using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class BookReservation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateOnly ReserveDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
