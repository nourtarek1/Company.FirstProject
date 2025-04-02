using System.ComponentModel.DataAnnotations;

namespace Company.FirstProject.PL.DTOS
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
