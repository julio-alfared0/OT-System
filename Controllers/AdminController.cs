using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OT.Services;
using OT.ViewModels;

namespace OT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILayananDashboardAdmin _layananDashboard;

        public AdminController(ILayananDashboardAdmin layananDashboard)
        {
            _layananDashboard = layananDashboard;
        }

        public async Task<IActionResult> Index()
        {
            var statistik = await _layananDashboard.DapatkanStatistikDashboardAsync();
            var jsonMentah = await _layananDashboard.DapatkanDataGrafikJsonAsync();

            var viewModel = new DashboardAdminViewModel
            {
                TotalPendapatan = statistik.TotalPendapatan,
                TotalPesanan = statistik.TotalPesanan,
                TotalMemberAktif = statistik.TotalMemberAktif,
                ProdukStokKritis = statistik.DaftarProdukStokKritis,
                DaftarTransaksiTerbaru = statistik.DaftarTransaksiTerbaru,
                DaftarSemuaProduk = await _layananDashboard.DapatkanSemuaProdukAsync()
            };

            using (var dokumenJson = JsonDocument.Parse(jsonMentah))
            {
                viewModel.DataGrafikPenjualanJson = dokumenJson.RootElement.GetProperty("PenjualanHarian").ToString();
                viewModel.DataDistribusiKategoriJson = dokumenJson.RootElement.GetProperty("DistribusiKategori").ToString();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStok(System.Guid produkId, int stokBaru)
        {
            await _layananDashboard.PerbaruiStokProdukAsync(produkId, stokBaru);
            TempData["PesanSukses"] = "Stok produk berhasil diperbarui secara real-time!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AturStatusStok(System.Guid produkId, bool isHabis)
        {
            await _layananDashboard.AturStatusStokAsync(produkId, isHabis);
            TempData["PesanSukses"] = isHabis ? "Produk diatur sebagai STOK HABIS dan langsung tercermin ke pengguna." : "Produk berhasil di-restock (50 unit)!";
            return RedirectToAction("Index");
        }
    }
}
