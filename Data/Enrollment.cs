using System;
using System.Collections.Generic;

namespace SchoolManagement.Data;

public partial class Enrollment
{
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public string? Grade { get; set; }

    public virtual Classez? Class { get; set; }

    public virtual Student? Student { get; set; }
}
