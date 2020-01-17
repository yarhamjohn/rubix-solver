using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver.Solvers
{
    public class ThirdLayerSolver
    {
        private readonly RubixCube _cube;

        public ThirdLayerSolver(RubixCube cube)
        {
            _cube = cube;
        }

        public void Solve()
        {
            FormYellowCross();
            ReorganiseMiddleEdges();
            ReorganiseCorners();
            CorrectCornerOrientation();
        }

        public void FormYellowCross()
        {
            while (!YellowCrossShowing())
            {
                var face = _cube.GetFace(Layer.Back);
                var crossBlocks = new Dictionary<(int, int), Block>
                {
                    {(0, 1), face[0, 1]},
                    {(1, 1), face[1, 1]},
                    {(2, 1), face[2, 1]},
                    {(1, 0), face[1, 0]},
                    {(1, 2), face[1, 2]},
                };

                if (crossBlocks.Values.Count(f => f.Back == Colour.Yellow) < 3)
                {
                    // Any rotation is fine here (providing the face matches the side)
                    PerformFruRufRotations(Layer.Right, Layer.Bottom);
                }
                else
                {
                    if (face[0, 1].Back == Colour.Yellow && face[1, 2].Back == Colour.Yellow &&
                        face[1, 1].Back == Colour.Yellow)
                    {
                        PerformFruRufRotations(Layer.Right, Layer.Bottom);
                    }
                    else if (face[0, 1].Back == Colour.Yellow && face[1, 0].Back == Colour.Yellow &&
                             face[1, 1].Back == Colour.Yellow)
                    {
                        PerformFruRufRotations(Layer.Bottom, Layer.Left);
                    }
                    else if (face[1, 2].Back == Colour.Yellow && face[2,1].Back == Colour.Yellow &&
                             face[1, 1].Back == Colour.Yellow)
                    {
                        PerformFruRufRotations(Layer.Top, Layer.Right);
                    }
                    else if (face[1, 0].Back == Colour.Yellow && face[2,1].Back == Colour.Yellow &&
                             face[1, 1].Back == Colour.Yellow)
                    {
                        PerformFruRufRotations(Layer.Left, Layer.Top);
                    }
                    else if (face[0, 1].Back == Colour.Yellow &&
                             face[1, 1].Back == Colour.Yellow &&
                             face[2, 1].Back == Colour.Yellow)
                    {
                        PerformFruRufRotations(Layer.Right, Layer.Bottom);
                    }
                    else if (face[1, 0].Back == Colour.Yellow &&
                             face[1, 1].Back == Colour.Yellow &&
                             face[1, 2].Back == Colour.Yellow)
                    {
                        PerformFruRufRotations(Layer.Bottom, Layer.Left);
                    }
                }
            }
        }
        
        public void ReorganiseMiddleEdges()
        {
            while (!MiddleEdgesAreSolved())
            {
                var middleEdges = GetMiddleEdges();
                var numIncorrectEdges = middleEdges.Count(b => b.isCorrect);

                if (numIncorrectEdges == 2)
                {
                    var firstIncorrectEdge = middleEdges.First(b => !b.isCorrect);
                    var startNode = middleEdges.Find(firstIncorrectEdge);
                    if (startNode == null)
                    {
                        throw new Exception("There should be an incorrect edge");
                    }

                    if (startNode.Next != null && !startNode.Next.Value.isCorrect) // TODO: managed to get a NullReferenceException here...
                    {
                        var sideToRotate = firstIncorrectEdge.index switch
                        {
                            (0, 1) => Layer.Right,
                            (1, 2) => Layer.Top,
                            (2, 1) => Layer.Left,
                            (1, 0) => Layer.Bottom,
                            _ => throw new Exception("The selected block is not a middle edge")
                        };

                        PerformRuRuRuuRuRotation(sideToRotate);
                    }
                    else if (startNode.Previous != null && !startNode.Previous.Value.isCorrect)
                    {
                        var sideToRotate = firstIncorrectEdge.index switch
                        {
                            (0, 1) => Layer.Bottom,
                            (1, 2) => Layer.Right,
                            (2, 1) => Layer.Top,
                            (1, 0) => Layer.Left,
                            _ => throw new Exception("The selected block is not a middle edge")
                        };

                        PerformRuRuRuuRuRotation(sideToRotate);
                    }
                    else
                    {
                        switch (firstIncorrectEdge.index)
                        {
                            case (0, 1):
                            case (2, 1):
                                PerformRuRuRuuRuRotation(Layer.Bottom);
                                PerformRuRuRuuRuRotation(Layer.Left);
                                PerformRuRuRuuRuRotation(Layer.Bottom);
                                break;
                            case (1, 2):
                            case (1, 0):
                                PerformRuRuRuuRuRotation(Layer.Right);
                                PerformRuRuRuuRuRotation(Layer.Bottom);
                                PerformRuRuRuuRuRotation(Layer.Right);
                                break;
                        }
                    }
                }
                else if (numIncorrectEdges == 3)
                {
                    var correctEdge = middleEdges.First(b => b.isCorrect);
                    var sideToRotate = correctEdge.index switch
                    {
                        (0, 1) => Layer.Left,
                        (1, 2) => Layer.Bottom,
                        (2, 1) => Layer.Right,
                        (1, 0) => Layer.Top,
                        _ => throw new Exception("The selected block is not a middle edge")
                    };

                    PerformRuRuRuuRuRotation(sideToRotate);
                }
                else
                {
                    _cube.RotateClockwise(Layer.Back);
                }
            }
        }

        public void ReorganiseCorners()
        {
            switch (GetIncorrectlyPositionedCornerBlocks().Count)
            {
                case 1:
                case 2:
                    throw new Exception(
                        "Something has gone wrong. There can only be 0, 3 or 4 incorrect blocks at this stage.");
            }

            while (GetIncorrectlyPositionedCornerBlocks().Count > 0)
            {
                if (GetIncorrectlyPositionedCornerBlocks().Count == 4)
                {
                    PerformUrulRotations(Layer.Top, Layer.Bottom);
                }

                if (GetIncorrectlyPositionedCornerBlocks().Count == 3)
                {
                    var ((x, y), _) = GetCorrectlyPositionedCornerBlocks().Single();
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
                            throw new Exception(
                                $"Cannot perform urul rotation as target is not a corner block: ({x}, {y})");
                    }
                }
            }
        }

        public void CorrectCornerOrientation()
        {
            if (RubixCubeStatusEvaluator.IsSolved(_cube))
            {
                return;
            }

            var incorrectCornerBlocks = GetIncorrectlyOrientatedCornerBlocks();
            if (incorrectCornerBlocks.Count == 1)
            {
                throw new Exception(
                    "Something is wrong with the Rubix Cube layout - it should not be possible to have only one incorrect corner block.");
            }

            var ((x, y), _) = incorrectCornerBlocks.First();

            while (!RubixCubeStatusEvaluator.IsSolved(_cube))
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
                        _ => throw new Exception(
                            $"Can't correct corner orientation in the third layer as it is not a corner piece: ({x}, {y})")
                    };

                    PerformRdrdRotations(sideToRotate);
                }
            }
        }

        private void PerformFruRufRotations(Layer face, Layer side)
        {
            _cube.RotateClockwise(face);
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(Layer.Back);

            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateAntiClockwise(face);
        }

        private void PerformRuRuRuuRuRotation(Layer sideToRotate)
        {
            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Layer.Back);

            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateClockwise(Layer.Back);

            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Layer.Back);
            _cube.RotateClockwise(Layer.Back);

            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateClockwise(Layer.Back);
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

        private void PerformRdrdRotations(Layer sideToRotate)
        {
            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateAntiClockwise(Layer.Front);
            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Layer.Front);
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

        private List<KeyValuePair<(int, int), Block>> GetCorrectlyPositionedCornerBlocks()
        {
            return GetCornerBlocks().Where(CornerBlockIsCorrectlyPositioned).ToList();
        }

        private List<KeyValuePair<(int, int), Block>> GetIncorrectlyPositionedCornerBlocks()
        {
            return GetCornerBlocks().Where(b => !CornerBlockIsCorrectlyPositioned(b)).ToList();
        }

        private List<KeyValuePair<(int, int), Block>> GetIncorrectlyOrientatedCornerBlocks()
        {
            return GetCornerBlocks().Where(b => b.Value.Back != Colour.Yellow).ToList();
        }

        private static bool CornerBlockIsCorrectlyPositioned(KeyValuePair<(int, int), Block> cornerBlock)
        {
            var ((x, y), block) = cornerBlock;
            switch (x, y)
            {
                case (0, 0):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Right, block.Top};
                    return blockFaces.Contains(Colour.Yellow) && blockFaces.Contains(Colour.Orange) &&
                           blockFaces.Contains(Colour.Green);
                }
                case (0, 2):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Left, block.Top};
                    return blockFaces.Contains(Colour.Yellow) && blockFaces.Contains(Colour.Red) &&
                           blockFaces.Contains(Colour.Green);
                }
                case (2, 0):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Right, block.Bottom};
                    return blockFaces.Contains(Colour.Yellow) && blockFaces.Contains(Colour.Orange) &&
                           blockFaces.Contains(Colour.Blue);
                }
                case (2, 2):
                {
                    var blockFaces = new List<Colour?> {block.Back, block.Left, block.Bottom};
                    return blockFaces.Contains(Colour.Yellow) && blockFaces.Contains(Colour.Red) &&
                           blockFaces.Contains(Colour.Blue);
                }
                default:
                    throw new Exception(
                        $"Cannot determine if corner block is an incorrect position as it is not a corner block: ({x}, {y})");
            }
        }

        private LinkedList<(bool isCorrect, Block, (int, int) index)> GetMiddleEdges()
        {
            var face = _cube.GetFace(Layer.Back);

            var middleEdges = new LinkedList<(bool isCorrect, Block, (int, int) index)>();
            middleEdges.AddLast((face[0, 1].Top == Colour.Green, face[0, 1], (0, 1)));
            middleEdges.AddLast((face[1, 2].Left == Colour.Red, face[1, 2], (1, 2)));
            middleEdges.AddLast((face[2, 1].Bottom == Colour.Blue, face[2, 1], (2, 1)));
            middleEdges.AddLast((face[1, 0].Right == Colour.Orange, face[1, 0], (1, 0)));
            return middleEdges;
        }

        private bool MiddleEdgesAreSolved()
        {
            var face = _cube.GetFace(Layer.Back);
            return YellowCrossShowing() &&
                   face[0, 1].Top == Colour.Green &&
                   face[1, 2].Left == Colour.Red &&
                   face[2, 1].Bottom == Colour.Blue &&
                   face[1, 0].Right == Colour.Orange;
        }

        private bool YellowCrossShowing()
        {
            var face = _cube.GetFace(Layer.Back);
            return face[0, 1].Back == Colour.Yellow &&
                   face[1, 2].Back == Colour.Yellow &&
                   face[2, 1].Back == Colour.Yellow &&
                   face[1, 0].Back == Colour.Yellow && 
                   face[1, 1].Back == Colour.Yellow;
        }
    }
}
