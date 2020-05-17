using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRR1_19_Visning
{
    interface IPowerUp
    {
        int timeactive { get; set; }
        int timetoclaim { get; set; }
    }
}
