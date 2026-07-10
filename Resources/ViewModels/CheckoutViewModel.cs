using System.Collections.Generic;
using OT.Models;

namespace OT.ViewModels
{
    public class CheckoutViewModel
    {
        public List<ItemKeranjang> DaftarItem { get; set; } = new List<ItemKeranjang>();
        public decimal TotalHarga { get; set; }
        public int SaldoPoinLoyalitas { get; set; }
    }
}
