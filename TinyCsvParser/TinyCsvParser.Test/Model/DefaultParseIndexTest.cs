using NUnit.Framework;
using TinyCsvParser.Load;

namespace TinyCsvParser.Test.Model
{
    [TestFixture]
    public class DefaultParseIndexTest
    {
        protected DefaultParseIndex parse = new DefaultParseIndex();
        private string[] m_indexs = new[] {"BB2","AA13:BB13"};
        private int[][] results = { new[]{53,1},new []{26,12,53,12}};

        [Test]
        public void ParseIndexTest()
        {
            for (var i = 0; i < m_indexs.Length; i++)
            {
                int[] ary = parse.ParseIndex(m_indexs[i]);
                for (var i1 = 0; i1 < results[i].Length; i1++)
                {
                    Assert.AreEqual(ary[i1],results[i][i1]);
                }
            }
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
            string res = parse.GetIndex(testdata);
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
            string res = parse.GetIndex(testdata2);
            Assert.AreEqual(res, "E18");
        }

    }
}
