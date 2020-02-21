using System;
using System.Collections.Generic;
using System.Linq;
using rubix_solver.Solvers;

namespace rubix_solver
{
    public class RubixCube
    {
        public Block[,,] Cube { get; set; }

        public List<(int num, string direction, Side side)> Instructions = new List<(int num, string direction, Side side)>();

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

        private Colour? ConvertToColour(string? col)
        {
            if (col == null)
            {
                return null;
            }

            return col switch

            {
                "red" => Colour.Red,
                "blue" => Colour.Blue,
                "green" => Colour.Green,
                "orange" => Colour.Orange,
                "yellow" => Colour.Yellow,
                "white" => Colour.White,
                _ => throw new ArgumentException($"Not a valid colour: {col}")
            };
        }

        public RubixCube(Block[,,] cube)
        {
            Cube = cube;
        }

        public RubixCube(Cube cube)
        {
            Cube = new[,,]
            {
                {
                    {
                        new Block(cube.FrontFace.TopRow.LeftColumn.LeftColour, ConvertToColour(cube.FrontFace.TopRow.LeftColumn.Right), ConvertToColour(cube.FrontFace.TopRow.LeftColumn.Top), ConvertToColour(cube.FrontFace.TopRow.LeftColumn.Bottom), ConvertToColour(cube.FrontFace.TopRow.LeftColumn.Front), ConvertToColour(cube.FrontFace.TopRow.LeftColumn.Back)),
                        new Block(cube.FrontFace.TopRow.MiddleColumn.LeftColour, ConvertToColour(cube.FrontFace.TopRow.MiddleColumn.Right), ConvertToColour(cube.FrontFace.TopRow.MiddleColumn.Top), ConvertToColour(cube.FrontFace.TopRow.MiddleColumn.Bottom), ConvertToColour(cube.FrontFace.TopRow.MiddleColumn.Front), ConvertToColour(cube.FrontFace.TopRow.MiddleColumn.Back)),
                        new Block(cube.FrontFace.TopRow.RightColumn.LeftColour, ConvertToColour(cube.FrontFace.TopRow.RightColumn.Right), ConvertToColour(cube.FrontFace.TopRow.RightColumn.Top), ConvertToColour(cube.FrontFace.TopRow.RightColumn.Bottom), ConvertToColour(cube.FrontFace.TopRow.RightColumn.Front), ConvertToColour(cube.FrontFace.TopRow.RightColumn.Back))
                    },
                    {
                        new Block(cube.FrontFace.MiddleRow.LeftColumn.LeftColour, ConvertToColour(cube.FrontFace.MiddleRow.LeftColumn.Right),
                            ConvertToColour(cube.FrontFace.MiddleRow.LeftColumn.Top), ConvertToColour(cube.FrontFace.MiddleRow.LeftColumn.Bottom),
                            ConvertToColour(cube.FrontFace.MiddleRow.LeftColumn.Front), ConvertToColour(cube.FrontFace.MiddleRow.LeftColumn.Back)),
                        new Block(cube.FrontFace.MiddleRow.MiddleColumn.LeftColour, ConvertToColour(cube.FrontFace.MiddleRow.MiddleColumn.Right),
                            ConvertToColour(cube.FrontFace.MiddleRow.MiddleColumn.Top), ConvertToColour(cube.FrontFace.MiddleRow.MiddleColumn.Bottom),
                            ConvertToColour(cube.FrontFace.MiddleRow.MiddleColumn.Front), ConvertToColour(cube.FrontFace.MiddleRow.MiddleColumn.Back)),
                        new Block(cube.FrontFace.MiddleRow.RightColumn.LeftColour, ConvertToColour(cube.FrontFace.MiddleRow.RightColumn.Right),
                            ConvertToColour(cube.FrontFace.MiddleRow.RightColumn.Top), ConvertToColour(cube.FrontFace.MiddleRow.RightColumn.Bottom),
                            ConvertToColour(cube.FrontFace.MiddleRow.RightColumn.Front), ConvertToColour(cube.FrontFace.MiddleRow.RightColumn.Back))
                    },
                    {
                        new Block(cube.FrontFace.BottomRow.LeftColumn.LeftColour, ConvertToColour(cube.FrontFace.BottomRow.LeftColumn.Right),
                            ConvertToColour(cube.FrontFace.BottomRow.LeftColumn.Top), ConvertToColour(cube.FrontFace.BottomRow.LeftColumn.Bottom),
                            ConvertToColour(cube.FrontFace.BottomRow.LeftColumn.Front), ConvertToColour(cube.FrontFace.BottomRow.LeftColumn.Back)),
                        new Block(cube.FrontFace.BottomRow.MiddleColumn.LeftColour, ConvertToColour(cube.FrontFace.BottomRow.MiddleColumn.Right),
                            ConvertToColour(cube.FrontFace.BottomRow.MiddleColumn.Top), ConvertToColour(cube.FrontFace.BottomRow.MiddleColumn.Bottom),
                            ConvertToColour(cube.FrontFace.BottomRow.MiddleColumn.Front), ConvertToColour(cube.FrontFace.BottomRow.MiddleColumn.Back)),
                        new Block(cube.FrontFace.BottomRow.RightColumn.LeftColour, ConvertToColour(cube.FrontFace.BottomRow.RightColumn.Right),
                            ConvertToColour(cube.FrontFace.BottomRow.RightColumn.Top), ConvertToColour(cube.FrontFace.BottomRow.RightColumn.Bottom),
                            ConvertToColour(cube.FrontFace.BottomRow.RightColumn.Front), ConvertToColour(cube.FrontFace.BottomRow.RightColumn.Back))

                    }
                },
                {
                    {
                        new Block(cube.MiddleFace.TopRow.LeftColumn.LeftColour, ConvertToColour(cube.MiddleFace.TopRow.LeftColumn.Right), ConvertToColour(cube.MiddleFace.TopRow.LeftColumn.Top), ConvertToColour(cube.MiddleFace.TopRow.LeftColumn.Bottom), ConvertToColour(cube.MiddleFace.TopRow.LeftColumn.Front), ConvertToColour(cube.MiddleFace.TopRow.LeftColumn.Back)),
                        new Block(cube.MiddleFace.TopRow.MiddleColumn.LeftColour, ConvertToColour(cube.MiddleFace.TopRow.MiddleColumn.Right), ConvertToColour(cube.MiddleFace.TopRow.MiddleColumn.Top), ConvertToColour(cube.MiddleFace.TopRow.MiddleColumn.Bottom), ConvertToColour(cube.MiddleFace.TopRow.MiddleColumn.Front), ConvertToColour(cube.MiddleFace.TopRow.MiddleColumn.Back)),
                        new Block(cube.MiddleFace.TopRow.RightColumn.LeftColour, ConvertToColour(cube.MiddleFace.TopRow.RightColumn.Right), ConvertToColour(cube.MiddleFace.TopRow.RightColumn.Top), ConvertToColour(cube.MiddleFace.TopRow.RightColumn.Bottom), ConvertToColour(cube.MiddleFace.TopRow.RightColumn.Front), ConvertToColour(cube.MiddleFace.TopRow.RightColumn.Back))
                    },
                    {
                        new Block(cube.MiddleFace.MiddleRow.LeftColumn.LeftColour, ConvertToColour(cube.MiddleFace.MiddleRow.LeftColumn.Right),
                            ConvertToColour(cube.MiddleFace.MiddleRow.LeftColumn.Top), ConvertToColour(cube.MiddleFace.MiddleRow.LeftColumn.Bottom),
                            ConvertToColour(cube.MiddleFace.MiddleRow.LeftColumn.Front), ConvertToColour(cube.MiddleFace.MiddleRow.LeftColumn.Back)),
                        new Block(cube.MiddleFace.MiddleRow.MiddleColumn.LeftColour, ConvertToColour(cube.MiddleFace.MiddleRow.MiddleColumn.Right),
                            ConvertToColour(cube.MiddleFace.MiddleRow.MiddleColumn.Top), ConvertToColour(cube.MiddleFace.MiddleRow.MiddleColumn.Bottom),
                            ConvertToColour(cube.MiddleFace.MiddleRow.MiddleColumn.Front), ConvertToColour(cube.MiddleFace.MiddleRow.MiddleColumn.Back)),
                        new Block(cube.MiddleFace.MiddleRow.RightColumn.LeftColour, ConvertToColour(cube.MiddleFace.MiddleRow.RightColumn.Right),
                            ConvertToColour(cube.MiddleFace.MiddleRow.RightColumn.Top), ConvertToColour(cube.MiddleFace.MiddleRow.RightColumn.Bottom),
                            ConvertToColour(cube.MiddleFace.MiddleRow.RightColumn.Front), ConvertToColour(cube.MiddleFace.MiddleRow.RightColumn.Back))
                    },
                    {
                        new Block(cube.MiddleFace.BottomRow.LeftColumn.LeftColour, ConvertToColour(cube.MiddleFace.BottomRow.LeftColumn.Right),
                            ConvertToColour(cube.MiddleFace.BottomRow.LeftColumn.Top), ConvertToColour(cube.MiddleFace.BottomRow.LeftColumn.Bottom),
                            ConvertToColour(cube.MiddleFace.BottomRow.LeftColumn.Front), ConvertToColour(cube.MiddleFace.BottomRow.LeftColumn.Back)),
                        new Block(cube.MiddleFace.BottomRow.MiddleColumn.LeftColour, ConvertToColour(cube.MiddleFace.BottomRow.MiddleColumn.Right),
                            ConvertToColour(cube.MiddleFace.BottomRow.MiddleColumn.Top), ConvertToColour(cube.MiddleFace.BottomRow.MiddleColumn.Bottom),
                            ConvertToColour(cube.MiddleFace.BottomRow.MiddleColumn.Front), ConvertToColour(cube.MiddleFace.BottomRow.MiddleColumn.Back)),
                        new Block(cube.MiddleFace.BottomRow.RightColumn.LeftColour, ConvertToColour(cube.MiddleFace.BottomRow.RightColumn.Right),
                            ConvertToColour(cube.MiddleFace.BottomRow.RightColumn.Top), ConvertToColour(cube.MiddleFace.BottomRow.RightColumn.Bottom),
                            ConvertToColour(cube.MiddleFace.BottomRow.RightColumn.Front), ConvertToColour(cube.MiddleFace.BottomRow.RightColumn.Back))

                    }
                },
                {
                    {
                        new Block(cube.BackFace.TopRow.LeftColumn.LeftColour, ConvertToColour(cube.BackFace.TopRow.LeftColumn.Right), ConvertToColour(cube.BackFace.TopRow.LeftColumn.Top), ConvertToColour(cube.BackFace.TopRow.LeftColumn.Bottom), ConvertToColour(cube.BackFace.TopRow.LeftColumn.Front), ConvertToColour(cube.BackFace.TopRow.LeftColumn.Back)),
                        new Block(cube.BackFace.TopRow.MiddleColumn.LeftColour, ConvertToColour(cube.BackFace.TopRow.MiddleColumn.Right), ConvertToColour(cube.BackFace.TopRow.MiddleColumn.Top), ConvertToColour(cube.BackFace.TopRow.MiddleColumn.Bottom), ConvertToColour(cube.BackFace.TopRow.MiddleColumn.Front), ConvertToColour(cube.BackFace.TopRow.MiddleColumn.Back)),
                        new Block(cube.BackFace.TopRow.RightColumn.LeftColour, ConvertToColour(cube.BackFace.TopRow.RightColumn.Right), ConvertToColour(cube.BackFace.TopRow.RightColumn.Top), ConvertToColour(cube.BackFace.TopRow.RightColumn.Bottom), ConvertToColour(cube.BackFace.TopRow.RightColumn.Front), ConvertToColour(cube.BackFace.TopRow.RightColumn.Back))
                    },
                    {
                        new Block(cube.BackFace.MiddleRow.LeftColumn.LeftColour, ConvertToColour(cube.BackFace.MiddleRow.LeftColumn.Right),
                            ConvertToColour(cube.BackFace.MiddleRow.LeftColumn.Top), ConvertToColour(cube.BackFace.MiddleRow.LeftColumn.Bottom),
                            ConvertToColour(cube.BackFace.MiddleRow.LeftColumn.Front), ConvertToColour(cube.BackFace.MiddleRow.LeftColumn.Back)),
                        new Block(cube.BackFace.MiddleRow.MiddleColumn.LeftColour, ConvertToColour(cube.BackFace.MiddleRow.MiddleColumn.Right),
                            ConvertToColour(cube.BackFace.MiddleRow.MiddleColumn.Top), ConvertToColour(cube.BackFace.MiddleRow.MiddleColumn.Bottom),
                            ConvertToColour(cube.BackFace.MiddleRow.MiddleColumn.Front), ConvertToColour(cube.BackFace.MiddleRow.MiddleColumn.Back)),
                        new Block(cube.BackFace.MiddleRow.RightColumn.LeftColour, ConvertToColour(cube.BackFace.MiddleRow.RightColumn.Right),
                            ConvertToColour(cube.BackFace.MiddleRow.RightColumn.Top), ConvertToColour(cube.BackFace.MiddleRow.RightColumn.Bottom),
                            ConvertToColour(cube.BackFace.MiddleRow.RightColumn.Front), ConvertToColour(cube.BackFace.MiddleRow.RightColumn.Back))
                    },
                    {
                        new Block(cube.BackFace.BottomRow.LeftColumn.LeftColour, ConvertToColour(cube.BackFace.BottomRow.LeftColumn.Right),
                            ConvertToColour(cube.BackFace.BottomRow.LeftColumn.Top), ConvertToColour(cube.BackFace.BottomRow.LeftColumn.Bottom),
                            ConvertToColour(cube.BackFace.BottomRow.LeftColumn.Front), ConvertToColour(cube.BackFace.BottomRow.LeftColumn.Back)),
                        new Block(cube.BackFace.BottomRow.MiddleColumn.LeftColour, ConvertToColour(cube.BackFace.BottomRow.MiddleColumn.Right),
                            ConvertToColour(cube.BackFace.BottomRow.MiddleColumn.Top), ConvertToColour(cube.BackFace.BottomRow.MiddleColumn.Bottom),
                            ConvertToColour(cube.BackFace.BottomRow.MiddleColumn.Front), ConvertToColour(cube.BackFace.BottomRow.MiddleColumn.Back)),
                        new Block(cube.BackFace.BottomRow.RightColumn.LeftColour, ConvertToColour(cube.BackFace.BottomRow.RightColumn.Right),
                            ConvertToColour(cube.BackFace.BottomRow.RightColumn.Top), ConvertToColour(cube.BackFace.BottomRow.RightColumn.Bottom),
                            ConvertToColour(cube.BackFace.BottomRow.RightColumn.Front), ConvertToColour(cube.BackFace.BottomRow.RightColumn.Back))

                    }
                }
            };
        }

