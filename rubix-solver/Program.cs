using rubix_solver.Solvers;

namespace rubix_solver
{
    class Program
    { 
        static void Main(string[] args)
        {
            var cube = new RubixCube();
            RubixCubePrinter.Print(cube);

            cube.Randomise();
            RubixCubePrinter.Print(cube);

            var solver = new RubixCubeSolver(cube);
            solver.Solve();
            RubixCubePrinter.Print(cube);
        }
    }
}
