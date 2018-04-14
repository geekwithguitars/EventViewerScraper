using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EventViewerScraper;

namespace EventViewerScraper.Tests
{
    public class evsTests
    {
        [TestFixture]
        public class test1
        {
            //EventViewerScraper123 asdf = new EventViewerScraper();

            [Test]
            public void Test12()
            {
                //Assert
                Assert.That("string", Is.Not.Null.And.EqualTo("string"));

            }
        }
    }
}
