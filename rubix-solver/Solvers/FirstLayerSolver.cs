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
                var middleEdges = GetMiddleEdges();
                var incorrectMiddleEdge = middleEdges.First(IsIncorrectMiddleEdge);
                if (incorrectMiddleEdge.SideOne == Side.Front || incorrectMiddleEdge.SideTwo == Side.Front)
                {
                    if (incorrectMiddleEdge.Block.Front == Colour.White)
                    {
                        SolveIncorrectEdgeWithWhiteOnFrontFace(incorrectMiddleEdge);
                    }
                    else
                    {
                        SolveIncorrectEdgeWithWhiteNotOnFrontFace(incorrectMiddleEdge);
                    }
                }

                if (incorrectMiddleEdge.SideOne == Side.Back || incorrectMiddleEdge.SideTwo == Side.Back)
                {
                    if (incorrectMiddleEdge.Block.Back == Colour.White)
                    {
                        SolveIncorrectEdgeWithWhiteOnBackFace(incorrectMiddleEdge);
                    }
                    else
                    {
                        SolveIncorrectEdgeWithWhiteFaceNotOnBackFace(incorrectMiddleEdge);
                    }
                }

                var whiteFace = incorrectMiddleEdge.Block.GetLayer(Colour.White);
                if (whiteFace == null)
                {
                    throw new Exception("Should have a white face");
                }

                var nonWhiteFace = incorrectMiddleEdge.SideOne == whiteFace
                    ? incorrectMiddleEdge.SideTwo
                    : incorrectMiddleEdge.SideOne;
                if (whiteFace == Side.Left && nonWhiteFace == Side.Bottom)
                {
                    switch (incorrectMiddleEdge.Block.Bottom)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Left, {incorrectMiddleEdge.Block.Bottom.ToString()}");
                    }
                }
                if (whiteFace == Side.Left && nonWhiteFace == Side.Top)
                {
                    switch (incorrectMiddleEdge.Block.Top)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Left, {incorrectMiddleEdge.Block.Top.ToString()}");
                    }
                }
                if (whiteFace == Side.Right && nonWhiteFace == Side.Top)
                {
                    switch (incorrectMiddleEdge.Block.Top)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Right, {incorrectMiddleEdge.Block.Top.ToString()}");
                    }
                }
                if (whiteFace == Side.Right && nonWhiteFace == Side.Bottom)
                {
                    switch (incorrectMiddleEdge.Block.Bottom)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Right, {incorrectMiddleEdge.Block.Bottom.ToString()}");
                    }
                }
                if (whiteFace == Side.Top && nonWhiteFace == Side.Left)
                {
                    switch (incorrectMiddleEdge.Block.Left)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Top, {incorrectMiddleEdge.Block.Left.ToString()}");
                    }
                }
                if (whiteFace == Side.Top && nonWhiteFace == Side.Right)
                {
                    switch (incorrectMiddleEdge.Block.Right)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Top, {incorrectMiddleEdge.Block.Right.ToString()}");
                    }
                }
                if (whiteFace == Side.Bottom && nonWhiteFace == Side.Right)
                {
                    switch (incorrectMiddleEdge.Block.Right)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Bottom, {incorrectMiddleEdge.Block.Right.ToString()}");
                    }
                }
                if (whiteFace == Side.Bottom && nonWhiteFace == Side.Left)
                {
                    switch (incorrectMiddleEdge.Block.Left)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Bottom, {incorrectMiddleEdge.Block.Left.ToString()}");
                    }
                }
            }
        }

        private void SolveCorners()
        {
            while (!RubixCubeStatusEvaluator.FirstLayerIsSolved(_cube))
            {
                var frontCorners = _cube.GetFrontCornerBlocks().Where(c => !c.IsCorrectlyPositioned()).ToList();
                if (frontCorners.Any())
                {
                    var corner = frontCorners.First();
                    RotateCornerToBack(corner);
                }

                var backCorners = _cube.GetBackCornerBlocks().Where(c => c.IsCorrectlyPositioned()).ToList();
                if (backCorners.Any())
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

        private void SolveIncorrectEdgeWithWhiteFaceNotOnBackFace(Edge incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.SideOne == Side.Back
                ? incorrectMiddleEdge.SideTwo
                : incorrectMiddleEdge.SideOne;

            if (nonBackLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Block.Back)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Bottom);
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
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Top);
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
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Left);
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
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Right);
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

        private void SolveIncorrectEdgeWithWhiteOnBackFace(Edge incorrectMiddleEdge)
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

        private void SolveIncorrectEdgeWithWhiteNotOnFrontFace(Edge incorrectMiddleEdge)
        {
            var nonFrontLayer = incorrectMiddleEdge.SideOne == Side.Front
                ? incorrectMiddleEdge.SideTwo
                : incorrectMiddleEdge.SideOne;
            
            if (nonFrontLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Block.Front)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontLayer == Side.Top)
            {
                switch (incorrectMiddleEdge.Block.Front)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Left);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontLayer == Side.Right)
            {
                switch (incorrectMiddleEdge.Block.Front)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Top);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontLayer == Side.Bottom)
            {
                switch (incorrectMiddleEdge.Block.Front)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private void SolveIncorrectEdgeWithWhiteOnFrontFace(Edge incorrectMiddleEdge)
        {
            var nonFrontEdge = incorrectMiddleEdge.SideOne == Side.Front
                ? incorrectMiddleEdge.SideTwo
                : incorrectMiddleEdge.SideOne;

            _cube.RotateClockwise(nonFrontEdge);
            _cube.RotateClockwise(nonFrontEdge);

            if (nonFrontEdge == Side.Left)
            {
                switch (incorrectMiddleEdge.Block.Left)
                {
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontEdge == Side.Right)
            {
                switch (incorrectMiddleEdge.Block.Right)
                {
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
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

            if (nonFrontEdge == Side.Top)
            {
                switch (incorrectMiddleEdge.Block.Top)
                {
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Bottom);
                        _cube.RotateClockwise(Side.Bottom);
                        break;
                    case Colour.Red:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontEdge == Side.Bottom)
            {
                switch (incorrectMiddleEdge.Block.Bottom)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Left);
                        _cube.RotateClockwise(Side.Left);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateAntiClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Top);
                        _cube.RotateClockwise(Side.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Side.Back);
                        _cube.RotateClockwise(Side.Right);
                        _cube.RotateClockwise(Side.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private bool IsIncorrectMiddleEdge(Edge edge)
        {
            if (edge.Block.Front != Colour.White)
            {
                return true;
            }

            var nonTopEdge = edge.SideOne == Side.Front ? edge.SideTwo : edge.SideOne;
            switch (nonTopEdge)
            {
                case Side.Left:
                {
                    var face = _cube.GetFace(Side.Left);
                    return edge.Block.Left != Colour.Red || face[1, 1].Left != Colour.Red;
                }
                case Side.Right:
                {
                    var face = _cube.GetFace(Side.Right);
                    return edge.Block.Right != Colour.Orange || face[1, 1].Right != Colour.Orange;
                }
                case Side.Top:
                {
                    var face = _cube.GetFace(Side.Top);
                    return edge.Block.Top != Colour.Green || face[1, 1].Top != Colour.Green;
                }
                case Side.Bottom:
                {
                    var face = _cube.GetFace(Side.Bottom);
                    return edge.Block.Bottom != Colour.Blue || face[1, 1].Bottom != Colour.Blue;
                }
                case Side.Back:
                {
                    var face = _cube.GetFace(Side.Back);
                    return edge.Block.Back != Colour.Yellow || face[1, 1].Back != Colour.Yellow;
                }
                default:
                    throw new Exception($"This edge ({nonTopEdge}) is not in the top layer which it should be");
            }
        }

        private List<Edge> GetMiddleEdges()
        {
            var edges = new List<Edge>();
            var front = _cube.GetFace(Side.Front);
            edges.AddRange(new List<Edge>
            {
                new Edge(Side.Front, Side.Top, front[0, 1]),
                new Edge(Side.Front, Side.Left, front[1, 0]),
                new Edge(Side.Front, Side.Right, front[1, 2]),
                new Edge(Side.Front, Side.Bottom, front[2, 1])
            });

            var back = _cube.GetFace(Side.Back);
            edges.AddRange(new List<Edge>
            {
                new Edge(Side.Back, Side.Top, back[0, 1]),
                new Edge(Side.Back, Side.Right, back[1, 0]),
                new Edge(Side.Back, Side.Left, back[1, 2]),
                new Edge(Side.Back, Side.Bottom, back[2, 1])
            });
            
            var left = _cube.GetFace(Side.Left);
            edges.AddRange(new List<Edge>
            {
                new Edge(Side.Left, Side.Top, left[0, 1]),
                new Edge(Side.Left, Side.Bottom, left[2, 1])
            });
            
            var right = _cube.GetFace(Side.Right);
            edges.AddRange(new List<Edge>
            {
                new Edge(Side.Right, Side.Top, right[0, 1]),
                new Edge(Side.Right, Side.Bottom, right[2, 1])
            });

            return edges.Where(e => e.Block.HasColour(Colour.White)).ToList();
        }
    }

    public class Edge
    {
        public Side SideOne { get; }
        public Side SideTwo { get; }
        public Block Block { get; }

        public Edge(Side sideOne, Side sideTwo, Block block)
        {
            SideOne = sideOne;
            SideTwo = sideTwo;
            Block = block;
        }
    }
}
