using System.ComponentModel.DataAnnotations;

namespace OT.ViewModels
{
    public class LupaKataSandiViewModel
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format Email tidak valid")]
        public string Email { get; set; } = string.Empty;
    }
}
