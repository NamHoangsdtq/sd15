using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class BienTheTruyVan
    {
        public Guid idSp { get; set; }
        public List<Guid> lstIdGTri { get; set; } 
    }
}
