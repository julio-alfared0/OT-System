using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OT.Data;
using OT.Models;

namespace OT.Services
{
    public class LayananDashboardAdmin : ILayananDashboardAdmin
    {
        private readonly ApplicationDbContext _konteks;

        public LayananDashboardAdmin(ApplicationDbContext konteks)
        {
            _konteks = konteks;
        }

        public async Task<StatistikDashboardDto> DapatkanStatistikDashboardAsync()
        {
            var totalPendapatan = await _konteks.Pesanan.SumAsync(p => p.TotalHarga);
            var totalPesanan = await _konteks.Pesanan.CountAsync();
            var totalMemberAktif = await _konteks.Pengguna.CountAsync(u => u.Peran == "Customer");
            var daftarProdukStokKritis = await _konteks.Produk
                .Where(p => p.Stok < 10)
                .ToListAsync();

            var daftarTransaksiTerbaru = await _konteks.Pesanan
                .Include(p => p.Pengguna)
                .OrderByDescending(p => p.TanggalPesanan)
                .Take(10)
                .ToListAsync();

            return new StatistikDashboardDto
            {
                TotalPendapatan = totalPendapatan,
                TotalPesanan = totalPesanan,
                TotalMemberAktif = totalMemberAktif,
                DaftarProdukStokKritis = daftarProdukStokKritis,
                DaftarTransaksiTerbaru = daftarTransaksiTerbaru
            };
        }

        public async Task<string> DapatkanDataGrafikJsonAsync()
        {
            var tanggalMulai = DateTime.UtcNow.AddDays(-7);
            
            var rentangPesanan = await _konteks.Pesanan
                .Where(p => p.TanggalPesanan >= tanggalMulai)
                .ToListAsync();

            var penjualanHarian = rentangPesanan
                .GroupBy(p => p.TanggalPesanan.Date)
                .Select(g => new
                {
                    Tanggal = g.Key.ToString("yyyy-MM-dd"),
                    Total = g.Sum(p => p.TotalHarga)
                })
                .OrderBy(d => d.Tanggal)
                .ToList();

            var distribusiKategori = await _konteks.Kategori
                .Select(k => new
                {
                    NamaKategori = k.NamaKategori,
                    JumlahProduk = k.DaftarProduk.Count
                })
                .ToListAsync();

            var dataFormatGrafik = new
            {
                PenjualanHarian = penjualanHarian,
                DistribusiKategori = distribusiKategori
            };

            return JsonSerializer.Serialize(dataFormatGrafik);
        }

        public async Task<List<Produk>> DapatkanSemuaProdukAsync()
        {
            return await _konteks.Produk
                .Include(p => p.Kategori)
                .OrderBy(p => p.NamaProduk)
                .ToListAsync();
        }

        public async Task<bool> PerbaruiStokProdukAsync(Guid produkId, int stokBaru)
        {
            var produk = await _konteks.Produk.FindAsync(produkId);
            if (produk == null) return false;

            produk.Stok = Math.Max(0, stokBaru);
            await _konteks.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AturStatusStokAsync(Guid produkId, bool isHabis)
        {
            var produk = await _konteks.Produk.FindAsync(produkId);
            if (produk == null) return false;

            produk.Stok = isHabis ? 0 : 50;
            await _konteks.SaveChangesAsync();
            return true;
        }
    }
}
