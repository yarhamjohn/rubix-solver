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

        public void Solve()
        {
            FormCross();
            SolveCorners();
        }

        private void FormCross()
        {
            while (!RubixCubeStatusEvaluator.CrossIsFormed(_cube, Side.Front))
            {
                var incorrectBackEdges = _cube.GetBackEdges().Where(b => b.HasColour(Colour.White)).ToList();
                if (incorrectBackEdges.Any())
                {
                    var block = incorrectBackEdges.First();
                    var nonBackSide = GetNonTargetSide(block, Side.Back);
                    SolveBackEdge(block, nonBackSide);
                }

                var incorrectFrontEdge = GetIncorrectFrontEdge();
                if (incorrectFrontEdge != null)
                {
                    var nonFrontSide = GetNonTargetSide(incorrectFrontEdge, Side.Front);
                    RotateFrontEdgeToBack(nonFrontSide);
                }

                var incorrectSideEdges = _cube.GetSideBlocks().Where(b => b.HasColour(Colour.White)).ToList();
                if (incorrectSideEdges.Any())
                {
                    var incorrectSideEdge = incorrectSideEdges.First();
                    RotateSideEdgeToBack(incorrectSideEdge);
                }
            }
        }

        private Block? GetIncorrectFrontEdge()
        {
            var blocks = _cube.GetFrontEdges().Where(b => b.HasColour(Colour.White));
            foreach (var block in blocks)
            {
                var nonFrontSide = GetNonTargetSide(block, Side.Front);
                if (block.Front != Colour.White || !RubixCubeStatusEvaluator.SideIsCorrectColour(nonFrontSide, block))
                {
                    return block;
                }
            }

            return null;
        }

        private static Side GetNonTargetSide(Block block, Side targetSide)
        {
            var nonNullSides = block.GetNonNullSides();
            if (nonNullSides.Count != 2)
            {
                throw new Exception("This block cannot be an edge as it doesn't have to coloured sides.");
            }

            return nonNullSides.Single(side => side != targetSide);
        }

        private void RotateFrontEdgeToBack(Side side)
        {
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(side);
        }

        private void SolveBackEdge(Block block, Side nonBackSide)
        {
            var nonWhiteColour = (block.Back == Colour.White ? block.GetColour(nonBackSide) : block.Back) ?? throw
                                     new ArgumentException("Edge blocks must have a white and a non-white side.");

            if (block.Back == nonWhiteColour)
            {
                ReOrientateBackEdge(nonBackSide);
                block = _cube.GetBlock(block);
            }

            while (!BackEdgeIsOnCorrectSide(block, nonWhiteColour))
            {
                _cube.RotateClockwise(Side.Back);
                block = _cube.GetBlock(block);
            }

            var layer = block.GetLayer(nonWhiteColour) ?? throw new Exception();
            _cube.RotateClockwise(layer);
            _cube.RotateClockwise(layer);
        }

        private static bool BackEdgeIsOnCorrectSide(Block block, Colour nonWhiteColour)
        {
            var layer = block.GetLayer(nonWhiteColour) ?? throw new ArgumentException("Cannot be null here");
            return RubixCubeStatusEvaluator.SideIsCorrectColour(layer, block);
        }

        private void ReOrientateBackEdge(Side side)
        {
            var sideToRotate = side switch
            {
                Side.Left => Side.Top,
                Side.Top => Side.Right,
                Side.Right => Side.Bottom,
                Side.Bottom => Side.Left,
                _ => throw new Exception("Not a valid side to rotate")
            };
                    
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(sideToRotate);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(sideToRotate);
            _cube.RotateAntiClockwise(side);
        }

        private void RotateSideEdgeToBack(Block block)
        {
            var sides = block.GetNonNullSides();
            if (sides.Count != 2)
            {
                throw new Exception("An edge block can only have two coloured sides");
            }

            var (sideOne, sideTwo) = (sides[0], sides[1]);
            var sideOneColour = block.GetColour(sideOne);
            var whiteFace = sideOneColour == Colour.White ? sideOne : sideTwo;
            var nonWhiteFace = sideOneColour == Colour.White ? sideTwo :sideOne;

            switch (whiteFace)
            {
                case Side.Left when nonWhiteFace == Side.Top:
                case Side.Right when nonWhiteFace == Side.Bottom:
                case Side.Top when nonWhiteFace == Side.Right:
                case Side.Bottom when nonWhiteFace == Side.Left:
                    _cube.RotateClockwise(nonWhiteFace);
                    _cube.RotateAntiClockwise(Side.Back);
                    _cube.RotateClockwise(nonWhiteFace);
                    break;
                case Side.Left when nonWhiteFace == Side.Bottom:
                case Side.Right when nonWhiteFace == Side.Top:
                case Side.Top when nonWhiteFace == Side.Left:
                case Side.Bottom when nonWhiteFace == Side.Right:
                    _cube.RotateAntiClockwise(nonWhiteFace);
                    _cube.RotateAntiClockwise(Side.Back);
                    _cube.RotateClockwise(nonWhiteFace);
                    break;
            }
        }

        private void SolveCorners()
        {
            while (!RubixCubeStatusEvaluator.FirstLayerIsSolved(_cube))
            {
                var frontCorners = _cube.GetFrontCornerBlocks().Where(c => !c.IsCorrectlyPositioned()).ToList();
                if (frontCorners.Select(c => c.Block.HasColour(Colour.White)).Any())
                {
                    var corner = frontCorners.First();
                    RotateCornerToBack(corner);
                }

                var backCorners = _cube.GetBackCornerBlocks().Where(c => c.IsCorrectlyPositioned()).ToList();
                if (backCorners.Select(c => c.Block.HasColour(Colour.White)).Any())
                {
                    var corner = backCorners.First();
                    RotateCornerToFront(corner);
                }
                else
                {
                    _cube.RotateClockwise(Side.Back);
                }
            }
        }

        private void RotateCornerToBack(FrontCorner corner)
        {
            _cube.RotateClockwise(corner.SideToRotate);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateAntiClockwise(corner.SideToRotate);
        }

        private void RotateCornerToFront(BackCorner corner)
        {
            if (corner.Block.GetLayer(Colour.White) == corner.SideOne)
            {
                _cube.RotateAntiClockwise(corner.SideOne);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateClockwise(corner.SideOne);
            }
            else if (corner.Block.GetLayer(Colour.White) == corner.SideTwo)
            {
                _cube.RotateClockwise(corner.SideTwo);
                _cube.RotateClockwise(Side.Back);
                _cube.RotateAntiClockwise(corner.SideTwo);
            }
            else
            {
                _cube.RotateAntiClockwise(corner.SideOne);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateClockwise(corner.SideOne);
                _cube.RotateClockwise(corner.SideTwo);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateAntiClockwise(corner.SideTwo);
                _cube.RotateAntiClockwise(corner.SideOne);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateClockwise(corner.SideOne);
            }
        }
    }
}
