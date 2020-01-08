using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver
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
                    // check second layer for piece in reverse orientation
                }

                // Rotate third layer until a middle edge matches its side, check which way it should switch and move it
            }
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

        public void PerformLeftSwitch(Layer top, Layer face, Layer side)
        {
            _cube.RotateAntiClockwise(top);
            _cube.RotateAntiClockwise(side);
            _cube.RotateClockwise(top);
            _cube.RotateClockwise(side);
            _cube.RotateClockwise(top);
            _cube.RotateClockwise(face);
            _cube.RotateAntiClockwise(top);
            _cube.RotateAntiClockwise(face);
        }

        public void PerformRightSwitch(Layer top, Layer face, Layer side)
        {
            _cube.RotateClockwise(top);
            _cube.RotateClockwise(side);
            _cube.RotateAntiClockwise(top);
            _cube.RotateAntiClockwise(side);
            _cube.RotateAntiClockwise(top);
            _cube.RotateAntiClockwise(face);
            _cube.RotateClockwise(top);
            _cube.RotateClockwise(face);
        }
    }
}