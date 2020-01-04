using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace rubix_solver.tests
{
    [TestFixture]
    public class GoldenTests
    {
        [Test]
        public void EndToEndTest()
        {
            var cube = new RubixCube();
            cube.Randomise();
            
            var task = Task.Run(() => cube.Solve());
            var isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(10000));
 
            if (isCompletedSuccessfully)
            {
                Assert.That(cube.IsSolved, Is.True);
            }
            else
            {
                throw new TimeoutException("The function has taken longer than the maximum time allowed.");
            }
        }
    }
}