using System;

namespace rubix_solver
{
    public static class RubixCubePrinter
    {
        public static void Print(RubixCube cube)
        {
            PrintFace(cube, Side.Top);
            PrintFaces(cube);
            PrintFace(cube, Side.Bottom);
        }

        private static void PrintFace(RubixCube cube, Side side)
        {
            var face = cube.GetFace(side);

            for (var row = 0; row < 3; row++)
            {
                Console.Write("                  "); // hack to indent top and bottom faces
                PrintColour(cube.GetColour(face[row, 0], side));
                Console.Write("   ");
                PrintColour(cube.GetColour(face[row, 1], side));
                Console.Write("   ");
                PrintColour(cube.GetColour(face[row, 2], side));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void PrintFaces(RubixCube cube)
        {
            for (var row = 0; row < 3; row++)
            {
                var left = cube.GetFace(Side.Left);
                PrintColour(cube.GetColour(left[row, 0], Side.Left));
                Console.Write("   ");
                PrintColour(cube.GetColour(left[row, 1], Side.Left));
                Console.Write("   ");
                PrintColour(cube.GetColour(left[row, 2], Side.Left));
                Console.Write("   ");

                var front = cube.GetFace(Side.Front);
                PrintColour(cube.GetColour(front[row, 0], Side.Front));
                Console.Write("   ");
                PrintColour(cube.GetColour(front[row, 1], Side.Front));
                Console.Write("   ");
                PrintColour(cube.GetColour(front[row, 2], Side.Front));
                Console.Write("   ");

                var right = cube.GetFace(Side.Right);
                PrintColour(cube.GetColour(right[row, 0], Side.Right));
                Console.Write("   ");
                PrintColour(cube.GetColour(right[row, 1], Side.Right));
                Console.Write("   ");
                PrintColour(cube.GetColour(right[row, 2], Side.Right));
                Console.Write("   ");

                var back = cube.GetFace(Side.Back);
                PrintColour(cube.GetColour(back[row, 0], Side.Back));
                Console.Write("   ");
                PrintColour(cube.GetColour(back[row, 1], Side.Back));
                Console.Write("   ");
                PrintColour(cube.GetColour(back[row, 2], Side.Back));

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void PrintColour(Colour? colour)
        {
            Console.BackgroundColor = colour switch
            {
                Colour.Blue => ConsoleColor.DarkBlue,
                Colour.Green => ConsoleColor.DarkGreen,
                Colour.Red => ConsoleColor.DarkRed,
                Colour.Orange => ConsoleColor.Red,
                Colour.Yellow => ConsoleColor.DarkYellow,
                Colour.White => ConsoleColor.White,
                _ => Console.BackgroundColor
            };
            Console.Write("   ");
            Console.ResetColor();
        }
    }
}
