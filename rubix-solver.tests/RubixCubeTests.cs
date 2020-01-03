using System;
using NUnit.Framework;

namespace rubix_solver.tests
{
    public class RubixCubeTests
    {
        private RubixCube cube;
        private Block[,] newFace = {
            {
                new Block(Colour.Blue, null, Colour.Yellow, null, Colour.Green, null),
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
                new Block(null, Colour.Red, null, Colour.Orange, Colour.White, null)
            }
        };
        
        [SetUp]
        public void Setup()
        {
            cube = new RubixCube();
        }

        [TestCase(Layer.Front, Colour.White)]
        [TestCase(Layer.Left, Colour.Red)]
        [TestCase(Layer.Right, Colour.Orange)]
        [TestCase(Layer.Top, Colour.Green)]
        [TestCase(Layer.Bottom, Colour.Blue)]
        [TestCase(Layer.Back, Colour.Yellow)]
        public void CubeStarts_With_CorrectFaceColours(Layer layer, Colour colour)
        {
            var face = cube.GetFace(layer);
            Assert.AreEqual(9, face.Length);

            foreach (var block in face)
            {
                var blockFace = layer switch
                {
                    Layer.Front => block.Front,
                    Layer.Back => block.Back,
                    Layer.Left => block.Left,
                    Layer.Right => block.Right,
                    Layer.Top => block.Top,
                    Layer.Bottom => block.Bottom,
                    _ => null
                };

                Assert.AreEqual(colour, blockFace);
            }
        }


        [TestCase(Layer.Front, Layer.Left, Colour.Red)]
        [TestCase(Layer.Front, Layer.Right, Colour.Orange)]
        [TestCase(Layer.Front, Layer.Top, Colour.Green)]
        [TestCase(Layer.Front, Layer.Bottom, Colour.Blue)]
        [TestCase(Layer.Back, Layer.Left, Colour.Red)]
        [TestCase(Layer.Back, Layer.Right, Colour.Orange)]
        [TestCase(Layer.Back, Layer.Top, Colour.Green)]
        [TestCase(Layer.Back, Layer.Bottom, Colour.Blue)]
        [TestCase(Layer.Left, Layer.Back, Colour.Yellow)]
        [TestCase(Layer.Left, Layer.Front, Colour.White)]
        [TestCase(Layer.Left, Layer.Top, Colour.Green)]
        [TestCase(Layer.Left, Layer.Bottom, Colour.Blue)]        
        [TestCase(Layer.Right, Layer.Back, Colour.Yellow)]
        [TestCase(Layer.Right, Layer.Front, Colour.White)]
        [TestCase(Layer.Right, Layer.Top, Colour.Green)]
        [TestCase(Layer.Right, Layer.Bottom, Colour.Blue)]
        [TestCase(Layer.Top, Layer.Back, Colour.Yellow)]
        [TestCase(Layer.Top, Layer.Front, Colour.White)]
        [TestCase(Layer.Top, Layer.Left, Colour.Red)]
        [TestCase(Layer.Top, Layer.Right, Colour.Orange)]
        [TestCase(Layer.Bottom, Layer.Back, Colour.Yellow)]
        [TestCase(Layer.Bottom, Layer.Front, Colour.White)]
        [TestCase(Layer.Bottom, Layer.Left, Colour.Red)]
        [TestCase(Layer.Bottom, Layer.Right, Colour.Orange)]
        public void CubeStarts_With_CorrectFaceEdgeColours(Layer faceLayer, Layer edgeLayer, Colour? colour)
        {
            var face = cube.GetFace(faceLayer);
            Assert.AreEqual(9, face.Length);

            for (var i = 0; i < 3; i++)
            {
                Block block;

                switch (edgeLayer)
                {
                    case Layer.Left when faceLayer == Layer.Front:
                        block = face[i, 0];
                        Assert.AreEqual(colour, block.Left);
                        break;                    
                    case Layer.Left when faceLayer == Layer.Back:
                        block = face[i, 2];
                        Assert.AreEqual(colour, block.Left);
                        break;
                    case Layer.Right when faceLayer == Layer.Front:
                        block = face[i, 2];
                        Assert.AreEqual(colour, block.Right);
                        break;
                    case Layer.Right when faceLayer == Layer.Back:
                        block = face[i, 0];
                        Assert.AreEqual(colour, block.Right);
                        break;
                    case Layer.Top:
                        block = face[0, i];
                        Assert.AreEqual(colour, block.Top);
                        break;
                    case Layer.Bottom:
                        block = face[2, i];
                        Assert.AreEqual(colour, block.Bottom);
                        break;
                }
            }
        }

