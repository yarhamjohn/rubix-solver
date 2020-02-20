using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver
{
    public static class RubixCubeStatusEvaluator
    {
        public static bool ThirdLayerIsSolved(RubixCube cube)
        {
            return ((Side[]) Enum.GetValues(typeof(Side))).All(side => CubeFaceIsSolved(cube, side));
        }

        public static bool SecondLayerIsSolved(RubixCube cube)
        {
            return FirstLayerIsSolved(cube) &&
                   MiddleLayerFacesAreSolved(cube, Side.Left) &&
                   MiddleLayerFacesAreSolved(cube, Side.Right) &&
                   MiddleLayerFacesAreSolved(cube, Side.Top) &&
                   MiddleLayerFacesAreSolved(cube, Side.Bottom);
        }

        public static bool FirstLayerIsSolved(RubixCube cube)
        {
            return CubeFaceIsSolved(cube, Side.Front) &&
                   CentreBlocksAreOnCorrectFaces(cube) &&
                   BottomLayerFacesAreSolved(cube, Side.Left) &&
                   BottomLayerFacesAreSolved(cube, Side.Right) &&
                   BottomLayerFacesAreSolved(cube, Side.Top) &&
                   BottomLayerFacesAreSolved(cube, Side.Bottom);
        }

        public static bool CrossIsFormed(RubixCube cube, Side side)
        {
            var face = cube.GetFace(side);
            return CrossFaceIsFormed(cube, side) &&
                   CenterEdgeBlockFacesAreSolved(face, side) &&
                   CentreBlocksAreOnCorrectFaces(cube);
        }

        public static bool CrossFaceIsFormed(RubixCube cube, Side side)
        {
            var face = cube.GetFace(side);
            return SideIsCorrectColour(side, face[0, 1]) &&
                   SideIsCorrectColour(side, face[1, 0]) &&
                   SideIsCorrectColour(side, face[1, 2]) &&
                   SideIsCorrectColour(side, face[2, 1]);
        }

        private static bool CentreBlocksAreOnCorrectFaces(RubixCube cube)
        {
            return cube.GetCenterBlockFace(Side.Front) == Colour.White &&
                   cube.GetCenterBlockFace(Side.Top) == Colour.Green &&
                   cube.GetCenterBlockFace(Side.Back) == Colour.Yellow &&
                   cube.GetCenterBlockFace(Side.Bottom) == Colour.Blue &&
                   cube.GetCenterBlockFace(Side.Left) == Colour.Red &&
                   cube.GetCenterBlockFace(Side.Right) == Colour.Orange;
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

        private static bool CenterEdgeBlockFacesAreSolved(Block[,] face, Side side)
        {
            var leftFaceBlock = side == Side.Front ? face[1, 0] : face[1, 2];
            var rightFaceBlock = side == Side.Front ? face[1, 2] : face[1, 0];
            return SideIsCorrectColour(Side.Top, face[0, 1]) &&
                   SideIsCorrectColour(Side.Left, leftFaceBlock) &&
                   SideIsCorrectColour(Side.Right, rightFaceBlock) &&
                   SideIsCorrectColour(Side.Bottom, face[2, 1]);
        }

        private static bool BlockFacesAreSolved(Side side, IEnumerable<Block> face)
        {
            return face.Select(block => SideIsCorrectColour(side, block)).All(correctColour => correctColour);
        }

        public static bool SideIsCorrectColour(Side side, Block block)
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

        public static bool EdgeIsInCorrectPosition(Block block)
        {
            var nonNullSides = block.GetNonNullSides();
            foreach (var side in nonNullSides)
            {
                if (!SideIsCorrectColour(side, block))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
