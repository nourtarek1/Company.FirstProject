using System.ComponentModel.DataAnnotations;

namespace Company.FirstProject.PL.DTOS
{
    public class SignInDtos
    {
        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress] public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RemberMe { get; set; }

    }
}
