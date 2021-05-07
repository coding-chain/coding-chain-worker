using NUnit.Framework;

namespace csharp_template
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            new Sut().Foo();
            
            //RESERVED
        }
    }
}