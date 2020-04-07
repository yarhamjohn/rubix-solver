using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestCase(Side.Front, Colour.White)]
        [TestCase(Side.Left, Colour.Red)]
        [TestCase(Side.Right, Colour.Orange)]
        [TestCase(Side.Top, Colour.Green)]
        [TestCase(Side.Bottom, Colour.Blue)]
        [TestCase(Side.Back, Colour.Yellow)]
        public void CubeStarts_With_CorrectFaceColours(Side side, Colour colour)
        {
            var face = cube.GetFace(side);
            Assert.AreEqual(9, face.Length);

            foreach (var block in face)
            {
                var blockFace = side switch
                {
                    Side.Front => block.Front,
                    Side.Back => block.Back,
                    Side.Left => block.Left,
                    Side.Right => block.Right,
                    Side.Top => block.Top,
                    Side.Bottom => block.Bottom,
                    _ => null
                };

                Assert.AreEqual(colour, blockFace);
            }
        }


        [TestCase(Side.Front, Side.Left, Colour.Red)]
        [TestCase(Side.Front, Side.Right, Colour.Orange)]
        [TestCase(Side.Front, Side.Top, Colour.Green)]
        [TestCase(Side.Front, Side.Bottom, Colour.Blue)]
        [TestCase(Side.Back, Side.Left, Colour.Red)]
        [TestCase(Side.Back, Side.Right, Colour.Orange)]
        [TestCase(Side.Back, Side.Top, Colour.Green)]
        [TestCase(Side.Back, Side.Bottom, Colour.Blue)]
        [TestCase(Side.Left, Side.Back, Colour.Yellow)]
        [TestCase(Side.Left, Side.Front, Colour.White)]
        [TestCase(Side.Left, Side.Top, Colour.Green)]
        [TestCase(Side.Left, Side.Bottom, Colour.Blue)]
        [TestCase(Side.Right, Side.Back, Colour.Yellow)]
        [TestCase(Side.Right, Side.Front, Colour.White)]
        [TestCase(Side.Right, Side.Top, Colour.Green)]
        [TestCase(Side.Right, Side.Bottom, Colour.Blue)]
        [TestCase(Side.Top, Side.Back, Colour.Yellow)]
        [TestCase(Side.Top, Side.Front, Colour.White)]
        [TestCase(Side.Top, Side.Left, Colour.Red)]
        [TestCase(Side.Top, Side.Right, Colour.Orange)]
        [TestCase(Side.Bottom, Side.Back, Colour.Yellow)]
        [TestCase(Side.Bottom, Side.Front, Colour.White)]
        [TestCase(Side.Bottom, Side.Left, Colour.Red)]
        [TestCase(Side.Bottom, Side.Right, Colour.Orange)]
        public void CubeStarts_With_CorrectFaceEdgeColours(Side faceSide, Side edgeSide, Colour? colour)
        {
            var face = cube.GetFace(faceSide);
            Assert.AreEqual(9, face.Length);

            for (var i = 0; i < 3; i++)
            {
                Block block;

                switch (edgeSide)
                {
                    case Side.Left when faceSide == Side.Front:
                        block = face[i, 0];
                        Assert.AreEqual(colour, block.Left);
                        break;                    
                    case Side.Left when faceSide == Side.Back:
                        block = face[i, 2];
                        Assert.AreEqual(colour, block.Left);
                        break;
                    case Side.Right when faceSide == Side.Front:
                        block = face[i, 2];
                        Assert.AreEqual(colour, block.Right);
                        break;
                    case Side.Right when faceSide == Side.Back:
                        block = face[i, 0];
                        Assert.AreEqual(colour, block.Right);
                        break;
                    case Side.Top:
                        block = face[0, i];
                        Assert.AreEqual(colour, block.Top);
                        break;
                    case Side.Bottom:
                        block = face[2, i];
                        Assert.AreEqual(colour, block.Bottom);
                        break;
                }
            }
        }

        [Test]
        public void GetFrontFace_Returns_CorrectlyOrientatedFace()
        {
            var face = cube.GetFace(Side.Front);
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
            var face = cube.GetFace(Side.Back);
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
            var face = cube.GetFace(Side.Top);
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
            var face = cube.GetFace(Side.Bottom);
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
            var face = cube.GetFace(Side.Left);
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
            var face = cube.GetFace(Side.Right);
            Assert.AreEqual(Colour.Orange, face[0, 0].Right);
            Assert.AreEqual(Colour.White, face[0, 0].Front);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Back);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }

        [Test]
        public void SetFrontFace_CorrectlySetsTheTargetedFace()
        {
            cube.SetFace(newFace, Side.Front);
            var face = cube.GetFace(Side.Front);

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
            cube.SetFace(newFace, Side.Back);
            var face = cube.GetFace(Side.Back);

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
            cube.SetFace(newFace, Side.Left);
            var face = cube.GetFace(Side.Left);

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
            cube.SetFace(newFace, Side.Right);
            var face = cube.GetFace(Side.Right);

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
            cube.SetFace(newFace, Side.Top);
            var face = cube.GetFace(Side.Top);

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
            cube.SetFace(newFace, Side.Bottom);
            var face = cube.GetFace(Side.Bottom);

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
            cube.RotateAntiClockwise(Side.Front);
            var face = cube.GetFace(Side.Front);

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
            cube.RotateAntiClockwise(Side.Back);
            var face = cube.GetFace(Side.Back);

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
            cube.RotateAntiClockwise(Side.Top);
            var face = cube.GetFace(Side.Top);

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
            cube.RotateAntiClockwise(Side.Bottom);
            var face = cube.GetFace(Side.Bottom);

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
            cube.RotateAntiClockwise(Side.Left);
            var face = cube.GetFace(Side.Left);

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
            cube.RotateAntiClockwise(Side.Right);
            var face = cube.GetFace(Side.Right);

            Assert.AreEqual(Colour.Orange, face[0, 0].Right);
            Assert.AreEqual(Colour.Green, face[0, 0].Front);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.Blue, face[2, 2].Back);
            Assert.AreEqual(Colour.White, face[2, 2].Bottom);
        }
        
        [Test]
        public void RotateClockwise_CorrectlyRotatesTheFrontFace()
        {
            cube.RotateClockwise(Side.Front);
            var face = cube.GetFace(Side.Front);

            Assert.AreEqual(Colour.Blue, face[0, 0].Left);
            Assert.AreEqual(Colour.White, face[0, 0].Front);
            Assert.AreEqual(Colour.Red, face[0, 0].Top);
            Assert.AreEqual(Colour.Green, face[2, 2].Right);
            Assert.AreEqual(Colour.White, face[2, 2].Front);
            Assert.AreEqual(Colour.Orange, face[2, 2].Bottom);
        }

        [Test]
        public void RotateClockwise_CorrectlyRotatesTheBackFace()
        {
            cube.RotateClockwise(Side.Back);
            var face = cube.GetFace(Side.Back);

            Assert.AreEqual(Colour.Blue, face[0, 0].Right);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Back);
            Assert.AreEqual(Colour.Orange, face[0, 0].Top);
            Assert.AreEqual(Colour.Green, face[2, 2].Left);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Back);
            Assert.AreEqual(Colour.Red, face[2, 2].Bottom);
        }

        [Test]
        public void RotateClockwise_CorrectlyRotatesTheTopFace()
        {
            cube.RotateClockwise(Side.Top);
            var face = cube.GetFace(Side.Top);

            Assert.AreEqual(Colour.White, face[0, 0].Left);
            Assert.AreEqual(Colour.Red, face[0, 0].Back);
            Assert.AreEqual(Colour.Green, face[0, 0].Top);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Right);
            Assert.AreEqual(Colour.Orange, face[2, 2].Front);
            Assert.AreEqual(Colour.Green, face[2, 2].Top);
        }

        [Test]
        public void RotateClockwise_CorrectlyRotatesTheBottomFace()
        {
            cube.RotateClockwise(Side.Bottom);
            var face = cube.GetFace(Side.Bottom);

            Assert.AreEqual(Colour.Yellow, face[0, 0].Left);
            Assert.AreEqual(Colour.Red, face[0, 0].Front);
            Assert.AreEqual(Colour.Blue, face[0, 0].Bottom);
            Assert.AreEqual(Colour.White, face[2, 2].Right);
            Assert.AreEqual(Colour.Orange, face[2, 2].Back);
            Assert.AreEqual(Colour.Blue, face[2, 2].Bottom);
        }

        [Test]
        public void RotateClockwise_CorrectlyRotatesTheLeftFace()
        {
            cube.RotateClockwise(Side.Left);
            var face = cube.GetFace(Side.Left);

            Assert.AreEqual(Colour.Red, face[0, 0].Left);
            Assert.AreEqual(Colour.Blue, face[0, 0].Back);
            Assert.AreEqual(Colour.Yellow, face[0, 0].Top);
            Assert.AreEqual(Colour.Red, face[2, 2].Left);
            Assert.AreEqual(Colour.Green, face[2, 2].Front);
            Assert.AreEqual(Colour.White, face[2, 2].Bottom);
        }
        
        [Test]
        public void RotateClockwise_CorrectlyRotatesTheRightFace()
        {
            cube.RotateClockwise(Side.Right);
            var face = cube.GetFace(Side.Right);

            Assert.AreEqual(Colour.Orange, face[0, 0].Right);
            Assert.AreEqual(Colour.Blue, face[0, 0].Front);
            Assert.AreEqual(Colour.White, face[0, 0].Top);
            Assert.AreEqual(Colour.Orange, face[2, 2].Right);
            Assert.AreEqual(Colour.Green, face[2, 2].Back);
            Assert.AreEqual(Colour.Yellow, face[2, 2].Bottom);
        }

        [Test]
        public void AddingFirstInstruction_AddsInstruction()
        {
            var testCube = new RubixCube { Instructions = new List<(int num, string direction, Side side)>()};
            
            testCube.RotateAntiClockwise(Side.Front);
            
            Assert.That(testCube.Instructions.Count, Is.EqualTo(1));
            Assert.That(testCube.Instructions.Single(), Is.EqualTo((1, "anti-clockwise", Side.Front)));
        }

        [Test]
        public void AddingOppositeDirectionToSameFaceAsPreviousInstruction_RemovesPreviousInstruction()
        {
            var testCube = new RubixCube { Instructions = new List<(int num, string direction, Side side)> { (1, "clockwise", Side.Front) }};
            
            testCube.RotateAntiClockwise(Side.Front);
            
            Assert.That(testCube.Instructions.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddingThirdConsecutiveSameDirectionToSameFace_ReversesDirectionOneRotation()
        {
            var testCube = new RubixCube { Instructions = new List<(int num, string direction, Side side)> { (1, "clockwise", Side.Front), (2, "clockwise", Side.Front) }};
            
            testCube.RotateClockwise(Side.Front);
            
            Assert.That(testCube.Instructions.Count, Is.EqualTo(1));
            Assert.That(testCube.Instructions.Single(), Is.EqualTo((1, "anti-clockwise", Side.Front)));
        }
    }
}
