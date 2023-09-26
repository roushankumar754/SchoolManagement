using SchoolManagement.Data;
namespace SchoolManagement.Models;

public class ClassEnrollmentViewModel
{
    public ClassViewModel ? Classez { get; set; }
    public List<StudentEnrollmentViewModel> Students{ get; set;}=new List<StudentEnrollmentViewModel>();

}
