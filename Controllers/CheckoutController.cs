using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OT.Data;
using OT.Models;
using OT.Services;
using OT.ViewModels;

namespace OT.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _konteks;
        private readonly ILayananLoyalitas _layananLoyalitas;
        private readonly ILayananStruk _layananStruk;

        public CheckoutController(ApplicationDbContext konteks, ILayananLoyalitas layananLoyalitas, ILayananStruk layananStruk)
        {
            _konteks = konteks;
            _layananLoyalitas = layananLoyalitas;
            _layananStruk = layananStruk;
        }

        private Guid DapatkanIdPengguna()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin") || User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                TempData["PesanInfo"] = "Akun Administrator tidak menggunakan fitur keranjang belanja.";
                return RedirectToAction("Index", "Admin");
            }

            var penggunaId = DapatkanIdPengguna();

            var daftarItem = await _konteks.ItemKeranjang
                .Include(i => i.Produk)
                .Where(i => i.PenggunaId == penggunaId)
                .ToListAsync();

            foreach (var item in daftarItem)
            {
                if (item.Produk != null && !string.IsNullOrEmpty(item.Produk.JalurGambar))
                {
                    item.Produk.JalurGambar = item.Produk.JalurGambar.Replace("/assets/image/", "/assets/webp/").Replace("/OT-Product/webp/", "/assets/webp/");
                }
            }

            var pengguna = await _konteks.Pengguna.FindAsync(penggunaId);
            var totalHarga = daftarItem.Sum(i => i.HargaSatuan * i.Kuantitas);

            var viewModel = new CheckoutViewModel
            {
                DaftarItem = daftarItem,
                TotalHarga = totalHarga,
                SaldoPoinLoyalitas = pengguna?.PoinLoyalitas ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProsesQris()
        {
            var penggunaId = DapatkanIdPengguna();
            var pengguna = await _konteks.Pengguna.FindAsync(penggunaId);

            if (pengguna == null) return Unauthorized();

            var daftarItem = await _konteks.ItemKeranjang
                .Include(i => i.Produk)
                .Where(i => i.PenggunaId == penggunaId)
                .ToListAsync();

            if (!daftarItem.Any())
            {
                return RedirectToAction("Index", "Beranda");
            }

            var totalHarga = daftarItem.Sum(i => i.HargaSatuan * i.Kuantitas);
            var poinDidapat = _layananLoyalitas.KalkulasiPoinBelanja(totalHarga);

            var pesananBaru = new Pesanan
            {
                Id = Guid.NewGuid(),
                PenggunaId = penggunaId,
                TanggalPesanan = DateTime.UtcNow,
                TotalHarga = totalHarga,
                PotonganHarga = 0,
                PoinDiperoleh = poinDidapat,
                MetodePembayaran = "QRIS",
                StatusPembayaran = "Menunggu Pembayaran",
                NomorSeriTransaksi = "QRIS-OT-" + DateTime.Now.Ticks
            };

            _konteks.Pesanan.Add(pesananBaru);

            foreach (var item in daftarItem)
            {
                var itemPesanan = new ItemPesanan
                {
                    Id = Guid.NewGuid(),
                    PesananId = pesananBaru.Id,
                    ProdukId = item.ProdukId,
                    Kuantitas = item.Kuantitas,
                    HargaSatuan = item.HargaSatuan
                };
                
                _konteks.ItemPesanan.Add(itemPesanan);
                
                if (item.Produk != null)
                {
                    item.Produk.Stok -= item.Kuantitas;
                }
            }

            _konteks.ItemKeranjang.RemoveRange(daftarItem);

            await _konteks.SaveChangesAsync();

            return RedirectToAction("Qris", new { pesananId = pesananBaru.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Qris(Guid pesananId)
        {
            var penggunaId = DapatkanIdPengguna();

            var pesanan = await _konteks.Pesanan
                .Include(p => p.DetailPesanan)
                    .ThenInclude(dp => dp.Produk)
                .FirstOrDefaultAsync(p => p.Id == pesananId && p.PenggunaId == penggunaId);

            if (pesanan == null)
            {
                return NotFound();
            }

            if (pesanan.StatusPembayaran == "Lunas")
            {
                return RedirectToAction("Sukses", new { pesananId = pesanan.Id });
            }

            return View(pesanan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KonfirmasiQris(Guid pesananId)
        {
            var penggunaId = DapatkanIdPengguna();

            var pesanan = await _konteks.Pesanan
                .Include(p => p.Pengguna)
                .FirstOrDefaultAsync(p => p.Id == pesananId && p.PenggunaId == penggunaId);

            if (pesanan != null && pesanan.StatusPembayaran == "Menunggu Pembayaran")
            {
                pesanan.StatusPembayaran = "Lunas";

                if (pesanan.Pengguna != null)
                {
                    pesanan.Pengguna.PoinLoyalitas += pesanan.PoinDiperoleh;
                    _layananLoyalitas.PerbaruiTingkatMember(pesanan.Pengguna);
                }

                await _konteks.SaveChangesAsync();
            }

            return RedirectToAction("Sukses", new { pesananId = pesananId });
        }

        [HttpGet]
        public async Task<IActionResult> Sukses(Guid pesananId)
        {
            var penggunaId = DapatkanIdPengguna();

            var pesanan = await _konteks.Pesanan
                .Include(p => p.DetailPesanan)
                    .ThenInclude(dp => dp.Produk)
                .FirstOrDefaultAsync(p => p.Id == pesananId && p.PenggunaId == penggunaId);

            if (pesanan == null)
            {
                return NotFound();
            }

            return View(pesanan);
        }

        [HttpGet]
        public async Task<IActionResult> Riwayat()
        {
            var penggunaId = DapatkanIdPengguna();

            var daftarPesanan = await _konteks.Pesanan
                .Include(p => p.DetailPesanan)
                    .ThenInclude(dp => dp.Produk)
                .Where(p => p.PenggunaId == penggunaId)
                .OrderByDescending(p => p.TanggalPesanan)
                .ToListAsync();

            return View(daftarPesanan);
        }

        [HttpGet]
        public async Task<IActionResult> Struk(Guid pesananId)
        {
            var penggunaId = DapatkanIdPengguna();

            var pesanan = await _konteks.Pesanan
                .Include(p => p.DetailPesanan)
                    .ThenInclude(dp => dp.Produk)
                .FirstOrDefaultAsync(p => p.Id == pesananId && p.PenggunaId == penggunaId);

            if (pesanan == null)
            {
                return NotFound();
            }

            string kutipan = _layananStruk.DapatkanKutipanAcak();
            var namaProdukUtama = pesanan.DetailPesanan.FirstOrDefault()?.Produk?.NamaProduk ?? "Produk OT Spesial";
            string stringBarcode = $"[BARCODE: {pesanan.NomorSeriTransaksi} - {namaProdukUtama}]";

            var viewModel = new StrukViewModel
            {
                Pesanan = pesanan,
                KutipanLegendarisOT = kutipan,
                StringBarcodeQrisSimulasi = stringBarcode
            };

            return View(viewModel);
        }
    }
}
