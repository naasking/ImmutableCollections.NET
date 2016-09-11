using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace ImmutableCollections
{
    /// <summary>
    /// A purely functional stack.
    /// </summary>
    /// <remarks>
    /// "null" is also a valid sequence value that can be used to
    /// construct lists (see example).
    /// </remarks>
    /// <example>
    /// <code>Seq&lt;T&gt; list = value1 &amp; value2 &amp; null;</code>
    /// </example>
    /// <typeparam name="T">The type of the sequence elements.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [Pure]
    public sealed class Lifo<T> : IImmutableCollection<Lifo<T>, T>
                                , IEnumerable<T>
    {
        internal readonly T value;
        internal readonly Lifo<T> next;
        
        /// <summary>
        /// Construct a new sequence from a new head value and an existing list.
        /// </summary>
        /// <param name="value">The new value at the head of the list.</param>
        /// <param name="tail">The remainder of the list.</param>
        Lifo(T value, Lifo<T> tail)
        {
            this.value = value;
            this.next = tail;
        }

        /// <summary>
        /// Construct a new single-element sequence.
        /// </summary>
        /// <param name="value">The new value at the head of the list.</param>
        public Lifo(T value) : this(value, Empty)
        {
        }

        /// <summary>
        /// Returns an empty stack.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static readonly Lifo<T> Empty = new Lifo<T>(default(T), null);
        
        /// <summary>
        /// Returns an enumerator over the given list.
        /// </summary>
        /// <returns>An enumeration over the list.</returns>
        [Pure]
        public IEnumerator<T> GetEnumerator()
        {
            for (var t = this; t != Empty; t = t.next)
            {
                yield return t.value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((Lifo<T>)this).GetEnumerator();
        }

        /// <summary>
        /// Returns true if the sequence is empty.
        /// </summary>
        [Pure]
        public bool IsEmpty
        {
            get { return this == Empty; }
        }

        /// <summary>
        /// Gets the current element of the collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the collection is empty.</exception>
        [Pure]
        public T Value
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException("Sequence is empty.");
                return value;
            }
        }

        /// <summary>
        /// Returns the next element in the sequence.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the collection is empty.</exception>
        [Pure]
        public Lifo<T> Next
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException("Sequence is empty.");
                return next;
            }
        }

        /// <summary>
        /// Tests structural equality of two sequences.
        /// </summary>
        /// <param name="other">The other sequence to compare to.</param>
        /// <returns>True if the sequences are equal, false otherwise.</returns>
        [Pure]
        public bool Equals(Lifo<T> other)
        {
            //FIXME: this is no different than SequenceEquals, so perhaps don't expose this method
            return this == other
                || !IsEmpty && !other.IsEmpty && EqualityComparer<T>.Default.Equals(value, other.value) && next.Equals(other.next);
        }

        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="obj">The other object to compare to.</param>
        /// <returns>Returns true if the objects are equal.</returns>
        [Pure]
        public override bool Equals(object obj)
        {
            return obj is Lifo<T> && Equals((Lifo<T>)obj);
        }

        /// <summary>
        /// Returns the hash code for the current sequence.
        /// </summary>
        /// <returns>The integer hash code.</returns>
        [Pure]
        public override int GetHashCode()
        {
            return (IsEmpty ? 0 : EqualityComparer<T>.Default.GetHashCode(value))
                 ^ typeof(Lifo<T>).GetHashCode();
        }

        /// <summary>
        /// Peeks at the current value in the sequence.
        /// </summary>
        /// <returns>The value at the head of the sequence.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the sequence is empty.</exception>
        [Pure]
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Sequence is empty.");
            return value;
        }

        /// <summary>
        /// Pops the first element off the sequence.
        /// </summary>
        /// <param name="value">The value in the first element of the sequence.</param>
        /// <returns>The remaining sequence.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the sequence is empty.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        [Pure]
        public Lifo<T> Pop(out T value)
        {
            if (IsEmpty) throw new InvalidOperationException("Sequence is empty.");
            value = this.value;
            return next;
        }
        
        /// <summary>
        /// Push an element on to the front of the sequence.
        /// </summary>
        /// <param name="value">The new head of the sequence.</param>
        /// <returns>A new sequence.</returns>
        [Pure]
        public Lifo<T> Push(T value)
        {
            return new Lifo<T>(value, this);
        }

        Lifo<T> IImmutableCollection<Lifo<T>, T>.Add(T value)
        {
            return Push(value);
        }

        Lifo<T> IImmutableCollection<Lifo<T>, T>.Remove(out T value)
        {
            return Pop(out value);
        }
        
        /// Checks whether a value is in the sequence.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True if the element is in the sequence, false otherwise.</returns>
        [Pure]
        public bool Contains(T value)
        {
            var eq = EqualityComparer<T>.Default;
            for (var s = this; s != Empty; s = s.next)
            {
                if (eq.Equals(value, s.value)) return true;
            }
            return false;
        }

        /// <summary>
        /// Reverse a sequence.
        /// </summary>
        /// <returns>A reversed sequence.</returns>
        [Pure]
        public Lifo<T> Reverse()
        {
            Lifo<T> rev = null;
            for (var s = this; s != Empty; s = s.next)
            {
                rev = new Lifo<T>(s.value, rev);
            }
            return rev;
        }

        /// <summary>
        /// Append the given sequence after the current sequence.
        /// </summary>
        /// <param name="other">The elements to append.</param>
        /// <returns>A new sequence constructed from the given parameters.</returns>
        [Pure]
        public Lifo<T> Append(Lifo<T> other)
        {
            return IsEmpty ? other : Next.Append(other).Push(Value);
        }

        /// <summary>
        /// Reverses the current sequence and appends another sequence to the end.
        /// </summary>
        /// <param name="append">The sequence to append.</param>
        /// <returns>A combined sequence.</returns>
        [Pure]
        public Lifo<T> ReverseAppend(Lifo<T> append)
        {
            for (var s = this; s != Empty; s = s.next)
            {
                append = append.Push(s.value);
            }
            return append;
        }

        /// <summary>
        /// Reverses the current sequence and appends it after other sequence.
        /// </summary>
        /// <param name="other">The sequence to append.</param>
        /// <returns>A combined sequence.</returns>
        [Pure]
        public Lifo<T> ReversePush(Lifo<T> other)
        {
            return other.IsEmpty ? Reverse() : ReversePush(other.Next).Push(other.Value);
        }

        ///// <summary>
        ///// Apply an operation to a deconstructed list.
        ///// </summary>
        ///// <typeparam name="TResult">The type of return value.</typeparam>
        ///// <param name="otherwise">The value to return if the sequence is empty.</param>
        ///// <param name="cons">The function to invoke with the deconstructed head of the list.</param>
        ///// <returns>Returns cons(head, tail), or otherwise if the sequence is empty.</returns>
        ///// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //[Pure]
        //public TResult Select<TResult>(TResult otherwise, Func<T, Lifo<T>, TResult> cons)
        //{
        //    if (cons == null) throw new ArgumentNullException("cons");
        //    return IsEmpty ? otherwise : cons(value, next);
        //}

        ///// <summary>
        ///// Return the value at the head of the list.
        ///// </summary>
        ///// <param name="otherwise">The value to return if the sequence is empty.</param>
        ///// <returns>Returns the value at the head of the list, or 'otherwise' if the sequence is empty.</returns>
        //[Pure]
        //public T Select(T otherwise)
        //{
        //    return IsEmpty ? otherwise : top.value;
        //}
        
        /// <summary>
        /// Remove an element from the sequence.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns>A new sequence without the element.</returns>
        [Pure]
        [CLSCompliant(false)]
        public Lifo<T> Remove(T value)
        {
            return EqualityComparer<T>.Default.Equals(this.value, value)
                ? next
                : new Lifo<T>(value, next.Remove(value));
        }
        
        /// <summary>
        /// Return a string representation of the given list.
        /// </summary>
        /// <returns>String represetation of the list.</returns>
        public override string ToString()
        {
            return this.Aggregate(new StringBuilder("["), (acc,x) => acc.Append(", ").Append(x))
                       .Append("]")
                       .ToString();
        }

        ///// <summary>
        ///// Test two sequences for equality.
        ///// </summary>
        ///// <param name="left">The left sequence.</param>
        ///// <param name="right">The right sequence.</param>
        ///// <returns>Returns true if they are equal.</returns>
        //[Pure]
        //public static bool operator ==(Lifo<T> left, Lifo<T> right)
        //{
        //    return left.Equals(right);
        //}

        ///// <summary>
        ///// Test two sequences for inequality.
        ///// </summary>
        ///// <param name="left">The left sequence.</param>
        ///// <param name="right">The right sequence.</param>
        ///// <returns>Returns true if they are not equal.</returns>
        //[Pure]
        //public static bool operator !=(Lifo<T> left, Lifo<T> right)
        //{
        //    return !left.Equals(right);
        //}
    }
}
