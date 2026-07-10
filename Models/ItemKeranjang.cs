using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OT.Models
{
    public class ItemKeranjang
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PenggunaId { get; set; }

        [Required]
        public Guid ProdukId { get; set; }

        [Required]
        public int Kuantitas { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HargaSatuan { get; set; }

        [ForeignKey("PenggunaId")]
        public virtual Pengguna? Pengguna { get; set; }

        [ForeignKey("ProdukId")]
        public virtual Produk? Produk { get; set; }
    }
}
