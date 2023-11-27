using System;
using System.Collections.Generic;

namespace coursach.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<EmployeeInformation> EmployeeInformations { get; set; } = new List<EmployeeInformation>();

    public virtual Role Role { get; set; } = null!;
}
