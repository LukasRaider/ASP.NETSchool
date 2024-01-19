using ASP.NETSchool.Models;
using ASP.NETSchool.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSchool.Services {
    public class GradeService {
        private ApplicationDbContext dbContext;

        public GradeService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<GradesDropdownsViewModel> GetDropdownValuesAsync() {
            return new GradesDropdownsViewModel() {
                Students = await dbContext.Students.OrderBy(x => x.LastName).ToListAsync(),
                Subjects = await dbContext.Subjects.ToListAsync()
            };
        }

        internal async Task CreateAsync(GradeViewModel gradeModel) {
            var gradeToInsert = new Grade() {
                Student = dbContext.Students.FirstOrDefault(st => st.Id == gradeModel.StudentId),
                Subject = dbContext.Subjects.FirstOrDefault(sub => sub.Id == gradeModel.SubjectId),
                Date = DateTime.Today,
                Topic = gradeModel.Topic,
                Mark = gradeModel.Mark
            };
            if (gradeToInsert.Student != null && gradeToInsert.Subject != null) {
                await dbContext.Grades.AddAsync(gradeToInsert);
                await dbContext.SaveChangesAsync();
            }
        }
        internal async Task<IEnumerable<Grade>> GetAllGrades() {
            return await dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject).ToListAsync();
        }

        internal GradeViewModel GetById(int id) {
            var gradeToEdit = dbContext.Grades.FirstOrDefault(gr => gr.Id == id);
            //GradeViewModel gradeViewModel = new GradeViewModel();
            if (gradeToEdit != null) {
                return new GradeViewModel() {
                    SubjectId = gradeToEdit.Subject.Id,
                    StudentId = gradeToEdit.Student.Id,
                    Id = gradeToEdit.Id,
                    Mark = gradeToEdit.Mark,
                    Date = gradeToEdit.Date,
                    Topic = gradeToEdit.Topic
                };
            }
            return null;
        }

        internal async Task UpdateAsync(GradeViewModel updatedGrade) {
            var gradeToUpdate = dbContext.Grades.FirstOrDefault(gr => gr.Id == updatedGrade.Id);
            if (gradeToUpdate != null) {
                gradeToUpdate.Subject = dbContext.Subjects.FirstOrDefault(sub => sub.Id == updatedGrade.SubjectId);
                gradeToUpdate.Student = dbContext.Students.FirstOrDefault(st => st.Id == updatedGrade.StudentId);
                gradeToUpdate.Topic = updatedGrade.Topic;
                gradeToUpdate.Mark = updatedGrade.Mark;
                //gradeToUpdate.Date = updatedGrade.Date;
            }
            dbContext.Update(gradeToUpdate);
            await dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id) {
            var gradeToDelete = dbContext.Grades.FirstOrDefault(gr => gr.Id == id);
            if (gradeToDelete != null) {
                dbContext.Remove(gradeToDelete);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
