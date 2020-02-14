using System;

namespace rubix_solver
{

    public abstract class Edge
    {
        public Side SideOne { get; }
        public abstract Side SideTwo { get; set; }
        public Block Block { get; }

        protected Edge(Side sideOne, Block block)
        {
            SideOne = sideOne;
            Block = block;
        }
    }

    public class SideEdge : Edge
    {
        public sealed override Side SideTwo { get; set; }

        public SideEdge(Side sideOne, (int x, int y) coordinates, Block block) : base(sideOne, block)
        {
            SideTwo = coordinates switch
            {
                (0, 1) => Side.Top,
                (1, 0) => Side.Right,
                (1, 2) => Side.Left,
                (2, 1) => Side.Bottom,
                _ => throw new ArgumentException($"Not a valid edge coordinate: {coordinates}")
            };
        }
    }

    public static class EdgeBuilder
    {
        public static Edge Build((int x, int y) coordinates, Block block, Side side)
        {
            if (side == Side.Left || side == Side.Right)
            {
                return new SideEdge(side, coordinates, block);
            }

            throw new ArgumentException($"Not a valid side: {side}");
        }
    }
}
