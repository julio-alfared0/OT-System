using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OT.Models
{
    public class Pesanan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PenggunaId { get; set; }

        [Required]
        public DateTime TanggalPesanan { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalHarga { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PotonganHarga { get; set; }

        [Required]
        public int PoinDiperoleh { get; set; }

        [Required]
        [MaxLength(50)]
        public string MetodePembayaran { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string StatusPembayaran { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NomorSeriTransaksi { get; set; } = string.Empty;

        [ForeignKey("PenggunaId")]
        public virtual Pengguna? Pengguna { get; set; }

        public virtual ICollection<ItemPesanan> DetailPesanan { get; set; } = new List<ItemPesanan>();
    }
}
