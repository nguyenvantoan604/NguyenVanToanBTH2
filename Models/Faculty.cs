using System.ComponentModel.DataAnnotations;

namespace DemoMvc.Models;

    public class Faculty
    {
        [Key]
        public string FacultyID {get;set;}
        public string FacultyName {get;set;}
    }