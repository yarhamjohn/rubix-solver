using System;
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
            [Option('j', "cube-json", Required = true, HelpText = "Json string representing the Rubix Cube.", SetName = "json")]
            public string CubeJson { get; set; }
            
            [Option('f', "cube-json-file-path", Required = true, HelpText = "Path to Json file containing the Rubix Cube definition.", SetName = "json-file")]
            public string CubeJsonFilePath { get; set; }
            
            [Option('o', "output", Required = true, HelpText = "Path to text file for instruction output.")]
            public string OutputPath { get; set; }
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(SolveRubixCube);
        }

        private static void SolveRubixCube(Options opts)
        {
            var cube = GetParsedCube(opts);
            var rubixCube = new RubixCube(cube);

            var solver = new RubixCubeSolver(rubixCube);
            solver.Solve();

            using var file = new StreamWriter(opts.OutputPath);
            foreach (var (num, direction, side) in rubixCube.Instructions)
            {
                file.WriteLine($"{num} - {direction}: {side}");
            }
        }

        private static Cube GetParsedCube(Options opts)
        {
            if (!string.IsNullOrWhiteSpace(opts.CubeJson))
            {
                return DeserializeOrThrow(opts.CubeJson);
            }
            
            if (!File.Exists(opts.CubeJsonFilePath))
            {
                throw new Exception(
                    $"The specified json file could not be found at the provided location: {opts.CubeJsonFilePath}");
            }

            var fileContents = File.ReadAllText(opts.CubeJsonFilePath);
            return DeserializeOrThrow(fileContents);
        }

        private static Cube DeserializeOrThrow(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Cube>(json);
            }
            catch
            {
                throw new Exception("The provided json string was not a valid cube.");
            }
        }
    }
}
