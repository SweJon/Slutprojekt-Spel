using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRR1_19_Visning
{
    class Randomnumbers
    {
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(-500, -50);
        }

    }
}
