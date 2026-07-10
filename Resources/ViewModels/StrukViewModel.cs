using OT.Models;

namespace OT.ViewModels
{
    public class StrukViewModel
    {
        public Pesanan? Pesanan { get; set; }
        public string KutipanLegendarisOT { get; set; } = string.Empty;
        public string StringBarcodeQrisSimulasi { get; set; } = string.Empty;
    }
}
