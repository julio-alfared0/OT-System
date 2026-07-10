using System;
using System.ComponentModel.DataAnnotations;

namespace OT.ViewModels
{
    public class DaftarViewModel
    {
        [Required(ErrorMessage = "Nama Lengkap wajib diisi")]
        [StringLength(150)]
        public string NamaLengkap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format Email tidak valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Pengguna wajib diisi")]
        [StringLength(100)]
        public string NamaPengguna { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kata Sandi wajib diisi")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Kata Sandi minimal 6 karakter")]
        public string KataSandi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konfirmasi Kata Sandi wajib diisi")]
        [DataType(DataType.Password)]
        [Compare("KataSandi", ErrorMessage = "Kata Sandi dan Konfirmasi tidak cocok")]
        public string KonfirmasiKataSandi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tanggal Lahir wajib diisi")]
        [DataType(DataType.Date)]
        public DateTime TanggalLahir { get; set; }
    }
}
