using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GenericCollections
{
    public class BinaryTreeSet<T> : ISet<T>
    {
        #region Fields
        private readonly IComparer<T> _comparer;
        private BinarySearchTree<T> _tree;
        #endregion

        #region Public API
        /// <summary>
        /// Initializes a new instance of the class that is empty 
        /// and uses the comparer for the set type.
        /// </summary>
        /// <param name="comparer"> Stategy of comparing </param>
        public BinaryTreeSet(IComparer<T> comparer = null)
        {
            _comparer = comparer ??
               (typeof(IComparable<T>).IsAssignableFrom(typeof(T)) ||
               typeof(IComparable).IsAssignableFrom(typeof(T)) ?
               _comparer = Comparer<T>.Default :
               throw new ArgumentNullException("Comparer's indefined for type of T!"));

            _tree = new BinarySearchTree<T>(_comparer);
        }

        /// <summary>
        /// Initializes a new instance of the class that is empty 
        /// and uses the comparer for the set type.
        /// </summary>
        /// <param name="comparison"> Method for comparing </param>
        public BinaryTreeSet(Comparison<T> comparison) : this(Comparer<T>.Create(comparison))
        {
        }

        /// <summary>
        /// Gets the number of elements that are contained in a set.
        /// </summary>
        public int Count => _tree.Count;

        /// <summary>
        /// Returns true if set only foe reading.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Adds the unique element to a set.
        /// </summary>
        /// <param name="item"> The element to add to the set. </param>
        /// <returns> If element's added to set, it's true, else - false </returns>
        public bool Add(T item)
        {
            if (Contains(item))
            {
                return false;
            }

            _tree.Add(item);

            return Contains(item);
        }

        /// <summary>
        /// Removes all elements from set.
        /// </summary>
        public void Clear()
        {
            _tree.Clear();
        }
        
        /// <summary>
        /// Checkes set on containing of element.
        /// </summary>
        /// <param name="item"> Element for checking. </param>
        /// <returns> If set contains element, it's true, else - false. </returns>
        public bool Contains(T item)
        {
            return _tree.Contains(item);
        }

        /// <summary>
        /// Copies the elements of set to array, starting at the specified array index.
        /// </summary>
        /// <param name="array"> Array for copying in. </param>
        /// <param name="arrayIndex"> Index in <paramref name="array" /> at which copying begins. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="arrayIndex" /> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="arrayIndex" /> is greater than the length of the destination <paramref name="array" />.-or-<paramref name="count" /> is larger than the size of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex > Count || 
                arrayIndex > array.Length)
            {
                throw new ArgumentException(nameof(arrayIndex));
            }

            int i = 0;
            int j = 0;
            foreach (var element in _tree)
            {
                if (i == arrayIndex)
                {
                    array[j++] = element;
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary> Removes all elements in the specified collection from the current set. </summary>
        /// <param name="other"> The collection of items to remove from the set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            foreach (var element in other)
            {
                Remove(element);
            }
        }

        /// <summary> Returns an enumerator that iterates through a set.</summary>
        /// <returns> A <see cref="BinaryTreeSet{T}.Enumerator" /> object for the set.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _tree.GetEnumerator();
        }

        /// <summary> Modifies the current set to contain only elements that are present in that object and in the specified collection. </summary>
        /// <param name="other"> The collection to compare to the current set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Count != 0)
            {
                foreach (var element in _tree)
                {
                    if (!other.Contains(element))
                    {
                        Remove(element);
                    }
                }
            }
        }

        /// <summary> Determines whether a set is a proper subset of the specified collection.</summary>
        /// <returns> true if the set is a proper subset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other"> The collection to compare to the current set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return IsProperSubset(this, other);
        }

        /// <summary> Determines whether a set is a proper superset of the specified collection. </summary>
        /// <returns> true if the set is a proper superset of <paramref name="other" />; otherwise, false. </returns>
        /// <param name="other"> The collection to compare to the current set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return IsProperSubset(other, this);
        }

        /// <summary> Determines whether a set is a subset of the specified collection.</summary>
        /// <returns> true if the set is a subset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other"> The collection to compare to the current set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return IsSubset(other, this);
        }

        /// <summary> Determines whether a set is a superset of the specified collection.</summary>
        /// <returns> true if the set is a superset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other"> The collection to compare to the current set.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return IsSubset(this, other);
        }

        /// <summary> Determines whether the current set and a specified collection share common elements.</summary>
        /// <returns>true if the set and <paramref name="other" /> share at least one common element; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (Count == 0)
            {
                return false;
            }

            foreach (var element in other)
            {
                if (Contains(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary> Removes the specified element from a set. </summary>
        /// <returns> true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="item" /> is not found in the set. </returns>
        /// <param name="item"> The element to remove. </param>
        public bool Remove(T item)
        {
            if (!Contains(item))
            {
                return false;
            }

            _tree.Remove(item);

            return !Contains(item);
        }

        /// <summary >Determines whether a set and the specified collection contain the same elements.</summary>
        /// <returns>true if the set is equal to <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Count != other.Count())
            {
                return false;
            }

            T[] array = other.ToArray();

            int i = 0;
            foreach (var element in _tree)
            {
                if (_comparer.Compare(element, array[i++]) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary> Modifies the current set to contain only elements that are present either in that object or in the specified collection, but not both. </summary>
        /// <param name="other"> The collection to compare to the current set. </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Count == 0)
            {
                UnionWith(other);
            }
            else if (SetEquals(other))
            {
                Clear();
            }
            else
            {
                SymmetricExpect(other);
            }
        }

        /// <summary> Modifies the current <see cref="BinaryTreeSet{T}`1" /> object to contain all elements that are present in both itself and in the specified collection. </summary>
        /// <param name="other"> The collection to compare to the current set.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is null.</exception>
        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            foreach (var element in other)
            {
                _tree.Add(element);
            }
        }

        /// <summary>
        /// Adds element to collection.
        /// </summary>
        /// <param name="item"> The element to add to the collection.</param>
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary> Returns an enumerator that iterates through a collection.</summary>
        /// <returns> An <see cref="Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tree.GetEnumerator();
        }
        #endregion

        #region Addition methods
        private bool IsSubset(IEnumerable<T> subset, IEnumerable<T> set)
        {
            if (subset == null)
            {
                throw new ArgumentNullException(nameof(subset));
            }

            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            return !subset.Except(set).Any();
        }

        private bool IsProperSubset(IEnumerable<T> set, IEnumerable<T> subset)
        {
            foreach (var element in subset)
            {
                if (!set.Contains(element))
                {
                    return false;
                }
            }

            return true;
        }

        private void SymmetricExpect(IEnumerable<T> other)
        {
            foreach (var element in other)
            {
                if (!Remove(element))
                {
                    Add(element);
                }
            }
        }
        #endregion
    }
}
