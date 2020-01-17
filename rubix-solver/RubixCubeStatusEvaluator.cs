namespace rubix_solver
{
    public static class RubixCubeStatusEvaluator
    {
        public static bool IsSolved(RubixCube cube)
        {
            var back = cube.GetFace(Layer.Back);
            foreach (var block in back)
            {
                if (block.Back != Colour.Yellow)
                {
                    return false;
                }
            }

            var front = cube.GetFace(Layer.Front);
            foreach (var block in front)
            {
                if (block.Front != Colour.White)
                {
                    return false;
                }
            }

            var left = cube.GetFace(Layer.Left);
            foreach (var block in left)
            {
                if (block.Left != Colour.Red)
                {
                    return false;
                }
            }

            var right = cube.GetFace(Layer.Right);
            foreach (var block in right)
            {
                if (block.Right != Colour.Orange)
                {
                    return false;
                }
            }

            var top = cube.GetFace(Layer.Top);
            foreach (var block in top)
            {
                if (block.Top != Colour.Green)
                {
                    return false;
                }
            }

            var bottom = cube.GetFace(Layer.Bottom);
            foreach (var block in bottom)
            {
                if (block.Bottom != Colour.Blue)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool FirstLayerIsSolved(RubixCube cube)
        {
            var front = cube.GetFace(Layer.Front);
            foreach (var block in front)
            {
                if (block.Front != Colour.White)
                {
                    return false;
                }
            }

            var left = cube.GetFace(Layer.Left);
            for (var x = 0; x < 3; x++)
            {
                if (left[x, 2].Left != Colour.Red)
                {
                    return false;
                }
            }

            if (left[1, 1].Left != Colour.Red)
            {
                return false;
            }

            var right = cube.GetFace(Layer.Right);
            for (var x = 0; x < 3; x++)
            {
                if (right[x, 0].Right != Colour.Orange)
                {
                    return false;
                }
            }

            if (right[1, 1].Right != Colour.Orange)
            {
                return false;
            }

            var top = cube.GetFace(Layer.Top);
            for (var y = 0; y < 3; y++)
            {
                if (top[2, y].Top != Colour.Green)
                {
                    return false;
                }
            }

            if (top[1, 1].Top != Colour.Green)
            {
                return false;
            }

            var bottom = cube.GetFace(Layer.Bottom);
            for (var y = 0; y < 3; y++)
            {
                if (bottom[0, y].Bottom != Colour.Blue)
                {
                    return false;
                }
            }

            if (bottom[1, 1].Bottom != Colour.Blue)
            {
                return false;
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
