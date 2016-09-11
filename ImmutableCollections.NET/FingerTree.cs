using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmutableCollections
{
    /// <summary>
    /// An immutable 2-3-4 finger tree.
    /// </summary>
    /// <typeparam name="T">The type of items in the tree.</typeparam>
    /// <remarks>
    /// References:
    /// http://www.soi.city.ac.uk/~ross/papers/FingerTree.pdf
    /// </remarks>
    public sealed class FingerTree<T> : IEnumerable<T>, IImmutableCollection<FingerTree<T>, T>
    {
        // a tree with only 1 element is kept in 'left', always, and 'right' and 'middle' are null
        // when more than one element exist
        internal T[] left;
        internal FingerTree<T[]> middle;
        internal T[] right;

        //FIXME: add splitting, and review the applications listed in the paper to see how to accomodate here

        FingerTree(T[] left, FingerTree<T[]> middle, T[] right) : this(left, right)
        {
            this.middle = middle;
        }

        FingerTree(T[] left, T[] right) : this(left)
        {
            this.right = right;
        }

        FingerTree(T[] left)
        {
            this.left = left;
        }

        FingerTree() { }

        /// <summary>
        /// An empty tree.
        /// </summary>
        public static readonly FingerTree<T> Empty = new FingerTree<T>();

        /// <summary>
        /// True if tree is empty, false otherwise.
        /// </summary>
        public bool IsEmpty
        {
            get { return left == null; }
        }

        bool IsSingle
        {
            get { return left != null && right == null; }
        }

        /// <summary>
        /// Push a new item into the tree.
        /// </summary>
        /// <param name="item">The item to push.</param>
        /// <returns>A new tree with <paramref name="item"/> at the front.</returns>
        public FingerTree<T> Push(T item)
        {
            if (IsEmpty) return new FingerTree<T>(new[] { item });
            if (IsSingle) return new FingerTree<T>(new[] { item }, left);
            // this differs from the standard implementation that splits and pushes left
            // into a 3 item array down the middle, and keeps 1 item at this level; the extra
            // work is unnecessary
            if (left.Length == 4) return new FingerTree<T>(new[] { item }, middle.Push(left), right);
            // inline array insert
            T[] n;
            var old = left;
            switch (old.Length)
            {
                case 3:  n = new[] { item, old[0], old[1], old[2] }; break;
                case 2:  n = new[] { item, old[0], old[1] }; break;
                default: n = new[] { item, old[0] }; break;
            }
            return new FingerTree<T>(n, middle, right);
        }

        /// <summary>
        /// Add an item to the end of the tree.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>A new tree with <paramref name="item"/> at the end.</returns>
        public FingerTree<T> Add(T item)
        {
            if (IsEmpty) return new FingerTree<T>(new[] { item });
            if (IsSingle) return new FingerTree<T>(left, new[] { item });
            // this differs from the standard implementation that splits and adds root.right
            // into a 3 item array down the middle, and keeps 1 item at this level; the extra
            // work is unnecessary
            if (right.Length == 4) return new FingerTree<T>(left, middle.Add(right), new[] { item });
            // inline array append
            T[] n;
            var old = right;
            switch (old.Length)
            {
                case 3:  n = new[] { old[0], old[1], old[2], item }; break;
                case 2:  n = new[] { old[0], old[1], item }; break;
                default: n = new[] { old[0], item }; break;
            }
            return new FingerTree<T>(left, middle, n);
        }

        /// <summary>
        /// Remove item from the beginning.
        /// </summary>
        /// <param name="item">The popped item.</param>
        /// <returns>The new tree without the first item.</returns>
        public FingerTree<T> Pop(out T item)
        {
            if (IsEmpty) throw new InvalidOperationException("Cannot pop from an empty FingerTree.");
            item = left[0];
            if (IsSingle) return Empty;
            if (left.Length == 1)
            {
                T[] node = null;
                var mid = middle.IsEmpty ? middle : middle.Pop(out node);
                return new FingerTree<T>(node, mid, right);
            }
            else
            {
                // inline array remove
                T[] n;
                var old = left;
                switch (old.Length)
                {
                    case 4:  n = new[] { old[1], old[2], old[3] }; break;
                    case 3:  n = new[] { old[1], old[2] }; break;
                    default: n = new[] { old[1] }; break;
                }
                return new FingerTree<T>(n, middle, right);
            }
        }

        /// <summary>
        /// Remove the last item.
        /// </summary>
        /// <param name="item">The removed item.</param>
        /// <returns>The tree without the removed item.</returns>
        public FingerTree<T> Remove(out T item)
        {
            if (IsEmpty) throw new InvalidOperationException("Cannot remove from an empty FingerTree.");
            if (IsSingle)
            {
                item = left[0];
                return Empty;
            }
            item = right[right.Length - 1];
            if (right.Length == 1)
            {
                T[] node = null;
                var mid = middle.IsEmpty ? middle : middle.Pop(out node);
                return new FingerTree<T>(left, mid, node);
            }
            else
            {
                // inline array remove
                T[] n;
                var old = right;
                switch (old.Length)
                {
                    case 4:  n = new[] { old[0], old[1], old[2] }; break;
                    case 3:  n = new[] { old[0], old[1] }; break;
                    default: n = new[] { old[0] }; break;
                }
                return new FingerTree<T>(left, middle, n);
            }
        }
        
        /// <summary>
        /// Get the first item in tree.
        /// </summary>
        /// <returns>The first item in tree.</returns>
        /// <exception cref="InvalidOperationException">FingerTree is empty.</exception>
        public T First()
        {
            if (IsEmpty) throw new InvalidOperationException("FingerTree has no elements.");
            return left[0];
        }

        /// <summary>
        /// Get the last item in tree.
        /// </summary>
        /// <returns>The last item in tree.</returns>
        /// <exception cref="InvalidOperationException">FingerTree is empty.</exception>
        public T Last()
        {
            if (IsEmpty) throw new InvalidOperationException("FingerTree has no elements.");
            return IsSingle ? left[0] : right[right.Length - 1];
        }

        /// <summary>
        /// Concatenate another tree.
        /// </summary>
        /// <param name="other">The tree to concatenate after this one.</param>
        /// <returns>A merged tree.</returns>
        public FingerTree<T> Concat(FingerTree<T> other)
        {
            if (IsEmpty) return other;
            if (IsSingle) return other.Push(left[0]);
            if (other.IsSingle) return Add(other.left[0]);
            return new FingerTree<T>(left, Nodes(middle, right, other.left).Concat(other.middle), other.right);
        }

        static FingerTree<T[]> Nodes(FingerTree<T[]> nodes, T[] left, T[] right)
        {
            // left and right are 4 items max, which is 4*4=16 possibilities
            // so use a dispatch table indexed by array lengths to efficiently construct the new tree
            var config = left.Length << 3 | right.Length;
            switch (config)
            {
                case (1 << 3) | 1:
                    nodes = nodes.Push(new[] { left[0], right[0] });
                    break;
                case (1 << 3) | 2:
                    nodes = nodes.Push(new[] { left[0], right[1], right[0] });
                    break;
                case (1 << 3) | 3:
                    nodes = nodes.Push(new[] { left[0], right[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (1 << 3) | 4:
                    nodes = nodes.Push(new[] { left[0], right[3], right[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (2 << 3) | 1:
                    nodes = nodes.Push(new[] { left[0], left[1], right[0] });
                    break;
                case (2 << 3) | 2:
                    nodes = nodes.Push(new[] { left[0], left[1] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (2 << 3) | 3:
                    nodes = nodes.Push(new[] { left[0], left[1], right[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (2 << 3) | 4:
                    nodes = nodes.Push(new[] { left[0], left[1], right[3] })
                                 .Push(new[] { right[2], right[1], right[0] });
                    break;
                case (3 << 3) | 1:
                    nodes = nodes.Push(new[] { left[0], left[1] })
                                 .Push(new[] { left[2], right[0] });
                    break;
                case (3 << 3) | 2:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (3 << 3) | 3:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { right[2], right[1], right[0] });
                    break;
                case (3 << 3) | 4:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { right[3], right[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (4 << 3) | 1:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { left[3], right[0] });
                    break;
                case (4 << 3) | 2:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { left[3], right[1], right[0] });
                    break;
                case (4 << 3) | 3:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { left[3], right[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                case (4 << 3) | 4:
                    nodes = nodes.Push(new[] { left[0], left[1], left[2] })
                                 .Push(new[] { left[3], right[3], right[2] })
                                 .Push(new[] { right[1], right[0] });
                    break;
                default:
                    throw new InvalidOperationException("Impossible!");
            }
            return nodes;
        }

        /// <summary>
        /// Compare for equality.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if equals, false otherwise.</returns>
        public bool Equals(FingerTree<T> other)
        {
            return this == other
                || left != null && other.left != null && this.SequenceEqual(other);
        }

        /// <summary>
        /// Get an enumerator for the tree's elements.
        /// </summary>
        /// <returns>An enumerator over the tree items.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty) yield break;
            if (right == null) yield return left[0];
            else
            {
                for (int i = 0; i < left.Length; ++i)
                    yield return left[i];
                foreach (var x in middle)
                    for (int i = 0; i < x.Length; ++i)
                        yield return x[i];
                for (int i = 0; i < right.Length; ++i)
                    yield return right[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((FingerTree<T>)this).GetEnumerator();
        }
        
        /// <summary>
        /// Compare for equality.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if equals, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is FingerTree<T> && Equals((FingerTree<T>)obj);
        }

        /// <summary>
        /// Compute the tree's hashcode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            //FIXME: cache the hash code at top-level
            return this.Aggregate(typeof(FingerTree<T>).GetHashCode(), (acc, x) => acc ^ x.GetHashCode());
        }
    }
}
