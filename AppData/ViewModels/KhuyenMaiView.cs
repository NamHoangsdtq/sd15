using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public  class KhuyenMaiView
    {
        [Key]
        public Guid ID { get; set; }
        [Required(ErrorMessage = "mời bạn nhập mã")]
        [StringLength(40, ErrorMessage = "Mã không được quá 40 kí tự")]
        public string Ten { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "mời bạn nhập giá trị lớn hơn 0")]
        public int GiaTri { get; set; }
      
        public DateTime NgayApDung { get; set; }
      
        public DateTime NgayKetThuc { get; set; }
        
        public string? MoTa { get; set; }
      
        public int TrangThai { get; set; }
    }
}
