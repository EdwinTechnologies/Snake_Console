using System;
using System.Collections.Generic;
using ConsoleEngine;
using System.Windows.Input;
using System.Threading;


namespace Snake
{
    class Program
    {
        #region Booleans

        public static bool MoveX = true;
        public static bool Death = false;
        public static bool JustEaten = false;
        public static bool StopGameDraw = false;

        #endregion Booleans

        #region Integers

        public static int MoveDirection = 1;
        public static int Score;

        #endregion Integers

        #region Objects

        public static List<Point> SnakeChain = new List<Point>();

        public static ConsoleGrid ConsoleGrid = new ConsoleGrid();

        public static Point Monkey = new Point(10, 12);

        public static Thread GameLoopSTA;

        #endregion Objects

        #region Chars

        public static char SnakeAliveSegment = '■';
        public static char SnakeDeathSegment = '+';
        public static char MonkeyChar = 'X';

        public static char LastKeyPressed = 'D';

        #endregion Chars
        
        #region MainFunction

        static void Main(string[] args)
        {
            Init();
        }

        #endregion MainFunction


        public static void Init ()
        {
            Console.CursorVisible = false;

            SnakeChain.Add(new Point(5, 10));    //Build first Snake
            SnakeChain.Add(new Point(6, 10));
            SnakeChain.Add(new Point(7, 10));

            DrawGame();                         //Draw first Graphic

            GameLoopSTA = new Thread(GameLoop);               //Start GameLoop in STA-Thread
            GameLoopSTA.SetApartmentState(ApartmentState.STA);
            GameLoopSTA.Start();
            GameLoopSTA.Join();
        }

        
        public static void GameLoop()
        {
            while (true)
            {

                if (MoveX)
                {
                    SnakeChain.Add(new Point(SnakeChain[SnakeChain.Count - 1].X + MoveDirection, SnakeChain[SnakeChain.Count - 1].Y));  //Move on X
                    SnakeChain.RemoveAt(0);

                }
                else
                {
                    SnakeChain.Add(new Point(SnakeChain[SnakeChain.Count - 1].X, SnakeChain[SnakeChain.Count - 1].Y + MoveDirection));  //Move on Y
                    SnakeChain.RemoveAt(0);
                }

                for (int i = 0; i < SnakeChain.Count; i++)                              //Check for Snake It-Self-Collision
                {
                    if (SnakeChain[0].Equals(SnakeChain[i]) && i != 0)
                    {
                        Death = true;
                    }
                }

                if (SnakeChain[SnakeChain.Count - 1].X <= 2 || SnakeChain[SnakeChain.Count - 1].X >= 31 || SnakeChain[SnakeChain.Count - 1].Y <= 1 || SnakeChain[SnakeChain.Count - 1].Y >= 17)     //Check if snake is out of game field
                {
                    Death = true;
                }


                if (SnakeChain[SnakeChain.Count - 1].Equals(Monkey))                        //Snake - Monkey [Food] Collision
                {
                    Random rnd = new Random();
                    Monkey = new Point(rnd.Next(4, 28), rnd.Next(2, 15));                   //Spawn new Monkey

                    for (int i = 0; i < SnakeChain.Count; i++)                              //Spawn-No-Food-On-Snake-Protection
                    {
                        while (Monkey.Equals(SnakeChain[i]))
                        {
                            Monkey = new Point(rnd.Next(4, 28), rnd.Next(2, 15));           //Spawn new Monkey
                        }
                    }


                    SnakeChain.Insert(0, SnakeChain[0]);                        //Add Snake Segment
                    Score += 8;
                    JustEaten = true;
                }
                

                DrawGame();                                                 //Draw new Scene

                System.Threading.Thread.Sleep(75);                         //Sleep for 100ms



                if ((Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up)) && LastKeyPressed != 'S')
                {
                    MoveDirection = -1;
                    MoveX = false;
                    LastKeyPressed = 'W';
                }
                else if ((Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left)) && LastKeyPressed != 'D')
                {
                    MoveDirection = -1;
                    MoveX = true;
                    LastKeyPressed = 'A';
                }
                else if ((Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down)) && LastKeyPressed != 'W')
                {
                    MoveDirection = 1;
                    MoveX = false;
                    LastKeyPressed = 'S';
                }
                else if ((Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right)) && LastKeyPressed != 'A')
                {
                    MoveDirection = 1;
                    MoveX = true;
                    LastKeyPressed = 'D';
                }
                else if (Keyboard.IsKeyDown(Key.F5))            //Start New Game with F5
                {
                    Console.Clear();
                    Score = 0;
                    SnakeChain.Clear();

                    StopGameDraw = false;
                    Death = false;
                    MoveDirection = 1;
                    MoveX = true;
                    LastKeyPressed = 'D';

                    Init();

                    GameLoopSTA.Abort();

                }

            }
        }



        static void DrawGame ()
        {
            if (!StopGameDraw)
            {
                Console.Clear();

                Console.WriteLine(" ╔═══════════════════════════════════════════════════════════╗    ╔════════════╗");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║   Score:   ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ║            ║");
                Console.WriteLine(" ║                                                           ║    ╚════════════╝");
                Console.WriteLine(" ║                                                           ║");
                Console.WriteLine(" ║                                                           ║");
                Console.WriteLine(" ║                                                           ║");
                Console.WriteLine(" ║                                                           ║");
                Console.WriteLine(" ║                                                           ║");
                Console.WriteLine(" ╚═══════════════════════════════════════════════════════════╝");

                ConsoleGrid.Instantiate(36, 7, Score.ToString());

                if (!Death)
                {
                    for (int i = 0; i < SnakeChain.Count; i++)
                    {
                        ConsoleGrid.Instantiate(SnakeChain[i].X, SnakeChain[i].Y, SnakeAliveSegment);
                        ConsoleGrid.Instantiate(Monkey.X, Monkey.Y, 'X');
                    }
                }
                else
                {
                    for (int i = 0; i < SnakeChain.Count; i++)
                    {
                        ConsoleGrid.Instantiate(SnakeChain[i].X, SnakeChain[i].Y, SnakeDeathSegment);
                        StopGameDraw = true;
                    }
                }
            }
        }

    }
}
