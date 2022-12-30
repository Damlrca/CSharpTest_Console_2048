using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Console2048
{
    internal class Program
    {
        private static int n = 4; // n * n  -  размер игрового поля
        private static Random rnd = new Random();
        private static int score = 0;

        static void Main(string[] args)
        {
            if (args.Length == 1) // первый аргумент - размер поля
            {
                int temp;
                if (int.TryParse(args[0], out temp))
                {
                    if (temp < 3 || temp > 10)
                    {
                        Console.WriteLine("Первый аргумент должен быть целым числом от 3 до 10");
                        return;
                    }
                    else
                    {
                        n = temp;
                    }
                }
                else
                {
                    Console.WriteLine("Первый аргумент должен быть целым числом от 3 до 10");
                    return;
                }
                
            }
            else if (args.Length > 1)
            {
                Console.WriteLine("Слишком много аргументов");
                return;
            }

            var defaultWidth = Console.WindowWidth;
            var defaultHeight = Console.WindowHeight;

            Console.WindowWidth = 3 * 2 + 9 * n;
            Console.WindowHeight = 3 + 1 + 3 * n;
            Console.Title = "2048";
            Console.CursorVisible = false;

            var defaultBackgroundColor = Console.BackgroundColor;
            var defaultForegroundColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.SetCursorPosition(3, 1);
            Console.WriteLine("Текущий счёт:");

            int[,] m = new int[n, n];
            Restart(m);

            //test
            /*for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = Convert.ToInt32(Math.Pow(2, j + (i * n)));
            m[0, 0] = 0;*/

            while (true)
            {
                Print(m, 3, 3);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(20 + 9 * (n - 3), 1);
                Console.Write($"{score,10}");

                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                ConsoleKeyInfo c = Console.ReadKey();

                switch(c.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        Transpose(ref m);
                        Do_move(ref m);
                        Transpose(ref m);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        Transpose(ref m);
                        Reflect_vertically(ref m);
                        Do_move(ref m);
                        Reflect_vertically(ref m);
                        Transpose(ref m);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        Reflect_vertically(ref m);
                        Do_move(ref m);
                        Reflect_vertically(ref m);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        Do_move(ref m);
                        break;
                    case ConsoleKey.R:
                        Restart(m);
                        break;
                    default:
                        break;
                }

                if (c.Key == ConsoleKey.Escape)
                    break;
            }

            Console.BackgroundColor = defaultBackgroundColor;
            Console.ForegroundColor = defaultForegroundColor;
            Console.Clear();
            Console.CursorVisible = true;
            Console.WindowWidth = defaultWidth;
            Console.WindowHeight = defaultHeight;
        }

        private static void Restart(int[,] m)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = 0;
            New_random_number(ref m);
            New_random_number(ref m);
            score = 0;
        }

        private static void Print(int[,] m, int left, int top)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    SetColor(m[i, j]);
                    Console.SetCursorPosition(left + j * 9, top + i * 3);
                    Console.Write("         ");
                    Console.SetCursorPosition(left + j * 9, top + i * 3 + 1);
                    if (m[i, j] < 100)
                        Console.Write($"{m[i, j],5}    ");
                    else
                        Console.Write($"{m[i, j],6}   ");
                    Console.SetCursorPosition(left + j * 9, top + i * 3 + 2);
                    Console.Write("         ");
                }
        }

        private static void SetColor(int x)
        {
            switch (x)
            {
                case 0:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 8:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 16:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 32:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 64:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 128:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 256:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 512:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 1024:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 2048:
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        private static int deltaScore;

        private static void Do_move(ref int[,] m)
        {
            deltaScore = 0;
            if (Can_move_left(m))
            {
                Move_left(ref m);
                Merge_left(ref m);
                Move_left(ref m);
                New_random_number(ref m);
                score += deltaScore / 2;
            }
        }

        private static bool Can_move_left(int[,] m)
        {
            int[,] t = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    t[i, j] = m[i, j];
            
            Move_left(ref t);
            Merge_left(ref t);
            //Move_left(ref t); // если Move_left и Merge_left ничего не сделали, то ещё один Move_left ничего не сделает

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (t[i, j] != m[i, j])
                        return true;

            return false;
        }

        private static void New_random_number(ref int[,] m)
        {
            int EmptyTylesCount = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (m[i, j] == 0)
                        EmptyTylesCount++;
            
            int temp = rnd.Next(1, EmptyTylesCount + 1);

            int CurrentEmptyTyle = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (m[i, j] == 0)
                        CurrentEmptyTyle++;

                    if (temp == CurrentEmptyTyle)
                    {
                        if (rnd.Next(0, 10) == 0) // 10% шанс выпадения 4, 90% шанс выпадения 2
                            m[i, j] = 4;
                        else
                            m[i, j] = 2;
                        return;
                    }
                }
        }

        private static void Move_left(ref int[,] m)
        {
            int[,] t = new int[n, n];
            int[] id = new int[n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (m[i, j] != 0)
                        t[i, id[i]++] = m[i, j];
            m = t;
        }

        private static void Merge_left(ref int[,] m)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j + 1 < n; j++)
                    if (m[i, j] != 0 && m[i, j] == m[i, j + 1])
                    {
                        m[i, j] *= 2;
                        m[i, j + 1] = 0;
                        deltaScore += m[i, j];
                    }
        }

        private static void Reflect_vertically(ref int[,] m)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n / 2; j++)
                    (m[i, j], m[i, n - 1 - j]) = (m[i, n - 1 - j], m[i, j]);
        }

        private static void Transpose(ref int[,] m)
        {
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    (m[i, j], m[j, i]) = (m[j, i], m[i, j]);
        }
    }
}
