using System;
using System.Collections.Generic;

namespace coursach.Models;

public partial class Request
{
    public int Id { get; set; }

    public string FullNameClient { get; set; } = null!;

    public int ClientId { get; set; }

    public string? TechnicalTask { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? TakeDate { get; set; }

    public int StatusId { get; set; }

    public int? EmployeeInfId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual EmployeeInformation? EmployeeInf { get; set; }

    public virtual Status Status { get; set; } = null!;
}
