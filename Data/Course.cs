using System;
using System.Collections.Generic;

namespace SchoolManagement.Data;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public int? Credits { get; set; }

    public virtual ICollection<Classez> Classezs { get; set; } = new List<Classez>();
}
