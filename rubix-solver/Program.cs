using System.IO;
using CommandLine;
using Newtonsoft.Json;
using rubix_solver.Solvers;

namespace rubix_solver
{
    class Program
    {
        private class Options
        {
            [Option('c', "cube", Required = true, HelpText = "Json string representing the Rubix Cube.")]
            public string Cube { get; set; }

            [Option('o', "output", Required = true, HelpText = "Path to text file for instruction output.")]
            public string OutputPath { get; set; }
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(SolveRubixCube);
        }

        private static void SolveRubixCube(Options opts)
        {
            var cube = JsonConvert.DeserializeObject<Cube>(opts.Cube);
            var rubixCube = new RubixCube(cube);

            var solver = new RubixCubeSolver(rubixCube);
            solver.Solve();

            using var file = new StreamWriter(opts.OutputPath);
            foreach (var (num, direction, side) in rubixCube.Instructions)
            {
                file.WriteLine($"{num} - {direction}: {side}");
            }
        }
    }
}
