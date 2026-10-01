# 🎓 Student Management System

A professional web-based Student Management System built using **ASP.NET Core MVC, C#, MySQL, and Entity Framework Core**.

The application provides a centralized platform for managing students, courses, departments, and user authentication with a clean and responsive dashboard.

---

## 🚀 Features

### 🔐 Authentication
- User Registration
- Secure Login
- Logout
- Cookie-based Authentication
- Protected Dashboard and Management Pages

### 👨‍🎓 Student Management
- Add Student
- View Student Details
- Edit Student
- Delete Student
- Search Students
- Filter by Course
- Filter by Department
- Filter by Enrollment Date
- Duplicate Email Validation

### 📚 Course Management
- Add Course
- View Course Details
- Edit Course
- Delete Course
- Student Count per Course
- Prevent deletion of courses assigned to students

### 🏢 Department Management
- Add Department
- View Department Details
- Edit Department
- Delete Department
- Student Count per Department
- Prevent deletion of departments assigned to students

### 📊 Dashboard
- Total Students
- Total Courses
- Total Departments
- Recent Students
- Course-wise Student Chart
- Department-wise Student Chart
- Quick Action Buttons

### 🛡️ Validation & Error Handling
- Form validation
- Duplicate email protection
- Custom error page
- Custom 404 page
- Database relationship protection

---

## 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| C# | Backend Programming |
| ASP.NET Core MVC | Web Application Framework |
| Entity Framework Core | ORM / Database Access |
| MySQL | Relational Database |
| Bootstrap | Responsive UI |
| Bootstrap Icons | UI Icons |
| Chart.js | Dashboard Charts |
| HTML5 | Frontend Structure |
| CSS3 | Styling |
| JavaScript | Client-side Interaction |

---

## 🗄️ Database Structure

The application uses a relational MySQL database.

### Main Tables

- `Users`
- `Students`
- `Courses`
- `Departments`

### Relationships

```text
Courses
   │
   └── Students

Departments
   │
   └── Students

Users
   │
   └── Authentication
