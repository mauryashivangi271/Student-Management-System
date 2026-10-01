
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // STUDENT LIST + SEARCH/FILTER
        // =========================
        public async Task<IActionResult> Index(
            string? search,
            int? courseId,
            int? departmentId)
        {
            var query = _context.Students
                .Include(s => s.Course)
                .Include(s => s.Department)
                .AsQueryable();

            // Search by Name, Email or Phone
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.Email.Contains(search) ||
                    s.Phone.Contains(search));
            }

            // Course filter
            if (courseId.HasValue)
            {
                query = query.Where(s =>
                    s.CourseId == courseId.Value);
            }

            // Department filter
            if (departmentId.HasValue)
            {
                query = query.Where(s =>
                    s.DepartmentId == departmentId.Value);
            }

            var students = await query
                .OrderByDescending(s => s.StudentId)
                .ToListAsync();

            // Preserve selected values
            ViewBag.Search = search;
            ViewBag.CourseId = courseId;
            ViewBag.DepartmentId = departmentId;

            // Course filter dropdown
            ViewBag.FilterCourses = new SelectList(
                await _context.Courses
                    .Where(c => c.Status == "Active")
                    .OrderBy(c => c.CourseName)
                    .ToListAsync(),
                "CourseId",
                "CourseName",
                courseId);

            // Department filter dropdown
            ViewBag.FilterDepartments = new SelectList(
                await _context.Departments
                    .Where(d => d.Status == "Active")
                    .OrderBy(d => d.DepartmentName)
                    .ToListAsync(),
                "DepartmentId",
                "DepartmentName",
                departmentId);

            return View(students);
        }

        // =========================
        // CREATE STUDENT - GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        // =========================
        // CREATE STUDENT - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Student student,
            string? newCourseName,
            string? newCourseCode,
            string? newCourseDuration)
        {
            // If user is creating a new course
            if (!string.IsNullOrWhiteSpace(newCourseName))
            {
                // CourseId is not required because
                // we will create a new course.
                ModelState.Remove(nameof(Student.CourseId));

                // Course code is required for new course
                if (string.IsNullOrWhiteSpace(newCourseCode))
                {
                    ModelState.AddModelError(
                        "NewCourseCode",
                        "Course code is required when adding a new course.");
                }
            }

            if (ModelState.IsValid)
            {
                // =========================
                // CREATE NEW COURSE
                // =========================
                if (!string.IsNullOrWhiteSpace(newCourseName))
                {
                    var courseName =
                        newCourseName.Trim().ToLower();

                    var courseCode =
                        newCourseCode!.Trim().ToLower();

                    // Check duplicate course
                    var existingCourse =
                        await _context.Courses
                            .FirstOrDefaultAsync(c =>
                                c.CourseName.ToLower() == courseName ||
                                c.CourseCode.ToLower() == courseCode);

                    if (existingCourse != null)
                    {
                        ModelState.AddModelError(
                            "NewCourseName",
                            "A course with this name or course code already exists.");

                        await LoadDropdownsAsync(
                            student.CourseId,
                            student.DepartmentId);

                        return View(student);
                    }

                    // Create new course
                    var newCourse = new Course
                    {
                        CourseName = newCourseName.Trim(),

                        CourseCode =
                            newCourseCode.Trim().ToUpper(),

                        Duration =
                            string.IsNullOrWhiteSpace(newCourseDuration)
                                ? null
                                : newCourseDuration.Trim(),

                        Status = "Active"
                    };

                    _context.Courses.Add(newCourse);

                    await _context.SaveChangesAsync();

                    // Connect student with new course
                    student.CourseId = newCourse.CourseId;
                }

                // =========================
                // SAVE STUDENT
                // =========================
                _context.Students.Add(student);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If validation fails
            await LoadDropdownsAsync(
                student.CourseId,
                student.DepartmentId);

            return View(student);
        }

        // =========================
        // STUDENT DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(
                    s => s.StudentId == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // =========================
        // EDIT STUDENT - GET
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(
                    s => s.StudentId == id);

            if (student == null)
                return NotFound();

            await LoadDropdownsAsync(
                student.CourseId,
                student.DepartmentId);

            return View(student);
        }

        // =========================
        // EDIT STUDENT - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Student student)
        {
            if (id != student.StudentId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Students.Update(student);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownsAsync(
                student.CourseId,
                student.DepartmentId);

            return View(student);
        }

        // =========================
        // DELETE STUDENT - GET
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(
                    s => s.StudentId == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // =========================
        // DELETE STUDENT - POST
        // =========================
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var student =
                await _context.Students
                    .FirstOrDefaultAsync(
                        s => s.StudentId == id);

            if (student != null)
            {
                _context.Students.Remove(student);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // LOAD DROPDOWNS
        // =========================
        private async Task LoadDropdownsAsync(
            int? selectedCourseId = null,
            int? selectedDepartmentId = null)
        {
            var courses =
                await _context.Courses
                    .Where(c => c.Status == "Active")
                    .OrderBy(c => c.CourseName)
                    .ToListAsync();

            var departments =
                await _context.Departments
                    .Where(d => d.Status == "Active")
                    .OrderBy(d => d.DepartmentName)
                    .ToListAsync();

            ViewBag.Courses =
                new SelectList(
                    courses,
                    "CourseId",
                    "CourseName",
                    selectedCourseId);

            ViewBag.Departments =
                new SelectList(
                    departments,
                    "DepartmentId",
                    "DepartmentName",
                    selectedDepartmentId);
        }
    }
}

