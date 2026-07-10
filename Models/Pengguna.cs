using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OT.Models
{
    public class Pengguna
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NamaPengguna { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string NamaLengkap { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string KataSandiHash { get; set; } = string.Empty;

        [Required]
        public DateTime TanggalLahir { get; set; }

        [Required]
        [MaxLength(20)]
        public string Peran { get; set; } = "Customer";

        [Required]
        public int PoinLoyalitas { get; set; } = 0;

        [Required]
        [MaxLength(20)]
        public string TingkatMember { get; set; } = "Bronze";

        [Required]
        public DateTime DibuatPada { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Pesanan> DaftarPesanan { get; set; } = new List<Pesanan>();
        public virtual ICollection<ItemKeranjang> DaftarItemKeranjang { get; set; } = new List<ItemKeranjang>();
    }
}
