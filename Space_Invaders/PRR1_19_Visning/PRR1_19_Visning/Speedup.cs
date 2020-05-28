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
        static public int Timeactive = RanPwr.Next(2, 4);   // Då alla värderna är static är de samma värde hela rundan. Dock Pillsen inte spawnar så ofta spelar detta inte så mycket roll. 

        static public int Timetoclaim = RanPwr.Next(-5, -2);

        static public int Powerpos = RanPwr.Next(0, 500); // Static gör att den spawnar fast på samma position hela rundan. Frågan är om det spelar någon roll eller om de bara ska spawna en gång per runda.

        static public float SpawnTime = RanPwr.Next(1, 6); // Bestämmer när powerdownen spawnar

        static public int EffectStrenght = RanPwr.Next(4, 10); // Ta bort ett standardvärde från enemyspeeden men multiplicera eller dividera först värdet med detta nummer
    }
}

// Då speedup är något dåligt ska den flytta sig mot spelaren och ibland är den oundviklig så den spawnar alltså på en random position väntar en sekund och sedan rör sig mot spelarn.
//  Detta kan dessutom användas med andra powerdowns.