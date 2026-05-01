using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALU_DAL.Models
{
    public class FuelEip
    {
        //[PrimaryKey]
        public int fuel { get; set; }
        public int? eipAmt { get; set; }
    }
}
