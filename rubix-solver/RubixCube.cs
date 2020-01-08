using System;
using System.Linq;
using rubix_solver.Solvers;

namespace rubix_solver
{
    public class RubixCube
    {
        public Block[,,] Cube { get; set; }

        // First index is layer from front to back (0 = Front layer, 1 = Middle Layer, 2 = Back Layer)
        // Second index is row in layer from top to bottom (0 = Top Row, 1 = Middle Row, 2 = Bottom Row)
        // Third index is column in Layer from left to right (0 = Left Columm, 1 = Middle Column, 3 = Right Column)

        // Makes assumption the cube and its faces can always be referred to in a static orientation:
        // white front, yellow back, red left, orange right, green top and blue bottom
        private readonly Block[,,] _solvedCube = {
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

        public RubixCube(Block[,,] cube)
        {
            Cube = cube;
        }

        public RubixCube()
        {
            Cube = _solvedCube.Clone() as Block[,,];
        }

        public bool IsSolved()
        {
            var back = GetFace(Layer.Back);
            foreach (var block in back)
            {
                if (block.Back != Colour.Yellow)
                {
                    return false;
                }
            }
            
            var front = GetFace(Layer.Front);
            foreach (var block in front)
            {
                if (block.Front != Colour.White)
                {
                    return false;
                }
            }

            var left = GetFace(Layer.Left);
            foreach (var block in left)
            {
                if (block.Left != Colour.Red)
                {
                    return false;
                }
            }
            
            var right = GetFace(Layer.Right);
            foreach (var block in right)
            {
                if (block.Right != Colour.Orange)
                {
                    return false;
                }
            }
            
            var top = GetFace(Layer.Top);
            foreach (var block in top)
            {
                if (block.Top != Colour.Green)
                {
                    return false;
                }
            }
            
            var bottom = GetFace(Layer.Bottom);
            foreach (var block in bottom)
            {
                if (block.Bottom != Colour.Blue)
                {
                    return false;
                }
            }

            return true;
        }
        
        

        private Colour? GetColour(Block block, Layer side)
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
                            faceToReturn[row, layer] = Cube[layer, row, 2];
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
                            faceToReturn[layer, col] = Cube[layer, 2, col];
                        }
                    }

