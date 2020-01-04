using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver
{
    public class ThirdLayerSolver
    {
        private readonly RubixCube _cube;
        
        public ThirdLayerSolver(RubixCube cube)
        {
            _cube = cube;
        }
        
        public void ReorganiseCorners()
        {
            var cornerBlocks = GetCornerBlocks();
            var incorrectCornerBlocks = cornerBlocks.Where(CornerBlockInWrongPosition).ToList();
            
            if (incorrectCornerBlocks.Count == 1 || incorrectCornerBlocks.Count == 2)
            {
                throw new Exception("Something has gone wrong. There can only be 0, 3 or 4 incorrect blocks at this stage.");
            }

            if (incorrectCornerBlocks.Count == 0)
            {
                return;
            }
            
            while (GetCornerBlocks().Where(CornerBlockInWrongPosition).ToList().Count > 0)
            {
                if (incorrectCornerBlocks.Count == 3)
                {
                    var ((x, y), _) = cornerBlocks.Except(incorrectCornerBlocks).First();
                    switch (x, y)
                    {
                        case (0, 0):
                        {
                            PerformUrulRotations(Layer.Right, Layer.Left);
                            break;
                        }
                        case (0, 2):
                        {
                            PerformUrulRotations(Layer.Top, Layer.Bottom);
                            break;
                        }
                        case (2, 0):
                        {
                            PerformUrulRotations(Layer.Bottom, Layer.Top);
                            break;
                        }
                        case (2, 2):
                        {
                            PerformUrulRotations(Layer.Left, Layer.Right);
                            break;
                        }
                        default:
                            throw new Exception($"Cannot perform urul rotation as target is not a corner block: ({x}, {y})");
                    }
                }
                else
                {
                    PerformUrulRotations(Layer.Top, Layer.Bottom);
                }
            }
        }

        private Dictionary<(int, int), Block> GetCornerBlocks()
        {
            var face = _cube.GetFace(Layer.Back);
            return new Dictionary<(int, int), Block>
            {
                {(0, 0), face[0, 0]},
                {(0, 2), face[0, 2]},
                {(2, 0), face[2, 0]},
                {(2, 2), face[2, 2]}
            };
        }

        private void PerformUrulRotations(Layer rightSide, Layer leftSide)
        {
            _cube.RotateClockwise(Layer.Back);
            _cube.RotateClockwise(rightSide);
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateAntiClockwise(leftSide);

            _cube.RotateClockwise(Layer.Back);
            _cube.RotateAntiClockwise(rightSide);
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateClockwise(leftSide);
        }

        private static bool CornerBlockInWrongPosition(KeyValuePair<(int, int), Block> cornerBlock)
        {
            var ((x, y), block) = cornerBlock;
            switch (x, y)
            {
                case (0, 0):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Right, block.Top};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Orange) || !blockFaces.Contains(Colour.Green);
                }
                case (0, 2):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Left, block.Top};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Red) || !blockFaces.Contains(Colour.Green);
                }
                case (2, 0):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Right, block.Bottom};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Orange) || !blockFaces.Contains(Colour.Blue);
                }
                case (2, 2):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Left, block.Bottom};
                    return !blockFaces.Contains(Colour.Yellow) || !blockFaces.Contains(Colour.Red) || !blockFaces.Contains(Colour.Blue);
                }
                default:
                    throw new Exception($"Cannot determine if corner block is an incorrect position as it is not a corner block: ({x}, {y})");
            }
        }

        public void CorrectCornerOrientation()
        {
            if (_cube.IsSolved())
            {
                return;
            }
            
            var initialFace = _cube.GetFace(Layer.Back);
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
                throw new Exception("Something is wrong with the Rubix Cube layout - it should not be possible to have only one incorrect corner block.");
            }
            var ((x, y), _) = incorrectCornerBlocks.First();

            while (!_cube.IsSolved())
            {

                var currentFace = _cube.GetFace(Layer.Back);
                if (currentFace[x, y].Back == Colour.Yellow)
                {
                    _cube.RotateClockwise(Layer.Back);
                }
                else
                {
                    var sideToRotate = (x, y) switch
                    {
                        (0, 0) => Layer.Right, 
                        (0, 2) => Layer.Top, 
                        (2, 0) => Layer.Bottom, 
                        (2, 2) => Layer.Left, 
                        _ => throw new Exception($"Can't correct corner orientation in the third layer as it is not a corner piece: ({x}, {y})")
                    };
                    
                    _cube.RotateAntiClockwise(sideToRotate);
                    _cube.RotateAntiClockwise(Layer.Front);
                    _cube.RotateClockwise(sideToRotate);
                    _cube.RotateClockwise(Layer.Front);
                }
            }
        }
    }
}