using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace rubix_solver.tests
{
    [TestFixture]
    public class GoldenTests
    {
        [Test, Repeat(100)]
        public void EndToEndTest()
        {
            var cube = new RubixCube();
            cube.Randomise();
            cube.Solve();

            Assert.That(cube.IsSolved, Is.True);
        }
    }
}
