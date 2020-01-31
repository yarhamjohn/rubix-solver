using System;

namespace rubix_solver.Solvers
{
    public abstract class Corner
    {
        public Block Block { get; }

        public CornerLocation Location { get; }

        protected Corner((int x, int y) coordinates, Block block)
        {
            Block = block;
            Location = coordinates switch
            {
                (0, 0) => CornerLocation.TopLeft,
                (0, 2) => CornerLocation.TopRight,
                (2, 0) => CornerLocation.BottomLeft,
                (2, 2) => CornerLocation.BottomRight,
                _ => throw new ArgumentException($"Not a corner location ({coordinates})")
            };
        }

        public abstract bool IsCorrectlyPositioned();
    }

    public class FrontCorner : Corner
    {
        public FrontCorner((int x, int y) coordinates, Block block) : base(coordinates, block) {}

        public override bool IsCorrectlyPositioned()
        {
            return IsInCorrectFrontCorner() && Block.Front == Colour.White;
        }

        private bool IsInCorrectFrontCorner()
        {
            return Location switch
            {
                CornerLocation.TopLeft => Block.HasColour(Colour.Green) && Block.HasColour(Colour.Red),
                CornerLocation.TopRight => Block.HasColour(Colour.Green) && Block.HasColour(Colour.Orange),
                CornerLocation.BottomLeft => Block.HasColour(Colour.Blue) && Block.HasColour(Colour.Red),
                CornerLocation.BottomRight => Block.HasColour(Colour.Blue) && Block.HasColour(Colour.Orange),
                _ => throw new Exception("This isn't a corner block...")
            };
        }
    }

    public class BackCorner : Corner
    {
        public BackCorner((int x, int y) coordinates, Block block) : base(coordinates, block) {}

        public override bool IsCorrectlyPositioned()
        {
            return IsInCorrectBackCorner() && Block.HasColour(Colour.White);
        }

        private bool IsInCorrectBackCorner()
        {
            return Location switch
            {
                CornerLocation.TopLeft => Block.HasColour(Colour.Green) && Block.HasColour(Colour.Orange),
                CornerLocation.TopRight => Block.HasColour(Colour.Green) && Block.HasColour(Colour.Red),
                CornerLocation.BottomLeft => Block.HasColour(Colour.Blue) && Block.HasColour(Colour.Orange),
                CornerLocation.BottomRight => Block.HasColour(Colour.Blue) && Block.HasColour(Colour.Red),
                _ => throw new Exception("This isn't a corner block...")
            };
        }
    }

    public static class CornerBuilder
    {
        public static Corner Build((int x, int y) coordinates, Block block, Side side)
        {
            return side switch
            {
                Side.Front => (Corner) new FrontCorner(coordinates, block),
                Side.Back => new BackCorner(coordinates, block),
                _ => throw new ArgumentException(
                    $"Corners can only be on either the front or back sides, not side: {side}")
            };
        }
    }
}
