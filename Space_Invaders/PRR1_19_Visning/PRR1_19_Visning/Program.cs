using System;
using System.IO;

namespace PRR1_19_Visning
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();


            // Skriver in score i ett dokument (inlagd här då det inte funkade när det var i en seperat klass)
            StreamReader sr = new StreamReader(@"Score.txt");
            // int hiscore = int.Parse(sr.ReadLine()); // Funkar inte att skriva såhär av ngn anledning
            int hiscore = 0;
            sr.Close();

            StreamWriter sw = new StreamWriter(@"Score.txt");        
            sw.WriteLine("Score this round: " + Game1.score); // Lägg till tid variabeln efter :

            if (Game1.score > hiscore)
            {
                hiscore = Game1.score;
            }

            sw.WriteLine("Highest score achieved: " + hiscore);
            sw.Close();
        }
    }
#endif
}
