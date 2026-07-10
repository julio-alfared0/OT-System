using System.ComponentModel.DataAnnotations;

namespace OT.ViewModels
{
    public class MasukViewModel
    {
        [Required(ErrorMessage = "Email atau Nama Pengguna wajib diisi")]
        public string EmailAtauUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kata Sandi wajib diisi")]
        [DataType(DataType.Password)]
        public string KataSandi { get; set; } = string.Empty;

        public bool IngatSaya { get; set; }
    }
}
