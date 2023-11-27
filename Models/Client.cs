using System;
using System.Collections.Generic;

namespace coursach.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
