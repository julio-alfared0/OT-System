using System;
using System.Collections.Generic;

namespace OT.Services
{
    public interface ILayananRiwayatLihat
    {
        void TambahRiwayatDilihat(Guid produkId);
        IEnumerable<Guid> DapatkanRiwayatDilihat();
    }
}
