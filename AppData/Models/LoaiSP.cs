using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class LoaiSP
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public Guid? IDLoaiSPCha { get; set; }
        public virtual IEnumerable<SanPham> SanPhams { get; set; }
        public virtual LoaiSP LoaiSPCha { get; set; }
    }
}
