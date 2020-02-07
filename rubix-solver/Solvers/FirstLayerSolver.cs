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
            if (BackEdgeIsSolvable(incorrectBackEdge))
            {
                _cube.RotateClockwise(incorrectBackEdge.SideTwo);
                _cube.RotateClockwise(incorrectBackEdge.SideTwo);
            }
            
            if (incorrectBackEdge.Block.Back == Colour.White)
            {
                SolveIncorrectBackEdgeWithWhiteOnBackFace(incorrectBackEdge);
            }
            else
            {
                SolveIncorrectBackEdgeWithWhiteFaceNotOnBackFace(incorrectBackEdge);
            }
        }

        private bool BackEdgeIsSolvable(BackEdge edge)
        {
            return edge.Block.Back == Colour.White
                   && edge.SideTwo == Side.Left && edge.Block.GetColour(edge.SideTwo) == Colour.Red
                   && edge.SideTwo == Side.Right && edge.Block.GetColour(edge.SideTwo) == Colour.Orange
                   && edge.SideTwo == Side.Top && edge.Block.GetColour(edge.SideTwo) == Colour.Green
                   && edge.SideTwo == Side.Bottom && edge.Block.GetColour(edge.SideTwo) == Colour.Blue;
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

        private void SolveIncorrectBackEdgeWithWhiteFaceNotOnBackFace(Edge incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.SideOne == Side.Back
                ? incorrectMiddleEdge.SideTwo
                : incorrectMiddleEdge.SideOne;

            if (nonBackLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Block.Back)
                {
                    case Colour.Red:
                        SolveIncorrectlyOrientatedBackEdge(nonBackLayer);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Side.Right)
            {
                switch (incorrectMiddleEdge.Block.Back)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    case Colour.Orange:
                        SolveIncorrectlyOrientatedBackEdge(nonBackLayer);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Side.Top)
            {
                switch (incorrectMiddleEdge.Block.Back)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Green:
                        SolveIncorrectlyOrientatedBackEdge(nonBackLayer);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Side.Bottom)
            {
                switch (incorrectMiddleEdge.Block.Back)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        SolveIncorrectlyOrientatedBackEdge(nonBackLayer);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private void SolveIncorrectlyOrientatedBackEdge(Side targetSide)
        {
            var sideToRotate = targetSide switch
            {
                Side.Left => Side.Bottom,
                Side.Right => Side.Top,
                Side.Top => Side.Left,
                Side.Bottom => Side.Right,
                _ => throw new ArgumentException($"This is not a valid side for a back edge piece: {targetSide}")
            };

            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(sideToRotate);
            _cube.RotateAntiClockwise(targetSide);
            _cube.RotateAntiClockwise(sideToRotate);
        }

        private void SolveIncorrectBackEdgeWithWhiteOnBackFace(Edge incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.SideOne == Side.Back
                ? incorrectMiddleEdge.SideTwo
                : incorrectMiddleEdge.SideOne;

            if (nonBackLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Block.Left)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Side.Right)
            {
                switch (incorrectMiddleEdge.Block.Right)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Side.Top)
            {
                switch (incorrectMiddleEdge.Block.Top)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Side.Bottom)
            {
                switch (incorrectMiddleEdge.Block.Bottom)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
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
