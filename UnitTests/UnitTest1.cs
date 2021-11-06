using System;
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
        public void ValidIdParserCheckTest()
        {
            // testing ValidIdParser.Check method. It should verify that the argument number equals to exact 6 digits (4 for year + 2 for month)
            
            int x = 202108; // case 1: Six Digit number and a valid date
            Assert.True(ValidIdParser.Check(x)); // should be true
            
            int y = 2024089; // case 2: Seven Digit number and an invalid date
            Assert.False(ValidIdParser.Check(y)); // should be false 
            
            int z = 20217; // case 3: Five Digit number and an invalid date
            Assert.False(ValidIdParser.Check(z)); // should be false 
            
            int a = 000001; // case 4: Six Digit number and an invalid date
            Assert.False(ValidIdParser.Check(a)); // should be true
            
            // Conclusion. This parser works with pure data. But it should be modified to accept only correct data
            // (these is no 0000 year) and make it work with a month digit without zero digit (like 20217, people sometimes are lazy)
            // But only with correct month. 20210 is still an invalid date
        }
        
        
        [Test]
        public void TakeNDigitsTest()
        {
            // MySimpleMath.TakeNDigits test 
            // This function takes digits from first digit, to it's n parameter
            
            int x = 2022, x1 = 2; // case 1: 2020, it's x2 two digits are 20
            Assert.AreEqual(MySimpleMath.TakeNDigits(x,x1),20); // should return 20 or true
            
            int y = 9999, y1 = 2; // case 2: 9999, it's y2 two digits are 99
            Assert.AreEqual(MySimpleMath.TakeNDigits(y,y1),99); // should return 99 or true
            
            int z = 0000, z1 = 4; // case 3: 0000, it's y2 four digits are 0000
            Assert.AreEqual(MySimpleMath.TakeNDigits(z,z1),0000); // should return 0000 or true
            
            int a = 1234123, a1 = 123; // case 4: 1234123, it's parameter is 123 and there are not so many digits 
            Assert.AreEqual(MySimpleMath.TakeNDigits(a,a1),1234123); // should return true and return all digits
            
            int b = 123, b1 = 123; // case 5: 123, it's parameter is 123 and there are not so many digits 
            Assert.AreEqual(MySimpleMath.TakeNDigits(b,b1),123); // should return true and return all digits anyway
        }
    }
}