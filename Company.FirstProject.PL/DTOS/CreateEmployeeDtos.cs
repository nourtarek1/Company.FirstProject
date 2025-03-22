using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.FirstProject.PL.DTOS
{
    public class CreateEmployeeDtos
    {
        [Required(ErrorMessage = "Name is Required !")]
        public string Name { get; set; }

        [Range(20, 60 ,ErrorMessage = "AgeMust Be Between 20 and 60")]

        public int? Age { get; set; }
        [DataType(DataType.EmailAddress,ErrorMessage ="Email is not Valid")]
        public string Email { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
                       , ErrorMessage ="Address must be Like 123-Street-city-country")]
        public string Adress { get; set; }

        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]

        public DateTime HiringDate { get; set; }

        [DisplayName("Date Of Create")]
        public DateTime CreateAt { get; set; }
    }
}
