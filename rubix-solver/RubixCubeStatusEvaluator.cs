using System;
using System.Collections.Generic;
using System.Linq;

namespace rubix_solver
{
    public static class RubixCubeStatusEvaluator
    {
        //TODO: Distinguish between layer and face (also cube vs block face)?

        public static bool IsSolved(RubixCube cube)
        {
            return CubeFaceIsSolved(cube, Layer.Back) &&
                   CubeFaceIsSolved(cube, Layer.Front) &&
                   CubeFaceIsSolved(cube, Layer.Left) &&
                   CubeFaceIsSolved(cube, Layer.Right) &&
                   CubeFaceIsSolved(cube, Layer.Top) &&
                   CubeFaceIsSolved(cube, Layer.Bottom);
        }

        private static bool CubeFaceIsSolved(RubixCube cube, Layer layer)
        {
            var face = cube.GetFace(layer);
            return BlockFacesAreSolved(layer, face.Cast<Block>());
        }

        private static bool BlockFacesAreSolved(Layer layer, IEnumerable<Block> face)
        {
            return face.Select(block => CorrectColour(layer, block)).All(correctColour => correctColour);
        }

        private static bool CorrectColour(Layer layer, Block block)
        {
            return layer switch
            {
                Layer.Back => block.Back == Colour.Yellow,
                Layer.Front => block.Front == Colour.White,
                Layer.Left => block.Left == Colour.Red,
                Layer.Right => block.Right == Colour.Orange,
                Layer.Top => block.Top == Colour.Green,
                Layer.Bottom => block.Bottom == Colour.Blue,
                _ => throw new Exception($"Not a valid layer: {layer.ToString()}")
            };
        }

        private static bool MiddleLayerBlockFacesAreSolved(RubixCube cube, Layer layer)
        {
            var blocks = cube.GetMiddleLayer(layer);
            return BlockFacesAreSolved(layer, blocks);
        }

        public static bool SecondLayerIsSolved(RubixCube cube)
        {
            if (!FirstLayerIsSolved(cube))
            {
                return false;
            }

            return MiddleLayerBlockFacesAreSolved(cube, Layer.Left) &&
                   MiddleLayerBlockFacesAreSolved(cube, Layer.Right) &&
                   MiddleLayerBlockFacesAreSolved(cube, Layer.Top) &&
                   MiddleLayerBlockFacesAreSolved(cube, Layer.Bottom);
        }

        public static bool FirstLayerIsSolved(RubixCube cube)
        {
            if (!CubeFaceIsSolved(cube, Layer.Front) || !CenterBlocksAreCorrect(cube))
            {
                return false;
            }

            var left = cube.GetFace(Layer.Left);
            for (var x = 0; x < 3; x++)
            {
                if (left[x, 2].Left != Colour.Red)
                {
                    return false;
                }
            }

            var right = cube.GetFace(Layer.Right);
            for (var x = 0; x < 3; x++)
            {
                if (right[x, 0].Right != Colour.Orange)
                {
                    return false;
                }
            }

            var top = cube.GetFace(Layer.Top);
            for (var y = 0; y < 3; y++)
            {
                if (top[2, y].Top != Colour.Green)
                {
                    return false;
                }
            }

            var bottom = cube.GetFace(Layer.Bottom);
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
