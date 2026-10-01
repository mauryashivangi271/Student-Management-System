using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }

        // Course Relationship
        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public Course? Course { get; set; }

        // Department Relationship
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
    }
}