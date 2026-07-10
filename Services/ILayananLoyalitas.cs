using OT.Models;

namespace OT.Services
{
    public interface ILayananLoyalitas
    {
        int KalkulasiPoinBelanja(decimal totalBelanja);
        void PerbaruiTingkatMember(Pengguna pengguna);
    }
}
