namespace AppData.Models
{
    public class HoaDon
    {
        public Guid ID { get; set; }
        public string MaHD { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayNhanHang { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public int TienShip { get; set; }
        public int? TongTien { get; set; }
        public int LoaiHD { get; set; } // Off-1 // On-0
        public string? PhuongThucThanhToan { get; set; }
        public string? GhiChu { get; set; }
        public int TrangThaiGiaoHang { get; set; }
        //Các trạng thái của đơn hàng
        /*
         * 1-đơn nháp
         * 2-Chờ xác nhận
         * 3-Đang giao hàng//ko đc hủy
         * 11-Giao không thành công
         * 6-thành công //nhận hàng thành công// đc đánh giá // đc hủy nếu ngày thanh toán < 3
         
         --Hoàn hàng
         * 9-chờ xác nhận hoàn hàng
         * 4-đang hoàn hàng
         * 5-Hoàn hàng thành công

         --Hủy khi chưa giao
         * 7-Đơn hủy
         * 8-Chờ xác nhận hủy
         */
        public Guid? IDNhanVien { get; set; }
        public Guid? IDVoucher { get; set; }
        public virtual IEnumerable<LichSuTichDiem>? LichSuTichDiems { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual IEnumerable<ChiTietHoaDon>? ChiTietHoaDons { get; set; }
    }
}