                    return faceToReturn;
                case Layer.Back:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var col = 0; col < 3; col++)
                        {
                            var newCol = Math.Abs(col - 2);
                            faceToReturn[row, newCol] = Cube[2, row, col];
                        }
                    }

                    return faceToReturn;
                default:
                    throw new Exception($"Not a valid side: {side}");
            }
        }

        public void SetFace(Block[,] face, Layer layer)
        {
            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    switch (layer)
                    {
                        case Layer.Front:
                            Cube[0, row, col] = face[row, col];
                            break;
                        case Layer.Back:
                            Cube[2, row, Math.Abs(col - 2)] = face[row, col];
                            break;
                        case Layer.Top:
                            Cube[Math.Abs(row - 2), 0, col] = face[row, col];
                            break;
                        case Layer.Bottom:
                            Cube[row, 2, col] = face[row, col];
                            break;
                        case Layer.Left:
                            Cube[Math.Abs(col - 2), row, 0] = face[row, col];
                            break;
                        case Layer.Right:
                            Cube[col, row, 2] = face[row, col];
                            break;
                        default:
                            throw new Exception($"Not a valid face: {layer}");
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
                    newFace[col, Math.Abs(row - 2)] = layer switch
                    {
                        Layer.Front => new Block(
                            face[row, col].Bottom, 
                            face[row, col].Top, 
                            face[row, col].Left,
                            face[row, col].Right, 
                            face[row, col].Front, 
                            face[row, col].Back),
                        Layer.Back => new Block(
                            face[row, col].Top, 
                            face[row, col].Bottom, 
                            face[row, col].Right,
                            face[row, col].Left, 
                            face[row, col].Front, 
                            face[row, col].Back),
                        Layer.Top => new Block(
                            face[row, col].Front, 
                            face[row, col].Back, 
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Right, 
                            face[row, col].Left),
                        Layer.Bottom => new Block(
                            face[row, col].Back, 
                            face[row, col].Front,
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Left, 
                            face[row, col].Right),
                        Layer.Left => new Block(
                            face[row, col].Left, 
                            face[row, col].Right, 
                            face[row, col].Back,
                            face[row, col].Front, 
                            face[row, col].Top, 
                            face[row, col].Bottom),
                        Layer.Right => new Block(
                            face[row, col].Left, 
                            face[row, col].Right, 
                            face[row, col].Front,
                            face[row, col].Back, 
                            face[row, col].Bottom, 
                            face[row, col].Top),
                        _ => newFace[col, Math.Abs(row - 2)]
                    };
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
                    newFace[Math.Abs(col - 2), row] = layer switch
                    {
                        Layer.Front => new Block(
                            face[row, col].Top, 
                            face[row, col].Bottom,
                            face[row, col].Right,
                            face[row, col].Left,
                            face[row, col].Front,
                            face[row, col].Back),
                        Layer.Back => new Block(
                            face[row, col].Bottom, 
                            face[row, col].Top, 
                            face[row, col].Left,
                            face[row, col].Right, 
                            face[row, col].Front, 
                            face[row, col].Back),
                        Layer.Top => new Block(
                            face[row, col].Back, 
                            face[row, col].Front, 
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Left, 
                            face[row, col].Right),
                        Layer.Bottom => new Block(
                            face[row, col].Front, 
                            face[row, col].Back, 
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Right, 
                            face[row, col].Left),
                        Layer.Left => new Block(
                            face[row, col].Left, 
                            face[row, col].Right, 
                            face[row, col].Front,
                            face[row, col].Back, 
                            face[row, col].Bottom, 
                            face[row, col].Top),
                        Layer.Right => new Block(
                            face[row, col].Left, 
                            face[row, col].Right, 
                            face[row, col].Back,
                            face[row, col].Front, 
                            face[row, col].Top, 
                            face[row, col].Bottom),
                        _ => newFace[Math.Abs(col - 2), row]
                    };
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
            PrintFace(Layer.Top);
            PrintFaces();
            PrintFace(Layer.Bottom);
        }

        private void PrintFace(Layer layer)
        {
            var face = GetFace(layer);

            for (var row = 0; row < 3; row++)
            {
                Console.Write("                  "); // hack to indent top and bottom faces
                PrintColour(GetColour(face[row, 0], layer));
                Console.Write("   ");
                PrintColour(GetColour(face[row, 1], layer));
                Console.Write("   ");
                PrintColour(GetColour(face[row, 2], layer));
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
                PrintColour(GetColour(right[row, 0], Layer.Right));
                Console.Write("   ");
                PrintColour(GetColour(right[row, 1], Layer.Right));
                Console.Write("   ");
                PrintColour(GetColour(right[row, 2], Layer.Right));
                Console.Write("   ");
                
                var back = GetFace(Layer.Back);
                PrintColour(GetColour(back[row, 0], Layer.Back));
                Console.Write("   ");
                PrintColour(GetColour(back[row, 1], Layer.Back));
                Console.Write("   ");
                PrintColour(GetColour(back[row, 2], Layer.Back));

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

        public Block[,,] CloneCube()
        {
            return new[,,]
            {
                {
                    {
                        new Block(Cube[0, 0, 0].Left, Cube[0, 0, 0].Right, Cube[0, 0, 0].Top, Cube[0, 0, 0].Bottom, Cube[0, 0, 0].Front, Cube[0, 0, 0].Back),
                        new Block(Cube[0, 0, 1].Left, Cube[0, 0, 1].Right, Cube[0, 0, 1].Top, Cube[0, 0, 1].Bottom, Cube[0, 0, 1].Front, Cube[0, 0, 1].Back),
                        new Block(Cube[0, 0, 2].Left, Cube[0, 0, 2].Right, Cube[0, 0, 2].Top, Cube[0, 0, 2].Bottom, Cube[0, 0, 2].Front, Cube[0, 0, 2].Back)
                    },
                    {
                        new Block(Cube[0, 1, 0].Left, Cube[0, 1, 0].Right, Cube[0, 1, 0].Top, Cube[0, 1, 0].Bottom, Cube[0, 1, 0].Front, Cube[0, 1, 0].Back),
                        new Block(Cube[0, 1, 1].Left, Cube[0, 1, 1].Right, Cube[0, 1, 1].Top, Cube[0, 1, 1].Bottom, Cube[0, 1, 1].Front, Cube[0, 1, 1].Back),
                        new Block(Cube[0, 1, 2].Left, Cube[0, 1, 2].Right, Cube[0, 1, 2].Top, Cube[0, 1, 2].Bottom, Cube[0, 1, 2].Front, Cube[0, 1, 2].Back)
                    },
                    {
                        new Block(Cube[0, 2, 0].Left, Cube[0, 2, 0].Right, Cube[0, 2, 0].Top, Cube[0, 2, 0].Bottom, Cube[0, 2, 0].Front, Cube[0, 2, 0].Back),
                        new Block(Cube[0, 2, 1].Left, Cube[0, 2, 1].Right, Cube[0, 2, 1].Top, Cube[0, 2, 1].Bottom, Cube[0, 2, 1].Front, Cube[0, 2, 1].Back),
                        new Block(Cube[0, 2, 2].Left, Cube[0, 2, 2].Right, Cube[0, 2, 2].Top, Cube[0, 2, 2].Bottom, Cube[0, 2, 2].Front, Cube[0, 2, 2].Back)
                    }
                },
                {
                    {
                        new Block(Cube[1, 0, 0].Left, Cube[1, 0, 0].Right, Cube[1, 0, 0].Top, Cube[1, 0, 0].Bottom, Cube[1, 0, 0].Front, Cube[1, 0, 0].Back),
                        new Block(Cube[1, 0, 1].Left, Cube[1, 0, 1].Right, Cube[1, 0, 1].Top, Cube[1, 0, 1].Bottom, Cube[1, 0, 1].Front, Cube[1, 0, 1].Back),
                        new Block(Cube[1, 0, 2].Left, Cube[1, 0, 2].Right, Cube[1, 0, 2].Top, Cube[1, 0, 2].Bottom, Cube[1, 0, 2].Front, Cube[1, 0, 2].Back)
                    },
                    {
                        new Block(Cube[1, 1, 0].Left, Cube[1, 1, 0].Right, Cube[1, 1, 0].Top, Cube[1, 1, 0].Bottom, Cube[1, 1, 0].Front, Cube[1, 1, 0].Back),
                        new Block(Cube[1, 1, 1].Left, Cube[1, 1, 1].Right, Cube[1, 1, 1].Top, Cube[1, 1, 1].Bottom, Cube[1, 1, 1].Front, Cube[1, 1, 1].Back),
                        new Block(Cube[1, 1, 2].Left, Cube[1, 1, 2].Right, Cube[1, 1, 2].Top, Cube[1, 1, 2].Bottom, Cube[1, 1, 2].Front, Cube[1, 1, 2].Back)
                    },
                    {
                        new Block(Cube[1, 2, 0].Left, Cube[1, 2, 0].Right, Cube[1, 2, 0].Top, Cube[1, 2, 0].Bottom, Cube[1, 2, 0].Front, Cube[1, 2, 0].Back),
                        new Block(Cube[1, 2, 1].Left, Cube[1, 2, 1].Right, Cube[1, 2, 1].Top, Cube[1, 2, 1].Bottom, Cube[1, 2, 1].Front, Cube[1, 2, 1].Back),
                        new Block(Cube[1, 2, 2].Left, Cube[1, 2, 2].Right, Cube[1, 2, 2].Top, Cube[1, 2, 2].Bottom, Cube[1, 2, 2].Front, Cube[1, 2, 2].Back)
                    }
                },
                {
                    {
                        new Block(Cube[2, 0, 0].Left, Cube[2, 0, 0].Right, Cube[2, 0, 0].Top, Cube[2, 0, 0].Bottom, Cube[2, 0, 0].Front, Cube[2, 0, 0].Back),
                        new Block(Cube[2, 0, 1].Left, Cube[2, 0, 1].Right, Cube[2, 0, 1].Top, Cube[2, 0, 1].Bottom, Cube[2, 0, 1].Front, Cube[2, 0, 1].Back),
                        new Block(Cube[2, 0, 2].Left, Cube[2, 0, 2].Right, Cube[2, 0, 2].Top, Cube[2, 0, 2].Bottom, Cube[2, 0, 2].Front, Cube[2, 0, 2].Back)
                    },
                    {
                        new Block(Cube[2, 1, 0].Left, Cube[2, 1, 0].Right, Cube[2, 1, 0].Top, Cube[2, 1, 0].Bottom, Cube[2, 1, 0].Front, Cube[2, 1, 0].Back),
                        new Block(Cube[2, 1, 1].Left, Cube[2, 1, 1].Right, Cube[2, 1, 1].Top, Cube[2, 1, 1].Bottom, Cube[2, 1, 1].Front, Cube[2, 1, 1].Back),
                        new Block(Cube[2, 1, 2].Left, Cube[2, 1, 2].Right, Cube[2, 1, 2].Top, Cube[2, 1, 2].Bottom, Cube[2, 1, 2].Front, Cube[2, 1, 2].Back)
                    },
                    {
                        new Block(Cube[2, 2, 0].Left, Cube[2, 2, 0].Right, Cube[2, 2, 0].Top, Cube[2, 2, 0].Bottom, Cube[2, 2, 0].Front, Cube[2, 2, 0].Back),
                        new Block(Cube[2, 2, 1].Left, Cube[2, 2, 1].Right, Cube[2, 2, 1].Top, Cube[2, 2, 1].Bottom, Cube[2, 2, 1].Front, Cube[2, 2, 1].Back),
                        new Block(Cube[2, 2, 2].Left, Cube[2, 2, 2].Right, Cube[2, 2, 2].Top, Cube[2, 2, 2].Bottom, Cube[2, 2, 2].Front, Cube[2, 2, 2].Back)
                    }
                }
            };
        }

        public void Solve()
        {
            SolveSecondLayer();
            SolveThirdLayer();
        }

        private void SolveSecondLayer()
        {
            var solver = new SecondLayerSolver(this);
            solver.Solve();
        }

        private void SolveThirdLayer()
        {
            var solver = new ThirdLayerSolver(this);
            
            // Form a cross on the back face
            solver.FormYellowCross();
            
            // Correct the location of the middle edge pieces
            solver.ReorganiseMiddleEdges();
            
            // Get the corner pieces in the right corners
            solver.ReorganiseCorners();
            
            // Correct the orientation of the corner pieces
            solver.CorrectCornerOrientation();
        }

        public bool SecondLayerIsSolved()
        {
            var front = GetFace(Layer.Front);
            foreach (var block in front)
            {
                if (block.Front != Colour.White)
                {
                    return false;
                }
            }

            var left = GetFace(Layer.Left);
            for (var x = 0; x < 3; x++)
            {
                for (var y = 1; y < 3; y++)
                {
                    if (left[x, y].Left != Colour.Red)
                    {
                        return false;
                    }
                }
            }
            
            var right = GetFace(Layer.Right);
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 2; y++)
                {
                    if (right[x, y].Right != Colour.Orange)
                    {
                        return false;
                    }
                }
            }
            
            var top = GetFace(Layer.Top);
            for (var x = 1; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (top[x, y].Top != Colour.Green)
                    {
                        return false;
                    }
                }
            }
            
            var bottom = GetFace(Layer.Bottom);
            for (var x = 0; x < 2; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (bottom[x, y].Bottom != Colour.Blue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}