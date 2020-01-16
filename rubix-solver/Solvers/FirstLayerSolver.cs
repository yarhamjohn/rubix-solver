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

        public void FormCross()
        {
            if (_cube.IsSolved())
            {
                return;
            }

            while (!CrossIsFormed())
            {
                Console.WriteLine("Cross not formed");
                _cube.PrintCube();
                var middleEdges = GetMiddleEdges();
                var incorrectMiddleEdge = middleEdges.First(IsIncorrectMiddleEdge);
                if (incorrectMiddleEdge.Item1.Item1 == Layer.Front || incorrectMiddleEdge.Item1.Item2 == Layer.Front)
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

                if (incorrectMiddleEdge.Item1.Item1 == Layer.Back || incorrectMiddleEdge.Item1.Item2 == Layer.Back)
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
                if (whiteFace == Layer.Left && nonWhiteFace == Layer.Bottom)
                {
                    switch (incorrectMiddleEdge.Item2.Bottom)
                    {
                        case Colour.Red:
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Bottom);
                            break;
                        case Colour.Orange:
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Bottom);
                            break;
                        case Colour.Green:
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Bottom);
                            break;
                        case Colour.Blue:
                            _cube.RotateClockwise(Layer.Bottom);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Left, {incorrectMiddleEdge.Item2.Bottom.ToString()}");
                    }
                }
                if (whiteFace == Layer.Left && nonWhiteFace == Layer.Right)
                {
                    switch (incorrectMiddleEdge.Item2.Right)
                    {
                        case Colour.Red:
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        case Colour.Orange:
                            _cube.RotateClockwise(Layer.Right);

                            break;
                        case Colour.Green:
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        case Colour.Blue:
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Left, {incorrectMiddleEdge.Item2.Right.ToString()}");
                    }
                }
                if (whiteFace == Layer.Right && nonWhiteFace == Layer.Top)
                {
                    switch (incorrectMiddleEdge.Item2.Top)
                    {
                        case Colour.Red:
                            _cube.RotateAntiClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Top);
                            break;
                        case Colour.Orange:
                            _cube.RotateAntiClockwise(Layer.Top);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Top);
                            break;
                        case Colour.Green:
                            _cube.RotateClockwise(Layer.Top);
                            break;
                        case Colour.Blue:
                            _cube.RotateAntiClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Top);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Right, {incorrectMiddleEdge.Item2.Top.ToString()}");
                    }
                }
                if (whiteFace == Layer.Right && nonWhiteFace == Layer.Bottom)
                {
                    switch (incorrectMiddleEdge.Item2.Bottom)
                    {
                        case Colour.Red:
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            break;
                        case Colour.Orange:
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            break;
                        case Colour.Green:
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            break;
                        case Colour.Blue:
                            _cube.RotateClockwise(Layer.Bottom);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Right, {incorrectMiddleEdge.Item2.Bottom.ToString()}");
                    }
                }
                if (whiteFace == Layer.Top && nonWhiteFace == Layer.Left)
                {
                    switch (incorrectMiddleEdge.Item2.Left)
                    {
                        case Colour.Red:
                            _cube.RotateClockwise(Layer.Left);
                            break;
                        case Colour.Orange:
                            _cube.RotateAntiClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Left);
                            break;
                        case Colour.Green:
                            _cube.RotateAntiClockwise(Layer.Left);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Left);
                            break;
                        case Colour.Blue:
                            _cube.RotateAntiClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Left);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Top, {incorrectMiddleEdge.Item2.Left.ToString()}");
                    }
                }
                if (whiteFace == Layer.Top && nonWhiteFace == Layer.Right)
                {
                    switch (incorrectMiddleEdge.Item2.Right)
                    {
                        case Colour.Red:
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        case Colour.Orange:
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        case Colour.Green:
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        case Colour.Blue:
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Top, {incorrectMiddleEdge.Item2.Right.ToString()}");
                    }
                }
                if (whiteFace == Layer.Bottom && nonWhiteFace == Layer.Right)
                {
                    switch (incorrectMiddleEdge.Item2.Right)
                    {
                        case Colour.Red:
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        case Colour.Orange:
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        case Colour.Green:
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        case Colour.Blue:                            
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Right);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Bottom, {incorrectMiddleEdge.Item2.Right.ToString()}");
                    }
                }
                if (whiteFace == Layer.Bottom && nonWhiteFace == Layer.Left)
                {
                    switch (incorrectMiddleEdge.Item2.Left)
                    {
                        case Colour.Red:
                            _cube.RotateAntiClockwise(Layer.Left);
                            break;
                        case Colour.Orange:
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateAntiClockwise(Layer.Left);
                            break;
                        case Colour.Green:
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateAntiClockwise(Layer.Left);
                            break;
                        case Colour.Blue:                            
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        default:
                            throw new Exception($"Incorrect edge piece: whiteFace = Bottom, {incorrectMiddleEdge.Item2.Left.ToString()}");
                    }
                }
            }
        }

        private void SolveIncorrectEdgeWithWhiteFaceNotOnBackFace(((Layer, Layer), Block) incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.Item1.Item1 == Layer.Back
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;

            if (nonBackLayer == Layer.Left)
            {
                switch (incorrectMiddleEdge.Item2.Back)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateAntiClockwise(Layer.Left);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateAntiClockwise(Layer.Top);
                        _cube.RotateAntiClockwise(Layer.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Layer.Right)
            {
                switch (incorrectMiddleEdge.Item2.Back)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        _cube.RotateAntiClockwise(Layer.Right);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateAntiClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Layer.Top)
            {
                switch (incorrectMiddleEdge.Item2.Back)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateAntiClockwise(Layer.Top);
                        _cube.RotateAntiClockwise(Layer.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateAntiClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Layer.Bottom)
            {
                switch (incorrectMiddleEdge.Item2.Back)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        _cube.RotateAntiClockwise(Layer.Right);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private void SolveIncorrectEdgeWithWhiteOnBackFace(((Layer, Layer), Block) incorrectMiddleEdge)
        {
            var nonBackLayer = incorrectMiddleEdge.Item1.Item1 == Layer.Back
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;

            if (nonBackLayer == Layer.Left)
            {
                switch (incorrectMiddleEdge.Item2.Left)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Layer.Right)
            {
                switch (incorrectMiddleEdge.Item2.Right)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Layer.Top)
            {
                switch (incorrectMiddleEdge.Item2.Top)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonBackLayer == Layer.Bottom)
            {
                switch (incorrectMiddleEdge.Item2.Bottom)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private void SolveIncorrectEdgeWithWhiteNotOnFrontFace(((Layer, Layer), Block) incorrectMiddleEdge)
        {
            var nonFrontLayer = incorrectMiddleEdge.Item1.Item1 == Layer.Front
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;
            
            if (nonFrontLayer == Layer.Left)
            {
                switch (incorrectMiddleEdge.Item2.Front)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontLayer == Layer.Top)
            {
                switch (incorrectMiddleEdge.Item2.Front)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Left);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontLayer == Layer.Right)
            {
                switch (incorrectMiddleEdge.Item2.Front)
                {
                    case Colour.Red:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Top);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontLayer == Layer.Bottom)
            {
                switch (incorrectMiddleEdge.Item2.Front)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Left);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateAntiClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(nonFrontLayer);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private void SolveIncorrectEdgeWithWhiteOnFrontFace(((Layer, Layer), Block) incorrectMiddleEdge)
        {
            var nonFrontEdge = incorrectMiddleEdge.Item1.Item1 == Layer.Front
                ? incorrectMiddleEdge.Item1.Item2
                : incorrectMiddleEdge.Item1.Item1;

            _cube.RotateClockwise(nonFrontEdge);
            _cube.RotateClockwise(nonFrontEdge);

            if (nonFrontEdge == Layer.Left)
            {
                switch (incorrectMiddleEdge.Item2.Left)
                {
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontEdge == Layer.Right)
            {
                switch (incorrectMiddleEdge.Item2.Right)
                {
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Green:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontEdge == Layer.Top)
            {
                switch (incorrectMiddleEdge.Item2.Top)
                {
                    case Colour.Orange:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    case Colour.Blue:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Bottom);
                        _cube.RotateClockwise(Layer.Bottom);
                        break;
                    case Colour.Red:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }

            if (nonFrontEdge == Layer.Bottom)
            {
                switch (incorrectMiddleEdge.Item2.Bottom)
                {
                    case Colour.Red:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Left);
                        _cube.RotateClockwise(Layer.Left);
                        break;
                    case Colour.Green:
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateAntiClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Top);
                        _cube.RotateClockwise(Layer.Top);
                        break;
                    case Colour.Orange:
                        _cube.RotateClockwise(Layer.Back);
                        _cube.RotateClockwise(Layer.Right);
                        _cube.RotateClockwise(Layer.Right);
                        break;
                    default:
                        throw new Exception("Not a valid edge piece");
                }
            }
        }

        private bool IsIncorrectMiddleEdge(((Layer, Layer), Block) edge)
        {
            if (edge.Item2.Front != Colour.White)
            {
                return true;
            }

            var nonTopEdge = edge.Item1.Item1 == Layer.Front ? edge.Item1.Item2 : edge.Item1.Item1;
            switch (nonTopEdge)
            {
                case Layer.Left:
                {
                    var face = _cube.GetFace(Layer.Left);
                    return edge.Item2.Left != Colour.Red || face[1, 1].Left != Colour.Red;
                }
                case Layer.Right:
                {
                    var face = _cube.GetFace(Layer.Right);
                    return edge.Item2.Right != Colour.Orange || face[1, 1].Right != Colour.Orange;
                }
                case Layer.Top:
                {
                    var face = _cube.GetFace(Layer.Top);
                    return edge.Item2.Top != Colour.Green || face[1, 1].Top != Colour.Green;
                }
                case Layer.Bottom:
                {
                    var face = _cube.GetFace(Layer.Bottom);
                    return edge.Item2.Bottom != Colour.Blue || face[1, 1].Bottom != Colour.Blue;
                }
                case Layer.Back:
                {
                    var face = _cube.GetFace(Layer.Back);
                    return edge.Item2.Back != Colour.Yellow || face[1, 1].Back != Colour.Yellow;
                }
                default:
                    throw new Exception($"This edge ({nonTopEdge}) is not in the top layer which it should be");
            }
        }

        private List<((Layer, Layer), Block)> GetMiddleEdges()
        {
            var edges = new List<((Layer, Layer), Block)>();
            var front = _cube.GetFace(Layer.Front);
            edges.AddRange(new List<((Layer, Layer), Block)>
            {
                ((Layer.Front, Layer.Top), front[0, 1]), 
                ((Layer.Front, Layer.Left), front[1, 0]), 
                ((Layer.Front, Layer.Right), front[1, 2]), 
                ((Layer.Front, Layer.Bottom), front[2, 1])
            });

            var back = _cube.GetFace(Layer.Back);
            edges.AddRange(new List<((Layer, Layer), Block)>
            {
                ((Layer.Back, Layer.Top), back[0, 1]), 
                ((Layer.Back, Layer.Right), back[1, 0]), 
                ((Layer.Back, Layer.Left), back[1, 2]), 
                ((Layer.Back, Layer.Bottom), back[2, 1])
            });
            
            var left = _cube.GetFace(Layer.Left);
            edges.AddRange(new List<((Layer, Layer), Block)>
            {
                ((Layer.Left, Layer.Top), left[0, 1]), 
                ((Layer.Left, Layer.Bottom), left[2, 1])
            });
            
            var right = _cube.GetFace(Layer.Right);
            edges.AddRange(new List<((Layer, Layer), Block)>
            {
                ((Layer.Right, Layer.Top), right[0, 1]), 
                ((Layer.Right, Layer.Bottom), right[2, 1])
            });

            return edges.Where(e => e.Item2.HasColour(Colour.White)).ToList();
        }

        private bool CrossIsFormed()
        {
            var face = _cube.GetFace(Layer.Front);
            return face[0, 1].Front == Colour.White &&
                   face[1, 0].Front == Colour.White &&
                   face[1, 2].Front == Colour.White &&
                   face[2, 1].Front == Colour.White &&
                   face[0, 1].Top == Colour.Green &&
                   face[1, 0].Left == Colour.Red &&
                   face[1, 2].Right == Colour.Orange &&
                   face[2, 1].Bottom == Colour.Blue && 
                   _cube.CenterBlocksAreCorrect();
        }

        public void SolveCorners()
        {
            if (_cube.IsSolved())
            {
                return;
            }

            while (!_cube.FirstLayerIsSolved())
            {
                Console.WriteLine("first layer not solved");
                _cube.PrintCube();
                if (GetCorners(Layer.Back).Any(c => c.Item2.HasColour(Colour.White)))
                {
                    Console.WriteLine("corner not in correct corner on bottom");
                    var corner = GetCorners(Layer.Back).First(c => c.Item2.HasColour(Colour.White));
                    if (!IsBetweenCorrectBackSides(corner))
                    {
                        corner = RotateToCorrectCorner(corner);
                    }
                    
                    RotateUpToFace(corner);
                }
                else
                {
                    var corner = GetCorners(Layer.Front).First(c => !IsCorrectlyPositioned(c));
                    Console.WriteLine("corner not correctly positioned on front");
                    switch (corner.Item1) { //TODO; this isn't right. Should rotate to the back and then round and back up. Right now it just rotates at the top
                        case (0, 0):
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Top);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Top);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Top);
                            break;
                        case (0, 2):
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Right);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Right);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Right);
                            break;
                        case (2, 0):
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Left);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Left);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Left);
                            break;
                        case (2, 2):
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            _cube.RotateAntiClockwise(Layer.Back);
                            _cube.RotateClockwise(Layer.Bottom);
                            _cube.RotateClockwise(Layer.Back);
                            _cube.RotateAntiClockwise(Layer.Bottom);
                            break;
                        default:
                            throw new Exception("Not a corner...");
                    }
                }
            }
        }

        private ((int x, int y), Block) RotateToCorrectCorner(((int x, int y), Block) corner)
        {
            if (corner.Item1 == (0, 0)) // Green and Orange
            {
                if (corner.Item2.HasColour(Colour.Blue) && corner.Item2.HasColour(Colour.Orange))
                {
                    _cube.RotateAntiClockwise(Layer.Back);
                    corner = ((2, 0), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Blue) && corner.Item2.HasColour(Colour.Red))
                {
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((2, 2), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Green) && corner.Item2.HasColour(Colour.Red))
                {
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((0, 2), corner.Item2);
                }
            }

            if (corner.Item1 == (0, 2)) // Green and Red
            {
                if (corner.Item2.HasColour(Colour.Blue) && corner.Item2.HasColour(Colour.Red))
                {
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((2, 2), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Green) && corner.Item2.HasColour(Colour.Orange))
                {
                    _cube.RotateAntiClockwise(Layer.Back);
                    corner = ((0, 0), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Blue) && corner.Item2.HasColour(Colour.Orange))
                {
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((2, 0), corner.Item2);
                }
            }

            if (corner.Item1 == (2, 0)) // Blue and Orange
            {
                if (corner.Item2.HasColour(Colour.Blue) && corner.Item2.HasColour(Colour.Red))
                {
                    _cube.RotateAntiClockwise(Layer.Back);
                    corner = ((2, 2), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Green) && corner.Item2.HasColour(Colour.Orange))
                {
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((0, 0), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Green) && corner.Item2.HasColour(Colour.Red))
                {
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((0, 2), corner.Item2);
                }
            }

            if (corner.Item1 == (2, 2)) // Blue and Red
            {
                if (corner.Item2.HasColour(Colour.Green) && corner.Item2.HasColour(Colour.Orange))
                {
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((0, 0), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Green) && corner.Item2.HasColour(Colour.Red))
                {
                    _cube.RotateAntiClockwise(Layer.Back);
                    corner = ((0, 2), corner.Item2);
                }

                if (corner.Item2.HasColour(Colour.Blue) && corner.Item2.HasColour(Colour.Orange))
                {
                    _cube.RotateClockwise(Layer.Back);
                    corner = ((2, 0), corner.Item2);
                }
            }

            return corner;
        }

        private void RotateUpToFace(((int x, int y), Block) corner)
        {
            if (corner.Item1 == (0, 0))
            {
                if (corner.Item2.Top == Colour.White)
                {
                    _cube.RotateAntiClockwise(Layer.Top);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Top);
                }
                else if (corner.Item2.Right == Colour.White)
                {
                    _cube.RotateClockwise(Layer.Right);
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Right);
                }
                else
                {
                    _cube.RotateAntiClockwise(Layer.Top);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Top);
                    _cube.RotateClockwise(Layer.Right);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Right);
                    _cube.RotateAntiClockwise(Layer.Top);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Top);
                }
            }

            if (corner.Item1 == (0, 2))
            {
                if (corner.Item2.Left == Colour.White)
                {
                    _cube.RotateAntiClockwise(Layer.Left);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Left);
                }
                else if (corner.Item2.Top == Colour.White)
                {
                    _cube.RotateClockwise(Layer.Top);
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Top);
                }
                else
                {
                    _cube.RotateAntiClockwise(Layer.Left);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Left);
                    _cube.RotateClockwise(Layer.Top);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Top);
                    _cube.RotateAntiClockwise(Layer.Left);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Left);
                }
            }

            if (corner.Item1 == (2, 0))
            {
                if (corner.Item2.Right == Colour.White)
                {
                    _cube.RotateAntiClockwise(Layer.Right);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Right);
                }
                else if (corner.Item2.Bottom == Colour.White)
                {
                    _cube.RotateClockwise(Layer.Bottom);
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Bottom);
                }
                else
                {
                    _cube.RotateAntiClockwise(Layer.Right);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Right);
                    _cube.RotateClockwise(Layer.Bottom);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Bottom);
                    _cube.RotateAntiClockwise(Layer.Right);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Right);
                }
            }

            if (corner.Item1 == (2, 2))
            {
                if (corner.Item2.Bottom == Colour.White)
                {
                    _cube.RotateAntiClockwise(Layer.Bottom);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Bottom);
                }
                else if (corner.Item2.Left == Colour.White)
                {
                    _cube.RotateClockwise(Layer.Left);
                    _cube.RotateClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Left);
                }
                else
                {
                    _cube.RotateAntiClockwise(Layer.Bottom);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Bottom);
                    _cube.RotateClockwise(Layer.Left);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateAntiClockwise(Layer.Left);
                    _cube.RotateAntiClockwise(Layer.Bottom);
                    _cube.RotateAntiClockwise(Layer.Back);
                    _cube.RotateClockwise(Layer.Bottom);
                }
            }
        }

        private bool IsCorrectlyPositioned(((int x, int y), Block block) corner)
        {
            return IsBetweenCorrectFrontSides(corner) && corner.block.Front == Colour.White;
        }

        private bool IsBetweenCorrectBackSides(((int x, int y), Block block) corner)
        {
            return corner.Item1 switch
            {
                (0, 0) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Orange),
                (0, 2) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Red),
                (2, 0) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Orange),
                (2, 2) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Red),
                _ => throw new Exception("This isn't a corner block...")
            };
        }
        
        private bool IsBetweenCorrectFrontSides(((int x, int y), Block block) corner)
        {
            return corner.Item1 switch
            {
                (0, 0) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Red),
                (0, 2) => corner.block.HasColour(Colour.Green) && corner.block.HasColour(Colour.Orange),
                (2, 0) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Red),
                (2, 2) => corner.block.HasColour(Colour.Blue) && corner.block.HasColour(Colour.Orange),
                _ => throw new Exception("This isn't a corner block...")
            };
        }

        private List<((int x, int y), Block)> GetCorners(Layer layer)
        {            
            var face = _cube.GetFace(layer);
            return new List<((int x, int y), Block)>
            {
                ((0, 0), face[0, 0]), 
                ((0, 2), face[0, 2]), 
                ((2, 0), face[2, 0]), 
                ((2, 2), face[2, 2])
            };
        }
    }
}