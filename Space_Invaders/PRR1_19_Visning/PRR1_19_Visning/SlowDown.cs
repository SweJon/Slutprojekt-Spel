using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRR1_19_Visning
{
    class SlowDown : Power // Saktar ner fienden och därmed hjälper spelaren
    {
        public int Timeactive = RanPwr.Next(5, 10);

        public int Timetoclaim = RanPwr.Next(2, 4);

        Vector2 Powerpos = new Vector2(0, RanPwr.Next(0, 500));

        double EffectStrenght = RanPwr.Next(1, 3); // Ta bort ett standardvärde från enemyspeeden men multiplicera eller dividera först värdet med detta nummer
    }
}
