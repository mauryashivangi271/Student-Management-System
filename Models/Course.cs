using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Display(Name = "Duration")]
        public string? Duration { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";

        // Students enrolled in this course
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}