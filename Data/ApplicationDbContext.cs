using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OT.Models;

namespace OT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pengguna> Pengguna { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Produk> Produk { get; set; }
        public DbSet<ItemKeranjang> ItemKeranjang { get; set; }
        public DbSet<Pesanan> Pesanan { get; set; }
        public DbSet<ItemPesanan> ItemPesanan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produk>()
                .Property(p => p.Harga)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ItemKeranjang>()
                .Property(i => i.HargaSatuan)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pesanan>()
                .Property(p => p.TotalHarga)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pesanan>()
                .Property(p => p.PotonganHarga)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ItemPesanan>()
                .Property(i => i.HargaSatuan)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pengguna>()
                .HasMany(p => p.DaftarPesanan)
                .WithOne(p => p.Pengguna)
                .HasForeignKey(p => p.PenggunaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pengguna>()
                .HasMany(p => p.DaftarItemKeranjang)
                .WithOne(i => i.Pengguna)
                .HasForeignKey(i => i.PenggunaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Kategori>()
                .HasMany(k => k.DaftarProduk)
                .WithOne(p => p.Kategori)
                .HasForeignKey(p => p.KategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pesanan>()
                .HasMany(p => p.DetailPesanan)
                .WithOne(i => i.Pesanan)
                .HasForeignKey(i => i.PesananId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Kategori>().HasData(
                new Kategori { Id = 1, NamaKategori = "Spirits", Deskripsi = "Minuman beralkohol hasil proses distilasi." },
                new Kategori { Id = 2, NamaKategori = "Wine & Minuman Tradisional", Deskripsi = "Anggur dan minuman fermentasi warisan Nusantara." }
            );

            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var hasher = new PasswordHasher<Pengguna>();
            var adminUser = new Pengguna
            {
                Id = adminId,
                NamaPengguna = "JulioAlfredo1",
                NamaLengkap = "Julio Alfredo",
                Email = "julioal1@otgroup.me",
                TanggalLahir = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Peran = "Admin",
                PoinLoyalitas = 9999,
                TingkatMember = "Platinum",
                DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            
            adminUser.KataSandiHash = hasher.HashPassword(adminUser, "superuser01");

            modelBuilder.Entity<Pengguna>().HasData(adminUser);

            modelBuilder.Entity<Produk>().HasData(
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                    KategoriId = 2,
                    NamaProduk = "OT Anggur Merah",
                    KodeSKU = "WNE-AMER-001",
                    Harga = 75000.00m,
                    PersentaseAlkohol = 14.7f,
                    VolumeML = 620,
                    Deskripsi = "Anggur merah legendaris warisan leluhur.",
                    JalurGambar = "/assets/webp/amer.webp",
                    Stok = 500,
                    ImbalanPoin = 10,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    KategoriId = 1,
                    NamaProduk = "Iceland Triple Distilled Vodka",
                    KodeSKU = "SPR-ICE-001",
                    Harga = 250000.00m,
                    PersentaseAlkohol = 40.0f,
                    VolumeML = 700,
                    Deskripsi = "Vodka murni dengan tiga kali proses distilasi untuk kelembutan ekstra.",
                    JalurGambar = "/assets/webp/iceland.webp",
                    Stok = 300,
                    ImbalanPoin = 50,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                    KategoriId = 1,
                    NamaProduk = "Batavia Blended Whisky",
                    KodeSKU = "SPR-BTV-001",
                    Harga = 320000.00m,
                    PersentaseAlkohol = 43.0f,
                    VolumeML = 700,
                    Deskripsi = "Whisky blend premium dengan karakter khas Batavia yang kuat namun elegan.",
                    JalurGambar = "/assets/webp/batavia.webp",
                    Stok = 200,
                    ImbalanPoin = 60,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222224"),
                    KategoriId = 1,
                    NamaProduk = "Código 1530 Añejo Tequila",
                    KodeSKU = "SPR-COD-001",
                    Harga = 1500000.00m,
                    PersentaseAlkohol = 40.0f,
                    VolumeML = 750,
                    Deskripsi = "Tequila Añejo ultra-premium yang dimatangkan sempurna di tong Cabernet tua.",
                    JalurGambar = "/assets/webp/codingo.webp",
                    Stok = 50,
                    ImbalanPoin = 300,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222225"),
                    KategoriId = 1,
                    NamaProduk = "Jose Cuervo Especial Blue Agave",
                    KodeSKU = "SPR-JCB-001",
                    Harga = 450000.00m,
                    PersentaseAlkohol = 38.0f,
                    VolumeML = 750,
                    Deskripsi = "Tequila klasik dunia yang diracik dari tanaman blue agave pilihan terbaik.",
                    JalurGambar = "/assets/webp/blueagave.webp",
                    Stok = 150,
                    ImbalanPoin = 90,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222226"),
                    KategoriId = 2,
                    NamaProduk = "API Anggur Blackcurrant",
                    KodeSKU = "WNE-API-001",
                    Harga = 85000.00m,
                    PersentaseAlkohol = 19.7f,
                    VolumeML = 620,
                    Deskripsi = "Minuman anggur modern dengan paduan rasa blackcurrant yang intens dan berani.",
                    JalurGambar = "/assets/webp/api.webp",
                    Stok = 400,
                    ImbalanPoin = 15,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222227"),
                    KategoriId = 1,
                    NamaProduk = "Manta Original Spiced Gold",
                    KodeSKU = "SPR-MNT-001",
                    Harga = 210000.00m,
                    PersentaseAlkohol = 35.0f,
                    VolumeML = 700,
                    Deskripsi = "Rum rempah keemasan dengan sensasi karamel manis yang hangat di setiap tegukan.",
                    JalurGambar = "/assets/webp/mantaa.webp",
                    Stok = 250,
                    ImbalanPoin = 40,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Produk
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222228"),
                    KategoriId = 1,
                    NamaProduk = "Maze Maze Gold Whisky",
                    KodeSKU = "SPR-MZM-001",
                    Harga = 180000.00m,
                    PersentaseAlkohol = 40.0f,
                    VolumeML = 700,
                    Deskripsi = "Whisky emas hasil racikan ahli dengan kelembutan yang mudah dinikmati kapan saja.",
                    JalurGambar = "/assets/webp/mazemaze.webp",
                    Stok = 350,
                    ImbalanPoin = 35,
                    DibuatPada = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
