using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRR1_19_Visning
{
    // Saktar ner fienden och därmed hjälper spelaren
    class SlowDown : Power
    {
        static public int Timeactive = RanPwr.Next(5, 10); // Tiden som effekten är aktiv

        static public int Timetoclaim = RanPwr.Next(-6, -3); // Tiden efter att Pillret spawnar som det kan hämtas

        static public int Powerpos = RanPwr.Next(0, 500); 

        static public float SpawnTime = RanPwr.Next(10, 60); // Bestämmer när poweruppen spawnar vilket är inom ett slumpmässigt intrevall som förnyas varje spel

        static public int EffectStrenght = RanPwr.Next(2, 6); // Ta bort ett standardvärde från enemyspeeden men multiplicera eller dividera först värdet med detta nummer
    }
}
