using ASP.NETSchool.Models;
using ASP.NETSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETSchool.Controllers {
    [Authorize(Roles ="Teacher, Admin")]
    public class StudentsController : Controller {
        private StudentService service;

        public StudentsController(StudentService studentService) {
            this.service = studentService;
        }

        public async Task<IActionResult> IndexAsync() {
            var allStudents = await service.GetAllAsync();
            return View(allStudents);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Student student) {
            await service.CreateStudentAsync(student);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id) {
            var studentToEdit = await service.GetById(id);
            if (studentToEdit == null) {
                return View("NotFound");
            }
            return View(studentToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id, FirstName, LastName, DateOfBirth")] Student student) {
            await service.UpdateAsync(student);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteAsync(int id) {
            var studentToDelete = await service.GetById(id);
            if (studentToDelete == null) {
                return View("NotFound");
            }
            await service.DeleteAsync(studentToDelete);
            return RedirectToAction("Index");
        }
    }
}
