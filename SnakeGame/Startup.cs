using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    public class Startup
    {
        struct Position 
        {
            public int row;
            public int col;
            public Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        static void Main()
        {
            Position[] directions = new Position[]
            {
                new Position(0, 1), // Right
                new Position(0, -1), // Left
                new Position(1, 0), // Down
                new Position(-1, 0) // Top
            };

            int startFoodTimer = 0;
            int foodDuration = 8000;

            double frames = 100;
            int direction = 0;
            Console.BufferHeight = Console.WindowHeight;
            Random foodPosition = new Random();

            List<Position> obstacles = new List<Position>();

            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
                snakeElements.Enqueue(new Position(0, i));

            Position food;
            do
            {
                food = new Position(foodPosition.Next(0, Console.WindowHeight),
                   foodPosition.Next(0, Console.WindowWidth));
            }
            while (snakeElements.Contains(food) || obstacles.Contains(food));

            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("@");

            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("*");
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.RightArrow)
                    { if (direction != 1) direction = 0; }
                    else if (userInput.Key == ConsoleKey.LeftArrow)
                    { if (direction != 0) direction = 1; }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    { if (direction != 3) direction = 2; }
                    else if (userInput.Key == ConsoleKey.UpArrow)
                    { if (direction != 2) direction = 3; }
                }

                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];
                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                    snakeHead.col + nextDirection.col);

                if (snakeNewHead.col < 0)
                    snakeNewHead.col = Console.WindowWidth - 1;
                if (snakeNewHead.row < 0)
                    snakeNewHead.row = Console.WindowHeight - 1;
                if (snakeNewHead.col >= Console.WindowWidth)
                    snakeNewHead.col = 0;
                if (snakeNewHead.row >= Console.WindowHeight)
                    snakeNewHead.row = 0;

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Your points are: {0}", (snakeElements.Count - 6) * 100);
                    Console.WriteLine("GameOver!");
                    return;
                }

                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("*");

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if (direction == 0) Console.Write(">");
                if (direction == 1) Console.Write("<");
                if (direction == 2) Console.Write("v");
                if (direction == 3) Console.Write("^");

                if (snakeNewHead.Equals(food))
                {
                    do
                    {
                        food = new Position(foodPosition.Next(0, Console.WindowHeight),
                                            foodPosition.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food));
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("@");
                    frames -= 2;

                    Position obstacle = new Position();
                    do
                    {
                        obstacle = new Position(foodPosition.Next(0, Console.WindowHeight),
                                            foodPosition.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(obstacle) ||
                        obstacles.Contains(obstacle) ||
                        (food.row != obstacle.row && food.col != obstacle.col));
                    obstacles.Add(obstacle);
                    Console.SetCursorPosition(obstacle.col, obstacle.row);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("=");
                }
                else
                {
                    Position lastSnakeElement = snakeElements.Dequeue();
                    Console.SetCursorPosition(lastSnakeElement.col, lastSnakeElement.row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - startFoodTimer >= foodDuration) 
                {
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(foodPosition.Next(0, Console.WindowHeight),
                                            foodPosition.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) && obstacles.Contains(food));
                    startFoodTimer = Environment.TickCount;
                }

                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("@");

                frames -= 0.01;
                Thread.Sleep((int)frames);
            }
        }
    }
}
