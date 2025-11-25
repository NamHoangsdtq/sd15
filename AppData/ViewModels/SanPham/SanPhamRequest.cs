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
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        public string? MoTa { get; set; }
        [Required(ErrorMessage = "Chất liệu sản phẩm không được để trống")]
        public string TenChatLieu { get; set; }
        [Required(ErrorMessage = "Chất liệu sản phẩm không được để trống")]

        //[MinLength(1, ErrorMessage = "Vui lòng thêm ít nhất một màu sắc")]
        public List<MauSac> MauSacs { get; set; }
        [Required(ErrorMessage = "Chất liệu sản phẩm không được để trống")]
        //[MinLength(1, ErrorMessage = "Vui lòng thêm ít nhất một kích cỡ")]

        public List<string> KichCos {  get; set; }
        [Required(ErrorMessage = "Tên sản phẩm cha không được để trống")]
        public string TenLoaiSPCha { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm con không được để trống")]
        public string TenLoaiSPCon { get; set; }
    }
}
