using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OT.Data.Migrations
{
    /// <inheritdoc />
    public partial class InisialisasiAwal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.CreateTable(
                name: "Kategori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NamaKategori = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Deskripsi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pengguna",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NamaPengguna = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NamaLengkap = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    KataSandiHash = table.Column<string>(type: "text", nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Peran = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PoinLoyalitas = table.Column<int>(type: "integer", nullable: false),
                    TingkatMember = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DibuatPada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengguna", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produk",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KategoriId = table.Column<int>(type: "integer", nullable: false),
                    NamaProduk = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    KodeSKU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Harga = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PersentaseAlkohol = table.Column<float>(type: "real", nullable: false),
                    VolumeML = table.Column<int>(type: "integer", nullable: false),
                    Deskripsi = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    JalurGambar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Stok = table.Column<int>(type: "integer", nullable: false),
                    ImbalanPoin = table.Column<int>(type: "integer", nullable: false),
                    DibuatPada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produk_Kategori_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pesanan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uuid", nullable: false),
                    TanggalPesanan = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalHarga = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PotonganHarga = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PoinDiperoleh = table.Column<int>(type: "integer", nullable: false),
                    MetodePembayaran = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusPembayaran = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NomorSeriTransaksi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pesanan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pesanan_Pengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalTable: "Pengguna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemKeranjang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdukId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kuantitas = table.Column<int>(type: "integer", nullable: false),
                    HargaSatuan = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemKeranjang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemKeranjang_Pengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalTable: "Pengguna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemKeranjang_Produk_ProdukId",
                        column: x => x.ProdukId,
                        principalTable: "Produk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemPesanan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PesananId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdukId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kuantitas = table.Column<int>(type: "integer", nullable: false),
                    HargaSatuan = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPesanan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPesanan_Pesanan_PesananId",
                        column: x => x.PesananId,
                        principalTable: "Pesanan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPesanan_Produk_ProdukId",
                        column: x => x.ProdukId,
                        principalTable: "Produk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kategori",
                columns: new[] { "Id", "Deskripsi", "NamaKategori" },
                values: new object[,]
                {
                    { 1, "Minuman beralkohol hasil proses distilasi.", "Spirits" },
                    { 2, "Anggur dan minuman fermentasi warisan Nusantara.", "Wine & Minuman Tradisional" }
                });

            migrationBuilder.InsertData(
                table: "Pengguna",
                columns: new[] { "Id", "DibuatPada", "Email", "KataSandiHash", "NamaLengkap", "NamaPengguna", "Peran", "PoinLoyalitas", "TanggalLahir", "TingkatMember" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "julioal1@otgroup.me", "AQAAAAIAAYagAAAAEFIX4XgXhajVoJrYcweFqMqmxkIgN/7DKGTumSLD4tYLD1OyyYxTkszP6KBx42YZjA==", "Julio Alfredo", "JulioAlfredo1", "Admin", 9999, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Platinum" });

            migrationBuilder.InsertData(
                table: "Produk",
                columns: new[] { "Id", "Deskripsi", "DibuatPada", "Harga", "ImbalanPoin", "JalurGambar", "KategoriId", "KodeSKU", "NamaProduk", "PersentaseAlkohol", "Stok", "VolumeML" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), "Anggur merah legendaris warisan leluhur.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 75000.00m, 10, "/OT-Product/webp/amer.webp", 2, "WNE-AMER-001", "OT Anggur Merah", 14.7f, 500, 620 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Vodka murni dengan tiga kali proses distilasi untuk kelembutan ekstra.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 250000.00m, 50, "/OT-Product/webp/iceland.webp", 1, "SPR-ICE-001", "Iceland Triple Distilled Vodka", 40f, 300, 700 },
                    { new Guid("22222222-2222-2222-2222-222222222223"), "Whisky blend premium dengan karakter khas Batavia yang kuat namun elegan.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 320000.00m, 60, "/OT-Product/webp/batavia.webp", 1, "SPR-BTV-001", "Batavia Blended Whisky", 43f, 200, 700 },
                    { new Guid("22222222-2222-2222-2222-222222222224"), "Tequila Añejo ultra-premium yang dimatangkan sempurna di tong Cabernet tua.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1500000.00m, 300, "/OT-Product/webp/codingo.webp", 1, "SPR-COD-001", "Código 1530 Añejo Tequila", 40f, 50, 750 },
                    { new Guid("22222222-2222-2222-2222-222222222225"), "Tequila klasik dunia yang diracik dari tanaman blue agave pilihan terbaik.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 450000.00m, 90, "/OT-Product/webp/blueagave.webp", 1, "SPR-JCB-001", "Jose Cuervo Especial Blue Agave", 38f, 150, 750 },
                    { new Guid("22222222-2222-2222-2222-222222222226"), "Minuman anggur modern dengan paduan rasa blackcurrant yang intens dan berani.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 85000.00m, 15, "/OT-Product/webp/api.webp", 2, "WNE-API-001", "API Anggur Blackcurrant", 19.7f, 400, 620 },
                    { new Guid("22222222-2222-2222-2222-222222222227"), "Rum rempah keemasan dengan sensasi karamel manis yang hangat di setiap tegukan.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 210000.00m, 40, "/OT-Product/webp/mantaa.webp", 1, "SPR-MNT-001", "Manta Original Spiced Gold", 35f, 250, 700 },
                    { new Guid("22222222-2222-2222-2222-222222222228"), "Whisky emas hasil racikan ahli dengan kelembutan yang mudah dinikmati kapan saja.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 180000.00m, 35, "/OT-Product/webp/mazemaze.webp", 1, "SPR-MZM-001", "Maze Maze Gold Whisky", 40f, 350, 700 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemKeranjang_PenggunaId",
                table: "ItemKeranjang",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemKeranjang_ProdukId",
                table: "ItemKeranjang",
                column: "ProdukId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPesanan_PesananId",
                table: "ItemPesanan",
                column: "PesananId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPesanan_ProdukId",
                table: "ItemPesanan",
                column: "ProdukId");

            migrationBuilder.CreateIndex(
                name: "IX_Pesanan_PenggunaId",
                table: "Pesanan",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produk_KategoriId",
                table: "Produk",
                column: "KategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemKeranjang");

            migrationBuilder.DropTable(
                name: "ItemPesanan");

            migrationBuilder.DropTable(
                name: "Pesanan");

            migrationBuilder.DropTable(
                name: "Produk");

            migrationBuilder.DropTable(
                name: "Pengguna");

            migrationBuilder.DropTable(
                name: "Kategori");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    ABV_Percentage = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Koleksi anggur berkualitas tinggi dengan fermentasi sempurna.", "Wine" },
                    { 2, "Minuman spirit murni yang disuling untuk mencapai kehalusan paripurna.", "Spirits" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ABV_Percentage", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 14.7f, 1, "Anggur Merah dengan sentuhan rempah eksotis, memberikan profil rasa klasik yang menghangatkan.", "/OT-Product/webp/amer.webp", "Anggur Merah", 75000.00m, 200 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 40f, 2, "Vodka murni dengan distilasi 3 kali. Terinspirasi dari kejernihan es glasial, memberikan kehalusan ekstra pada setiap tegukan.", "/OT-Product/webp/iceland.webp", "Iceland Vodka", 120000.00m, 150 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 43f, 2, "Whisky bernuansa lokal dengan kedalaman rasa ek yang dipanggang (toasted oak) dan aroma karamel yang kaya.", "/OT-Product/webp/batavia.webp", "Batavia Whisky", 150000.00m, 100 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 38f, 2, "Tequila premium yang difermentasi dari agave biru pilihan. Memberikan keseimbangan sempurna antara rasa manis dan pedas lada.", "/OT-Product/webp/codingo.webp", "Codigo Tequila", 850000.00m, 50 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 40f, 2, "Pilihan klasik bagi penikmat tequila sejati. Menawarkan jejak aroma citrus dan agave panggang yang intens.", "/OT-Product/webp/blueagave.webp", "Jose Cuervo", 650000.00m, 80 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 12f, 1, "Minuman anggur dengan dominasi rasa blackcurrant segar. Cocok dinikmati dingin dalam suasana santai.", "/OT-Product/webp/api.webp", "API Blackcurrant", 90000.00m, 300 },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 35f, 2, "Rum aromatik yang dipadukan dengan rempah tropis. Memiliki karakter rasa manis, hangat, dan kompleks.", "/OT-Product/webp/mantaa.webp", "Manta Spiced", 110000.00m, 120 },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 40f, 2, "Varian eksklusif dengan sentuhan warna keemasan dan profil rasa yang kaya akan nuansa vanila dan madu.", "/OT-Product/webp/mazemaze.webp", "Maze Maze Gold", 135000.00m, 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }
    }
}
