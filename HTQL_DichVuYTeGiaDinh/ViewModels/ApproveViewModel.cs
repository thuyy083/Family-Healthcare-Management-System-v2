using HTQL_DichVuYTeGiaDinh.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HTQL_DichVuYTeGiaDinh.ViewModels
{
    public class ApproveViewModel
    {
        public SuDungDichVu SuDungDichVu { get; set; } = new SuDungDichVu();
        public int SelectedNhanVienYTe { get; set; }

        public List<SelectListItem> NhanVienYTeList { get; set; } = new List<SelectListItem>();
    }


}
