using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PRR1_19_Visning
{
    class Power
    {

        int Timeactive { get; set; }
        int Timetoclaim { get; set; }
        Vector2 Powerpos { get; set; }
        double EffectStrenght { get; set; }

        public static Random RanPwr = new Random(); // Exakt som randomi game skapar detta ett random nummer
    }
}
