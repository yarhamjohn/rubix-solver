using System;

namespace rubix_solver
{
    public static class RubixCubeStatusEvaluator
    {
        public static bool IsSolved(RubixCube cube)
        {
            return FaceIsSolved(cube, Layer.Back) &&
                   FaceIsSolved(cube, Layer.Front) &&
                   FaceIsSolved(cube, Layer.Left) &&
                   FaceIsSolved(cube, Layer.Right) &&
                   FaceIsSolved(cube, Layer.Top) &&
                   FaceIsSolved(cube, Layer.Bottom);
        }

        private static bool FaceIsSolved(RubixCube cube, Layer layer)
        {
            var face = cube.GetFace(layer);
            foreach (var block in face)
            {
                var correctColour = layer switch
                {
                    Layer.Back => block.Back == Colour.Yellow,
                    Layer.Front => block.Front == Colour.White,
                    Layer.Left => block.Left == Colour.Red,
                    Layer.Right => block.Right == Colour.Orange,
                    Layer.Top => block.Top == Colour.Green,
                    Layer.Bottom => block.Bottom == Colour.Blue,
                    _ => throw new Exception($"Not a valid layer: {layer.ToString()}")
                };

                if (!correctColour)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool SecondLayerIsSolved(RubixCube cube)
        {
            if (!FaceIsSolved(cube, Layer.Front))
            {
                return false;
            }

            var left = cube.GetFace(Layer.Left);
            for (var x = 0; x < 3; x++)
            {
                for (var y = 1; y < 3; y++)
                {
                    if (left[x, y].Left != Colour.Red)
                    {
                        return false;
                    }
                }
            }

            var right = cube.GetFace(Layer.Right);
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 2; y++)
                {
                    if (right[x, y].Right != Colour.Orange)
                    {
                        return false;
                    }
                }
            }

            var top = cube.GetFace(Layer.Top);
            for (var x = 1; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (top[x, y].Top != Colour.Green)
                    {
                        return false;
                    }
                }
            }

            var bottom = cube.GetFace(Layer.Bottom);
            for (var x = 0; x < 2; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (bottom[x, y].Bottom != Colour.Blue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool FirstLayerIsSolved(RubixCube cube)
        {
            if (!FaceIsSolved(cube, Layer.Front) || !CenterBlocksAreCorrect(cube))
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
