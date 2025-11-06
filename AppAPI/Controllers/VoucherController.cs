using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private  readonly IVoucherServices _services;
        public VoucherController(IVoucherServices services)
        {
            _services = services;
        }

        // GET: api/<VoucherController>
        [HttpGet]
        public List<Voucher> Get()
        {
            return _services.GetAll();
        }

        // GET api/<VoucherController>/5
        [HttpGet("{id}")]
        public Voucher Get(Guid id)
        {
            return _services.GetById(id);
        }

        // POST api/<VoucherController>
        [HttpPost]
        public bool Post(VoucherView voucher)
        {
            return _services.Add(voucher);
        }

        // PUT api/<VoucherController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, VoucherView voucherview)
        {
            var voucher= _services.GetById(id);
            if(voucher != null)
            {
                return _services.Update(voucher.ID,voucherview);
            }
            else
            {
                return false;   
            }
        }

        // DELETE api/<VoucherController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var voucher = _services.GetById(id);
            if (voucher != null)
            {
                return _services.Delete(voucher.ID);
            }
            else
            {
                return false;
            }
        }
        [HttpGet("GetVoucherByMa")]
        public Voucher? GetVoucherByMa(string ma)
        {
            return _services.GetVoucherByMa(ma);
        }
        [HttpGet("GetAllVoucherByTien")]
        public List<Voucher> GetAllVoucherByTien(int tongTien)
        {
            return _services.GetAllVoucherByTien(tongTien);
        }
    }
}
