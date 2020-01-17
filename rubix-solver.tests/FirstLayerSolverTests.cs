using NUnit.Framework;
using rubix_solver.Solvers;

namespace rubix_solver.tests
{
    [TestFixture]
    public class FirstLayerSolverTests
    {
        private readonly Block[,,] _solvedCube =
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
        public void FirstLayerSolver_CorrectlySolvesCorners_GivenCornerBetweenWrongSidesOnFront()
        {
            var cube = new RubixCube(new[,,]
            {
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, Colour.White, null),
                        new Block(null, null, Colour.Green, null, Colour.White, null),
                        new Block(null, Colour.Blue, Colour.White, null, Colour.Red, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, Colour.White, null),
                        new Block(null, null, null, null, Colour.White, null),
                        new Block(null, Colour.Orange, null, null, Colour.White, null)
                    },
                    {
                        new Block(Colour.Orange, null, null, Colour.Green, Colour.White, null),
                        new Block(null, null, null, Colour.Blue, Colour.White, null),
                        new Block(null, Colour.Yellow, null, Colour.Orange, Colour.Green, null)
                    }
                },
                {
                    {
                        new Block(Colour.Yellow, null, Colour.Orange, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Yellow, Colour.Green, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, null),
                        new Block(null, null, null, null, null, null),
                        new Block(null, Colour.Orange, null, null, null, null)
                    },
                    {
                        new Block(Colour.Green, null, null, Colour.Orange, null, null),
                        new Block(null, null, null, Colour.Blue, null, null),
                        new Block(null, Colour.Yellow, null, Colour.Red, null, null)
                    }
                },
                {
                    {
                        new Block(Colour.Green, null, Colour.Yellow, null, null, Colour.Red),
                        new Block(null, null, Colour.Yellow, null, null, Colour.Blue),
                        new Block(null, Colour.Orange, Colour.Blue, null, null, Colour.White)
                    },
                    {
                        new Block(Colour.Blue, null, null, null, null, Colour.Red),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Orange, null, null, null, Colour.Blue)
                    },
                    {
                        new Block(Colour.Yellow, null, null, Colour.Blue, null, Colour.Orange),
                        new Block(null, null, null, Colour.Red, null, Colour.Green),
                        new Block(null, Colour.Yellow, null, Colour.Blue, null, Colour.Red)
                    }
                }
            });

            var solver = new FirstLayerSolver(cube);
            solver.SolveCorners();
            
            var secondLayerSolver = new SecondLayerSolver(cube);
            secondLayerSolver.Solve();

            var thirdLayerSolver = new ThirdLayerSolver(cube);
            thirdLayerSolver.Solve();

            Assert.That(_solvedCube, Is.EquivalentTo(cube.Cube));
        }

        [Test]
        public void FirstLayerSolver_CorrectlySolvesCorners_GivenCornerBetweenCorrectSidesOnFront()
        {
            var cube = new RubixCube(new[,,]
            {
                {
                    {
                        new Block(Colour.Green, null, Colour.White, null, Colour.Red, null),
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
                        new Block(null, Colour.Blue, null, Colour.White, Colour.Orange, null)
                    }
                },
                {
                    {
                        new Block(Colour.Red, null, Colour.Green, null, null, null),
                        new Block(null, null, Colour.Green, null, null, null),
                        new Block(null, Colour.Blue, Colour.Orange, null, null, null)
                    },
                    {
                        new Block(Colour.Red, null, null, null, null, null),
                        new Block(null, null, null, null, null, null),
                        new Block(null, Colour.Orange, null, null, null, null)
                    },
                    {
                        new Block(Colour.Yellow, null, null, Colour.Red, null, null),
                        new Block(null, null, null, Colour.Blue, null, null),
                        new Block(null, Colour.Blue, null, Colour.Red, null, null)
                    }
                },
                {
                    {
                        new Block(Colour.Blue, null, Colour.Red, null, null, Colour.Yellow),
                        new Block(null, null, Colour.Yellow, null, null, Colour.Green),
                        new Block(null, Colour.Yellow, Colour.Green, null, null, Colour.Red)
                    },
                    {
                        new Block(Colour.Blue, null, null, null, null, Colour.Yellow),
                        new Block(null, null, null, null, null, Colour.Yellow),
                        new Block(null, Colour.Yellow, null, null, null, Colour.Orange)
                    },
                    {
                        new Block(Colour.Yellow, null, null, Colour.Orange, null, Colour.Green),
                        new Block(null, null, null, Colour.Green, null, Colour.Orange),
                        new Block(null, Colour.Orange, null, Colour.Blue, null, Colour.Yellow)
                    }
                }
            });

            var solver = new FirstLayerSolver(cube);
            solver.SolveCorners();

            var secondLayerSolver = new SecondLayerSolver(cube);
            secondLayerSolver.Solve();

            var thirdLayerSolver = new ThirdLayerSolver(cube);
            thirdLayerSolver.Solve();

            Assert.That(_solvedCube, Is.EquivalentTo(cube.Cube));
        }
    }
}
