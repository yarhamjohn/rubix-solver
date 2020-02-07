using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool HasColour(Colour colour)
        {
            return Left == colour || 
                   Right == colour || 
                   Top == colour || 
                   Bottom == colour || 
                   Front == colour ||
                   Back == colour;
        }

        public Side? GetLayer(Colour colour)
        {
            if (Left == colour)
            {
                return Side.Left;
            }

            if (Right == colour)
            {
                return Side.Right;
            }
            
            if (Top == colour)
            {
                return Side.Top;
            }

            if (Bottom == colour)
            {
                return Side.Bottom;
            }
            
            if (Front == colour)
            {
                return Side.Front;
            }

            if (Back == colour)
            {
                return Side.Back;
            }

            return null;
        }
        
        public Colour? GetColour(Side side)
        {
            if (Side.Left == side)
            {
                return Left;
            }

            if (Side.Right == side)
            {
                return Right;
            }
            
            if (Side.Top == side)
            {
                return Top;
            }

            if (Side.Bottom == side)
            {
                return Bottom;
            }
            
            if (Side.Front == side)
            {
                return Front;
            }

            if (Side.Back == side)
            {
                return Back;
            }

            return null;
        }
        
        private List<Colour?> GetColours()
        {
            return new List<Colour?>
            {
                Left,
                Right,
                Top,
                Bottom,
                Front,
                Back
            };
        }

        public bool HasMatchingColours(Block block)
        {
            var colours = GetColours().Where(c => c != null);
            var blockColours = block.GetColours().Where(c => c != null);
            return colours.All(c => blockColours.Contains(c));
        }

    }
}
