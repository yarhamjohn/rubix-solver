using System;

namespace rubix_solver
{
    class Program
    { 
        static void Main(string[] args)
        {
            var cube = new RubixCube();
            RubixCubePrinter.PrintCube(cube);
            cube.Randomise();
            RubixCubePrinter.PrintCube(cube);
            cube.Solve();
            RubixCubePrinter.PrintCube(cube);
        }
    }
}
