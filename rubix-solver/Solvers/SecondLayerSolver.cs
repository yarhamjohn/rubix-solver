using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver.Solvers
{
    public class SecondLayerSolver
    {
        private readonly RubixCube _cube;

        public SecondLayerSolver(RubixCube cube)
        {
            _cube = cube;
        }
        
        public void Solve()
        {
            while (!RubixCubeStatusEvaluator.SecondLayerIsSolved(_cube))
            {
                var backEdges = _cube.GetBackEdges();
                if (backEdges.All(b => b.HasColour(Colour.Yellow)))
                {
                    RotateIncorrectSideEdgeToBack();
                }

                while (!BackHasSolvableEdge())
                {
                    _cube.RotateClockwise(Side.Back);
                }

                SolveBackEdge();
            }
        }

        private void SolveBackEdge()
        {
            var (face, targetFace) = GetCorrectlyPositionedMiddleEdge(_cube.Cube);
            {
                if (face == Side.Top && targetFace == Side.Left ||
                    face == Side.Left && targetFace == Side.Bottom ||
                    face == Side.Bottom && targetFace == Side.Right ||
                    face == Side.Right && targetFace == Side.Top)
                {
                    PerformLeftSwitch(face, targetFace);
                }

                if (face == Side.Top && targetFace == Side.Right ||
                    face == Side.Right && targetFace == Side.Bottom ||
                    face == Side.Bottom && targetFace == Side.Left ||
                    face == Side.Left && targetFace == Side.Top)
                {
                    PerformRightSwitch(face, targetFace);
                }
            }
        }

        private void RotateIncorrectSideEdgeToBack()
        {
            var incorrectEdge = _cube.GetSideEdges().First(b => !RubixCubeStatusEvaluator.EdgeIsInCorrectPosition(b));
            var sides = incorrectEdge.GetNonNullSides();

            if (sides.Contains(Side.Top) && sides.Contains(Side.Left))
            {
                PerformRightSwitch(Side.Left, Side.Top);
            }

            if (sides.Contains(Side.Right) && sides.Contains(Side.Top))
            {
                PerformRightSwitch(Side.Top, Side.Right);
            }

            if (sides.Contains(Side.Bottom) && sides.Contains(Side.Right))
            {
                PerformRightSwitch(Side.Right, Side.Bottom);
            }

            if (sides.Contains(Side.Left) && sides.Contains(Side.Bottom))
            {
                PerformRightSwitch(Side.Bottom, Side.Left);
            }
        }

        private (Side face, Side targetFace) GetCorrectlyPositionedMiddleEdge(Block[,,] cube)
        {
            if (cube[2, 0, 1].Top == Colour.Green && cube[2, 0, 1].Back == Colour.Red)
            {
                return (Side.Top, Side.Left);
            } 
            
            if (cube[2, 0, 1].Top == Colour.Green && cube[2, 0, 1].Back == Colour.Orange)
            {
                return (Side.Top, Side.Right);
            } 
            
            if (cube[2, 1, 0].Left == Colour.Red && cube[2, 1, 0].Back == Colour.Green)
            {
                return (Side.Left, Side.Top);
            } 
            
            if (cube[2, 1, 0].Left == Colour.Red && cube[2, 1, 0].Back == Colour.Blue)
            {
                return (Side.Left, Side.Bottom);
            } 
            
            if (cube[2, 1, 2].Right == Colour.Orange && cube[2, 1, 2].Back == Colour.Green)
            {
                return (Side.Right, Side.Top);
            } 
            
            if (cube[2, 1, 2].Right == Colour.Orange && cube[2, 1, 2].Back == Colour.Blue)
            {
                return (Side.Right, Side.Bottom);
            } 
            
            if (cube[2, 2, 1].Bottom == Colour.Blue && cube[2, 2, 1].Back == Colour.Red)
            {
                return (Side.Bottom, Side.Left);
            } 
            
            if (cube[2, 2, 1].Bottom == Colour.Blue && cube[2, 2, 1].Back == Colour.Orange)
            {
                return (Side.Bottom, Side.Right);
            }
            
            throw new Exception("There are no matching middle edges but there should be...");
        }
        
        private bool BackHasSolvableEdge()
        {
            var backEdges = _cube.GetBackEdges().Where(b => !b.HasColour(Colour.Yellow));
            foreach (var block in backEdges)
            {
                var sides = block.GetNonNullSides();
                var nonBackSide = sides.Single(side => side != Side.Back);
                if (RubixCubeStatusEvaluator.SideIsCorrectColour(nonBackSide, block))
                {
                    return true;
                }
            }

            return false;
        }

        private void PerformLeftSwitch(Side face, Side side)
        {
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(side);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(face);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(face);
        }

        private void PerformRightSwitch(Side face, Side side)
        {
            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(side);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(Side.Back);
            _cube.RotateAntiClockwise(face);
            _cube.RotateClockwise(Side.Back);
            _cube.RotateClockwise(face);
        }
    }
}
