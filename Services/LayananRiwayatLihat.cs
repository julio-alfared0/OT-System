using System;
using System.Collections.Generic;

namespace OT.Services
{
    public class LayananRiwayatLihat : ILayananRiwayatLihat
    {
        private class SimpulGanda
        {
            public Guid ProdukId { get; set; }
            public SimpulGanda? Sebelumnya { get; set; }
            public SimpulGanda? Selanjutnya { get; set; }

            public SimpulGanda(Guid produkId)
            {
                ProdukId = produkId;
            }
        }

        private SimpulGanda? _kepala;
        private SimpulGanda? _ekor;
        private int _jumlahSimpul;
        private readonly int _batasMaksimal = 5;
        
        private readonly object _kunci = new object();

        public void TambahRiwayatDilihat(Guid produkId)
        {
            lock (_kunci)
            {
                SimpulGanda? saatIni = _kepala;
                SimpulGanda? ditemukan = null;

                while (saatIni != null)
                {
                    if (saatIni.ProdukId == produkId)
                    {
                        ditemukan = saatIni;
                        break;
                    }
                    saatIni = saatIni.Selanjutnya;
                }

                if (ditemukan != null)
                {
                    if (ditemukan == _kepala)
                        return;

                    if (ditemukan.Sebelumnya != null)
                        ditemukan.Sebelumnya.Selanjutnya = ditemukan.Selanjutnya;
                    
                    if (ditemukan.Selanjutnya != null)
                        ditemukan.Selanjutnya.Sebelumnya = ditemukan.Sebelumnya;

                    if (ditemukan == _ekor)
                        _ekor = ditemukan.Sebelumnya;

                    ditemukan.Sebelumnya = null;
                    ditemukan.Selanjutnya = _kepala;

                    if (_kepala != null)
                        _kepala.Sebelumnya = ditemukan;

                    _kepala = ditemukan;
                }
                else
                {
                    var simpulBaru = new SimpulGanda(produkId);

                    if (_kepala == null)
                    {
                        _kepala = simpulBaru;
                        _ekor = simpulBaru;
                    }
                    else
                    {
                        simpulBaru.Selanjutnya = _kepala;
                        _kepala.Sebelumnya = simpulBaru;
                        _kepala = simpulBaru;
                    }

                    _jumlahSimpul++;

                    if (_jumlahSimpul > _batasMaksimal && _ekor != null)
                    {
                        _ekor = _ekor.Sebelumnya;
                        
                        if (_ekor != null)
                            _ekor.Selanjutnya = null;

                        _jumlahSimpul--;
                    }
                }
            }
        }

        public IEnumerable<Guid> DapatkanRiwayatDilihat()
        {
            var daftarRiwayat = new List<Guid>();
            
            lock (_kunci)
            {
                SimpulGanda? saatIni = _kepala;
                
                while (saatIni != null)
                {
                    daftarRiwayat.Add(saatIni.ProdukId);
                    saatIni = saatIni.Selanjutnya;
                }
            }
            
            return daftarRiwayat;
        }
    }
}
