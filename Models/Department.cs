using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; } = string.Empty;

        [Display(Name = "Head of Department")]
        public string? HeadOfDepartment { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";

        // Students belonging to this department
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}