using BLL.Utils;
using NUnit.Framework;

namespace UnitTests
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
            // testing ValidIdParser.Check method. It should verify that the argument number equals to exact 6 digits (4 for year + 2 for month)
            
            int x = 202108; // case 1: Six Digit number and a valid date
            Assert.True(ValidIdParser.Check(x)); // should be true
            
            int y = 2024089; // case 2: Seven Digit number and an invalid date
            Assert.False(ValidIdParser.Check(y)); // should be false 
            
            int z = 20217; // case 3: Five Digit number and an invalid date
            Assert.False(ValidIdParser.Check(z)); // should be false 
            
            int a = 000001; // case 4: Six Digit number and an invalid date
            Assert.True(ValidIdParser.Check(a)); // should be true
            
            // Conclusion. This parser works with pure data. But it should be modified to accept only correct data
            // (these is no 0000 year) and make it work with a month digit without zero digit (like 20217, people sometimes are lazy)
            // But only with correct month. 20210 is still an invalid date
        }
    }
}