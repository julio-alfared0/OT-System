using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OT.Services;
using OT.ViewModels;

namespace OT.Controllers
{
    public class OtentikasiController : Controller
    {
        private readonly ILayananOtentikasi _layananOtentikasi;

        public OtentikasiController(ILayananOtentikasi layananOtentikasi)
        {
            _layananOtentikasi = layananOtentikasi;
        }

        [HttpGet]
        public async Task<IActionResult> Masuk()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Masuk(MasukViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["PesanError"] = "Mohon isi Email/Username dan Kata Sandi dengan format yang benar.";
                return View(model);
            }

            var pengguna = await _layananOtentikasi.ValidasiMasukAsync(model.EmailAtauUsername, model.KataSandi);

            if (pengguna == null)
            {
                ModelState.AddModelError(string.Empty, "Email/Username atau Kata Sandi salah.");
                TempData["PesanError"] = "Email/Username atau Kata Sandi salah.";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, pengguna.Id.ToString()),
                new Claim(ClaimTypes.Name, pengguna.NamaPengguna),
                new Claim(ClaimTypes.Email, pengguna.Email),
                new Claim(ClaimTypes.Role, pengguna.Peran)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.IngatSaya
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);

            if (pengguna.Peran == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            
            return RedirectToAction("Index", "Beranda");
        }

        [HttpGet]
        public async Task<IActionResult> Daftar()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Daftar(DaftarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["PesanError"] = "Mohon periksa kembali data pendaftaran Anda.";
                return View(model);
            }

            try
            {
                var penggunaBaru = await _layananOtentikasi.RegistrasiPelangganAsync(
                    model.NamaPengguna,
                    model.NamaLengkap,
                    model.Email,
                    model.KataSandi,
                    model.TanggalLahir);

                if (penggunaBaru != null)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    Response.Cookies.Delete("OT-AntiforgeryToken");
                    TempData["PesanSukses"] = "Registrasi berhasil! Silakan masuk dengan akun baru Anda.";
                    return RedirectToAction("Masuk");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TempData["PesanError"] = ex.Message;
            }

            return View(model);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Keluar()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("OT-AntiforgeryToken");
            return RedirectToAction("Index", "Beranda");
        }

        [HttpGet]
        public IActionResult LupaKataSandi()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LupaKataSandi(LupaKataSandiViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var otp = await _layananOtentikasi.BuatOtpResetKataSandi(model.Email);

            if (otp != null)
            {
                TempData["AlertOtp"] = $"[Simulasi Sistem OTP] Kode OTP Anda adalah: {otp}";
                TempData["EmailReset"] = model.Email;
                return RedirectToAction("VerifikasiOtp");
            }

            ModelState.AddModelError(string.Empty, "Email tidak terdaftar dalam sistem.");
            return View(model);
        }

        [HttpGet]
        public IActionResult VerifikasiOtp()
        {
            var email = TempData["EmailReset"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("LupaKataSandi");
            }

            var model = new VerifikasiOtpViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("otp")]
        public IActionResult VerifikasiOtp(VerifikasiOtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["EmailResetOTPVerified"] = model.Email;
            TempData["OTPVerified"] = model.KodeOtp;

            return RedirectToAction("ResetKataSandi");
        }

        [HttpGet]
        public IActionResult ResetKataSandi()
        {
            var email = TempData["EmailResetOTPVerified"]?.ToString();
            var otp = TempData["OTPVerified"]?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                return RedirectToAction("LupaKataSandi");
            }

            var model = new ResetKataSandiViewModel 
            { 
                Email = email,
                KodeOtp = otp
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetKataSandi(ResetKataSandiViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool sukses = await _layananOtentikasi.VerifikasiOtpDanResetKataSandiAsync(model.Email, model.KodeOtp, model.KataSandiBaru);

            if (sukses)
            {
                TempData["PesanSukses"] = "Kata Sandi berhasil diperbarui. Silakan masuk menggunakan kata sandi baru Anda.";
                return RedirectToAction("Masuk");
            }

            ModelState.AddModelError(string.Empty, "OTP tidak valid atau telah kadaluwarsa.");
            return View(model);
        }

        [HttpGet]
        public IActionResult AksesDitolak(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
