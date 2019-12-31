using System;
using System.ComponentModel;
using System.Linq;

namespace rubix_solver
{
    public class RubixCube
    {
        public Block[,,] Cube { get; set; }

        public RubixCube()
        {
            // First index is layer from front to back (0 = Front layer, 1 = Middle Layer, 2 = Back Layer)
            // Second index is row in layer from top to bottom (0 = Top Row, 1 = Middle Row, 2 = Bottom Row)
            // Third index is column in Layer from left to right (0 = Left Columm, 1 = Middle Column, 3 = Right Column)

            // Makes assumption the cube and its faces scan always be referred to in a static orientation:
            // white RubiRiubi nfront, yellow back, red left, orange right, green top and blue bottom
            Cube = new[,,]
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
                        new Block(null, Colour.Orange, Colour.Green, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, Colour.Yellow),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Blue, null, Colour.Yellow),
                        new Block(null, null, null, Colour.Blue, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, Colour.Blue, null, Colour.Yellow)
                    }
                }
            };
        }

        public Colour? GetColour(Block block, Layer side)
        {
            return side switch
            {
                Layer.Left => block.Left, 
                Layer.Right => block.Right, 
                Layer.Front => block.Front,
                Layer.Back => block.Back, 
                Layer.Top => block.Top, 
                Layer.Bottom => block.Bottom, 
                _ => null
            };
        }

        public Block[,] GetFace(Layer side)
        {
            var faceToReturn = new Block[3, 3];

            switch (side)
            {
                case Layer.Front:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var col = 0; col < 3; col++)
                        {
                            faceToReturn[row, col] = Cube[0, row, col];
                        }
                    }

                    return faceToReturn;

                case Layer.Left:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            var col = Math.Abs(layer - 2);
                            faceToReturn[row, col] = Cube[layer, row, 0];
                        }
                    }

                    return faceToReturn;
                case Layer.Right:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            var col = Math.Abs(layer - 2);
                            faceToReturn[row, col] = Cube[layer, row, 2];
                        }
                    }

                    return faceToReturn;
                case Layer.Top:
                    for (var col = 0; col < 3; col++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            var row = Math.Abs(layer - 2);
                            faceToReturn[row, col] = Cube[layer, 0, col];
                        }
                    }

                    return faceToReturn;
                case Layer.Bottom:
                    for (var col = 0; col < 3; col++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            var row = Math.Abs(layer - 2);
                            faceToReturn[row, col] = Cube[layer, 2, col];
                        }
                    }

                    return faceToReturn;
                case Layer.Back:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var col = 0; col < 3; col++)
                        {
                            faceToReturn[row, col] = Cube[2, row, col];
                        }
                    }

                    return faceToReturn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private void SetFace(Block[,] face, Layer layer)
        {
            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    if (layer == Layer.Front)
                    {
                        Cube[0, row, col] = face[row, col];
                    }
                    
                    if (layer == Layer.Back)
                    {
                        Cube[2, row, col] = face[row, col];
                    }

                    if (layer == Layer.Top)
                    {
                        Cube[Math.Abs(row - 2), 0, col] = face[row, col];
                    }
                    
                    if (layer == Layer.Bottom)
                    {
                        Cube[Math.Abs(row - 2), 2, col] = face[row, col];
                    }

                    if (layer == Layer.Left)
                    {
                        Cube[Math.Abs(col - 2), row, 0] = face[row, col];
                    }
                    
                    if (layer == Layer.Right)
                    {
                        Cube[Math.Abs(col - 2), row, 2] = face[row, col];
                    }
                }
            }
        }
        
        public void RotateClockwise(Layer layer)
        {
            var face = GetFace(layer);
            var newFace = new Block[3, 3];

            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    switch (layer)
                    {
                        case Layer.Front:
                        case Layer.Back:
                            newFace[col, Math.Abs(row - 2)] = new Block(
                                face[row, col].Bottom,
                                face[row, col].Top,
                                face[row, col].Left,
                                face[row, col].Right,
                                face[row, col].Front,
                                face[row, col].Back);
                            break;
                        case Layer.Top:
                        case Layer.Bottom:
                            newFace[col, Math.Abs(row - 2)] = new Block(
                                face[row, col].Front,
                                face[row, col].Back,
                                face[row, col].Top,
                                face[row, col].Bottom,
                                face[row, col].Right,
                                face[row, col].Left);
                            break;
                        case Layer.Left:
                        case Layer.Right:
                            newFace[col, Math.Abs(row - 2)] = new Block(
                                face[row, col].Left,
                                face[row, col].Right,
                                face[row, col].Back,
                                face[row, col].Front,
                                face[row, col].Top,
                                face[row, col].Bottom);
                            break;
                    }
                }
            }

            SetFace(newFace, layer);
        }

        public void RotateAntiClockwise(Layer layer)
        {
            var face = GetFace(layer);
            var newFace = new Block[3, 3];

            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    switch (layer)
                    {
                        case Layer.Front:
                        case Layer.Back:
                            newFace[Math.Abs(col - 2), row] = new Block(
                                face[row, col].Top,
                                face[row, col].Bottom,
                                face[row, col].Right,
                                face[row, col].Left,
                                face[row, col].Front,
                                face[row, col].Back);
                            break;
                        case Layer.Top:
                        case Layer.Bottom:
                            newFace[Math.Abs(col - 2), row] = new Block(
                                face[row, col].Back,
                                face[row, col].Front,
                                face[row, col].Top,
                                face[row, col].Bottom,
                                face[row, col].Left,
                                face[row, col].Right);
                            break;
                        case Layer.Left:
                        case Layer.Right:
                            newFace[Math.Abs(col - 2), row] = new Block(
                                face[row, col].Left,
                                face[row, col].Right,
                                face[row, col].Front,
                                face[row, col].Back,
                                face[row, col].Bottom,
                                face[row, col].Top);
                            break;
                    }
                }
            }

            SetFace(newFace, layer);
        }

        public void Randomise()
        {
            var random = new Random();
            foreach (var _ in Enumerable.Range(0, 100))
            {
                var layer = (Layer)random.Next(0, 5);
                var direction = random.Next(0, 1);

                if (direction == 0)
                {
                    RotateClockwise(layer);
                }
                else
                {
                    RotateAntiClockwise(layer);
                }
            }
        }
        
        public void PrintCube()
        {
            PrintTopFace();
            PrintFaces();
            PrintBottomFace();
        }

        private void PrintTopFace()
        {
            var face = GetFace(Layer.Top);

            for (var row = 0; row < 3; row++)
            {
                Console.Write("                  "); // hack to indent top and bottom faces
                PrintColour(GetColour(face[row, 0], Layer.Top));
                Console.Write("   ");
                PrintColour(GetColour(face[row, 1], Layer.Top));
                Console.Write("   ");
                PrintColour(GetColour(face[row, 2], Layer.Top));
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private void PrintBottomFace()
        {
            var face = GetFace(Layer.Bottom);

            for (var row = 2; row >= 0; row--)
            {
                Console.Write("                  "); // hack to indent top and bottom faces
                PrintColour(GetColour(face[row, 0], Layer.Bottom));
                Console.Write("   ");
                PrintColour(GetColour(face[row, 1], Layer.Bottom));
                Console.Write("   ");
                PrintColour(GetColour(face[row, 2], Layer.Bottom));
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        
        private void PrintFaces()
        {
            for (var row = 0; row < 3; row++)
            {
                var left = GetFace(Layer.Left);
                PrintColour(GetColour(left[row, 0], Layer.Left));
                Console.Write("   ");
                PrintColour(GetColour(left[row, 1], Layer.Left));
                Console.Write("   ");
                PrintColour(GetColour(left[row, 2], Layer.Left));
                Console.Write("   ");
                
                var front = GetFace(Layer.Front);
                PrintColour(GetColour(front[row, 0], Layer.Front));
                Console.Write("   ");
                PrintColour(GetColour(front[row, 1], Layer.Front));
                Console.Write("   ");
                PrintColour(GetColour(front[row, 2], Layer.Front));
                Console.Write("   ");

                var right = GetFace(Layer.Right);
                PrintColour(GetColour(right[row, 2], Layer.Right));
                Console.Write("   ");
                PrintColour(GetColour(right[row, 1], Layer.Right));
                Console.Write("   ");
                PrintColour(GetColour(right[row, 0], Layer.Right));
                Console.Write("   ");
                
                var back = GetFace(Layer.Back);
                PrintColour(GetColour(back[row, 2], Layer.Back));
                Console.Write("   ");
                PrintColour(GetColour(back[row, 1], Layer.Back));
                Console.Write("   ");
                PrintColour(GetColour(back[row, 0], Layer.Back));

                Console.WriteLine();
                Console.WriteLine();
            }
        }
        
        private void PrintColour(Colour? colour)
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