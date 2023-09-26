using System;
using System.Collections.Generic;

namespace SchoolManagement.Data;

public partial class Lecturer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public virtual ICollection<Classez> Classezs { get; set; } = new List<Classez>();
}
