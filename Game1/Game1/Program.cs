// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
using System;
using Microsoft.Xna.Framework;

namespace Game1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (QuestGame game = new QuestGame())
            //using (Game game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

