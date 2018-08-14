using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCollections
{
    public static class SetExtension
    {
        public static ISet<T> Union<T>(this ISet<T> left, ISet<T> right)
        {
            var result = new BinaryTreeSet<T>();

            result.UnionWith(left);

            result.UnionWith(right);

            return result;
        }
    }
}
