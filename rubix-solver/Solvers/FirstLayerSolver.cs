using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver.Solvers
{
    public class FirstLayerSolver
    {
        private readonly RubixCube _cube;

        public FirstLayerSolver(RubixCube cube)
        {
            _cube = cube;
        }

        public void SolveCorners()
        {
            if (_cube.IsSolved())
            {
                return;
            }

            while (!_cube.FirstLayerIsSolved())
            {
                if (GetCorners(Layer.Back).Any(c => c.Item2.HasColour(Colour.White)))
                {
                    var corner = GetCorners(Layer.Back).First(c => c.Item2.HasColour(Colour.White));
                    if (!IsBetweenCorrectBackSides(corner))
                    {
                        _cube.RotateClockwise(Layer.Back);
                    }
                    else
                    {
                        if (corner.Item1 == (0, 0))
                        {
                            if (corner.Item2.Top == Colour.White)
                            {
                                _cube.RotateAntiClockwise(Layer.Top);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Top);
                            } else if (corner.Item2.Right == Colour.White)
                            {
                                _cube.RotateClockwise(Layer.Right);
                                _cube.RotateClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Right);
                            }
                            else
                            {
                                _cube.RotateAntiClockwise(Layer.Top);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Top);
                                _cube.RotateClockwise(Layer.Right);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Right);
                                _cube.RotateAntiClockwise(Layer.Top);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Top);
                            }
                        }

                        if (corner.Item1 == (0, 2))
                        {
                            if (corner.Item2.Left == Colour.White)
                            {
                                _cube.RotateAntiClockwise(Layer.Left);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Left);
                            } else if (corner.Item2.Top == Colour.White)
                            {
                                _cube.RotateClockwise(Layer.Top);
                                _cube.RotateClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Top);
                            }
                            else
                            {
                                _cube.RotateAntiClockwise(Layer.Left);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Left);
                                _cube.RotateClockwise(Layer.Top);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Top);
                                _cube.RotateAntiClockwise(Layer.Left);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Left);
                            }
                        }

                        if (corner.Item1 == (2, 0))
                        {
                            if (corner.Item2.Right == Colour.White)
                            {
                                _cube.RotateAntiClockwise(Layer.Right);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Right);
                            } else if (corner.Item2.Bottom == Colour.White)
                            {
                                _cube.RotateClockwise(Layer.Bottom);
                                _cube.RotateClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Bottom);
                            }
                            else
                            {
                                _cube.RotateAntiClockwise(Layer.Right);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Right);
                                _cube.RotateClockwise(Layer.Bottom);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Bottom);
                                _cube.RotateAntiClockwise(Layer.Right);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Right);
                            }
                        }

                        if (corner.Item1 == (2, 2))
                        {
                            if (corner.Item2.Bottom == Colour.White)
                            {
                                _cube.RotateAntiClockwise(Layer.Bottom);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Bottom);
                            } else if (corner.Item2.Left == Colour.White)
                            {
                                _cube.RotateClockwise(Layer.Left);
                                _cube.RotateClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Left);
                            }
                            else
                            {
                                _cube.RotateAntiClockwise(Layer.Bottom);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Bottom);
                                _cube.RotateClockwise(Layer.Left);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateAntiClockwise(Layer.Left);
                                _cube.RotateAntiClockwise(Layer.Bottom);
                                _cube.RotateAntiClockwise(Layer.Back);
                                _cube.RotateClockwise(Layer.Bottom);
                            }
                        }
                    }
                }
                else
                {
                    var corner = GetCorners(Layer.Front).First(c => !IsCorrectlyPositioned(c));
                    switch (corner.Item1) {
                        case (0, 0):
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Top);
                            break;
                        case (0, 2):
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        case (2, 0):
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Left);
                            break;
                        case (2, 2):
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            break;
                        default:
                            throw new Exception("Not a corner...");
                    }
                }
            }
        }

        private bool IsCorrectlyPositioned(((int x, int y), Block block) corner)
        {
            return IsBetweenCorrectFrontSides(corner) && corner.block.Front == Colour.White;
        }

        private bool IsBetweenCorrectBackSides(((int x, int y), Block block) corner)
        {
            return corner.Item1 switch
            {
                (0, 0) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Orange),
                (0, 2) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Red),
                (2, 0) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Orange),
                (2, 2) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Red),
                _ => throw new Exception("This isn't a corner block...")
            };
        }
        
        private bool IsBetweenCorrectFrontSides(((int x, int y), Block block) corner)
        {
            return corner.Item1 switch
            {
                (0, 0) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Red),
                (0, 2) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Orange),
                (2, 0) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Red),
                (2, 2) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Orange),
                _ => throw new Exception("This isn't a corner block...")
            };
        }

        private List<((int x, int y), Block)> GetCorners(Layer layer)
        {            
            var face = _cube.GetFace(layer);
            return new List<((int x, int y), Block)>
            {
                ((0, 0), face[0, 0]), 
                ((0, 2), face[0, 2]), 
                ((2, 0), face[2, 0]), 
                ((2, 2), face[2, 2])
            };
        }
    }
}