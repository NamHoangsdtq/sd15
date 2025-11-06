using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamRequest
    {
        [Required(ErrorMessage = "Thiếu tên sản phẩm")]
        public string Ten { get; set; }
        public string? MoTa { get; set; }
        [Required(ErrorMessage = "Thiếu chất liệu sản phẩm")]
        public string TenChatLieu { get; set; }
        [Required(ErrorMessage = "Thiếu màu sắc")]
        public List<MauSac> MauSacs { get; set; }
        [Required(ErrorMessage = "Thiếu kích cỡ sản phẩm")]
        public List<string> KichCos {  get; set; }
        public string TenLoaiSPCha { get; set; }
        [Required(ErrorMessage = "Thiếu loại sản phẩm con (nhánh)")]
        public string TenLoaiSPCon { get; set; }
    }
}
