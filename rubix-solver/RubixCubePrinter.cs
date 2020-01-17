using System;

namespace rubix_solver
{
    public static class RubixCubePrinter
    {
        public static void PrintCube(RubixCube cube)
        {
            PrintFace(cube, Layer.Top);
            PrintFaces(cube);
            PrintFace(cube, Layer.Bottom);
        }

        private static void PrintFace(RubixCube cube, Layer layer)
        {
            var face = cube.GetFace(layer);

            for (var row = 0; row < 3; row++)
            {
                Console.Write("                  "); // hack to indent top and bottom faces
                PrintColour(cube.GetColour(face[row, 0], layer));
                Console.Write("   ");
                PrintColour(cube.GetColour(face[row, 1], layer));
                Console.Write("   ");
                PrintColour(cube.GetColour(face[row, 2], layer));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void PrintFaces(RubixCube cube)
        {
            for (var row = 0; row < 3; row++)
            {
                var left = cube.GetFace(Layer.Left);
                PrintColour(cube.GetColour(left[row, 0], Layer.Left));
                Console.Write("   ");
                PrintColour(cube.GetColour(left[row, 1], Layer.Left));
                Console.Write("   ");
                PrintColour(cube.GetColour(left[row, 2], Layer.Left));
                Console.Write("   ");

                var front = cube.GetFace(Layer.Front);
                PrintColour(cube.GetColour(front[row, 0], Layer.Front));
                Console.Write("   ");
                PrintColour(cube.GetColour(front[row, 1], Layer.Front));
                Console.Write("   ");
                PrintColour(cube.GetColour(front[row, 2], Layer.Front));
                Console.Write("   ");

                var right = cube.GetFace(Layer.Right);
                PrintColour(cube.GetColour(right[row, 0], Layer.Right));
                Console.Write("   ");
                PrintColour(cube.GetColour(right[row, 1], Layer.Right));
                Console.Write("   ");
                PrintColour(cube.GetColour(right[row, 2], Layer.Right));
                Console.Write("   ");

                var back = cube.GetFace(Layer.Back);
                PrintColour(cube.GetColour(back[row, 0], Layer.Back));
                Console.Write("   ");
                PrintColour(cube.GetColour(back[row, 1], Layer.Back));
                Console.Write("   ");
                PrintColour(cube.GetColour(back[row, 2], Layer.Back));

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
