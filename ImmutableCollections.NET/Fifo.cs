using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace ImmutableCollections
{
    /// <summary>
    /// A persistent queue.
    /// </summary>
    /// <typeparam name="T">The type of the queue elements.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [Pure]
    public sealed class Fifo<T> : IImmutableCollection<Fifo<T>, T>
                                , IEnumerable<T>
    {
        // invariant: head == null || !head.forwards.IsEmpty
        Lifo<T> forwards;
        Lifo<T> backwards;

        Fifo(Lifo<T> forwards, Lifo<T> backwards)
        {
            this.forwards = forwards;
            this.backwards = backwards;
        }

        /// <summary>
        /// Initialize the queue with the given list of values.
        /// </summary>
        /// <param name="values">The list of values.</param>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        [Pure]
        public Fifo(IEnumerable<T> values)
        {
            if (values == null) throw new ArgumentNullException("values");
            var q = Lifo<T>.Empty;
            foreach (var v in values)
            {
                q = q.Push(v);
            }
            backwards = q;
        }

        /// <summary>
        /// Construct a single-element queue.
        /// </summary>
        /// <param name="value">The initial queue value.</param>
        public Fifo(T value)
        {
            forwards = new Lifo<T>(value);
        }

        /// <summary>
        /// An empty queue.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static readonly Fifo<T> Empty = new Fifo<T>(null, null);

        /// <summary>
        /// Returns true if the queue is empty.
        /// </summary>
        [Pure]
        public bool IsEmpty
        {
            get { return this == Empty; }
        }

        /// <summary>
        /// Returns the first value in the queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the queue is empty.</exception>
        [Pure]
        public T Value
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException("Queue is empty.");
                return forwards.Peek();
            }
        }

        /// <summary>
        /// Tests structural equality of two queues.
        /// </summary>
        /// <param name="other">The other queue to compare to.</param>
        /// <returns>True if the queues are equal, false otherwise.</returns>
        [Pure]
        public bool Equals(Fifo<T> other)
        {
            return this == other
                || this.SequenceEqual(other);
        }

        // Either this.forwards is subset of other.forwards, or vice versa; need to match forwards
        // and backwards elements:
        //
        // 1. check whether this.forwards is subset of other.forwards; if so, return remainder of other.forwards to be matched := F
        // 2. recursive call down this.backwards spine, then continue comparison of F on return up spine
        // 3. once other.forwards exhausted, return the this.backwards node where iteration stopped := N
        // 4. check that other.backwards is a subset of this.backwards, and check that remainder node == N
        //
        // If so, return true. If false, swap other and this, and retry the above procedure. If still false,
        // return false.
        //
        //static Lifo<T>? Subset(Lifo<T> first, Lifo<T> second)
        //{
        //    while (!first.IsEmpty && !second.IsEmpty)
        //    {
        //        if (!EqualityComparer<T>.Default.Equals(first.Value, second.Value)) return null;
        //        first = first.Next;
        //        second = second.Next;
        //    }
        //    return !second.IsEmpty || first.IsEmpty ? second : new Lifo<T>?();
        //}
        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="obj">The other object to compare to.</param>
        /// <returns>Returns true if the objects are equal.</returns>
        [Pure]
        public override bool Equals(object obj)
        {
            return obj is Fifo<T> && Equals((Fifo<T>)obj);
        }

        /// <summary>
        /// Returns the hash code for the current sequence.
        /// </summary>
        /// <returns>The integer hash code.</returns>
        public override int GetHashCode()
        {
            //FIXME: cache the hash code while building the list?
            return this.Aggregate(typeof(FingerTree<T>).GetHashCode(), (acc, x) => acc ^ x.GetHashCode());
        }

        /// <summary>
        /// Enqueue a value and return a new queue.
        /// </summary>
        /// <param name="value">The value to enqueue.</param>
        /// <returns>A new queue with <paramref name="value"/> as its last element.</returns>
        [Pure]
        public Fifo<T> Enqueue(T value)
        {
            return IsEmpty ? new Fifo<T>(Lifo<T>.Empty.Push(value), Lifo<T>.Empty):
                             new Fifo<T>(forwards, backwards.Push(value));
        }

        /// <summary>
        /// Appends the elements of two queues.
        /// </summary>
        /// <param name="other">The queue whose elements we should append.</param>
        /// <returns>A new queue consisting of this queue's elements followed by <paramref name="other"/>'s elements.</returns>
        [Pure]
        public Fifo<T> Append(Fifo<T> other)
        {
            return IsEmpty       ? other:
                   other.IsEmpty ? this:
                                   new Fifo<T>(forwards.Append(backwards.ReverseAppend(other.forwards)), other.backwards);
        }

        /// <summary>
        /// Dequeue the first value in the queue.
        /// </summary>
        /// <param name="value">The first value in the queue.</param>
        /// <returns>Returns a new queue minus the first value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public Fifo<T> Dequeue(out T value)
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");
            var f = forwards.Pop(out value);
            var b = backwards;
            return !f.IsEmpty ? new Fifo<T>(f, b):
                    b.IsEmpty ? Empty:
                                new Fifo<T>(b.Reverse(), Lifo<T>.Empty);
        }

        /// <summary>
        /// Returns an enumerator over the given list.
        /// </summary>
        /// <returns>An enumeration over the list.</returns>
        [Pure]
        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty) yield break;
            //FIXME: there should be a way to destructively but transparently update this instance
            //to cache the reversal of 'backwards' -- really needs backwards.ReverseAppend(forwards)
            for (var x = backwards.Reverse(); !x.IsEmpty; x = x.Next)
            {
                yield return x.Value;
            }
            // backwards queue is empty, so just traverse the forward queue
            for (var x = forwards; !x.IsEmpty; x = x.Next)
            {
                yield return x.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((Fifo<T>)this).GetEnumerator();
        }

        Fifo<T> IImmutableCollection<Fifo<T>, T>.Remove(out T item)
        {
            return Dequeue(out item);
        }

        Fifo<T> IImmutableCollection<Fifo<T>, T>.Add(T item)
        {
            return Enqueue(item);
        }

        ///// <summary>
        ///// Test two queues for equality.
        ///// </summary>
        ///// <param name="left">The left queue.</param>
        ///// <param name="right">The right queue.</param>
        ///// <returns>Returns true if they are equal.</returns>
        //public static bool operator ==(Fifo<T> left, Fifo<T> right)
        //{
        //    return left.Equals(right);
        //}
        ///// <summary>
        ///// Test two sequences for inequality.
        ///// </summary>
        ///// <param name="left">The left sequence.</param>
        ///// <param name="right">The right sequence.</param>
        ///// <returns>Returns true if they are not equal.</returns>
        //public static bool operator !=(Fifo<T> left, Fifo<T> right)
        //{
        //    return !left.Equals(right);
        //}

        /// <summary>
        /// Converts a queue to a string.
        /// </summary>
        /// <returns>A string representation of the queue.</returns>
        public override string ToString()
        {
            return this.Aggregate(new StringBuilder("["), (acc,x) => acc.Append(", ").Append(x))
                       .Append(']')
                       .ToString();
        }
    }
}
