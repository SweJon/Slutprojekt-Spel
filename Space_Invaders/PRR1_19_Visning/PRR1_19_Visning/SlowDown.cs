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
        static public int Timeactive = RanPwr.Next(5, 10);

        static public int Timetoclaim = RanPwr.Next(-6, -3);

        static public int Powerpos = RanPwr.Next(0, 500);

        static public float SpawnTime = RanPwr.Next(1, 6); // Bestämmer när poweruppen spawnar

        static public int EffectStrenght = RanPwr.Next(2, 6); // Ta bort ett standardvärde från enemyspeeden men multiplicera eller dividera först värdet med detta nummer
    }
}
