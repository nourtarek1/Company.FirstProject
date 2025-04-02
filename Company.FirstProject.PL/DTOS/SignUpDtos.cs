using System.ComponentModel.DataAnnotations;

namespace Company.FirstProject.PL.DTOS
{
    public class SignUpDtos
    {
        [Required(ErrorMessage ="User Name Is Requird")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First Name Is Requird")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Is Requird")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Confirm Password dose not match the Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "You must agree to the terms and conditions.")]
        public bool IsAgree { get; set; }
    }
}
