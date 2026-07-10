using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OT.Models
{
    public class Kategori
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NamaKategori { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Deskripsi { get; set; } = string.Empty;

        public virtual ICollection<Produk> DaftarProduk { get; set; } = new List<Produk>();
    }
}
