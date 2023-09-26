using System;
using System.Collections.Generic;

namespace SchoolManagement.Data;

public partial class Classez
{
    public int Id { get; set; }

    public int? LecturerId { get; set; }

    public int? CoursesId { get; set; }

    public TimeSpan? Time { get; set; }

    public virtual Course? Courses { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Lecturer? Lecturer { get; set; }
}
