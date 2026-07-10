using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OT.Data;
using OT.Models;

namespace OT.Services
{
    public class LayananOtentikasi : ILayananOtentikasi
    {
        private readonly ApplicationDbContext _konteks;
        private readonly IMemoryCache _memoriKuki;
        private readonly PasswordHasher<Pengguna> _pembuatHash;

        public LayananOtentikasi(ApplicationDbContext konteks, IMemoryCache memoriKuki)
        {
            _konteks = konteks;
            _memoriKuki = memoriKuki;
            _pembuatHash = new PasswordHasher<Pengguna>();
        }

        public async Task<Pengguna?> RegistrasiPelangganAsync(string namaPengguna, string namaLengkap, string email, string kataSandi, DateTime tanggalLahir)
        {
            int usia = DateTime.Today.Year - tanggalLahir.Year;
            if (tanggalLahir.Date > DateTime.Today.AddYears(-usia)) 
                usia--;

            if (usia < 21)
            {
                throw new InvalidOperationException("Pelanggan harus berusia minimal 21 tahun untuk dapat melakukan registrasi.");
            }

            bool penggunaSudahAda = await _konteks.Pengguna
                .AnyAsync(p => p.Email == email || p.NamaPengguna == namaPengguna);
            
            if (penggunaSudahAda)
            {
                throw new InvalidOperationException("Email atau Nama Pengguna telah digunakan.");
            }

            var pelangganBaru = new Pengguna
            {
                Id = Guid.NewGuid(),
                NamaPengguna = namaPengguna,
                NamaLengkap = namaLengkap,
                Email = email,
                TanggalLahir = DateTime.SpecifyKind(tanggalLahir, DateTimeKind.Utc),
                Peran = "Customer",
                PoinLoyalitas = 0,
                TingkatMember = "Bronze",
                DibuatPada = DateTime.UtcNow
            };

            pelangganBaru.KataSandiHash = _pembuatHash.HashPassword(pelangganBaru, kataSandi);

            _konteks.Pengguna.Add(pelangganBaru);
            await _konteks.SaveChangesAsync();

            return pelangganBaru;
        }

        public async Task<Pengguna?> ValidasiMasukAsync(string emailAtauUsername, string kataSandi)
        {
            var pengguna = await _konteks.Pengguna
                .FirstOrDefaultAsync(p => p.Email == emailAtauUsername || p.NamaPengguna == emailAtauUsername);
            
            if (pengguna == null)
            {
                return null;
            }

            var hasilVerifikasi = _pembuatHash.VerifyHashedPassword(pengguna, pengguna.KataSandiHash, kataSandi);

            if (hasilVerifikasi == PasswordVerificationResult.Success)
            {
                return pengguna;
            }

            return null;
        }

        public async Task<string?> BuatOtpResetKataSandi(string email)
        {
            var pengguna = await _konteks.Pengguna.FirstOrDefaultAsync(p => p.Email == email);
            if (pengguna == null)
            {
                return null;
            }

            Random acak = new Random();
            string otp = acak.Next(100000, 999999).ToString();

            var opsiCache = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            
            _memoriKuki.Set($"OTP_Reset_{email}", otp, opsiCache);

            return otp;
        }

        public async Task<bool> VerifikasiOtpDanResetKataSandiAsync(string email, string otp, string kataSandiBaru)
        {
            if (_memoriKuki.TryGetValue($"OTP_Reset_{email}", out string? otpTersimpan))
            {
                if (otpTersimpan == otp)
                {
                    var pengguna = await _konteks.Pengguna.FirstOrDefaultAsync(p => p.Email == email);
                    if (pengguna != null)
                    {
                        pengguna.KataSandiHash = _pembuatHash.HashPassword(pengguna, kataSandiBaru);
                        await _konteks.SaveChangesAsync();
                        
                        _memoriKuki.Remove($"OTP_Reset_{email}");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
