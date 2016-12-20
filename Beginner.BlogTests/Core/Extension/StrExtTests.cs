using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beginner.Blog.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beginner.Blog.Core.Extension.Tests
{
    [TestClass()]
    public class StrExtTests
    {
        [TestMethod()]
        public void ToMd5Test()
        {
            
            Assert.Fail(StrExt.ToMd5("123456"), "E10ADC3949BA59ABBE56E057F20F883E");
        }

        [TestMethod()]
        public void mainTest()
        {
            Assert.Fail();
        }
    }
}