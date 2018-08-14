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

        public BinaryTreeSet(IComparer<T> comparer = null)
        {
            _comparer = comparer ??
               (typeof(IComparable<T>).IsAssignableFrom(typeof(T)) ||
               typeof(IComparable).IsAssignableFrom(typeof(T)) ?
               _comparer = Comparer<T>.Default :
               throw new ArgumentNullException("Comparer's indefined for type of T!"));

            _tree = new BinarySearchTree<T>(_comparer);
        }

        public BinaryTreeSet(Comparison<T> comparison) : this(Comparer<T>.Create(comparison))
        {
        }

        public int Count => _tree.Count;

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Add(T item)
        {
            if (Contains(item))
            {
                return false;
            }

            _tree.Add(item);

            return Contains(item);
        }

        public void Clear()
        {
            _tree.Clear();
        }
        
        public bool Contains(T item)
        {
            return _tree.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            int j = 0;
            foreach(var element in _tree)
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
        
        public IEnumerator<T> GetEnumerator()
        {
            return _tree.GetEnumerator();
        }
        
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
        
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return IsProperSubset(this, other);
        }
        
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return IsProperSubset(other, this);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return IsSubset(other, this);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return IsSubset(this, other);
        }

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
        
        public bool Remove(T item)
        {
            if (!Contains(item))
            {
                return false;
            }

            _tree.Remove(item);

            return !Contains(item);
        }
        
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
                if(_comparer.Compare(element, array[i++]) != 0)
                {
                    return false;
                }
            }

            return true;
        }
        
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
        
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tree.GetEnumerator();
        }

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
