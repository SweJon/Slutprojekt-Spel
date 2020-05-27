using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRR1_19_Visning
{
    class SpeedUp : Power // Gör fienderna snabbare och därmed förstör för spelaren
    {
        public int Timeactive = RanPwr.Next(5, 10);

        public int Timetoclaim = RanPwr.Next(2, 4);

        Vector2 Powerpos = new Vector2(0, RanPwr.Next(0, 500));

        double EffectStrenght = RanPwr.Next(1, 2); // Ta bort ett standardvärde från enemyspeeden men multiplicera eller dividera först värdet med detta nummer
    }
}

// Då speedup är något dåligt ska den flytta sig mot spelaren och ibland är den oundviklig så den spawnar alltså på en random position väntar en sekund och sedan rör sig mot spelarn.
//  Detta kan dessutom användas med andra powerdowns.