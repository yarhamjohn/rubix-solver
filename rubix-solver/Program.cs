using System;

namespace rubix_solver
{
    class Program
    { 
        static void Main(string[] args)
        {
            var cube = new RubixCube();
            cube.PrintCube();
            cube.Randomise();
            cube.PrintCube();
        }
    }

    public enum Layer
    {
        Left,
        Right,
        Top,
        Bottom,
        Front,
        Back,
        MiddleVertical,
        MiddleHorizontal
    }

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
    }

    public enum Colour
    {
        White,
        Green,
        Blue,
        Red,
        Orange,
        Yellow
    }
}