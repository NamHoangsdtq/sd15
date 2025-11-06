using AppAPI.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuyDoiDiemController : ControllerBase
    {
        private readonly IQuyDoiDiemServices _quydoidiem;
        public QuyDoiDiemController(IQuyDoiDiemServices quyDoiDiemServices)
        {
            this._quydoidiem = quyDoiDiemServices;
        }
        // GET: api/<QuyDoiDiemController>
        [HttpGet]
        public List<QuyDoiDiem> Get()
        {
            return _quydoidiem.GetAll();
        }

        // GET api/<QuyDoiDiemController>/5
        [HttpGet("{id}")]
        public QuyDoiDiem Get(Guid id)
        {
            return _quydoidiem.GetById(id);
        }

        // POST api/<QuyDoiDiemController>
        [HttpPost]
        public bool Post( int TiLeTichDiem, int TiLeTieuDiem, int TrangThai)
        {
            return _quydoidiem.Add(TiLeTichDiem, TiLeTieuDiem, TrangThai);
        }

        // PUT api/<QuyDoiDiemController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id,int TrangThai)
        {
            var quydoidiem = _quydoidiem.GetById(id);
            if(quydoidiem != null)
            {
               return _quydoidiem.Update(quydoidiem.ID,  TrangThai);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<QuyDoiDiemController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var quydoidiem = _quydoidiem.GetById(id);
            if (quydoidiem != null)
            {
                return _quydoidiem.Delete(quydoidiem.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