        [Test]
        public void GetFrontFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Layer.Front);
            Assert.AreEqual(Colour.Red, face[0, 0].Left);
            Assert.AreEqual(Colour.White, face[0, 0].Front);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }
        
        [Test]
        public void GetBackFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Layer.Back);
            Assert.AreEqual(Colour.Orange, face[0, 0].Right);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Back);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Left);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Back);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }        
        
        [Test]
        public void GetTopFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Layer.Top);
            Assert.AreEqual(Colour.Red, face[0, 0].Left);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Back);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Green, face[2, 2].Top);
        }
        
        [Test]
        public void GetBottomFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Layer.Bottom);
            Assert.AreEqual(Colour.Red, face[0, 0].Left);
            Assert.AreEqual(Colour.White, face[0, 0].Front);
            Assert.AreEqual(Colour.Blue, face[0, 0].Bottom);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Back);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }
        
        [Test]
        public void GetLeftFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Layer.Left);
            Assert.AreEqual(Colour.Red, face[0, 0].Left);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Back);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Left);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }
        
        [Test]
        public void GetRightFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Layer.Right);
            Assert.AreEqual(Colour.Orange, face[0, 0].Right);
            Assert.AreEqual(Colour.White, face[0, 0].Front);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Back);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }

        [Test]
        public void IsSolved_Correctly_Identifies_SolvedRubixCube()
        {
            Assert.IsTrue(cube.IsSolved());
        }
        
        [Test]
        public void IsSolved_Correctly_Identifies_NonSolvedRubixCube()
        {
            var unsolvedCube = new RubixCube(new[,,]
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
            });
            
            Assert.IsFalse(unsolvedCube.IsSolved());
        }

        [Test]
        public void SetFrontFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Layer.Front);
            var face = cube.GetFace(Layer.Front);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }
        
        [Test]
        public void SetBackFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Layer.Back);
            var face = cube.GetFace(Layer.Back);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }
        
        [Test]
        public void SetLeftFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Layer.Left);
            var face = cube.GetFace(Layer.Left);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }
        
        [Test]
        public void SetRightFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Layer.Right);
            var face = cube.GetFace(Layer.Right);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }
        
        [Test]
        public void SetTopFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Layer.Top);
            var face = cube.GetFace(Layer.Top);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }
        
        [Test]
        public void SetBottomFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Layer.Bottom);
            var face = cube.GetFace(Layer.Bottom);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }

        [Test]
        public void RotateAntiClockwise_CorrectlyRotatesTheFrontFace()
        {
            cube.RotateAntiClockwise(Layer.Front);
            var face = cube.GetFace(Layer.Front);

            Assert.AreEqual(Colour.Green, face[0, 0].Left);
            Assert.AreEqual(Colour.White, face[0, 0].Front);
            Assert.AreEqual(Colour.Orange, face[0, 0].Top);
            Assert.AreEqual(Colour.Blue, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Red, face[2, 2].Bottom);
        }

        [Test]
        public void RotateAntiClockwise_CorrectlyRotatesTheBackFace()
        {
            cube.RotateAntiClockwise(Layer.Back);
            var face = cube.GetFace(Layer.Back);

            Assert.AreEqual(Colour.Green, face[0, 0].Right);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Back);
            Assert.AreEqual(Colour.Red, face[0, 0].Top);
            Assert.AreEqual(Colour.Blue, face[2, 2].Left);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Back);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }

        [Test]
        public void RotateAntiClockwise_CorrectlyRotatesTheTopFace()
        {
            cube.RotateAntiClockwise(Layer.Top);
            var face = cube.GetFace(Layer.Top);

            Assert.AreEqual(Colour.Yellow, face[0, 0].Left);
            Assert.AreEqual(Colour.Orange, face[0, 0].Back);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.White, face[2, 2].Right);
            Assert.AreEqual(Colour.Red, face[2, 2].Front);
            Assert.AreEqual(Colour.Green, face[2, 2].Top);
        }

        [Test]
        public void RotateAntiClockwise_CorrectlyRotatesTheBottomFace()
        {
            cube.RotateAntiClockwise(Layer.Bottom);
            var face = cube.GetFace(Layer.Bottom);

            Assert.AreEqual(Colour.White, face[0, 0].Left);
            Assert.AreEqual(Colour.Orange, face[0, 0].Front);
            Assert.AreEqual(Colour.Blue, face[0, 0].Bottom);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Right);
            Assert.AreEqual(Colour.Red, face[2, 2].Back);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }

        [Test]
        public void RotateAntiClockwise_CorrectlyRotatesTheLeftFace()
        {
            cube.RotateAntiClockwise(Layer.Left);
            var face = cube.GetFace(Layer.Left);

            Assert.AreEqual(Colour.Red, face[0, 0].Left);
            Assert.AreEqual(Colour.Green, face[0, 0].Back);
            Assert.AreEqual(Colour.White, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Left);
            Assert.AreEqual(Colour.Blue, face[2, 2].Front);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Bottom);
        }
        
        [Test]
        public void RotateAntiClockwise_CorrectlyRotatesTheRightFace()
        {
            cube.RotateAntiClockwise(Layer.Right);
            var face = cube.GetFace(Layer.Right);

            Assert.AreEqual(Colour.Orange, face[0, 0].Right);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.Blue, face[2, 2].Back);
            Assert.AreEqual(Colour.White, face[2, 2].Bottom);
        }
        
        //TODO: Test RotateClockwise
    }
}