        public RubixCube()
        {
            Cube = _solvedCube.Clone() as Block[,,];
        }

        public Colour? GetColour(Block block, Side side)
        {
            return side switch
            {
                Side.Left => block.Left,
                Side.Right => block.Right,
                Side.Front => block.Front,
                Side.Back => block.Back,
                Side.Top => block.Top,
                Side.Bottom => block.Bottom,
                _ => null
            };
        }

        public Block[,] GetFace(Side side)
        {
            var faceToReturn = new Block[3, 3];

            switch (side)
            {
                case Side.Front:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var col = 0; col < 3; col++)
                        {
                            faceToReturn[row, col] = Cube[0, row, col];
                        }
                    }

                    return faceToReturn;
                case Side.Left:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            var col = Math.Abs(layer - 2);
                            faceToReturn[row, col] = Cube[layer, row, 0];
                        }
                    }

                    return faceToReturn;
                case Side.Right:
                    for (var row = 0; row < 3; row++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            faceToReturn[row, layer] = Cube[layer, row, 2];
                        }
                    }

                    return faceToReturn;
                case Side.Top:
                    for (var col = 0; col < 3; col++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            var row = Math.Abs(layer - 2);
                            faceToReturn[row, col] = Cube[layer, 0, col];
                        }
                    }

                    return faceToReturn;
                case Side.Bottom:
                    for (var col = 0; col < 3; col++)
                    {
                        for (var layer = 0; layer < 3; layer++)
                        {
                            faceToReturn[layer, col] = Cube[layer, 2, col];
                        }
                    }

                    return faceToReturn;
                case Side.Back:
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

        public void SetFace(Block[,] face, Side side)
        {
            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    switch (side)
                    {
                        case Side.Front:
                            Cube[0, row, col] = face[row, col];
                            break;
                        case Side.Back:
                            Cube[2, row, Math.Abs(col - 2)] = face[row, col];
                            break;
                        case Side.Top:
                            Cube[Math.Abs(row - 2), 0, col] = face[row, col];
                            break;
                        case Side.Bottom:
                            Cube[row, 2, col] = face[row, col];
                            break;
                        case Side.Left:
                            Cube[Math.Abs(col - 2), row, 0] = face[row, col];
                            break;
                        case Side.Right:
                            Cube[col, row, 2] = face[row, col];
                            break;
                        default:
                            throw new Exception($"Not a valid face: {side}");
                    }
                }
            }
        }
        
        public void RotateClockwise(Side side)
        {
            var face = GetFace(side);
            var newFace = new Block[3, 3];

            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    newFace[col, Math.Abs(row - 2)] = side switch
                    {
                        Side.Front => new Block(
                            face[row, col].Bottom, 
                            face[row, col].Top, 
                            face[row, col].Left,
                            face[row, col].Right, 
                            face[row, col].Front, 
                            face[row, col].Back),
                        Side.Back => new Block(
                            face[row, col].Top, 
                            face[row, col].Bottom, 
                            face[row, col].Right,
                            face[row, col].Left, 
                            face[row, col].Front, 
                            face[row, col].Back),
                        Side.Top => new Block(
                            face[row, col].Front, 
                            face[row, col].Back, 
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Right, 
                            face[row, col].Left),
                        Side.Bottom => new Block(
                            face[row, col].Back, 
                            face[row, col].Front,
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Left, 
                            face[row, col].Right),
                        Side.Left => new Block(
                            face[row, col].Left, 
                            face[row, col].Right, 
                            face[row, col].Back,
                            face[row, col].Front, 
                            face[row, col].Top, 
                            face[row, col].Bottom),
                        Side.Right => new Block(
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

            SetFace(newFace, side);
            Instructions.Add((Instructions.Count + 1, "clockwise", side));
        }

        public void RotateAntiClockwise(Side side)
        {
            var face = GetFace(side);
            var newFace = new Block[3, 3];

            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    newFace[Math.Abs(col - 2), row] = side switch
                    {
                        Side.Front => new Block(
                            face[row, col].Top, 
                            face[row, col].Bottom,
                            face[row, col].Right,
                            face[row, col].Left,
                            face[row, col].Front,
                            face[row, col].Back),
                        Side.Back => new Block(
                            face[row, col].Bottom, 
                            face[row, col].Top, 
                            face[row, col].Left,
                            face[row, col].Right, 
                            face[row, col].Front, 
                            face[row, col].Back),
                        Side.Top => new Block(
                            face[row, col].Back, 
                            face[row, col].Front, 
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Left, 
                            face[row, col].Right),
                        Side.Bottom => new Block(
                            face[row, col].Front, 
                            face[row, col].Back, 
                            face[row, col].Top,
                            face[row, col].Bottom, 
                            face[row, col].Right, 
                            face[row, col].Left),
                        Side.Left => new Block(
                            face[row, col].Left, 
                            face[row, col].Right, 
                            face[row, col].Front,
                            face[row, col].Back, 
                            face[row, col].Bottom, 
                            face[row, col].Top),
                        Side.Right => new Block(
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

            SetFace(newFace, side);
            Instructions.Add((Instructions.Count + 1, "anti-clockwise", side));
        }

        public void Randomise()
        {
            var random = new Random();
            foreach (var _ in Enumerable.Range(0, 100))
            {
                var layer = (Side)random.Next(0, 5);
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

            Instructions.Clear();
        }

        public Colour? GetCenterBlockFace(Side side)
        {
            return side switch
            {
                Side.Front => Cube[0, 1, 1].Front,
                Side.Top => Cube[1, 0, 1].Top,
                Side.Left => Cube[1, 1, 0].Left,
                Side.Right => Cube[1, 1, 2].Right,
                Side.Bottom => Cube[1, 2, 1].Bottom,
                Side.Back => Cube[2, 1, 1].Back,
                _ => throw new Exception($"Not a valid side ({side.ToString()}")
            };
        }

        public IEnumerable<Block> GetMiddleLayer(Side side)
        {
            var face = GetFace(side);
            if (side == Side.Left || side == Side.Right)
            {
                return new List<Block> {face[0, 1], face[1, 1], face[2, 1]};
            }
            return new List<Block> {face[1, 0], face[1, 1], face[1, 2]};
        }

        public IEnumerable<Block> GetBottomLayer(Side side)
        {
            if (side == Side.Back || side == Side.Front)
            {
                throw new Exception(
                    $"This side ({side}) has no concept of layers as these are calculated in relation to the front side.");
            }

            var face = GetFace(side);
            return side switch
            {
                Side.Left => new List<Block> {face[0, 2], face[1, 2], face[2, 2]},
                Side.Right => new List<Block> {face[0, 0], face[1, 0], face[2, 0]},
                Side.Bottom => new List<Block> {face[0, 0], face[0, 1], face[0, 2]},
                _ => new List<Block> {face[2, 0], face[2, 1], face[2, 2]}
            };
        }

        public IEnumerable<FrontCorner> GetFrontCornerBlocks()
        {
            return GetCornerBlocks(Side.Front).Select(x => (FrontCorner) x);
        }

        public IEnumerable<BackCorner> GetBackCornerBlocks()
        {
            return GetCornerBlocks(Side.Back).Select(x => (BackCorner) x);
        }

        private IEnumerable<Corner> GetCornerBlocks(Side side)
        {
            var face = GetFace(side);
            return new List<Corner>
            {
                CornerBuilder.Build((0, 0), face[0, 0], side),
                CornerBuilder.Build((0, 2), face[0, 2], side),
                CornerBuilder.Build((2, 0), face[2, 0], side),
                CornerBuilder.Build((2, 2), face[2, 2], side)
            };
        }

        public List<Block> GetFrontEdges()
        {
            var face = GetFace(Side.Front);
            return new List<Block>
            {
                face[0, 1],
                face[1, 0],
                face[1, 2],
                face[2, 1]
            };
        }

        public IEnumerable<Block> GetBackEdges()
        {
            var face = GetFace(Side.Back);
            return new List<Block>
            {
                face[0, 1],
                face[1, 0],
                face[1, 2],
                face[2, 1]
            };
        }

        public IEnumerable<Block> GetSideEdges()
        {
            var left = GetFace(Side.Left);
            var right = GetFace(Side.Right);
            return new List<Block>
            {
                left[0, 1],
                left[2, 1],
                right[0, 1],
                right[2, 1]
            };
        }

        public IEnumerable<Block> GetBackFaceCrossBlocks()
        {
            var face = GetFace(Side.Back);
            return new List<Block>
            {
                face[0, 1],
                face[1, 1],
                face[2, 1],
                face[1, 0],
                face[1, 2],
            };
        }

        public Block GetBlock(Block oldBlock)
        {
            foreach (var block in Cube)
            {
                if (block.HasMatchingColours(oldBlock))
                {
                    return block;
                }
            }
            
            throw new Exception("The provided block was not found in the cube.");
        }

        public Side GetExpectedSide(Colour colour)
        {
            return colour switch
            {
                Colour.Blue => Side.Bottom,
                Colour.Green => Side.Top,
                Colour.Orange => Side.Right,
                Colour.Red => Side.Left,
                Colour.White => Side.Front,
                Colour.Yellow => Side.Back
            };
        }
    }
}
