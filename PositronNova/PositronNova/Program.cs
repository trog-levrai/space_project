using System;

namespace PositronNova
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PositronNova game = new PositronNova())
            {
                game.Run();
            }
        }
    }
#endif
}

