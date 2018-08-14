using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GenericCollections.Tests
{
    [TestFixture]
    public class SetTests
    {
        BinaryTreeSet<int> _intSet;

        [SetUp]
        public void Init()
        {
            _intSet = new BinaryTreeSet<int>();
        }

        #region Exceptions
        #endregion

        #region Add and Contains
        [TestCase(new int[] { 1, 2, 3})]
        [TestCase(new int[] { -100, 10567, 2000, -3453, 0, int.MinValue, 800, int.MaxValue })]
        public void Add_DifferentIntValues_CorrectInserting(int[] values)
        {
            AddValues(values);

            foreach (var value in values)
            {
                _intSet.Contains(value);
            }
        }

        [TestCase(1, ExpectedResult = false)]
        [TestCase(int.MinValue, ExpectedResult = false)]
        [TestCase(int.MaxValue, ExpectedResult = false)]
        public bool Add_SameIntValues_CorrectInserting(int value)
        {
            _intSet.Add(value);

            return _intSet.Add(value);
        }
        #endregion

        #region Remove
        [TestCase(new int[] { 100, 300, 1}, 300, ExpectedResult = false)]
        [TestCase(new int[] { -100, 10567, 2000, -3453, 0, int.MinValue, 800, int.MaxValue }, int.MinValue, ExpectedResult = false)]
        public bool Remove_IntValue_CorrectedResult(int[] values, int value)
        {
            AddValues(values);

            _intSet.Remove(value);

            return _intSet.Contains(value);
        }

        [TestCase(new int[] { 100, 300, 1 }, -100, ExpectedResult = false)]
        public bool Remove_UnexistingIntValue_CorrectedResult(int[] values, int value)
        {
            AddValues(values);

            return _intSet.Remove(value);
        }
        #endregion

        #region UnionWith
        [TestCase(new int[] { 4, 5, 6 }, new int[] { 2, 3 },  new int[] { 2, 3, 4, 5, 6 })]
        public void Unionwith_DifferentIntValues_CorrectInserting(int[] values, int[] valuesForUnion, int[] expected)
        {
            AddValues(values);

            _intSet.UnionWith(valuesForUnion);

            int i = 0;
            foreach (var element in _intSet)
            {
                Assert.AreEqual(expected[i++], element);
            }
        }
        #endregion

        private void AddValues(int[] values)
        {
            foreach (var value in values)
            {
                _intSet.Add(value);
            }
        }
    }
}
