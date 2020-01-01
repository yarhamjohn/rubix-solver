using System;
using System.Linq;
using System.Collections.Generic;

namespace rubix_solver
{
    class Program
    { 
        static void Main(string[] args)
        {
            // var cube = new RubixCube();
            // cube.PrintCube();
            // cube.Randomise();
            // cube.PrintCube();

            Console.WriteLine("Start: ");
            var cube = new RubixCube(new[,,]
            {
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, Colour.White, null),
                        new Block(null, null, Colour.Green, null, Colour.White, null),
                        new Block(null, Colour.Orange, Colour.Green, null, Colour.White, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, Colour.White, null),
                        new Block(null, null, null, null, Colour.White, null),
                        new Block(null, Colour.Orange, null, null, Colour.White, null)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Blue, Colour.White, null),
                        new Block(null, null, null, Colour.Blue, Colour.White, null),
                        new Block(null, Colour.Orange, null, Colour.Blue, Colour.White, null)
                    }
                },
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Orange, Colour.Green, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, null),
                        new Block(null, null, null, null, null, null),
                        new Block(null, Colour.Orange, null, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Blue, null, null),
                        new Block(null, null, null, Colour.Blue, null, null),
                        new Block(null, Colour.Orange, null, Colour.Blue, null, null)
                    }
                },
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, null, Colour.Yellow),
                        new Block(null, null, Colour.Green, null, null, Colour.Yellow),
                        new Block(null, Colour.Green, Colour.Yellow, null, null, Colour.Orange)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, Colour.Yellow),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Yellow, null, null, Colour.Red, null, Colour.Blue),
                        new Block(null, null, null, Colour.Blue, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, Colour.Blue, null, Colour.Yellow)
                    }
                }
            });
            
            cube.PrintCube();
            
            
            SolveThirdLayer(cube);
        }

        private static void SolveThirdLayer(RubixCube cube)
        {
            // Form a cross on the back face
            // Correct the location of the middle edge pieces
            // Get the corner pieces in the right corners
            
            // Correct the orientation of the corner pieces
            CorrectCornerOrientation(cube);
        }


        private static void CorrectCornerOrientation(RubixCube cube)
        {
            if (cube.IsSolved())
            {
                return;
            }
            
            var initialFace = cube.GetFace(Layer.Back);
            var cornerBlocks = new Dictionary<(int, int), Block>
            {
                {(0, 0), initialFace[0, 0]}, 
                {(0, 2), initialFace[0, 2]}, 
                {(2, 0), initialFace[2, 0]}, 
                {(2, 2), initialFace[2, 2]}
            };
            
            var incorrectCornerBlocks = cornerBlocks.Where(b => b.Value.Back != Colour.Yellow).ToList();
            if (incorrectCornerBlocks.Count == 1)
            {
                throw new Exception("Something is wrong with the Rubix Cube layout - it should be possible to have only one incorrect corner block.");
            }
            var ((x, y), _) = incorrectCornerBlocks.First();

            while (!cube.IsSolved())
            {
                var currentFace = cube.GetFace(Layer.Back);
                if (currentFace[x, y].Back == Colour.Yellow)
                {
                    cube.RotateClockwise(Layer.Back);
                }
                else
                {
                    var sideToRotate = (x, y) switch
                    {
                        (0, 0) => Layer.Top, 
                        (0, 2) => Layer.Right, 
                        (2, 0) => Layer.Left, 
                        (2, 2) => Layer.Bottom, 
                        _ => throw new Exception($"Can't correct corner orientation in the third layer as it is not a corner piece: ({x}, {y})")
                    };
                    
                    cube.RotateClockwise(sideToRotate);
                    cube.RotateAntiClockwise(Layer.Front);
                    cube.RotateAntiClockwise(sideToRotate);
                    cube.RotateClockwise(Layer.Front);
                }

                cube.PrintCube();
            }
        }
    }

    public enum Layer
    {
        Left,
        Right,
        Top,
        Bottom,
        Front,
        Back
    }

    public class Block
    {
        public Colour? Left { get; set; }
        public Colour? Right { get; set; }
        public Colour? Top { get; set; }
        public Colour? Bottom { get; set; }
        public Colour? Front { get; set; }
        public Colour? Back { get; set; }

        public Block(Colour? left, Colour? right, Colour? top, Colour? bottom, Colour? front, Colour? back)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Front = front;
            Back = back;
        }
    }

    public enum Colour
    {
        White,
        Green,
        Blue,
        Red,
        Orange,
        Yellow
    }
}