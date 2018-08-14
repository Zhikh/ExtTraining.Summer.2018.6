using System;
using System.Linq;
using NUnit.Framework;

namespace GenericCollections.Tests
{
    [TestFixture]
    public class SetTests
    {
        BinaryTreeSet<int> _intSet;
        BinaryTreeSet<int> _filledIntSet;

        [SetUp]
        public void Init()
        {
            _intSet = new BinaryTreeSet<int>();

            _filledIntSet = new BinaryTreeSet<int>();
            _filledIntSet.Add(57);
        }

        #region Exceptions
        [Test]
        public void ExpectWith_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.ExceptWith(null));

        [Test]
        public void IntersectWith_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.IntersectWith(null));

        [Test]
        public void IsProperSubsetOf_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.IsProperSubsetOf(null));

        [Test]
        public void IsProperSupersetOf_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.IsProperSupersetOf(null));

        [Test]
        public void IsSubsetOf_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.IsSubsetOf(null));

        [Test]
        public void IsSupersetOf_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.IsSupersetOf(null));

        [Test]
        public void Overlaps_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.Overlaps(null));

        [Test]
        public void SetEquals_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.SetEquals(null));

        [Test]
        public void SymmetricExceptWith_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.SymmetricExceptWith(null));

        [Test]
        public void UnionWith_NullOther_ArgumentNullException()
            => Assert.Catch<ArgumentNullException>(() => _filledIntSet.UnionWith(null));
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

        #region Count
        [TestCase(new int[] { 1, 2, 3 }, ExpectedResult = 3)]
        [TestCase(new int[] { -4, 2, 3, 0, 18, 189, 290878, 509, 190 }, ExpectedResult = 9)]
        [TestCase(new int[] { -4, 2, 3, 0, 18, 189, 290878, 509, 0, 0, 190 }, ExpectedResult = 9)]
        [TestCase(new int[] { -4, 2, 3, 0, int.MaxValue, 18, 189, 290878, 509, int.MinValue, 190, int.MaxValue },
            ExpectedResult = 11)]
        public int Count_DifferentIntValues_CorrectResult(int[] values)
        {
            AddValues(values);

            return _intSet.Count();
        }
        #endregion

        #region Clear
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, -10 })]
        [TestCase(new int[] { -100, 10567, 2000, -3453, 0, int.MinValue, 800, int.MaxValue })]
        public void Clear_IntValues_CountIsZero(int[] values)
        {
            AddValues(values);

            _intSet.Clear();

            int expected = 0;
            Assert.AreEqual(expected, _intSet.Count);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5, 6})]
        [TestCase(new int[] { -100, 10567, 2000, -3453, 0, int.MinValue, 800, int.MaxValue })]
        public void Clear_IntValues_ElementsWasRemoved(int[] values)
        {
            AddValues(values);

            _intSet.Clear();

            bool expected = false;
            foreach (var value in values)
            {
                Assert.AreEqual(expected, _intSet.Contains(value));
            }
        }
        #endregion

        #region CopyTo
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { -100, 10567, 2000, -3453, 0, int.MinValue, 800, int.MaxValue })]
        public void CopyTo_IntValues_CopyToArrayFromBeginning(int[] values)
        {
            AddValues(values);
            int[] actual = new int[values.Length];

            _intSet.CopyTo(actual, 0);
            Array.Sort(values);

            Assert.AreEqual(values, actual);
        }

        [TestCase(new int[] { 1, 2, 3, 7, 5, 8, 9 }, 3, 
            ExpectedResult = new int[] { 5, 7, 8, 9})]
        [TestCase(new int[] { -100, 10567, 2000, -3453, 0, int.MinValue, 800, int.MaxValue }, 5, 
            ExpectedResult = new int[] { 2000, 10567, int.MaxValue })]
        public int[] CopyTo_IntValues_CopyToArrayFromIndex(int[] values, int index)
        {
            AddValues(values);
            int[] actual = new int[values.Length - index];

            _intSet.CopyTo(actual, index);
            return actual;
        }

        #endregion

        #region ExpectWith
        [TestCase(new int[] { 1, 2, 3 }, new int[] {}, ExpectedResult = new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 2, 1 }, ExpectedResult = new int[] { 3 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 2, 4 }, ExpectedResult = new int[] { 1, 3 })]
        [TestCase(new int[] { 5, 6, 7, 4 }, new int[] { 2, 1 }, ExpectedResult = new int[] { 4, 5, 6, 7 })]
        [TestCase(new int[] { -5, 0, 8, 290, -5908 }, new int[] { -5908, 290, 0 }, 
            ExpectedResult = new int[] { -5, 8 })]
        [TestCase(new int[] { -5, 0, 8, 290, -5908, int.MinValue, 8908, int.MaxValue }, 
            new int[] { 90, 6, int.MaxValue, - 5, 0, 8, -5908, int.MinValue, 290, 8908 }, 
            ExpectedResult = new int[] { })]
        public int[] ExpectWith_IntValues_ElementsFromSecondSetWasRemoved(int[] set, int[] other)
        {
            AddValues(set);

            _intSet.ExceptWith(other);

            return _intSet.ToArray();
        }
        #endregion

        #region IntersectWith
        [TestCase(new int[] { 1, 2, 3 }, new int[] { }, ExpectedResult = new int[] { })]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 2, 1 }, ExpectedResult = new int[] { 1, 2 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 2, 4 }, ExpectedResult = new int[] { 2 })]
        [TestCase(new int[] { 5, 6, 7, 4 }, new int[] { 2, 1 }, ExpectedResult = new int[] { })]
        [TestCase(new int[] { -5, 0, 8, 290, -5908 }, new int[] { -5908, 290, 0 },
          ExpectedResult = new int[] { -5908, 0, 290 })]
        [TestCase(new int[] { -5, 0, 8, 290, -5908, int.MinValue, 8908, int.MaxValue },
          new int[] { 90, 6, int.MaxValue, -5, 0, 8, -5908, int.MinValue, 290, 8908 },
          ExpectedResult = new int[] { int.MinValue, -5908, -5, 0, 8, 290, 8908, int.MaxValue })]
        public int[] IntersectWith_IntValues_ElementsThatNotInSecondSetWasRemoved(int[] set, int[] other)
        {
            AddValues(set);

            _intSet.IntersectWith(other);

            return _intSet.ToArray();
        }
        #endregion

        #region IsProperSubsetOf
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 3 }, ExpectedResult = true)]
        [TestCase(new int[] { }, new int[] { }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 2, 4 }, ExpectedResult = false)]
        [TestCase(new int[] { 5, 6, 7, 4 }, new int[] { 2, 1 }, ExpectedResult = false)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908 }, new int[] { -5908, 290, 0 },
         ExpectedResult = true)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908, int.MinValue, 8908, int.MaxValue },
         new int[] { 90, 6, int.MaxValue, -5, 0, 8, -5908, int.MinValue, 290, 8908 },
         ExpectedResult = false)]
        public bool IsProperSubsetOf_IntValues_CorrectResult(int[] set, int[] subset)
        {
            AddValues(set);
            
            return _intSet.IsProperSubsetOf(subset);
        }
        #endregion

        #region IsProperSupersetOf
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 3, 2, 9, 8 }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 2, 4, 3 }, ExpectedResult = false)]
        [TestCase(new int[] { 5, 6, 7, 4 }, new int[] { 2, 1, 80, 15, 19 }, ExpectedResult = false)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908 }, new int[] { -5908, 290, 0, -5, 8, 9},
        ExpectedResult = true)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908, int.MinValue, 7890, int.MaxValue },
        new int[] { 90, 6, int.MaxValue, -5, 0, 8, -5908, int.MinValue, 290, 8908 },
        ExpectedResult = false)]
        public bool IsProperSupersetOf_IntValues_CorrectResult(int[] subset, int[] set)
        {
            AddValues(subset);

            return _intSet.IsProperSupersetOf(set);
        }
        #endregion

        #region IsSubsetOf
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 3 }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2 }, ExpectedResult = true)]
        [TestCase(new int[] { 5, 6, 7, 4 }, new int[] { 0, 4, 5, 6 }, ExpectedResult = false)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908 }, new int[] { -5, 0, 8, 290 },
         ExpectedResult = true)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908, int.MinValue, 8908, int.MaxValue },
         new int[] {8, 290, 8908, 8908 },
         ExpectedResult = true)]
        public bool IsSubsetOf_IntValues_CorrectResult(int[] set, int[] subset)
        {
            AddValues(set);

            return _intSet.IsSubsetOf(subset);
        }
        #endregion

        #region IsSupersetOf
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3, 4}, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 4 }, new int[] { 1, 2, 5, 7 }, ExpectedResult = false)]
        [TestCase(new int[] { 5, 6, 7, 4 }, new int[] {4, 5, 6, 7 }, ExpectedResult = true)]
        [TestCase(new int[] { -5, 0, 8, 290, -5908 }, new int[] { -5, 0, 8, 290, -5908 },
         ExpectedResult = true)]
        [TestCase(new int[] { 0, 8, 290 },
         new int[] { 0, 8, 290, -5908, int.MinValue, 8908, int.MaxValue },
         ExpectedResult = true)]
        public bool IsSupersetOf_IntValues_CorrectResult(int[] subset, int[] set)
        {
            AddValues(subset);

            return _intSet.IsSupersetOf(set);
        }
        #endregion

        #region Overlaps
        [TestCase(new int[] { 1, 2, 3 }, new int[] {}, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 3, 4, 5 }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2 }, ExpectedResult = true)]
        [TestCase(new int[] { -80, 0, -100, 1000 }, new int[] { 90, 58}, ExpectedResult = false)]
        [TestCase(new int[] { -80, 0, -100, 1000 }, new int[] { 90, 1000, 58 }, ExpectedResult = true)]
        [TestCase(new int[] { -80, 0, int.MaxValue, -100, 1000 }, 
            new int[] { 90, 58, 0, int.MaxValue }, ExpectedResult = true)]
        [TestCase(new int[] { -90, int.MinValue, int.MinValue + 1, int.MaxValue}, 
            new int[] { -90, int.MinValue, int.MaxValue}, ExpectedResult = true)]
        public bool Overlaps_IntValues_CorrectResult(int[] firstSet, int[] secondset)
        {
            AddValues(firstSet);

            return _intSet.Overlaps(secondset);
        }
        #endregion

        #region SetEquals
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1 }, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 3, 2 }, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2 }, ExpectedResult = false)]
        [TestCase(new int[] { -80, 0, -100, 1000 }, new int[] { -100, -80, 0, 1000 }, ExpectedResult = true)]
        [TestCase(new int[] { -80, 0, -100, 1000 }, new int[] { -80, 0, -100, 1000 }, ExpectedResult = false)]
        [TestCase(new int[] { -80, 0, int.MaxValue, -100, 1000 },
           new int[] { -100, -80, 0, 1000, int.MaxValue }, ExpectedResult = true)]
        [TestCase(new int[] { -90, int.MinValue, int.MinValue + 1, int.MaxValue },
           new int[] { int.MinValue, int.MinValue + 1, -90, int.MaxValue }, ExpectedResult = true)]
        public bool SetEquals_IntValues_CorrectResult(int[] firstSet, int[] secondset)
        {
            AddValues(firstSet);

            return _intSet.SetEquals(secondset);
        }
        #endregion

        #region SymmetricExceptWith
        [TestCase(new int[] { }, new int[] { 1, 4, 5 },
            ExpectedResult = new int[] { 1, 4, 5 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1 }, 
            ExpectedResult = new int[] { 2, 3 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 },
            ExpectedResult = new int[] { })]
        [TestCase(new int[] { 968, 0, 567, -1000, 89, -5984, 390 }, new int[] { 968, 0, 1},
            ExpectedResult = new int[] { -5984, -1000, 1, 89, 390, 567})]
        [TestCase(new int[] { 968, 0, 567, -1000, 89, -5984, 390, int.MaxValue, 390}, new int[] { 968, 0, int.MinValue, 1},
            ExpectedResult = new int[] { int.MinValue, -5984, -1000, 1, 89, 390, 567, int.MaxValue })]
        public int[] SymmetricExceptWith_IntValues_CorrectResult(int[] firstSet, int[] secondSet)
        {
            AddValues(firstSet);

            _intSet.SymmetricExceptWith(secondSet);

            return _intSet.ToArray();
        }
        #endregion

        #region Additional methods
        private void AddValues(int[] values)
        {
            foreach (var value in values)
            {
                _intSet.Add(value);
            }
        }
        #endregion
    }
}
