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

        public static bool SecondLayerIsSolved(RubixCube cube)
        {
            if (!FirstLayerIsSolved(cube))
            {
                return false;
            }

            return MiddleLayerFacesAreSolved(cube, Side.Left) &&
                   MiddleLayerFacesAreSolved(cube, Side.Right) &&
                   MiddleLayerFacesAreSolved(cube, Side.Top) &&
                   MiddleLayerFacesAreSolved(cube, Side.Bottom);
        }

        public static bool FirstLayerIsSolved(RubixCube cube)
        {
            if (!CubeFaceIsSolved(cube, Side.Front) || !CenterBlocksAreCorrect(cube))
            {
                return false;
            }

            return BottomLayerFacesAreSolved(cube, Side.Left) &&
                   BottomLayerFacesAreSolved(cube, Side.Right) &&
                   BottomLayerFacesAreSolved(cube, Side.Top) &&
                   BottomLayerFacesAreSolved(cube, Side.Bottom);
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

        private static bool CubeFaceIsSolved(RubixCube cube, Side side)
        {
            var face = cube.GetFace(side);
            return BlockFacesAreSolved(side, face.Cast<Block>());
        }

        private static bool MiddleLayerFacesAreSolved(RubixCube cube, Side side)
        {
            var blocks = cube.GetMiddleLayer(side);
            return BlockFacesAreSolved(side, blocks);
        }

        private static bool BottomLayerFacesAreSolved(RubixCube cube, Side side)
        {
            var blocks = cube.GetBottomLayer(side);
            return BlockFacesAreSolved(side, blocks);
        }

        private static bool BlockFacesAreSolved(Side side, IEnumerable<Block> face)
        {
            return face.Select(block => IsCorrectColour(side, block)).All(correctColour => correctColour);
        }

        private static bool IsCorrectColour(Side side, Block block)
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
    }
}
