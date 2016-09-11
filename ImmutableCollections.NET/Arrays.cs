using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ImmutableCollections
{
    /// <summary>
    /// Array extensions.
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Combine the values of two arrays into a new array.
        /// </summary>
        /// <typeparam name="T">The type in the array.</typeparam>
        /// <param name="first">The first array.</param>
        /// <param name="second">The second array.</param>
        /// <returns>Returns a new array with the values of <paramref name="first"/>, followed by the values in <paramref name="second"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="first"/> or <paramref name="second"/> are null.</exception>
        /// <remarks>
        /// <see cref="Arrays.Concat{T}"/> operates much like <see cref="Enumerable.Concat{T}"/>, but returns an array instead of
        /// an <see cref="IEnumerable{T}"/>:
        /// <code>
        /// var array = new[] { 1, 2, 3 }.Concat(4, 5);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 2, 3, 4, 5, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Concat<T>(this T[] first, params T[] second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            var app = new T[first.Length + second.Length];
            Array.Copy(first, 0, app, 0, first.Length);
            Array.Copy(second, 0, app, first.Length, second.Length);
            return app;
        }

        /// <summary>
        /// Combine the values of two arrays into a new array.
        /// </summary>
        /// <typeparam name="T">The type in the array.</typeparam>
        /// <param name="array">The first array.</param>
        /// <param name="last">The second array.</param>
        /// <returns>Returns a new array with the values of <paramref name="array"/>, followed by <paramref name="last"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <remarks>
        /// This extension method creates a new array with <paramref name="last"/> appended to the end:
        /// <code>
        /// var array = new[] { 1, 2, 3 }.Append(4);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 2, 3, 4, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Append<T>(this T[] array, T last)
        {
            if (array == null) throw new ArgumentNullException("array");
            return array.InternalAppend(last);
        }

        /// <summary>
        /// Sets the index of the array.
        /// </summary>
        /// <typeparam name="T">The type of array element.</typeparam>
        /// <param name="array">The array to set.</param>
        /// <param name="index">The index to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>A new array with the slot</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside the array bounds.</exception>
        /// <remarks>
        /// This extension method creates a new array with the given index initialized to the given value:
        /// <code>
        /// var orig = new[] { 1, 2, 3 };
        /// var array = orig.Set(index: 1, value: 99);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 99, 3, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Set<T>(this T[] array, int index, T value)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (index >= array.Length) throw new ArgumentOutOfRangeException("index", "Index cannot exceed the array length.");
            return array.InternalSet(index, value);
        }

        /// <summary>
        /// Insert a new element into an array.
        /// </summary>
        /// <typeparam name="T">The type of array element.</typeparam>
        /// <param name="array">The array to expand.</param>
        /// <param name="index">The index at which to insert the element.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>Returns a new array with the values of <paramref name="array"/>, followed by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside the array bounds.</exception>
        /// <remarks>
        /// This extension method creates a new array with the given element inserted at the given index:
        /// <code>
        /// var orig = new[] { 1, 2, 3 };
        /// var array = orig.Insert(index: 1, value: 99);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 99, 2, 3, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Insert<T>(this T[] array, int index, T value)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (index > array.Length) throw new ArgumentOutOfRangeException("index", "Index cannot exceed the array length + 1.");
            var dst = new T[array.Length + 1];
            Array.Copy(array, 0, dst, 0, index);
            dst[index] = value;
            if (index < array.Length) Array.Copy(array, index, dst, index + 1, array.Length - index);
            return dst;
        }

        /// <summary>
        /// Remove an element from an array.
        /// </summary>
        /// <typeparam name="T">The type of array element.</typeparam>
        /// <param name="array">The array to contract.</param>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>Returns a new array with the value at <paramref name="index"/> removed from <paramref name="array"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside the array bounds.</exception>
        /// <remarks>
        /// This extension method creates a new array with the element at the given index removed:
        /// <code>
        /// var orig = new[] { 1, 99, 2, 3 };
        /// var array = orig.Remove(1);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 2, 3, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Remove<T>(this T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (index > array.Length) throw new ArgumentOutOfRangeException("index", "Index cannot exceed the array length.");
            var app = new T[array.Length - 1];
            Array.Copy(array, 0, app, 0, index);
            if (index < array.Length) Array.Copy(array, index + 1, app, index, array.Length - index - 1);
            return app;
        }

        /// <summary>
        /// A syntactic shortcut to create arrays of values leveraging type inference.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="values">The values to create.</param>
        /// <returns>An array of the provided values.</returns>
        /// <remarks>
        /// This is a simple convenience method for constructing arrays:
        /// <code>
        /// var array = Arrays.Create(1, 2, 3, 4);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 2, 3, 4, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Create<T>(params T[] values)
        {
            return values;
        }

        /// <summary>
        /// Return a slice of an array delineated by the start and end indices.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array to slice.</param>
        /// <param name="start">The starting index of the slice.</param>
        /// <param name="end">The first index not included in the slice.</param>
        /// <returns>The array slice.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if indices are outside the array bounds.</exception>
        /// <remarks>
        /// This extension method extracts a sub-array from the given array delimited by a
        /// <paramref name="start"/> and <paramref name="end"/> index:
        /// <code>
        /// var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 }
        ///             .Slice(start: 2, end: 5);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 3, 4, 5, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Slice<T>(this T[] array, int start, int end)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (start < 0) throw new ArgumentOutOfRangeException("start", "Parameter must be greater than or equal to 0.");
            if (end < start) throw new ArgumentOutOfRangeException("end", "Parameter must be greater than start.");
            if (array.Length < end || end < 0) throw new ArgumentOutOfRangeException("end", "Parameter must be greater than or equal to 0 and less than array length.");
            if (start == 0 && end == array.Length) return array;
            var count = end - start;
            T[] newt = new T[count];
            Array.Copy(array, (int)start, newt, 0, (int)count);
            return newt;
        }

        /// <summary>
        /// Attempts to access the array by index.
        /// </summary>
        /// <typeparam name="T">The type of array items.</typeparam>
        /// <param name="items">The array to access.</param>
        /// <param name="index">The array index to access.</param>
        /// <param name="value">The value at the array index.</param>
        /// <returns>
        /// True if the value was successfully accessed, false otherwise.
        /// </returns>
        public static bool TryGetValue<T>(this T[] items, int index, out T value)
        {
            if (items == null || index < 0 || index >= items.Length)
            {
                value = default(T);
                return false;
            }
            else
            {
                value = items[index];
                return true;
            }
        }

        //public static void StableSort<T>(T[] array, int start, int count)
        //{
        //    //FIXME: add a fast in-place stable sort:
        //    //http://thomas.baudel.name/Visualisation/VisuTri/inplacestablesort.html
        //}

        /// <summary>
        /// Repeats all entries in <paramref name="array"/> up to <paramref name="start"/>
        /// as many times as will fit.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array to slice.</param>
        /// <param name="start">The index at which to start duplicating elements.</param>
        /// <returns>Returns <paramref name="array"/> after the update.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside the array bounds.</exception>
        /// <remarks>
        /// This extension method repeats all elements up to the given index as many
        /// times as will fit in the given array:
        /// <code>
        /// var array = new[] { 1, 2, 3, 4, 5 }.Repeat(2);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 2, 1, 2, 1, 
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Repeat<T>(this T[] array, int start)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (array.Length < start || start < 0) throw new ArgumentOutOfRangeException("start", "Parameter must be greater than 0 and less than array length.");
            int i;
            int count = start + 1;
            for (i = count; i + count < array.Length; i += count)
            {
                Array.Copy(array, 0, array, i, count);
            }
            Array.Copy(array, 0, array, i, array.Length - i);
            return array;
        }

        /// <summary>
        /// Populates the given array with <paramref name="item"/>, starting at the given index
        /// for <paramref name="count"/> entries.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item">The item with which to fill the array.</param>
        /// <param name="start">The index to start filling.</param>
        /// <param name="count">The number of entries to set.</param>
        /// <returns>Returns <paramref name="array"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside the array bounds.</exception>
        /// <remarks>
        /// This extension method fills the given array with a given value between certain bounds:
        /// <code>
        /// var array = new int[4];
        /// array.Fill(item: 1, start: 0, count: 2);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 1, 0, 0, 
        /// </code>
        /// </remarks>
        public static T[] Fill<T>(this T[] array, T item, int start, int count)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (array.Length < start || start < 0) throw new ArgumentOutOfRangeException("start", "Parameter must be greater than or equal to 0 and less than array length.");
            var j = start + count;
            while (start < j)
            {
                array[start++] = item;
            }
            return array;
        }

        /// <summary>
        /// Duplicates a given array.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>A duplicate of the given array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <remarks>
        /// This extension method creates an exact duplicate of the given array:
        /// <code>
        /// var orig = new[] { 1, 2, 3 };
        /// var dup = orig.Copy();
        /// Console.WriteLine(orig == dup);
        /// Console.WriteLine(orig.SequenceEqual(dup));
        /// // output:
        /// // false
        /// // true
        /// </code>
        /// </remarks>
        [Pure]
        public static T[] Copy<T>(this T[] array)
        {
            if (array == null) throw new ArgumentNullException("array");
            return array.InternalCopy();
        }

        /// <summary>
        /// Returns an array with the given length, seeded with the values from
        /// <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="count">The number of items in the returned array.</param>
        /// <returns>An array of length <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is outside the array bounds.</exception>
        /// <remarks>
        /// If <paramref name="count"/> equals a.Length, then the same array
        /// is returned. If <paramref name="count"/> is greater than a.Length, then
        /// a new array is created and seeded with the original values in <paramref name="array"/>
        /// with the remainder of the array remaining uninitialized:
        /// <code>
        /// var array = new[] { 1, 2, 3, 4 }.Bound(2);
        /// foreach (var x in array)
        ///     Console.Write("{0}, ", x);
        /// // output:
        /// // 1, 2, 
        /// </code>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 0.</exception>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        [Pure]
        public static T[] Bound<T>(this T[] array, int count)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (count < 0) throw new ArgumentOutOfRangeException("count", "Parameter must be greater than or equal to 0.");
            if (array.Length == count) return array;
            if (array.Length > count) return array.Slice(0, count);
            var t = new T[count];
            Array.Copy(array, 0, t, 0, array.Length);
            return t;
        }

        /// <summary>
        /// Searches the array for a matching index.
        /// </summary>
        /// <typeparam name="T">The type of array elements.</typeparam>
        /// <param name="array">The array to index.</param>
        /// <param name="match">The predicate to apply.</param>
        /// <returns>The index of the first matching element.</returns>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        /// <remarks>
        /// This extension method searches an array from beginning to end and returns the first item that
        /// matches a predicate, or -1 if no item matches:
        /// <code>
        /// var array = new[] { 1, 2, 3, 4 };
        /// var i = array.IndexOf(x =&gt; x &gt; 2);
        /// Console.WriteLine(i);
        /// // output:
        /// // 2
        /// </code>
        /// </remarks>
        [Pure]
        public static int IndexOf<T>(this T[] array, Predicate<T> match)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (match == null) throw new ArgumentNullException("match");
            for (int i = 0; i < array.Length; ++i)
            {
                if (match(array[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Copies <paramref name="count"/> entries from <paramref name="source"/> to <paramref name="target"/> array while performing a conversion.
        /// </summary>
        /// <typeparam name="T0">The source type.</typeparam>
        /// <typeparam name="T1">The target type.</typeparam>
        /// <param name="source">The source stream.</param>
        /// <param name="target">The target array.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of items to copy.</param>
        /// <param name="selector">The conversion function.</param>
        /// <returns>The number of items that were copied.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is outside the array bounds.</exception>
        public static int CopyTo<T0, T1>(this IEnumerable<T0> source, T1[] target, int start, int count, Func<T0, T1> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (target == null) throw new ArgumentNullException("target");
            if (selector == null) throw new ArgumentNullException("selector");
            if (start < 0) throw new ArgumentOutOfRangeException("start");
            if (count < 0 || start + count > target.Length) throw new ArgumentOutOfRangeException("count");
            int i = 0;
            foreach (var x in source)
            {
                if (i == count) break;
                target[start + i++] = selector(x);
            }
            return i;
        }

        /// <summary>
        /// Converts a stream to an array while performing a conversion.
        /// </summary>
        /// <typeparam name="T0">The source type.</typeparam>
        /// <typeparam name="T1">The target type.</typeparam>
        /// <param name="source">The source stream.</param>
        /// <param name="count">The maximum number of items to copy.</param>
        /// <param name="selector">The conversion function.</param>
        /// <returns>An array of max size <paramref name="count"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is outside the array bounds.</exception>
        [Pure]
        public static T1[] ToArray<T0, T1>(this IEnumerable<T0> source, int count, Func<T0, T1> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            var x = new T1[count];
            source.CopyTo(x, 0, count, selector);
            return x;
        }

        #region Internal operations that run without bound checks
        internal static T[] InternalSet<T>(this T[] array, int index, T value)
        {
            var narr = array.InternalCopy();
            narr[index] = value;
            return narr;
        }
        internal static T[] InternalCopy<T>(this T[] array)
        {
            if (array == null) throw new ArgumentNullException("array");
            var l = array.Length;
            var tmp = new T[l];
            Array.Copy(array, 0, tmp, 0, l);
            return tmp;
        }
        internal static T[] InternalCopy<T>(ref T[] items)
        {
            return items = items.Copy();
        }
        internal static T[] InternalAppend<T>(this T[] array, T last)
        {
            var app = new T[array.Length + 1];
            Array.Copy(array, 0, app, 0, array.Length);
            app[array.Length] = last;
            return app;
        }
        #endregion
    }
}
