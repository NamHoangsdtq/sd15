using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class ChatLieu
    {
        public Guid ID { get; set; }
        [StringLength(20, ErrorMessage = "Tên chất liệu không được vượt quá 20 kí tự ")]
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<SanPham> SanPhams { get; set; }
    }
}
