using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver
{
    public static class RubixCubeStatusEvaluator
    {
        public static bool IsSolved(RubixCube cube)
        {
            return CubeFaceIsSolved(cube, Side.Back) &&
                   CubeFaceIsSolved(cube, Side.Front) &&
                   CubeFaceIsSolved(cube, Side.Left) &&
                   CubeFaceIsSolved(cube, Side.Right) &&
                   CubeFaceIsSolved(cube, Side.Top) &&
                   CubeFaceIsSolved(cube, Side.Bottom);
        }

        private static bool CubeFaceIsSolved(RubixCube cube, Side side)
        {
            var face = cube.GetFace(side);
            return BlockFacesAreSolved(side, face.Cast<Block>());
        }

        private static bool BlockFacesAreSolved(Side side, IEnumerable<Block> face)
        {
            return face.Select(block => CorrectColour(side, block)).All(correctColour => correctColour);
        }

        private static bool CorrectColour(Side side, Block block)
        {
            return side switch
            {
                Side.Back => block.Back == Colour.Yellow,
                Side.Front => block.Front == Colour.White,
                Side.Left => block.Left == Colour.Red,
                Side.Right => block.Right == Colour.Orange,
                Side.Top => block.Top == Colour.Green,
                Side.Bottom => block.Bottom == Colour.Blue,
                _ => throw new Exception($"Not a valid layer: {side.ToString()}")
            };
        }

        private static bool MiddleLayerBlockFacesAreSolved(RubixCube cube, Side side)
        {
            var blocks = cube.GetMiddleLayer(side);
            return BlockFacesAreSolved(side, blocks);
        }

        public static bool SecondLayerIsSolved(RubixCube cube)
        {
            if (!FirstLayerIsSolved(cube))
            {
                return false;
            }

            return MiddleLayerBlockFacesAreSolved(cube, Side.Left) &&
                   MiddleLayerBlockFacesAreSolved(cube, Side.Right) &&
                   MiddleLayerBlockFacesAreSolved(cube, Side.Top) &&
                   MiddleLayerBlockFacesAreSolved(cube, Side.Bottom);
        }

        public static bool FirstLayerIsSolved(RubixCube cube)
        {
            if (!CubeFaceIsSolved(cube, Side.Front) || !CenterBlocksAreCorrect(cube))
            {
                return false;
            }

            var left = cube.GetFace(Side.Left);
            for (var x = 0; x < 3; x++)
            {
                if (left[x, 2].Left != Colour.Red)
                {
                    return false;
                }
            }

            var right = cube.GetFace(Side.Right);
            for (var x = 0; x < 3; x++)
            {
                if (right[x, 0].Right != Colour.Orange)
                {
                    return false;
                }
            }

            var top = cube.GetFace(Side.Top);
            for (var y = 0; y < 3; y++)
            {
                if (top[2, y].Top != Colour.Green)
                {
                    return false;
                }
            }

            var bottom = cube.GetFace(Side.Bottom);
            for (var y = 0; y < 3; y++)
            {
                if (bottom[0, y].Bottom != Colour.Blue)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CenterBlocksAreCorrect(RubixCube cube)
        {
            return cube.Cube[0, 1, 1].Front == Colour.White &&
                   cube.Cube[1, 0, 1].Top == Colour.Green &&
                   cube.Cube[1, 1, 0].Left == Colour.Red &&
                   cube.Cube[1, 1, 2].Right == Colour.Orange &&
                   cube.Cube[1, 2, 1].Bottom == Colour.Blue &&
                   cube.Cube[2, 1, 1].Back == Colour.Yellow;
        }
    }
}
