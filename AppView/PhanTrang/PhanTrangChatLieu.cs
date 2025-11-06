using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangChatLieu
    {
        public IEnumerable<ChatLieu> listNv { get; set; } = new List<ChatLieu>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
