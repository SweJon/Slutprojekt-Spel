using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRR1_19_Visning
{
    public class Timer // Mäter tid överlevd
    {
        public void timer()
        {
            StreamWriter sw = new StreamWriter("timer.txt");



            sw.WriteLine("Time survived this round: ", Game1.score); // Lägg till tid variabeln efter :

            if(Game1.score > Game1.hiscore)
            {
                Game1.hiscore = Game1.score;
            }
            sw.WriteLine("Longest time survived: ", Game1.hiscore); // Lägg till tid en if sats här alltså if score > hiscore     
            //sw.Close();

            Console.WriteLine(Game1.score);


            StreamReader sr = new StreamReader("timer.txt");
            String tid = sr.ReadToEnd();

            Console.WriteLine(tid);
            Console.ReadLine();
            sr.Close();
        }
    }
}
