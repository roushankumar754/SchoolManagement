using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagement.Data;
public class ClassMetaData
{
    [Display(Name ="Lecturer")]
    public int LecturerId{get;set;}
    [Display(Name ="Course")]
    public int CoursesId{get;set;}
    
}
[ModelMetadataType(typeof(ClassMetaData))]
public partial class Classez{}