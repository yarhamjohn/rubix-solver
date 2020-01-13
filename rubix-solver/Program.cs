using System;

namespace rubix_solver
{
    class Program
    { 
        static void Main(string[] args)
        {
            // var cube2 = new RubixCube();
            // cube2.PrintCube();
            // cube.Randomise();
            // cube.PrintCube();
            
            var cube = new RubixCube(new[,,]
            {
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, Colour.White, null),
                        new Block(null, null, Colour.Green, null, Colour.White, null),
                        new Block(null, Colour.Orange, Colour.Green, null, Colour.White, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, Colour.White, null),
                        new Block(null, null, null, null, Colour.White, null),
                        new Block(null, Colour.Orange, null, null, Colour.White, null)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Blue, Colour.White, null),
                        new Block(null, null, null, Colour.Blue, Colour.White, null),
                        new Block(null, Colour.Orange, null, Colour.Blue, Colour.White, null)
                    }
                },
                {
                    {
                        new Block(Colour.Orange, null, Colour.Green, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Green, Colour.Red, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, null),
                        new Block(null, null, null, null, null, null),
                        new Block(null, Colour.Orange, null, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Blue, null, null),
                        new Block(null, null, null, Colour.Blue, null, null),
                        new Block(null, Colour.Orange, null, Colour.Blue, null, null)
                    }
                },
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, null, Colour.Yellow),
                        new Block(null, null, Colour.Orange, null, null, Colour.Yellow),
                        new Block(null, Colour.Red, Colour.Blue, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Yellow, null, null, null, null, Colour.Blue),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Red, null, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Yellow, null, null, Colour.Blue, null, Colour.Orange),
                        new Block(null, null, null, Colour.Green, null, Colour.Yellow),
                        new Block(null, Colour.Yellow, null, Colour.Green, null, Colour.Orange)
                    }
                }
            });
            
            cube.PrintCube();
            cube.Solve();
            cube.PrintCube();
        }
    }
}