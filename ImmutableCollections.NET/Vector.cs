using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmutableCollections
{
    /// <summary>
    /// An immutable vector over 32-bit indices.
    /// </summary>
    /// <typeparam name="T">The type of values contained in the vector.</typeparam>
    /// <remarks>
    /// This vector is basically an unrolled variant of a 32-way tree.
    /// </remarks>
    public class Vector<T> : IEnumerable<T>
    {
        internal const int MASK = 0x1F;

        // fastest vector by far is the unrolled version used here, since the .NET JIT
        // can eliminate many of the bounds checks
        
        //FIXME: add batch operations, like a constructor accepting a size
        //FIXME: add TryGetValue, TrySetValue?

        /// <summary>
        /// The number of elements in the vector.
        /// </summary>
        public int Count { get; internal set; }

        /// <summary>
        /// The vector indexer.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual T this[int i]
        {
            get { throw new IndexOutOfRangeException(); }
        }

        /// <summary>
        /// Update a value in the vector.
        /// </summary>
        /// <param name="i">The index to update.</param>
        /// <param name="value">The value to store.</param>
        /// <returns>A new vector with the given value.</returns>
        public virtual Vector<T> Set(int i, T value)
        {
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Expand the vector.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>A vector expanded by 1 value.</returns>
        public virtual Vector<T> Add(T value)
        {
            return new Vector0<T> { Count = Count + 1, items = new[] { value } };
        }

        ///// <summary>
        ///// Add a whole sequence of values.
        ///// </summary>
        ///// <param name="values"></param>
        ///// <returns></returns>
        //public virtual Vector<T> AddRange(IEnumerable<T> values)
        //{
        //    var items = new T[32];
        //    var leaves = new List<T[]>();
        //    var count = 0;
        //    foreach (var x in values)
        //    {
        //        items[count++] = x;
        //        if (count == 32)
        //        {
        //            leaves.Add(items);
        //            items = new T[32];
        //        }
        //    }
        //    // count == 0 || count < 32 || count == 32
        //    if (count < 32) items = items.Slice(0, count);
        //    if (count > 0) leaves.Add(items);
        //    // case leaves.Length
        //    // < 32 => return Vec0
        //    // < 32^2 => return Vec1
        //    // ...
        //    return this;
        //}

        /// <summary>
        /// Get the vector's enumerator.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this as Vector<T>).GetEnumerator();
        }
        
        /// <summary>
        /// An empty vector.
        /// </summary>
        public static readonly Vector<T> Empty = new Vector<T>();
    }
}
