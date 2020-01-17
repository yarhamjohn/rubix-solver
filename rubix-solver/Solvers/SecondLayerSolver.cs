using System;
using System.Collections.Generic;

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
            if (_cube.IsSolved())
            {
                return;
            }

            while (!_cube.SecondLayerIsSolved())
            {
                var middleEdges = GetMiddleEdges();
                if (NumSwitchableMiddleEdges(middleEdges) == 0)
                {
                    if (_cube.Cube[1, 0, 0].Top != Colour.Yellow && _cube.Cube[1, 0, 0].Left != Colour.Yellow)
                    {
                        PerformRightSwitch(Layer.Left, Layer.Top);
                    }
                    
                    if (_cube.Cube[1, 0, 2].Top != Colour.Yellow && _cube.Cube[1, 0, 2].Right != Colour.Yellow)
                    {
                        PerformRightSwitch(Layer.Top, Layer.Right);
                    }
                    
                    if (_cube.Cube[1, 2, 0].Bottom != Colour.Yellow && _cube.Cube[1, 2, 0].Left != Colour.Yellow)
                    {
                        PerformRightSwitch(Layer.Bottom, Layer.Left);
                    }
                    
                    if (_cube.Cube[1, 2, 2].Bottom != Colour.Yellow && _cube.Cube[1, 2, 2].Right != Colour.Yellow)
                    {
                        PerformRightSwitch(Layer.Right, Layer.Bottom);
                    }
                }

                while (!MiddleEdgeIsCorrectlyPositioned(_cube.Cube))
                {
                    _cube.RotateClockwise(Layer.Back);
                }

                var (face, targetFace) = GetCorrectlyPositionedMiddleEdge(_cube.Cube);
                {
                    if (face == Layer.Top && targetFace == Layer.Left ||
                        face == Layer.Left && targetFace == Layer.Bottom ||
                        face == Layer.Bottom && targetFace == Layer.Right ||
                        face == Layer.Right && targetFace == Layer.Top)
                    {
                        PerformLeftSwitch(face, targetFace);
                    }

                    if (face == Layer.Top && targetFace == Layer.Right ||
                        face == Layer.Right && targetFace == Layer.Bottom ||
                        face == Layer.Bottom && targetFace == Layer.Left ||
                        face == Layer.Left && targetFace == Layer.Top)
                    {
                        PerformRightSwitch(face, targetFace);
                    }
                }
            }
        }

        private (Layer face, Layer targetFace) GetCorrectlyPositionedMiddleEdge(Block[,,] cube)
        {
            if (cube[2, 0, 1].Top == Colour.Green && cube[2, 0, 1].Back == Colour.Red)
            {
                return (Layer.Top, Layer.Left);
            } 
            
            if (cube[2, 0, 1].Top == Colour.Green && cube[2, 0, 1].Back == Colour.Orange)
            {
                return (Layer.Top, Layer.Right);
            } 
            
            if (cube[2, 1, 0].Left == Colour.Red && cube[2, 1, 0].Back == Colour.Green)
            {
                return (Layer.Left, Layer.Top);
            } 
            
            if (cube[2, 1, 0].Left == Colour.Red && cube[2, 1, 0].Back == Colour.Blue)
            {
                return (Layer.Left, Layer.Bottom);
            } 
            
            if (cube[2, 1, 2].Right == Colour.Orange && cube[2, 1, 2].Back == Colour.Green)
            {
                return (Layer.Right, Layer.Top);
            } 
            
            if (cube[2, 1, 2].Right == Colour.Orange && cube[2, 1, 2].Back == Colour.Blue)
            {
                return (Layer.Right, Layer.Bottom);
            } 
            
            if (cube[2, 2, 1].Bottom == Colour.Blue && cube[2, 2, 1].Back == Colour.Red)
            {
                return (Layer.Bottom, Layer.Left);
            } 
            
            if (cube[2, 2, 1].Bottom == Colour.Blue && cube[2, 2, 1].Back == Colour.Orange)
            {
                return (Layer.Bottom, Layer.Right);
            }
            
            throw new Exception("There are no matching middle edges but there should be...");
        }
        
        private bool MiddleEdgeIsCorrectlyPositioned(Block[,,] cube)
        {
            return cube[2, 0, 1].Top == Colour.Green && cube[2, 0, 1].Back == Colour.Red ||
                   cube[2, 0, 1].Top == Colour.Green && cube[2, 0, 1].Back == Colour.Orange ||
                   cube[2, 1, 0].Left == Colour.Red && cube[2, 1, 0].Back == Colour.Green ||
                   cube[2, 1, 0].Left == Colour.Red && cube[2, 1, 0].Back == Colour.Blue || 
                   cube[2, 1, 2].Right == Colour.Orange && cube[2, 1, 2].Back == Colour.Green || 
                   cube[2, 1, 2].Right == Colour.Orange && cube[2, 1, 2].Back == Colour.Blue || 
                   cube[2, 2, 1].Bottom == Colour.Blue && cube[2, 2, 1].Back == Colour.Red || 
                   cube[2, 2, 1].Bottom == Colour.Blue && cube[2, 2, 1].Back == Colour.Orange;
        }

        private static int NumSwitchableMiddleEdges(Dictionary<(int, int), Block> middleEdges)
        {
            var num = middleEdges.Count;
            foreach (var (index, block) in middleEdges)
            {
                switch (index)
                {
                    case (0, 1):
                        if (block.Top == Colour.Yellow || block.Back == Colour.Yellow)
                        {
                            num--;
                        }

                        break;
                    case (1, 2):
                        if (block.Left == Colour.Yellow || block.Back == Colour.Yellow)
                        {
                            num--;
                        }

                        break;
                    case (2, 1):
                        if (block.Bottom == Colour.Yellow || block.Back == Colour.Yellow)
                        {
                            num--;
                        }

                        break;
                    case (1, 0):
                        if (block.Right == Colour.Yellow || block.Back == Colour.Yellow)
                        {
                            num--;
                        }

                        break;
                }
            }

            return num;
        }

        private Dictionary<(int, int), Block> GetMiddleEdges()
        {
            var face = _cube.GetFace(Layer.Back);
            return new Dictionary<(int, int), Block>
            {
                {(0, 1), face[0, 1]}, 
                {(1, 2), face[1, 2]}, 
                {(2, 1), face[2, 1]}, 
                {(1, 0), face[1, 0]}
            };
        }

        public void PerformLeftSwitch(Layer face, Layer side)
        {
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateAntiClockwise(side);
            _cube.RotateClockwise(Layer.Back);
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(Layer.Back);
            _cube.RotateClockwise(face);
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateAntiClockwise(face);
        }

        public void PerformRightSwitch(Layer face, Layer side)
        {
            _cube.RotateClockwise(Layer.Back);
            _cube.RotateClockwise(side);
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(Layer.Back);
            _cube.RotateAntiClockwise(face);
            _cube.RotateClockwise(Layer.Back);
            _cube.RotateClockwise(face);
        }
    }
}
