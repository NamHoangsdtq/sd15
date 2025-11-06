using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class QuyDoiDiem
    {
       
        public Guid ID { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
       
        public int TiLeTichDiem { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        
        public int TiLeTieuDiem { get; set; }
        [Required(ErrorMessage = "mời bạn chọn trạng thái")]
        public int TrangThai { get; set; }//0 là ko sử dụng,1 là chỉ tích hoặc tiêu, 2 là vừa tích vừa tiêu.
       
        public virtual IEnumerable<LichSuTichDiem> LichSuTichDiems { get; set; }
    }
}
