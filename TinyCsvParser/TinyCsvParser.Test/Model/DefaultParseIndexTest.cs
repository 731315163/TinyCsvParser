using NUnit.Framework;
using TinyCsvParser.Ranges;

namespace TinyCsvParser.Test.Model
{
    [TestFixture]
    public class DefaultParseIndexTest
    {
        protected IParseAddress parse = ParseAddress.Instance;

        [Test]
        public void ParseIndexTest()
        {
            string index = "AA13:BB13";
            int[] result = { 26, 12, 53, 12 };
            TableRect rect = parse.ParseRect(index);
            Assert.AreEqual(rect.x, result[0]);
            Assert.AreEqual(rect.y, result[1]);
            Assert.AreEqual(rect.width, result[2] - result[0]+1);
            Assert.AreEqual(rect.heigh, result[3] - result[1]+1);
        }
        [Test]
        public void ParseIndexTest1()
        {
            string index = "BB2";
            int[] result = { 53, 1 };
            TableRect rect = parse.ParseRect(index);
            Assert.AreEqual(rect.x, result[0]);
            Assert.AreEqual(rect.y, result[1]);



        }
        protected string testdata = "[Table]Sheet!D4:E18";
        [Test]
       public void GetTableNameTest1()
        {
            string res = parse.GetTableName(testdata);
            Assert.AreEqual(res,"Table");
        }
        [Test]
       public void GetSheetNameTest1()
        {
            string res = parse.GetSheetName(testdata);
            Assert.AreEqual(res,"Sheet");
        }
        [Test]
        public void GetIndexTest1()
        {
            string res = parse.GetRect(testdata);
            Assert.AreEqual(res, "D4:E18");
        }
        protected string testdata2 = "E18";
        [Test]
        public void GetTableNameTest2()
        {
            string res = parse.GetTableName(testdata2);
            Assert.AreEqual(res, string.Empty);
        }
        [Test]
        public void GetSheetNameTest2()
        {
            string res = parse.GetSheetName(testdata2);
            Assert.AreEqual(res, string.Empty);
        }
        [Test]
        public void GetIndexTest2()
        {
            string res = parse.GetRect(testdata2);
            Assert.AreEqual(res, "E18");
        }

    }
}
