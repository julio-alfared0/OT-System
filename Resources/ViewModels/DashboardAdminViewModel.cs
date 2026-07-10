using System.Collections.Generic;
using OT.Models;

namespace OT.ViewModels
{
    public class DashboardAdminViewModel
    {
        public decimal TotalPendapatan { get; set; }
        public int TotalPesanan { get; set; }
        public int TotalMemberAktif { get; set; }
        public string DataGrafikPenjualanJson { get; set; } = string.Empty;
        public string DataDistribusiKategoriJson { get; set; } = string.Empty;
        public List<Produk> ProdukStokKritis { get; set; } = new List<Produk>();
        public List<Pesanan> DaftarTransaksiTerbaru { get; set; } = new List<Pesanan>();
        public List<Produk> DaftarSemuaProduk { get; set; } = new List<Produk>();
    }
}
