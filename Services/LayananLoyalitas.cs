using OT.Models;

namespace OT.Services
{
    public class LayananLoyalitas : ILayananLoyalitas
    {
        public int KalkulasiPoinBelanja(decimal totalBelanja)
        {
            return (int)(totalBelanja / 10000m);
        }

        public void PerbaruiTingkatMember(Pengguna pengguna)
        {
            if (pengguna.PoinLoyalitas >= 5000)
            {
                pengguna.TingkatMember = "Platinum";
            }
            else if (pengguna.PoinLoyalitas >= 1500)
            {
                pengguna.TingkatMember = "Gold";
            }
            else if (pengguna.PoinLoyalitas >= 500)
            {
                pengguna.TingkatMember = "Silver";
            }
            else
            {
                pengguna.TingkatMember = "Bronze";
            }
        }
    }
}
