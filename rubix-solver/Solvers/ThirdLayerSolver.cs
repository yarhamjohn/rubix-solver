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

        private void FormYellowCross()
        {
            while (!RubixCubeStatusEvaluator.CrossFaceIsFormed(_cube, Side.Back))
            {
                var crossBlocks = _cube.GetBackFaceCrossBlocks().ToList();

                if (CrossHasFewerThanThreeCorrectBlocks(crossBlocks))
                {
                    // Any rotation is fine here (providing the face matches the side)
                    PerformFruRufRotations(Side.Right, Side.Bottom);
                }

                if (CrossIsCurrentlyAnArrow(crossBlocks))
                {
                    var sides = GetIncorrectCrossBlockSides(crossBlocks);
                    var faceToRotate = GetFaceToRotate(sides);
                    var sideToRotate = GetSideToRotate(faceToRotate);
                    PerformFruRufRotations(faceToRotate, sideToRotate);
                }

                if (CrossIsCurrentlyALine(crossBlocks))
                {
                    var faceToRotate = GetIncorrectCrossBlockSides(crossBlocks).First();
                    var sideToRotate = GetSideToRotate(faceToRotate);
                    PerformFruRufRotations(faceToRotate, sideToRotate);
                }
            }
        }

        private Side GetSideToRotate(Side faceToRotate)
        {
            return faceToRotate switch
            {
                Side.Left => Side.Top,
                Side.Right => Side.Bottom,
                Side.Top => Side.Right,
                Side.Bottom => Side.Left,
                _ => throw new Exception("Not a valid face")
            };
        }

        private static Side GetFaceToRotate(List<Side> sides)
        {
            return sides.Contains(Side.Left) && sides.Contains(Side.Top)
                ? Side.Left
                : sides.Contains(Side.Left) && sides.Contains(Side.Bottom)
                    ? Side.Bottom
                    : sides.Contains(Side.Right) && sides.Contains(Side.Top)
                        ? Side.Top
                        : sides.Contains(Side.Right) && sides.Contains(Side.Bottom)
                            ? Side.Right
                            : throw new Exception($"This isn't right {string.Join(", ", sides)}");
        }

        private static List<Side> GetIncorrectCrossBlockSides(List<Block> crossBlocks)
        {
            return crossBlocks
                .Where(b => b.Back != Colour.Yellow)
                .Select(b => b.GetNonNullSides().Single(s => s != Side.Back))
                .ToList();
        }

        private bool CrossIsCurrentlyAnArrow(List<Block> crossBlocks)
        {
            var blocks = GetIncorrectCrossBlockSides(crossBlocks);
            return blocks.Count == 2 && !CrossIsCurrentlyALine(crossBlocks);
        }

        private bool CrossIsCurrentlyALine(List<Block> crossBlocks)
        {
            var blocks = GetIncorrectCrossBlockSides(crossBlocks);
            return blocks.Count == 2 &&
                   blocks.Contains(Side.Left) && blocks.Contains(Side.Right) ||
                   blocks.Contains(Side.Top) && blocks.Contains(Side.Bottom);
        }

        private bool CrossHasFewerThanThreeCorrectBlocks(IEnumerable<Block> blocks)
        {
            // Orientation of the cube matters only once there are 3 blocks with yellow faces on the back in the cross
            return blocks.Count(b => b.Back == Colour.Yellow) < 3;
        }

        private void ReorganiseMiddleEdges()
        {
            while (!RubixCubeStatusEvaluator.CrossIsFormed(_cube, Side.Back))
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
                            (0, 1) => Side.Right,
                            (1, 2) => Side.Top,
                            (2, 1) => Side.Left,
                            (1, 0) => Side.Bottom,
                            _ => throw new Exception("The selected block is not a middle edge")
                        };

                        PerformRuRuRuuRuRotation(sideToRotate);
                    }
                    else if (startNode.Previous != null && !startNode.Previous.Value.isCorrect)
                    {
                        var sideToRotate = firstIncorrectEdge.index switch
                        {
                            (0, 1) => Side.Bottom,
                            (1, 2) => Side.Right,
                            (2, 1) => Side.Top,
                            (1, 0) => Side.Left,
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
                                PerformRuRuRuuRuRotation(Side.Bottom);
                                PerformRuRuRuuRuRotation(Side.Left);
                                PerformRuRuRuuRuRotation(Side.Bottom);
                                break;
                            case (1, 2):
                            case (1, 0):
                                PerformRuRuRuuRuRotation(Side.Right);
                                PerformRuRuRuuRuRotation(Side.Bottom);
                                PerformRuRuRuuRuRotation(Side.Right);
                                break;
                        }
                    }
                }
                else if (numIncorrectEdges == 3)
                {
                    var correctEdge = middleEdges.First(b => b.isCorrect);
                    var sideToRotate = correctEdge.index switch
                    {
                        (0, 1) => Side.Left,
                        (1, 2) => Side.Bottom,
                        (2, 1) => Side.Right,
                        (1, 0) => Side.Top,
                        _ => throw new Exception("The selected block is not a middle edge")
                    };

                    PerformRuRuRuuRuRotation(sideToRotate);
                }
                else
                {
                    _cube.RotateClockwise(Side.Back);
                }
            }
        }

        private void ReorganiseCorners()
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
                    PerformUrulRotations(Side.Top, Side.Bottom);
                }

                if (GetIncorrectlyPositionedCornerBlocks().Count == 3)
                {
                    var ((x, y), _) = GetCorrectlyPositionedCornerBlocks().Single();
                    switch (x, y)
                    {
                        case (0, 0):
                        {
                            PerformUrulRotations(Side.Right, Side.Left);
                            break;
                        }
                        case (0, 2):
                        {
                            PerformUrulRotations(Side.Top, Side.Bottom);
                            break;
                        }
                        case (2, 0):
                        {
                            PerformUrulRotations(Side.Bottom, Side.Top);
                            break;
                        }
                        case (2, 2):
                        {
                            PerformUrulRotations(Side.Left, Side.Right);
                            break;
                        }
                        default:
                            throw new Exception(
                                $"Cannot perform urul rotation as target is not a corner block: ({x}, {y})");
                    }
                }
            }
        }

        private void CorrectCornerOrientation()
        {
            if (RubixCubeStatusEvaluator.ThirdLayerIsSolved(_cube))
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

            while (!RubixCubeStatusEvaluator.ThirdLayerIsSolved(_cube))
            {
                var currentFace = _cube.GetFace(Side.Back);
                if (currentFace[x, y].Back == Colour.Yellow)
                {
                    _cube.RotateClockwise(Side.Back);
                }
                else
                {
                    var sideToRotate = (x, y) switch
                    {
                        (0, 0) => Side.Right,
                        (0, 2) => Side.Top,
                        (2, 0) => Side.Bottom,
                        (2, 2) => Side.Left,
                        _ => throw new Exception(
                            $"Can't correct corner orientation in the third layer as it is not a corner piece: ({x}, {y})")
                    };

                    PerformRdrdRotations(sideToRotate);
                }
            }
        }

        private void PerformFruRufRotations(Side face, Side side)
        {
            _cube.RotateClockwise(face);
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(Side.Back);

            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(face);
        }

        private void PerformRuRuRuuRuRotation(Side sideToRotate)
        {
            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Side.Back);

            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateClockwise(Side.Back);

            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(Side.Back);

            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateClockwise(Side.Back);
        }

        private void PerformUrulRotations(Side rightSide, Side leftSide)
        {
            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(rightSide);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(leftSide);

            _cube.RotateClockwise(Side.Back);
            _cube.RotateAntiClockwise(rightSide);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateClockwise(leftSide);
        }

        private void PerformRdrdRotations(Side sideToRotate)
        {
            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateAntiClockwise(Side.Front);
            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Side.Front);
        }

        private Dictionary<(int, int), Block> GetCornerBlocks()
        {
            var face = _cube.GetFace(Side.Back);
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
            var face = _cube.GetFace(Side.Back);

            var middleEdges = new LinkedList<(bool isCorrect, Block, (int, int) index)>();
            middleEdges.AddLast((face[0, 1].Top == Colour.Green, face[0, 1], (0, 1)));
            middleEdges.AddLast((face[1, 2].Left == Colour.Red, face[1, 2], (1, 2)));
            middleEdges.AddLast((face[2, 1].Bottom == Colour.Blue, face[2, 1], (2, 1)));
            middleEdges.AddLast((face[1, 0].Right == Colour.Orange, face[1, 0], (1, 0)));
            return middleEdges;
        }
    }
}
