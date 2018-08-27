using NUnit.Framework;
using TinyCsvParser.Load;

namespace TinyCsvParser.Test.Model
{
    [TestFixture]
    public class DefaultParseIndexTest
    {
        private string[] m_indexs = new[] {"BB2","AA13:BB13"};
        private int[][] results = { new[]{53,1},new []{26,12,53,12}};
        [Test]
        public void ParseIndexTest()
        {
            var  parse = new DefaultParseIndex();
            for (var i = 0; i < m_indexs.Length; i++)
            {
                int[] ary = parse.ParseIndex(m_indexs[i]);
                for (var i1 = 0; i1 < results[i].Length; i1++)
                {
                   // throw new Exception(ary[i1].ToString());
                    Assert.AreEqual(ary[i1],results[i][i1]);
                }
            }
        }
    }
}
