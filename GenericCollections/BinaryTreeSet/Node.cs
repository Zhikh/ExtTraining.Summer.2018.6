using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCollections
{
    /// <summary>
    /// Node for binary tree
    /// </summary>
    /// <typeparam name="T"> Type of saving data </typeparam>
    internal sealed class Node<T>
    {
        /// <summary>
        /// Data
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Left child
        /// </summary>
        public Node<T> Left { get; set; }

        /// <summary>
        /// Right child
        /// </summary>
        public Node<T> Right { get; set; }
    }
}
