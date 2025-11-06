using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamUpdateRequest
    {
        public Guid ID { get; set; }
        [Required]
        public string Ten { get; set; }
        public string MoTa { get; set; }
        [Required]
        public string TenChatLieu { get; set; }
        [Required]
        public string TenLoaiSPCha { get; set; }
        public string TenLoaiSPCon { get; set; }
    }
}
