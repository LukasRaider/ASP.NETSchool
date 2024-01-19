using System.ComponentModel;

namespace ASP.NETSchool.Models {
    public class Student {
        public int Id { get; set; }
        [DisplayName("First name")]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
