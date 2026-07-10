using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OT.Models
{
    public class Produk
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int KategoriId { get; set; }

        [Required]
        [MaxLength(200)]
        public string NamaProduk { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string KodeSKU { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Harga { get; set; }

        [Required]
        public float PersentaseAlkohol { get; set; }

        [Required]
        public int VolumeML { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Deskripsi { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string JalurGambar { get; set; } = string.Empty;

        [Required]
        public int Stok { get; set; } = 0;

        [Required]
        public int ImbalanPoin { get; set; } = 0;

        [Required]
        public DateTime DibuatPada { get; set; } = DateTime.UtcNow;

        [ForeignKey("KategoriId")]
        public virtual Kategori? Kategori { get; set; }
    }
}
