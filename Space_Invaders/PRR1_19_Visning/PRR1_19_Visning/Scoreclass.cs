using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PRR1_19_Visning
{
    public class Scoreclass // Mäter tid överlevd
    {
        public void eyyo()
        {

            StreamWriter sw = new StreamWriter("Score.txt");

            sw.WriteLine("Time survived this round: ", Game1.score); // Lägg till tid variabeln efter :

            if (Game1.score > Game1.hiscore)
            {
                Game1.hiscore = Game1.score;
            }
            sw.WriteLine("Longest time survived: ", Game1.hiscore);
            sw.Close();

            Console.WriteLine(Game1.score);


            StreamReader sr = new StreamReader("timer.txt");
            String tid = sr.ReadToEnd();

            //Console.ReadLine();
            sr.Close();
        }
    }
}
