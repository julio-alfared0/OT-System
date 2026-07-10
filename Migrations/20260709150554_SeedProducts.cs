using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OT.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f2e30737-1234-4321-8765-a1b2c3d4e5f6"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Koleksi anggur berkualitas tinggi dengan fermentasi sempurna.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Minuman spirit murni yang disuling untuk mencapai kehalusan paripurna.");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Varian anggur hasil fermentasi pilihan dengan kualitas standar internasional.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Minuman distilasi dengan kandungan alkohol tinggi untuk kebutuhan mixology dan konsumsi premium.");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ABV_Percentage", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { new Guid("f2e30737-1234-4321-8765-a1b2c3d4e5f6"), 14.7f, 1, "Minuman anggur merah klasik produksi Orang Tua, diracik dengan rempah pilihan untuk memberikan kehangatan dan rasa yang otentik.", "/images/products/anggur-merah-ot.jpg", "Anggur Merah OT", 75000.00m, 150 });
        }
    }
}
