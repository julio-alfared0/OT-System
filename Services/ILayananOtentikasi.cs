using System;
using System.Threading.Tasks;
using OT.Models;

namespace OT.Services
{
    public interface ILayananOtentikasi
    {
        Task<Pengguna?> RegistrasiPelangganAsync(string namaPengguna, string namaLengkap, string email, string kataSandi, DateTime tanggalLahir);
        Task<Pengguna?> ValidasiMasukAsync(string emailAtauUsername, string kataSandi);
        Task<string?> BuatOtpResetKataSandi(string email);
        Task<bool> VerifikasiOtpDanResetKataSandiAsync(string email, string otp, string kataSandiBaru);
    }
}
