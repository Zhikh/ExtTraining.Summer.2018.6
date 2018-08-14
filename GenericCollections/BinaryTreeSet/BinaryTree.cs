using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericCollections
{
    public sealed class BinarySearchTree<T> : IEnumerable<T>
    {
        #region Fields
        private readonly IComparer<T> _comparer;
        private Node<T> _root;
        private int _version;
        #endregion

        #region Public methods
        /// <summary>
        /// Initialize BinarySearchTree
        /// </summary>
        /// <param name="comparer"> Protocol for comparing values of type T </param>
        /// <exception cref="ArgumentNullException"> If T hasn't default comparer </exception>
        public BinarySearchTree(IComparer<T> comparer = null)
        {
            _comparer = comparer ??
               (typeof(IComparable<T>).IsAssignableFrom(typeof(T)) ||
               typeof(IComparable).IsAssignableFrom(typeof(T)) ?
               _comparer = Comparer<T>.Default :
               throw new ArgumentNullException("Comparer's indefined for type of T!"));
        }

        /// <summary>
        /// Initialize BinarySearchTree
        /// </summary>
        /// <param name="value"> Value for inserting </param>
        /// <param name="comparer"> Protocol for comparing values of type T </param>
        /// <exception cref="ArgumentNullException"> If T hasn't default comparer </exception>
        public BinarySearchTree(T value, IComparer<T> comparer = null) : this(comparer)
        {
            _root = new Node<T>
            {
                Value = value
            };

            Count++;
        }

        /// <summary>
        /// Count of elements in tree
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Add value to tree
        /// </summary>
        /// <param name="value"> Value for inserting </param>
        public void Add(T value)
        {
            var node = new Node<T>()
            {
                Value = value
            };

            if (IsEmpty())
            {
                _root = node;
                Count++;
                return;
            }

            int result = 0;

            Node<T> current = _root, parent = null;
            while (current != null)
            {
                result = _comparer.Compare(current.Value, value);
                if (result == 0)
                {
                    return;
                }
                else if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
            }

            Count++;
            if (result > 0)
            {
                parent.Left = node;
            }
            else
            {
                parent.Right = node;
            }

            _version++;
        }

        /// <summary>
        /// Check tree on containing item
        /// </summary>
        /// <param name="item"> Item for finding </param>
        /// <returns> If tree containts item, it's true, else - false </returns>
        public bool Contains(T item) 
            => IsContain(_root, item);


        /// <summary>
        /// Clear tree
        /// </summary>
        public void Clear()
        {
            Count = 0;
            _root = null;
        }

        /// <summary>
        /// Use in base inorder of tree
        /// </summary>
        /// <returns> IEnumerator </returns>
        /// <exception cref="ArgumentException"> If collection is changed </exception>
        public IEnumerator<T> GetEnumerator()
        {
            int oldVersion = _version;

            var result = Inorder().GetEnumerator();

            if (oldVersion != _version)
            {
                throw new ArgumentException("Collection can't change during iteration!");
            }

            return result;
        }

        /// <summary>
        /// Move in direction of: root, left child, right child
        /// </summary>
        /// <returns> Collection of elements of tree </returns>
        /// <exception cref="ArgumentException"> If collection is changed </exception>
        public IEnumerable<T> Preorder()
        {
            if (IsEmpty())
            {
                return null;
            }

            int oldVersion = _version;

            var result = GetValuePreorder(_root);

            if (oldVersion != _version)
            {
                throw new ArgumentException("Collection can't change during iteration!");
            }

            return result;
        }

        /// <summary>
        /// Move in direction of: left child, root, right child
        /// </summary>
        /// <returns> Collection of elements of tree </returns>
        /// <exception cref="ArgumentException"> If collection is changed </exception>
        public IEnumerable<T> Inorder()
        {
            if (IsEmpty())
            {
                return null;
            }

            int oldVersion = _version;

            var result = GetValueInorder(_root);

            if (oldVersion != _version)
            {
                throw new ArgumentException("Collection can't change during iteration!");
            }

            return result;
        }

        /// <summary>
        /// Move in direction of: left child, right child, root
        /// </summary>
        /// <returns> Collection of elements of tree </returns>
        /// <exception cref="ArgumentException"> If collection is changed </exception>
        public IEnumerable<T> Postorder()
        {
            if (IsEmpty())
            {
                return null;
            }

            int oldVersion = _version;

            var result = GetValuePostorder(_root);

            if (oldVersion != _version)
            {
                throw new ArgumentException("Collection can't change during iteration!");
            }

            return result;
        }

        /// <summary>
        /// Chech tree on cantaining elements
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _root == null;
        }

        /// <summary>
        /// Remove value form tree
        /// </summary>
        /// <param name="value"></param>
        /// <returns> If value is in tree, true, else - false</returns>
        public bool Remove(T value)
        {
            if (_root == null)
            {
                return false;
            }

            Node<T> parent = null,
                node = FindNode(_root, value, ref parent); 
            
            if (node == null)
            {
                return false;
            }

            Count--;

            if (node.Right == null)
            {
                RelocateNode(parent, node.Left, node.Value);
            }
            else if (node.Right.Left == null)
            {
                node.Right.Left = node.Left;

                RelocateNode(parent, node.Right, node.Value);
            }
            else
            {
                BalanceTree(parent, node);
            }

            return true;
        }
        #endregion

        #region Private method
        // root, left child, right child
        private IEnumerable<T> GetValuePreorder(Node<T> node)
        {
            if (node != null)
            {
                yield return node.Value;

                foreach (var element in GetValuePreorder(node.Left))
                {
                    yield return element;
                }

                foreach (var element in GetValuePreorder(node.Right))
                {
                    yield return element;
                }
            }
        }

        // left child, root, right child
        private IEnumerable<T> GetValueInorder(Node<T> node)
        {
            if (node != null)
            {
                foreach (var element in GetValueInorder(node.Left))
                {
                    yield return element;
                }

                yield return node.Value;

                foreach (var element in GetValueInorder(node.Right))
                {
                    yield return element;
                }
            }
        }

        // left child, right child, root
        private IEnumerable<T> GetValuePostorder(Node<T> node)
        {
            if (node != null)
            {
                foreach (var element in GetValuePostorder(node.Left))
                {
                    yield return element;
                }

                foreach (var element in GetValuePostorder(node.Right))
                {
                    yield return element;
                }

                yield return node.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Node<T> FindNode(Node<T> node, T value, ref Node<T> parent)
        {
            if (node != null)
            {
                int result = _comparer.Compare(node.Value, value);

                if (result == 0)
                {
                    return node;
                }

                parent = node;

                if (result > 0)
                {
                    return FindNode(node.Left, value, ref parent);
                }
                else if (result < 0)
                {
                    return FindNode(node.Right, value, ref parent);
                }
            }

            return null;
        }

        private void BalanceTree(Node<T> parent, Node<T> node)
        {
            Node<T> leftBranch = node.Right.Left,
                parentOfBranch = node.Right;

            while (leftBranch.Left != null)
            {
                parentOfBranch = leftBranch;
                leftBranch = leftBranch.Left;
            }

            parentOfBranch.Left = leftBranch.Right;

            leftBranch.Left = node.Left;
            leftBranch.Right = node.Right;

            if (parent == null)
            {
                _root = leftBranch;
            }
            else
            {
                int result = _comparer.Compare(parent.Value, node.Value);

                if (result > 0)
                {
                    parent.Left = leftBranch;
                }
                else if (result < 0)
                {
                    parent.Right = leftBranch;
                }
            }
        }

        private bool IsContain(Node<T> node, T item)
        {
            if (node != null)
            {
                if (_comparer.Compare(item, node.Value) == 0)
                {
                    return true;
                }

                if (_comparer.Compare(item, node.Value) < 0)
                {
                    return IsContain(node.Left, item);
                }

                if (_comparer.Compare(item, node.Value) > 0)
                {
                    return IsContain(node.Right, item);
                }
            }

            return false;
        }

        private void RelocateNode(Node<T> parent, Node<T> node, T value)
        {
            if (parent == null)
            {
                _root = node;
            }
            else
            {
                int result = _comparer.Compare(parent.Value, value);
                if (result > 0)
                {
                    parent.Left = node;
                }
                else if (result < 0)
                {
                    parent.Right = node;
                }
            }
        }
        #endregion
    }
}
