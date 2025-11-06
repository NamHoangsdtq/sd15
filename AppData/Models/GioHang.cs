namespace AppData.Models
{
    public class GioHang
    {
        //add-migration InitialMigration
        public Guid IDKhachHang { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
        public virtual IEnumerable<ChiTietGioHang> ChiTietGioHangs { get; set; }
    }
}
