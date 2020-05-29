using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRR1_19_Visning
{
    // Gör fienderna snabbare och därmed förstör för spelaren
    class SpeedUp : Power
    {
        static public int Timeactive = RanPwr.Next(3, 5);   // Då alla värderna är static är de samma värde hela rundan. Dock spawnar bara pills 1 gång per runda så det är irrelevant. 

        static public int Timetoclaim = RanPwr.Next(-5, -2); // Tiden efter att Pillret spawnar som det kan åka mot dig

        static public int Powerpos = RanPwr.Next(0, 500); // Plats det BadPill "spawnar" på

        static public float SpawnTime = RanPwr.Next(10, 60); // Bestämmer när powerdownen spawnar vilket är inom ett slumpmässigt intrevall som förnyas varje spel

        static public int EffectStrenght = RanPwr.Next(4, 8); 
    }
}
