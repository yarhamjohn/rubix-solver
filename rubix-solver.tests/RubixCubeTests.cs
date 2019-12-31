using System;
using NUnit.Framework;

namespace rubix_solver.tests
{
    public class RubixCubeTests
    {
        private RubixCube cube;

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
                    case Layer.Left:
                        block = face[i, 0];
                        Assert.AreEqual(colour, block.Left);
                        break;
                    case Layer.Right:
                        block = face[i, 2];
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
    }
}