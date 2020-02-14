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
                SolveBackEdges();
                SolveFrontEdges();
                SolveSideEdges();
            }
        }

        private void SolveSideEdges()
        {
            while (_cube.GetSideEdgeBlocks().Any(e => e.Block.HasColour(Colour.White)))
            {
                var incorrectSideEdge = _cube.GetSideEdgeBlocks().First(e => e.Block.HasColour(Colour.White));
                SolveSideEdge(incorrectSideEdge);
            }
        }

        private void SolveFrontEdges()
        {
            while (_cube.GetFrontEdgeBlocks().Any(e => !e.IsCorrectlyPositioned() && e.Block.HasColour(Colour.White)))
            {
                var incorrectFrontEdge = _cube.GetFrontEdgeBlocks().First(e => !e.IsCorrectlyPositioned() && e.Block.HasColour(Colour.White));
                SolveFrontEdge(incorrectFrontEdge.SideTwo);
            }
        }

        private void SolveBackEdges()
        {
            while (_cube.GetBackEdgeBlocks().Any(e => e.Block.HasColour(Colour.White)))
            {
                var incorrectBackEdge = _cube.GetBackEdgeBlocks().First(e => e.Block.HasColour(Colour.White));
                SolveBackEdge(incorrectBackEdge.Block, incorrectBackEdge.SideTwo);
            }
        }

        private void SolveFrontEdge(Side side)
        {
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(side);

            SolveBackEdges();
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

        private void SolveSideEdge(Edge incorrectSideEdge)
        {
            var whiteFace = incorrectSideEdge.Block.GetLayer(Colour.White);
            if (whiteFace == null)
            {
                throw new Exception("Should have a white face");
            }

            var nonWhiteFace = incorrectSideEdge.SideOne == whiteFace
                ? incorrectSideEdge.SideTwo
                : incorrectSideEdge.SideOne;
            if (whiteFace == Side.Left && nonWhiteFace == Side.Bottom)
            {
                switch (incorrectSideEdge.Block.Bottom)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Left, {incorrectSideEdge.Block.Bottom.ToString()}");
                }
            }

            if (whiteFace == Side.Left && nonWhiteFace == Side.Top)
            {
                switch (incorrectSideEdge.Block.Top)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Top);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Top);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Left, {incorrectSideEdge.Block.Top.ToString()}");
                }
            }

            if (whiteFace == Side.Right && nonWhiteFace == Side.Top)
            {
                switch (incorrectSideEdge.Block.Top)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Right, {incorrectSideEdge.Block.Top.ToString()}");
                }
            }

            if (whiteFace == Side.Right && nonWhiteFace == Side.Bottom)
            {
                switch (incorrectSideEdge.Block.Bottom)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Right, {incorrectSideEdge.Block.Bottom.ToString()}");
                }
            }

            if (whiteFace == Side.Top && nonWhiteFace == Side.Left)
            {
                switch (incorrectSideEdge.Block.Left)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Top, {incorrectSideEdge.Block.Left.ToString()}");
                }
            }

            if (whiteFace == Side.Top && nonWhiteFace == Side.Right)
            {
                switch (incorrectSideEdge.Block.Right)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Top, {incorrectSideEdge.Block.Right.ToString()}");
                }
            }

            if (whiteFace == Side.Bottom && nonWhiteFace == Side.Right)
            {
                switch (incorrectSideEdge.Block.Right)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Bottom, {incorrectSideEdge.Block.Right.ToString()}");
                }
            }

            if (whiteFace == Side.Bottom && nonWhiteFace == Side.Left)
            {
                switch (incorrectSideEdge.Block.Left)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Left);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception(
                            $"Incorrect edge piece: whiteFace = Bottom, {incorrectSideEdge.Block.Left.ToString()}");
                }
            }


            var incorrectBackEdges = _cube.GetBackEdgeBlocks().Where(e => e.Block.HasColour(Colour.White)).ToList();
            if (incorrectBackEdges.Any())
            {
                var incorrectBackEdge = incorrectBackEdges.First();
                SolveBackEdge(incorrectBackEdge.Block, incorrectBackEdge.SideTwo);
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
