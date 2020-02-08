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
                var incorrectFrontEdges = _cube.GetFrontEdgeBlocks().Where(e => !e.IsCorrectlyPositioned() && e.Block.HasColour(Colour.White)).ToList();
                if (incorrectFrontEdges.Any())
                {
                    var incorrectFrontEdge = incorrectFrontEdges.First();
                    SolveIncorrectFrontEdge(incorrectFrontEdge);
                }

                var incorrectBackEdges = _cube.GetBackEdgeBlocks().Where(e => e.Block.HasColour(Colour.White)).ToList();
                if (incorrectBackEdges.Any())
                {
                    var incorrectBackEdge = incorrectBackEdges.First();
                    SolveIncorrectBackEdge(incorrectBackEdge);
                }

                var incorrectSideEdges = _cube.GetSideEdgeBlocks().Where(e => e.Block.HasColour(Colour.White)).ToList();
                if (incorrectSideEdges.Any())
                {
                    var incorrectSideEdge = incorrectSideEdges.First();
                    SolveIncorrectMiddleEdge(incorrectSideEdge);
                }
            }
        }

        private void SolveIncorrectBackEdge(BackEdge incorrectBackEdge)
        {
            var block = incorrectBackEdge.Block;
            var nonWhiteColour = block.GetColour(incorrectBackEdge.SideTwo) ?? throw new Exception();

            while (!RubixCubeStatusEvaluator.IsCorrectColour(block.GetLayer(nonWhiteColour) ?? throw new Exception(),
                block))
            {
                if (block.Back == Colour.White)
                {
                    _cube.RotateClockwise(Side.Back);
                    block = _cube.GetBlock(incorrectBackEdge.Block);
                }
                else
                {
                    ReOrientateBackEdge(block.GetLayer(nonWhiteColour) ?? throw new Exception());
                    block = _cube.GetBlock(incorrectBackEdge.Block);
                }
            }

            _cube.RotateClockwise(block.GetLayer(nonWhiteColour) ?? throw new Exception());
            _cube.RotateClockwise(block.GetLayer(nonWhiteColour) ?? throw new Exception());

        }
        
        private void SolveIncorrectFrontEdge(FrontEdge incorrectFrontEdge)
        {
            _cube.RotateClockwise(incorrectFrontEdge.SideTwo);
            _cube.RotateClockwise(incorrectFrontEdge.SideTwo);
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
            _cube.RotateClockwise(Side.Back);
            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(sideToRotate);
        }

        private void SolveIncorrectMiddleEdge(Edge incorrectSideEdge)
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
        }
    }
}
