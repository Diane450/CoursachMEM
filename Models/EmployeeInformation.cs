using System;
using System.Collections.Generic;

namespace coursach.Models;

public partial class EmployeeInformation
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
