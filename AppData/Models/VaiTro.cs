namespace AppData.Models
{
    public class VaiTro
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<NhanVien>? NhanViens { get; set; }
    }
}
