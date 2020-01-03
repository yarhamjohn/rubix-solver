using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace rubix_solver
{
    class Program
    { 
        static void Main(string[] args)
        {
            var cube = new RubixCube();
            cube.PrintCube();
            cube.RotateAntiClockwise(Layer.Back);
            cube.PrintCube();
            //
            // Console.WriteLine("Start: ");
            // var cube = new RubixCube(new[,,]
            // {
            //     {
            //         {
            //             new Block(Colour.Red, null, Colour.Green, null, Colour.White, null),
            //             new Block(null, null, Colour.Green, null, Colour.White, null),
            //             new Block(null, Colour.Orange, Colour.Green, null, Colour.White, null)
            //         },
            //         {
            //             new Block(Colour.Red, null, null, null, Colour.White, null),
            //             new Block(null, null, null, null, Colour.White, null),
            //             new Block(null, Colour.Orange, null, null, Colour.White, null)
            //         },
            //         {
            //             new Block(Colour.Red, null, null, Colour.Blue, Colour.White, null),
            //             new Block(null, null, null, Colour.Blue, Colour.White, null),
            //             new Block(null, Colour.Orange, null, Colour.Blue, Colour.White, null)
            //         }
            //     },
            //     {
            //         {
            //             new Block(Colour.Red, null, Colour.Green, null, null, null),
            //             new Block(null, null, Colour.Green, null, null, null),
            //             new Block(null, Colour.Orange, Colour.Green, null, null, null)
            //         },
            //         {
            //             new Block(Colour.Red, null, null, null, null, null),
            //             new Block(null, null, null, null, null, null),
            //             new Block(null, Colour.Orange, null, null, null, null)
            //         },
            //         {
            //             new Block(Colour.Red, null, null, Colour.Blue, null, null),
            //             new Block(null, null, null, Colour.Blue, null, null),
            //             new Block(null, Colour.Orange, null, Colour.Blue, null, null)
            //         }
            //     },
            //     {
            //         {
            //             new Block(Colour.Blue, null, Colour.Yellow, null, null, Colour.Orange),
            //             new Block(null, null, Colour.Green, null, null, Colour.Yellow),
            //             new Block(null, Colour.Green, Colour.Yellow, null, null, Colour.Orange)
            //         },
            //         {
            //             new Block(Colour.Red, null, null, null, null, Colour.Yellow),
            //             new Block(null, null, null, null, null, Colour.Yellow),
            //             new Block(null, Colour.Orange, null, null, null, Colour.Yellow)
            //         },
            //         {
            //             new Block(Colour.Yellow, null, null, Colour.Green, null, Colour.Red),
            //             new Block(null, null, null, Colour.Blue, null, Colour.Yellow),
            //             new Block(null, Colour.Yellow, null, Colour.Blue, null, Colour.Red)
            //         }
            //     }
            // });
            
            // cube.PrintCube();

            // SolveThirdLayer(cube);
        }

        private static void SolveThirdLayer(RubixCube cube)
        {
            // Form a cross on the back face
            // Correct the location of the middle edge pieces
            
            // Get the corner pieces in the right corners
            ReorganiseCorners(cube);
            
            // Correct the orientation of the corner pieces
            CorrectCornerOrientation(cube);
        }

        private static void ReorganiseCorners(RubixCube cube)
        {
            // Can be 0, 1 or 4 correct blocks
            var cornerBlocks = GetCornerBlocks(cube);
            var incorrectCornerBlocks = cornerBlocks.Where(CornerBlockInWrongPosition).ToList();
            
            if (incorrectCornerBlocks.Count == 1 || incorrectCornerBlocks.Count == 2)
            {
                throw new Exception("Something has gone wrong. There can only be 0, 3 or 4 incorrect blocks at this stage.");
            }

            if (incorrectCornerBlocks.Count == 0)
            {
                return;
            }
            
            while (GetCornerBlocks(cube).Where(CornerBlockInWrongPosition).ToList().Count > 0)
            {
                if (incorrectCornerBlocks.Count == 3)
                {
                    var ((x, y), _) = cornerBlocks.Except(incorrectCornerBlocks).First();
                    switch (x, y)
                    {
                        case (0, 0):
                        {
                            PerformUrulRotations(cube, Layer.Top, Layer.Bottom);
                            break;
                        }
                        case (0, 2):
                        {
                            PerformUrulRotations(cube, Layer.Right, Layer.Left);
                            break;
                        }
                        case (2, 0):
                        {
                            PerformUrulRotations(cube, Layer.Left, Layer.Right);
                            break;
                        }
                        case (2, 2):
                        {
                            PerformUrulRotations(cube, Layer.Bottom, Layer.Top);
                            break;
                        }
                        default:
                            throw new Exception($"Cannot perform urul rotation as target is not a corner block: ({x}, {y})");
                    }
                }
                else
                {
                    PerformUrulRotations(cube, Layer.Top, Layer.Bottom);
                }
            }
        }

        private static Dictionary<(int, int), Block> GetCornerBlocks(RubixCube cube)
        {
            var face = cube.GetFace(Layer.Back);
            return new Dictionary<(int, int), Block>
            {
                {(0, 0), face[0, 0]},
                {(0, 2), face[0, 2]},
                {(2, 0), face[2, 0]},
                {(2, 2), face[2, 2]}
            };
        }

        private static void PerformUrulRotations(RubixCube cube, Layer rightSide, Layer leftSide)
        {
            Console.WriteLine("Begin Urul Urul rotation");
            
            cube.RotateAntiClockwise(Layer.Back);
            cube.PrintCube();
            cube.RotateAntiClockwise(rightSide);
            cube.PrintCube();
            cube.RotateClockwise(Layer.Back);
            cube.PrintCube();
            cube.RotateAntiClockwise(leftSide);
            cube.PrintCube();

            cube.RotateAntiClockwise(Layer.Back);
            cube.PrintCube();
            cube.RotateClockwise(rightSide);
            cube.PrintCube();
            cube.RotateClockwise(Layer.Back);
            cube.PrintCube();
            cube.RotateClockwise(leftSide);
            cube.PrintCube();
        }

        private static bool CornerBlockInWrongPosition(KeyValuePair<(int, int), Block> cornerBlock)
        {
            var ((x, y), block) = cornerBlock;
            switch (x, y)
            {
                case (0, 0):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Left, block.Top};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Red) || !blockFaces.Contains(Colour.Green);
                }
                case (0, 2):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Right, block.Top};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Orange) || !blockFaces.Contains(Colour.Green);
                }
                case (2, 0):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Left, block.Bottom};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Red) || !blockFaces.Contains(Colour.Blue);
                }
                case (2, 2):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Right, block.Bottom};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Orange) || !blockFaces.Contains(Colour.Blue);
                }
                default:
                    throw new Exception($"Cannot determine if corner block is an incorrect position as it is not a corner block: ({x}, {y})");
            }
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
                    Console.WriteLine($"Reorientate for RDRD rotation");
                    cube.RotateClockwise(Layer.Back);
                }
                else
                {
                    Console.WriteLine($"Begin RDRD rotation to solve {x}, {y}");
                    var sideToRotate = (x, y) switch
                    {
                        (0, 0) => Layer.Top, 
                        (0, 2) => Layer.Right, 
                        (2, 0) => Layer.Left, 
                        (2, 2) => Layer.Bottom, 
                        _ => throw new Exception($"Can't correct corner orientation in the third layer as it is not a corner piece: ({x}, {y})")
                    };
                    
                    // Does this change? I.e. enticlockwise for top/bottom, clockwise for left right? I think so! Need some tests - can't rely on manual testing. Think my clockwise/eanticloswise movements are wrong?
                    cube.RotateAntiClockwise(sideToRotate);
                    cube.PrintCube();

                    cube.RotateAntiClockwise(Layer.Front);
                    cube.PrintCube();
                    cube.RotateClockwise(sideToRotate);
                    cube.PrintCube();
                    cube.RotateClockwise(Layer.Front);
                    cube.PrintCube();
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