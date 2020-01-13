using NUnit.Framework;
using rubix_solver.Solvers;

namespace rubix_solver.tests
{
    [TestFixture]
    public class SecondLayerSolverTests
    {
        private readonly Block[,,] _solvedCube = {
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
                        new Block(Colour.Red, null, Colour.Green, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Orange, Colour.Green, null, null, null)
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
                        new Block(null, null, Colour.Green, null, null, Colour.Yellow),
                        new Block(null, Colour.Orange, Colour.Green, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, Colour.Yellow),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Blue, null, Colour.Yellow),
                        new Block(null, null, null, Colour.Blue, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, Colour.Blue, null, Colour.Yellow)
                    }
                }
            };
        
        [Test]
        public void SolveSecondLayer_IsCorrect_GivenSwitchableMiddleEdges()
        {
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
                        new Block(Colour.Red, null, Colour.Green, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Orange, Colour.Green, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, null),
                        new Block(null, null, null, null, null, null),
                        new Block(null, Colour.Orange, null, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, Colour.Yellow, null, null),
                        new Block(null, null, null, Colour.Blue, null, null),
                        new Block(null, Colour.Green, null, Colour.Yellow, null, null)
                    }
                },
                {
                    {
                        new Block(Colour.Orange, null, Colour.Blue, null, null, Colour.Yellow),
                        new Block(null, null, Colour.Blue, null, null, Colour.Red),
                        new Block(null, Colour.Blue, Colour.Yellow, null, null, Colour.Red)
                    },
                    {
                        new Block(Colour.Yellow, null, null, null, null, Colour.Orange),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Blue, null, null, null, Colour.Yellow)
                    },
                    {
                        new Block(Colour.Green, null, null, Colour.Red, null, Colour.Yellow),
                        new Block(null, null, null, Colour.Orange, null, Colour.Blue),
                        new Block(null, Colour.Orange, null, Colour.Yellow, null, Colour.Green)
                    }
                }
            });
            
            var solver = new SecondLayerSolver(cube);
            solver.Solve();
            
            var thirdLayerSolver = new ThirdLayerSolver(cube);
            thirdLayerSolver.Solve();
            
            Assert.That(_solvedCube, Is.EquivalentTo(cube.Cube));
        }
        
        [Test]
        public void SolveSecondLayer_IsCorrect_GivenNoSwitchableMiddleEdges()
        {
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
            
            var solver = new SecondLayerSolver(cube);
            solver.Solve();
            
            var thirdLayerSolver = new ThirdLayerSolver(cube);
            thirdLayerSolver.Solve();
            
            Assert.That(_solvedCube, Is.EquivalentTo(cube.Cube));
        }
    }
}