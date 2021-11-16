using NUnit.Framework;
using Studying_app_kg;
using Studying_app_kg.Model;

namespace FractalTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RecursionTest()
        {
            Fractal_ZSinZ fractal = new Fractal_ZSinZ();

            Assert.Pass();
        }
    }
}