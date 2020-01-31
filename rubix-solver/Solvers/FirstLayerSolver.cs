﻿using System;
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
                if (incorrectMiddleEdge.Item1.Item1 == Side.Front || incorrectMiddleEdge.Item1.Item2 == Side.Front)
                {
                    if (incorrectMiddleEdge.Item2.Front == Colour.White)
                    {
                        SolveIncorrectEdgeWithWhiteOnFrontFace(incorrectMiddleEdge);
                    }
                    else
                    {
                        SolveIncorrectEdgeWithWhiteNotOnFrontFace(incorrectMiddleEdge);
                    }
                }

                if (incorrectMiddleEdge.Item1.Item1 == Side.Back || incorrectMiddleEdge.Item1.Item2 == Side.Back)
                {
                    if (incorrectMiddleEdge.Item2.Back == Colour.White)
                    {
                        SolveIncorrectEdgeWithWhiteOnBackFace(incorrectMiddleEdge);
                    }
                    else
                    {
                        SolveIncorrectEdgeWithWhiteFaceNotOnBackFace(incorrectMiddleEdge);
                    }
                }

                var whiteFace = incorrectMiddleEdge.Item2.GetLayer(Colour.White);
                if (whiteFace == null)
                {
                    throw new Exception("Should have a white face");
                }

                var nonWhiteFace = incorrectMiddleEdge.Item1.Item1 == whiteFace
                    ? incorrectMiddleEdge.Item1.Item2
                    : incorrectMiddleEdge.Item1.Item1;
                if (whiteFace == Side.Left && nonWhiteFace == Side.Bottom)
                {
                    switch (incorrectMiddleEdge.Item2.Bottom)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Left, {incorrectMiddleEdge.Item2.Bottom.ToString()}");
                    }
                }
                if (whiteFace == Side.Left && nonWhiteFace == Side.Top)
                {
                    switch (incorrectMiddleEdge.Item2.Top)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Left, {incorrectMiddleEdge.Item2.Top.ToString()}");
                    }
                }
                if (whiteFace == Side.Right && nonWhiteFace == Side.Top)
                {
                    switch (incorrectMiddleEdge.Item2.Top)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Right, {incorrectMiddleEdge.Item2.Top.ToString()}");
                    }
                }
                if (whiteFace == Side.Right && nonWhiteFace == Side.Bottom)
                {
                    switch (incorrectMiddleEdge.Item2.Bottom)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Right, {incorrectMiddleEdge.Item2.Bottom.ToString()}");
                    }
                }
                if (whiteFace == Side.Top && nonWhiteFace == Side.Left)
                {
                    switch (incorrectMiddleEdge.Item2.Left)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Top, {incorrectMiddleEdge.Item2.Left.ToString()}");
                    }
                }
                if (whiteFace == Side.Top && nonWhiteFace == Side.Right)
                {
                    switch (incorrectMiddleEdge.Item2.Right)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Top, {incorrectMiddleEdge.Item2.Right.ToString()}");
                    }
                }
                if (whiteFace == Side.Bottom && nonWhiteFace == Side.Right)
                {
                    switch (incorrectMiddleEdge.Item2.Right)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Bottom, {incorrectMiddleEdge.Item2.Right.ToString()}");
                    }
                }
                if (whiteFace == Side.Bottom && nonWhiteFace == Side.Left)
                {
                    switch (incorrectMiddleEdge.Item2.Left)
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
                            throw new Exception($"Incorrect edge piece: whiteFace = Bottom, {incorrectMiddleEdge.Item2.Left.ToString()}");
                    }
                }
            }
        }

        private void SolveCorners()
        {
            while (!RubixCubeStatusEvaluator.FirstLayerIsSolved(_cube))
            {
                var frontCorners = _cube.GetCornerBlocks(Side.Front).Where(c => !c.IsCorrectlyPositioned()).ToList();
                if (frontCorners.Any())
                {
                    var corner = frontCorners.First();
                    RotateToBack(corner);
                }

                var backCorners = _cube.GetCornerBlocks(Side.Back).Where(c => c.IsCorrectlyPositioned()).ToList();
                if (backCorners.Any())
                {
                    var corner = backCorners.First();
                    RotateToFront(corner);
                }
                else
                {
                    _cube.RotateClockwise(Side.Back);
                }
            }
        }

        private void RotateToBack(Corner corner)
        {
            var sideToRotate = corner.Location switch
            {
                CornerLocation.TopLeft => Side.Top,
                CornerLocation.TopRight => Side.Right,
                CornerLocation.BottomLeft => Side.Left,
                CornerLocation.BottomRight => Side.Bottom,
                _ => throw new ArgumentException($"Invalid cornerLocation: {corner.Location}")
            };

            _cube.RotateClockwise(sideToRotate);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateAntiClockwise(sideToRotate);
        }

        private void RotateToFront(Corner corner)
        {
            var sideOne = corner.Location switch
            {
                CornerLocation.TopLeft => Side.Top,
                CornerLocation.TopRight => Side.Left,
                CornerLocation.BottomLeft => Side.Right,
                CornerLocation.BottomRight => Side.Bottom,
                _ => throw new Exception($"Not a valid location {corner.Location}")
            };

            var sideTwo = corner.Location switch
            {
                CornerLocation.TopLeft => Side.Right,
                CornerLocation.TopRight => Side.Top,
                CornerLocation.BottomLeft => Side.Bottom,
                CornerLocation.BottomRight => Side.Left,
                _ => throw new Exception($"Not a valid location {corner.Location}")
            };

            if (corner.Block.GetLayer(Colour.White) == sideOne)
            {
                _cube.RotateAntiClockwise(sideOne);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateClockwise(sideOne);
            }
            else if (corner.Block.GetLayer(Colour.White) == sideTwo)
            {
                _cube.RotateClockwise(sideTwo);
                _cube.RotateClockwise(Side.Back);
                _cube.RotateAntiClockwise(sideTwo);
            }
            else
            {
                _cube.RotateAntiClockwise(sideOne);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateClockwise(sideOne);
                _cube.RotateClockwise(sideTwo);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateAntiClockwise(sideTwo);
                _cube.RotateAntiClockwise(sideOne);
                _cube.RotateAntiClockwise(Side.Back);
                _cube.RotateClockwise(sideOne);
            }
        }

        private void RotateToFrontThree(Side sideOne, Side sideTwo)
        {
            _cube.RotateAntiClockwise(sideOne);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateClockwise(sideOne);
            _cube.RotateClockwise(sideTwo);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(sideTwo);
            _cube.RotateAntiClockwise(sideOne);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateClockwise(sideOne);
        }

        private void RotateToFrontTwo(Side side)
        {
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateAntiClockwise(side);
        }

        private void RotateToFrontOne(Side side)
        {
            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateClockwise(side);
        }

        private void SolveIncorrectEdgeWithWhiteFaceNotOnBackFace(((Side, Side), Block) incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.Item1.Item1 == Side.Back
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;

            if (nonBackLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Item2.Back)
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
                switch (incorrectMiddleEdge.Item2.Back)
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
                switch (incorrectMiddleEdge.Item2.Back)
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
                switch (incorrectMiddleEdge.Item2.Back)
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

        private void SolveIncorrectEdgeWithWhiteOnBackFace(((Side, Side), Block) incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.Item1.Item1 == Side.Back
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;

            if (nonBackLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Item2.Left)
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
                switch (incorrectMiddleEdge.Item2.Right)
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
                switch (incorrectMiddleEdge.Item2.Top)
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
                switch (incorrectMiddleEdge.Item2.Bottom)
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

        private void SolveIncorrectEdgeWithWhiteNotOnFrontFace(((Side, Side), Block) incorrectMiddleEdge)
        {
            var nonFrontLayer = incorrectMiddleEdge.Item1.Item1 == Side.Front
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;
            
            if (nonFrontLayer == Side.Left)
            {
                switch (incorrectMiddleEdge.Item2.Front)
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
                switch (incorrectMiddleEdge.Item2.Front)
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
                switch (incorrectMiddleEdge.Item2.Front)
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
                switch (incorrectMiddleEdge.Item2.Front)
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

        private void SolveIncorrectEdgeWithWhiteOnFrontFace(((Side, Side), Block) incorrectMiddleEdge)
        {
            var nonFrontEdge = incorrectMiddleEdge.Item1.Item1 == Side.Front
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;

            _cube.RotateClockwise(nonFrontEdge);
            _cube.RotateClockwise(nonFrontEdge);

            if (nonFrontEdge == Side.Left)
            {
                switch (incorrectMiddleEdge.Item2.Left)
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
                switch (incorrectMiddleEdge.Item2.Right)
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
                switch (incorrectMiddleEdge.Item2.Top)
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
                switch (incorrectMiddleEdge.Item2.Bottom)
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

        private bool IsIncorrectMiddleEdge(((Side, Side), Block) edge)
        {
            if (edge.Item2.Front != Colour.White)
            {
                return true;
            }

            var nonTopEdge = edge.Item1.Item1 == Side.Front ? edge.Item1.Item2 : edge.Item1.Item1;
            switch (nonTopEdge)
            {
                case Side.Left:
                {
                    var face = _cube.GetFace(Side.Left);
                    return edge.Item2.Left != Colour.Red || face[1, 1].Left != Colour.Red;
                }
                case Side.Right:
                {
                    var face = _cube.GetFace(Side.Right);
                    return edge.Item2.Right != Colour.Orange || face[1, 1].Right != Colour.Orange;
                }
                case Side.Top:
                {
                    var face = _cube.GetFace(Side.Top);
                    return edge.Item2.Top != Colour.Green || face[1, 1].Top != Colour.Green;
                }
                case Side.Bottom:
                {
                    var face = _cube.GetFace(Side.Bottom);
                    return edge.Item2.Bottom != Colour.Blue || face[1, 1].Bottom != Colour.Blue;
                }
                case Side.Back:
                {
                    var face = _cube.GetFace(Side.Back);
                    return edge.Item2.Back != Colour.Yellow || face[1, 1].Back != Colour.Yellow;
                }
                default:
                    throw new Exception($"This edge ({nonTopEdge}) is not in the top layer which it should be");
            }
        }

        private List<((Side, Side), Block)> GetMiddleEdges()
        {
            var edges = new List<((Side, Side), Block)>();
            var front = _cube.GetFace(Side.Front);
            edges.AddRange(new List<((Side, Side), Block)>
            {
                ((Side.Front, Side.Top), front[0, 1]),
                ((Side.Front, Side.Left), front[1, 0]),
                ((Side.Front, Side.Right), front[1, 2]),
                ((Side.Front, Side.Bottom), front[2, 1])
            });

            var back = _cube.GetFace(Side.Back);
            edges.AddRange(new List<((Side, Side), Block)>
            {
                ((Side.Back, Side.Top), back[0, 1]),
                ((Side.Back, Side.Right), back[1, 0]),
                ((Side.Back, Side.Left), back[1, 2]),
                ((Side.Back, Side.Bottom), back[2, 1])
            });
            
            var left = _cube.GetFace(Side.Left);
            edges.AddRange(new List<((Side, Side), Block)>
            {
                ((Side.Left, Side.Top), left[0, 1]),
                ((Side.Left, Side.Bottom), left[2, 1])
            });
            
            var right = _cube.GetFace(Side.Right);
            edges.AddRange(new List<((Side, Side), Block)>
            {
                ((Side.Right, Side.Top), right[0, 1]),
                ((Side.Right, Side.Bottom), right[2, 1])
            });

            return edges.Where(e => e.Item2.HasColour(Colour.White)).ToList();
        }
    }

    public enum CornerLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
