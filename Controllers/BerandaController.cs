using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OT.Data;
using OT.Services;

namespace OT.Controllers
{
    public class BerandaController : Controller
    {
        private readonly ApplicationDbContext _konteks;
        private readonly ILayananRiwayatLihat _layananRiwayatLihat;

        public BerandaController(ApplicationDbContext konteks, ILayananRiwayatLihat layananRiwayatLihat)
        {
            _konteks = konteks;
            _layananRiwayatLihat = layananRiwayatLihat;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var produkKatalog = await _konteks.Produk
                    .Include(p => p.Kategori)
                    .ToListAsync();
                    
                return View("Beranda", produkKatalog);
            }

            return View("PraOtentikasi");
        }

        public IActionResult Warisan()
        {
            return View("Warisan");
        }

        public async Task<IActionResult> Katalog(string? cari)
        {
            var query = _konteks.Produk.Include(p => p.Kategori).AsQueryable();
            if (!string.IsNullOrWhiteSpace(cari))
            {
                var keyword = cari.ToLower();
                query = query.Where(p => p.NamaProduk.ToLower().Contains(keyword) || 
                                         (p.Deskripsi != null && p.Deskripsi.ToLower().Contains(keyword)) ||
                                         (p.Kategori != null && p.Kategori.NamaKategori.ToLower().Contains(keyword)));
            }
            var produkKatalog = await query.ToListAsync();
            ViewBag.KeywordCari = cari;
            return View("Katalog", produkKatalog);
        }

        [HttpGet]
        public async Task<IActionResult> CariProdukJson(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>());
            }
            var keyword = query.ToLower();
            var hasil = await _konteks.Produk
                .Include(p => p.Kategori)
                .Where(p => p.NamaProduk.ToLower().Contains(keyword) || 
                            (p.Kategori != null && p.Kategori.NamaKategori.ToLower().Contains(keyword)))
                .Take(5)
                .Select(p => new {
                    id = p.Id,
                    nama = p.NamaProduk,
                    kategori = p.Kategori != null ? p.Kategori.NamaKategori : "Mahakarya",
                    harga = p.Harga,
                    gambar = p.JalurGambar
                })
                .ToListAsync();
            return Json(hasil);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var produk = await _konteks.Produk
                .Include(p => p.Kategori)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produk == null)
            {
                return NotFound();
            }

            _layananRiwayatLihat.TambahRiwayatDilihat(id);
            var riwayatId = _layananRiwayatLihat.DapatkanRiwayatDilihat().ToList();

            var produkRiwayat = await _konteks.Produk
                .Where(p => riwayatId.Contains(p.Id))
                .ToListAsync();
                
            produkRiwayat = produkRiwayat.OrderBy(p => riwayatId.IndexOf(p.Id)).ToList();

            ViewBag.ProdukRiwayat = produkRiwayat;

            return View(produk);
        }
    }
}
