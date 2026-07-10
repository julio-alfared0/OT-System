using System.ComponentModel.DataAnnotations;

namespace OT.ViewModels
{
    public class ResetKataSandiViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string KodeOtp { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kata Sandi Baru wajib diisi")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Kata Sandi minimal 6 karakter")]
        public string KataSandiBaru { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konfirmasi Kata Sandi Baru wajib diisi")]
        [DataType(DataType.Password)]
        [Compare("KataSandiBaru", ErrorMessage = "Kata Sandi tidak cocok")]
        public string KonfirmasiKataSandiBaru { get; set; } = string.Empty;
    }
}
