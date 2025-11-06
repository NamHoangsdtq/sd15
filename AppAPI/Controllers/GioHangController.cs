    using AppAPI.IServices;
    using AppAPI.Services;
    using AppData.Models;
    using AppData.ViewModels.SanPham;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    // For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

    namespace AppAPI.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class GioHangController : ControllerBase
        {
            private readonly IGioHangServices gioHangServices;
            public GioHangController(IGioHangServices gioHangServices)
            {
                this.gioHangServices = gioHangServices;
            }
            // GET: api/<GioHangController>
            [HttpGet]
            public List<GioHang> Get()
            {
                return gioHangServices.GetAll();
            }

            // GET api/<GioHangController>/5
            [HttpGet("{id}")]
            public GioHang Get(Guid id)
            {
                return gioHangServices.GetById(id);
            }


            // POST api/<GioHangController>
            [HttpPost]
            public bool Post(Guid IdKhachHang, DateTime ngaytao)
            {
                return gioHangServices.Add(IdKhachHang, ngaytao);
            }

            // PUT api/<GioHangController>/5
            [HttpPut("{id}")]
            public bool Put(Guid id, DateTime ngaytao)
            {
                var gioHang = gioHangServices.GetById(id);
                if (gioHang != null)
                {
                    return gioHangServices.Update(gioHang.IDKhachHang, ngaytao);
                }
                else
                {
                    return false;
                }
            }

            // DELETE api/<GioHangController>/5
            [HttpDelete("{id}")]
            public bool Delete(Guid id)
            {
                var gioHang = gioHangServices.GetById(id);
                if (gioHang != null)
                {
                    return gioHangServices.Delete(gioHang.IDKhachHang);
                }
                else
                {
                    return false;
                }
            }
            [HttpDelete("Deletebyid")]
            public bool Deletebyidctsp(Guid idctsp, Guid idNguoiDung)
            {
                return gioHangServices.DeleteCartbyIDCTSP(idctsp,idNguoiDung).Result;
            }
            [HttpPut("UpdateCart")]
            public bool UpdateCart(Guid idctsp, int soluong, Guid idNguoiDung)
            {
                return gioHangServices.UpdateCart(idctsp,soluong, idNguoiDung).Result;
            }
            [HttpGet("GetCart")]
            public GioHangViewModel GetCart(string request)
            {
                var lst = JsonConvert.DeserializeObject<List<GioHangRequest>>(request); 
                return gioHangServices.GetCart(lst);
            }
            [HttpGet("GetCartLogin")]
            public GioHangViewModel GetCartLogin(string idNguoiDung)
            {
                return gioHangServices.GetCartLogin(idNguoiDung);
            }
            //backup nếu có lỗi
            [HttpPost("AddCart")]
            public async Task<IActionResult> AddCart(ChiTietGioHang chiTietGioHang)
            {
                var result = await gioHangServices.AddCart(chiTietGioHang);
                return result;
            }
            [HttpDelete("ClearCartLogin")]
            public bool ClearCartLogin(Guid idNguoiDung)
            {
                // Gọi service để xóa tất cả ChiTietGioHang của người dùng
                return gioHangServices.ClearCartLogin(idNguoiDung);
            }
    }
    }
