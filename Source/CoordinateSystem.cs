using System;

namespace ConsoleEngine
{
    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point (int x, int y) : this()
        {
            this.X = x;
            this.Y = y;
        }
    }

    class ConsoleGrid
    {

        public void Instantiate (int X, int Y, char obj)
        {
            string xString = obj.ToString();

            Console.SetCursorPosition((X - 1) * 2, Y - 1);

            Console.WriteLine(xString);
        }

        public void Instantiate(int X, int Y, int obj)
        {
            string xString = obj.ToString();

            Console.SetCursorPosition((X - 1) * 2, Y - 1);

            Console.WriteLine(xString);
        }

        public void Instantiate(int X, int Y, string obj)
        {
            string xString = obj;

            Console.SetCursorPosition((X - 1) * 2, Y - 1);

            Console.WriteLine(xString);
        }
    }
}
