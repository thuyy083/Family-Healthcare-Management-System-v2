using HTQL_DichVuYTeGiaDinh.Models;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class HoSoYTeGroupViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public List<HoSoYTe> HoSoYTes { get; set; }
    }
}
