using System;

namespace rubix_solver
{
    public class Block
    {
        public Colour? Left { get; set; }
        public Colour? Right { get; set; }
        public Colour? Top { get; set; }
        public Colour? Bottom { get; set; }
        public Colour? Front { get; set; }
        public Colour? Back { get; set; }

        public Block(Colour? left, Colour? right, Colour? top, Colour? bottom, Colour? front, Colour? back)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Front = front;
            Back = back;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var block = (Block) obj;
            return Left == block.Left && 
                   Right == block.Right && 
                   Top == block.Top && 
                   Bottom == block.Bottom &&
                   Front == block.Front && 
                   Back == block.Back;
        }
    }
}