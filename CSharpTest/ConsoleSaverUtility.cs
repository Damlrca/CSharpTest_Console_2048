using System;

namespace ConsoleSaverUtility
{
    public static class ConsoleSaver
    {
        private static int defaultWidth;
        private static int defaultHeight;
        private static ConsoleColor defaultBackgroundColor;
        private static ConsoleColor defaultForegroundColor;

        public static void Save()
        {
            defaultWidth = Console.WindowWidth;
            defaultHeight = Console.WindowHeight;
            defaultBackgroundColor = Console.BackgroundColor;
            defaultForegroundColor = Console.ForegroundColor;
        }

        public static void Load()
        {
            Console.BackgroundColor = defaultBackgroundColor;
            Console.ForegroundColor = defaultForegroundColor;
            Console.WindowWidth = defaultWidth;
            Console.WindowHeight = defaultHeight;

            Console.CursorVisible = true;
            Console.Clear();
        }

        public static void Terminate()
        {
            Load();
            Environment.Exit(0);
        }
    }
}