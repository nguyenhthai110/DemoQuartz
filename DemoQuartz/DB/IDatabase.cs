using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoQuartz.DB
{
    public interface IDatabase
    {
        public List<string> LayDanhSachXn(List<string> DanhSachDaCo, int year, int xnCodeMin, int xnCodeMax, string systemWeb);
    }
}
