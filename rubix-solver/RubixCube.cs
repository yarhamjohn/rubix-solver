using System;
using System.Collections.Generic;
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
                CornerBuilder.Build((2, 2), face[2, 2], side),
            };
        }
    }
}
