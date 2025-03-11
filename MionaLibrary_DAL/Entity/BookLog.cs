using System;
using System.Collections.Generic;

namespace MionaLibrary_DAL.Entity;

public partial class BookLog
{
    public int Id { get; set; }

    public int ManagerId { get; set; }

    public int? BookId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime ActionDate { get; set; }
}
