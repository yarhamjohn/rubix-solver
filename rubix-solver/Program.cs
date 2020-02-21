using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using rubix_solver.Solvers;

namespace rubix_solver
{
    class Program
    {
        private static readonly JObject Input = JObject.Parse(
        @"{
            'frontFace': {
                'topRow': {
                    'leftColumn': {
                        'top': 'green', 'front': 'white', 'left': 'red'
                    },
                    'middleColumn': {
                        'top': 'green', 'front': 'white'
                    },
                    'rightColumn': {
                        'top': 'green', 'front': 'white', 'right': 'orange'
                    }
                },
                'middleRow': {
                    'leftColumn': {
                        'front': 'white', 'left': 'red'
                    },
                    'middleColumn': {
                        'front': 'white'
                    },
                    'rightColumn': {
                        'front': 'white', 'right': 'orange'
                    }
                },
                'bottomRow': {
                    'leftColumn': {
                        'bottom': 'blue', 'front': 'white', 'left': 'red'
                    },
                    'middleColumn': {
                        'bottom': 'blue', 'front': 'white'
                    },
                    'rightColumn': {
                        'bottom': 'blue', 'front': 'white', 'right': 'orange'
                    }
                }
            },
            'middleFace': {
                'topRow': {
                    'leftColumn': {
                        'top': 'green', 'left': 'red'
                    },
                    'middleColumn': {
                        'top': 'green'
                    },
                    'rightColumn': {
                        'top': 'green', 'right': 'orange'
                    }
                },
                'middleRow': {
                    'leftColumn': {
                        'left': 'red'
                    },
                    'middleColumn': {
                    },
                    'rightColumn': {
                        'right': 'orange'
                    }
                },
                'bottomRow': {
                    'leftColumn': {
                        'bottom': 'blue', 'left': 'red'
                    },
                    'middleColumn': {
                        'bottom': 'blue'
                    },
                    'rightColumn': {
                        'bottom': 'blue', 'right': 'orange'
                    }
                }
            },
            'backFace': {
                'topRow': {
                    'leftColumn': {
                        'top': 'green', 'back': 'yellow', 'left': 'red'
                    },
                    'middleColumn': {
                        'top': 'green', 'back': 'yellow'
                    },
                    'rightColumn': {
                        'top': 'green', 'back': 'yellow', 'right': 'orange'
                    }
                },
                'middleRow': {
                    'leftColumn': {
                        'back': 'yellow', 'left': 'red'
                    },
                    'middleColumn': {
                        'back': 'yellow'
                    },
                    'rightColumn': {
                        'back': 'yellow', 'right': 'orange'
                    }
                },
                'bottomRow': {
                    'leftColumn': {
                        'bottom': 'blue', 'back': 'yellow', 'left': 'red'
                    },
                    'middleColumn': {
                        'bottom': 'blue', 'back': 'yellow'
                    },
                    'rightColumn': {
                        'bottom': 'blue', 'back': 'yellow', 'right': 'orange'
                    }
                }
            }
        }");

        static void Main(string[] args)
        {
            Cube outObject = JsonConvert.DeserializeObject<Cube>(Input.ToString());

            var cube = new RubixCube(outObject);
            RubixCubePrinter.Print(cube);
            //
            // cube.Randomise();
            // RubixCubePrinter.Print(cube);
            //
            // var solver = new RubixCubeSolver(cube);
            // solver.Solve();
            // RubixCubePrinter.Print(cube);
            //
            // using var file = new StreamWriter(@"C:\Users\John.Yarham\Documents\Projects\rubix-solver\Instructions.txt");
            // foreach (var (num, direction, side) in cube.Instructions)
            // {
            //     file.WriteLine($"{num} - {direction}: {side}");
            // }
        }
    }

    public class Cube
    {
        public class Face
        {
            public class Row
            {
                public class Column
                {
                    [JsonProperty("left")] public string? Left { get; set; }
                    [JsonProperty("right")] public string? Right { get; set; }
                    [JsonProperty("top")] public string? Top { get; set; }
                    [JsonProperty("bottom")] public string? Bottom { get; set; }
                    [JsonProperty("front")] public string? Front { get; set; }
                    [JsonProperty("back")] public string? Back { get; set; }
                }

                [JsonProperty("leftColumn")] public Column LeftColumn { get; set; }
                [JsonProperty("middleColumn")] public Column MiddleColumn { get; set; }
                [JsonProperty("rightColumn")] public Column RightColumn { get; set; }
            }

            [JsonProperty("topRow")] public Row TopRow { get; set; }
            [JsonProperty("middleRow")] public Row MiddleRow { get; set; }
            [JsonProperty("bottomRow")] public Row BottomRow { get; set; }
        }

        [JsonProperty("frontFace")] public Face FrontFace { get; set; }

        [JsonProperty("middleFace")] public Face MiddleFace { get; set; }

        [JsonProperty("backFace")] public Face BackFace { get; set; }
    }
}
