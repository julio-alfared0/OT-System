using System.Collections.Generic;
using OT.Models;

namespace OT.ViewModels
{
    public class ProfilViewModel
    {
        public Pengguna? DetailPengguna { get; set; }
        public List<Pesanan> RiwayatPesanan { get; set; } = new List<Pesanan>();
        public int SisaPoin { get; set; }
        public int PersentaseKeTingkatSelanjutnya { get; set; }
    }
}
