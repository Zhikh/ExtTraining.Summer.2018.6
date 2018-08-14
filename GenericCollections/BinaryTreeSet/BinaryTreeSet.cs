using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GenericCollections
{
    public class BinaryTreeSet<T> : ISet<T>
    {
        private BinarySearchTree<T> _tree;
        // TODO: add version

        public BinaryTreeSet()
        {
            _tree = new BinarySearchTree<T>();

            Count = 0;
        }

        public int Count { get; private set; }

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

            bool result = Contains(item);
            if (result)
            {
                Count++;
            }

            return result;
        }

        public void Clear()
        {
            _tree.Clear();
            Count = 0;
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
            return IsSubset(this, other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return IsSubset(other, this);
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

            bool result = !Contains(item);
            if (result)
            {
                Count--;
            }

            return result;
        }

        // TODO
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            BinaryTreeSet<T> set = other as BinaryTreeSet<T>;



            throw new NotImplementedException();
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
            else if (other == this)
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
