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
                var incorrectBackEdges = _cube.GetBackEdgeBlocks().Where(e => e.Block.HasColour(Colour.White)).ToList();
                if (incorrectBackEdges.Any())
                {
                    var incorrectBackEdge = incorrectBackEdges.First();
                    SolveBackEdge(incorrectBackEdge.Block, incorrectBackEdge.SideTwo);
                }

                var incorrectFrontEdges = _cube.GetFrontEdgeBlocks()
                    .Where(e => !e.IsCorrectlyPositioned() && e.Block.HasColour(Colour.White)).ToList();
                if (incorrectFrontEdges.Any())
                {
                    var incorrectFrontEdge = incorrectFrontEdges.First();
                    RotateFrontEdgeToBack(incorrectFrontEdge.SideTwo);
                }

                var incorrectSideEdges = _cube.GetSideEdgeBlocks().Where(e => e.Block.HasColour(Colour.White)).ToList();
                if (incorrectSideEdges.Any())
                {
                    var incorrectSideEdge = incorrectSideEdges.First();
                    RotateSideEdgeToBack(incorrectSideEdge.Block, incorrectSideEdge.SideOne, incorrectSideEdge.SideTwo);
                }
            }
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

        private void RotateSideEdgeToBack(Block block, Side sideOne, Side sideTwo)
        {
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
