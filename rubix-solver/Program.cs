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