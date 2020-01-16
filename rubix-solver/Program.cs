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
            cube.Solve();
            cube.PrintCube();
        }
    }
}

/*

var cube = new RubixCube(new[,,]
            {
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, Colour.White, null),
                        new Block(null, null, Colour.Red, null, Colour.Blue, null),
                        new Block(null, Colour.Blue, Colour.White, null, Colour.Red, null)
                    },
                    {
                        new Block(Colour.Green, null, null, null, Colour.White, null),
                        new Block(null, null, null, null, Colour.White, null),
                        new Block(null, Colour.Orange, null, null, Colour.Green, null)
                    },
                    {
                        new Block(Colour.Green, null, null, Colour.Orange, Colour.Yellow, null),
                        new Block(null, null, null, Colour.Blue, Colour.White, null),
                        new Block(null, Colour.Red, null, Colour.Blue, Colour.Yellow, null)
                    }
                },
                {
                    {
                        new Block(Colour.Green, null, Colour.Yellow, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Yellow, Colour.Orange, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, null),
                        new Block(null, null, null, null, null, null),
                        new Block(null, Colour.Orange, null, null, null, null)
                    },
                    {
                        new Block(Colour.Green, null, null, Colour.Red, null, null),
                        new Block(null, null, null, Colour.Blue, null, null),
                        new Block(null, Colour.Yellow, null, Colour.Blue, null, null)
                    }
                },
                {
                    {
                        new Block(Colour.Green, null, Colour.White, null, null, Colour.Orange),
                        new Block(null, null, Colour.Yellow, null, null, Colour.Red),
                        new Block(null, Colour.Blue, Colour.Orange, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Blue, null, null, null, null, Colour.Orange),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.White, null, null, null, Colour.Red)
                    },
                    {
                        new Block(Colour.Green, null, null, Colour.Red, null, Colour.Yellow),
                        new Block(null, null, null, Colour.Orange, null, Colour.White),
                        new Block(null, Colour.Orange, null, Colour.White, null, Colour.Blue)
                    }
                }
            });
            */