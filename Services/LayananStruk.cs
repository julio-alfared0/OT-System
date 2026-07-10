using System;

namespace OT.Services
{
    public class LayananStruk : ILayananStruk
    {
        private static readonly string[] _kutipanFilosofis = new string[]
        {
            "Kualitas adalah warisan yang tak lekang oleh waktu.",
            "Setiap tetes adalah dedikasi rasa sejak 1948.",
            "Tradisi dan inovasi, menyatu dalam sebuah mahakarya.",
            "Ketulusan meracik resep leluhur, menciptakan cita rasa paripurna.",
            "Dari alam, dirawat oleh waktu, dipersembahkan untuk Anda.",
            "Kesabaran dalam proses adalah rahasia di balik kesempurnaan rasa.",
            "Bukan sekadar minuman, melainkan pengikat tali persaudaraan.",
            "Menjaga kemurnian warisan Nusantara dari generasi ke generasi.",
            "Tegukan kenangan, merayakan setiap langkah perjalanan hidup.",
            "Kejujuran bahan baku membentuk identitas sejati dari kualitas."
        };

        public string DapatkanKutipanAcak()
        {
            Random acak = new Random();
            int indeksAcak = acak.Next(_kutipanFilosofis.Length);
            return _kutipanFilosofis[indeksAcak];
        }
    }
}
