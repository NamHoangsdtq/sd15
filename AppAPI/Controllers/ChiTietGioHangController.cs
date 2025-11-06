using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietGioHangController : ControllerBase
    {
        private readonly IChiTietGioHangServices chiTietGioHangServices;
        public ChiTietGioHangController(ChiTietGioHangServices chiTietGioHang)
        {
            this.chiTietGioHangServices = chiTietGioHang;
        }
        // GET: api/<ChiTietGioHangController>
        [HttpGet]
        public List<ChiTietGioHang> Get()
        {
            return chiTietGioHangServices.GetAll();
        }

        // GET api/<ChiTietGioHangController>/5
        [HttpGet("{id}")]
        public ChiTietGioHang Get(Guid id)
        {
            return chiTietGioHangServices.GetById(id);
        }

        // POST api/<ChiTietGioHangController>
        [HttpPost]
        public string Post(Guid IdBienThe, Guid IdKhachHang, int soluong)
        {
            return chiTietGioHangServices.Add(IdBienThe, IdKhachHang, soluong);
        }

        // PUT api/<ChiTietGioHangController>/5
        [HttpPut("{id}")]
        public string Put(Guid id, Guid IdBienThe, Guid IdKhachHang, int soluong)
        {
            var chitietgiohang = chiTietGioHangServices.GetById(id);
            if (chitietgiohang != null)
            {
                return chiTietGioHangServices.Update(chitietgiohang.ID, IdBienThe, IdKhachHang, soluong);
            }
            else
            {
                return "";
            }
        }

        // DELETE api/<ChiTietGioHangController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var chitietgiohang = chiTietGioHangServices.GetById(id);
            if (chitietgiohang != null)
            {
                return chiTietGioHangServices.Delete(chitietgiohang.ID);
            }
            else
            {
                return false;
            }

        }
    }
}
