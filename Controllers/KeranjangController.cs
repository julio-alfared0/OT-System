using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OT.Data;
using OT.Models;

namespace OT.Controllers
{
    [Authorize]
    public class KeranjangController : Controller
    {
        private readonly ApplicationDbContext _konteks;

        public KeranjangController(ApplicationDbContext konteks)
        {
            _konteks = konteks;
        }

        private Guid DapatkanIdPengguna()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpPost]
        public async Task<IActionResult> Tambah(Guid produkId)
        {
            if (User.IsInRole("Admin") || User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return Json(new { sukses = false, pesan = "Akun Administrator tidak berbelanja." });
            }

            var penggunaId = DapatkanIdPengguna();
            
            var produk = await _konteks.Produk.FindAsync(produkId);
            if (produk == null) return Json(new { sukses = false, pesan = "Produk tidak ditemukan." });

            var itemKeranjang = await _konteks.ItemKeranjang
                .FirstOrDefaultAsync(k => k.PenggunaId == penggunaId && k.ProdukId == produkId);

            if (itemKeranjang != null)
            {
                itemKeranjang.Kuantitas += 1;
            }
            else
            {
                itemKeranjang = new ItemKeranjang
                {
                    Id = Guid.NewGuid(),
                    PenggunaId = penggunaId,
                    ProdukId = produkId,
                    Kuantitas = 1,
                    HargaSatuan = produk.Harga
                };
                _konteks.ItemKeranjang.Add(itemKeranjang);
            }

            await _konteks.SaveChangesAsync();
            return Json(new { sukses = true, pesan = "Produk berhasil ditambahkan ke keranjang." });
        }

        [HttpPost]
        public async Task<IActionResult> Kurang(Guid produkId)
        {
            var penggunaId = DapatkanIdPengguna();

            var itemKeranjang = await _konteks.ItemKeranjang
                .FirstOrDefaultAsync(k => k.PenggunaId == penggunaId && k.ProdukId == produkId);

            if (itemKeranjang != null)
            {
                if (itemKeranjang.Kuantitas > 1)
                {
                    itemKeranjang.Kuantitas -= 1;
                }
                else
                {
                    _konteks.ItemKeranjang.Remove(itemKeranjang);
                }
                
                await _konteks.SaveChangesAsync();
                return Json(new { sukses = true, pesan = "Kuantitas produk berhasil dikurangi." });
            }

            return Json(new { sukses = false, pesan = "Item tidak ditemukan di keranjang." });
        }

        [HttpPost]
        public async Task<IActionResult> Hapus(Guid produkId)
        {
            var penggunaId = DapatkanIdPengguna();

            var itemKeranjang = await _konteks.ItemKeranjang
                .FirstOrDefaultAsync(k => k.PenggunaId == penggunaId && k.ProdukId == produkId);

            if (itemKeranjang != null)
            {
                _konteks.ItemKeranjang.Remove(itemKeranjang);
                await _konteks.SaveChangesAsync();
                return Json(new { sukses = true, pesan = "Produk berhasil dihapus dari keranjang." });
            }

            return Json(new { sukses = false, pesan = "Item tidak ditemukan di keranjang." });
        }

        [HttpGet]
        public async Task<IActionResult> Hitung()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated || User.IsInRole("Admin") || User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return Json(new { total = 0 });
            }

            try
            {
                var penggunaId = DapatkanIdPengguna();
                var total = await _konteks.ItemKeranjang
                    .Where(k => k.PenggunaId == penggunaId)
                    .SumAsync(k => k.Kuantitas);
                return Json(new { total });
            }
            catch
            {
                return Json(new { total = 0 });
            }
        }
    }
}
