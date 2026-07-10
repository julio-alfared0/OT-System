using System.ComponentModel.DataAnnotations;

namespace OT.ViewModels
{
    public class VerifikasiOtpViewModel
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kode OTP wajib diisi")]
        public string KodeOtp { get; set; } = string.Empty;
    }
}
