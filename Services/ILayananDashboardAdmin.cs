using System.Collections.Generic;
using System.Threading.Tasks;
using OT.Models;

namespace OT.Services
{
    public class StatistikDashboardDto
    {
        public decimal TotalPendapatan { get; set; }
        public int TotalPesanan { get; set; }
        public int TotalMemberAktif { get; set; }
        public List<Produk> DaftarProdukStokKritis { get; set; } = new List<Produk>();
        public List<Pesanan> DaftarTransaksiTerbaru { get; set; } = new List<Pesanan>();
    }

    public interface ILayananDashboardAdmin
    {
        Task<StatistikDashboardDto> DapatkanStatistikDashboardAsync();
        Task<string> DapatkanDataGrafikJsonAsync();
        Task<List<Produk>> DapatkanSemuaProdukAsync();
        Task<bool> PerbaruiStokProdukAsync(System.Guid produkId, int stokBaru);
        Task<bool> AturStatusStokAsync(System.Guid produkId, bool isHabis);
    }
}
