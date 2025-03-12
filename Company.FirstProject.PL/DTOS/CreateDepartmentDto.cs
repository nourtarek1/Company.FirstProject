using System.ComponentModel.DataAnnotations;

namespace Company.FirstProject.PL.DTOS
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage ="Code is requird")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is requird")]

        public string Name { get; set; }
        [Required(ErrorMessage = "CreateAt is requird")]

        public DateTime CreateAt { get; set; }
    }
}
