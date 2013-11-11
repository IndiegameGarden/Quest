using System;

namespace TTengineTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TTUnitTest game = new TTUnitTest())
            {
                game.Run();
            }
        }
    }
#endif
}

