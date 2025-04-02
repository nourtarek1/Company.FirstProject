using System.ComponentModel.DataAnnotations;

namespace Company.FirstProject.PL.DTOS
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "New Password Is Requird")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "New Password Is Requird")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose not match the New Password")]
        public string ConfirmPassword { get; set; }
    }
}
