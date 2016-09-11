using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmutableCollections
{
    /// <summary>
    /// The interface describing a purely functional collection.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TItem">The type of the elements contained within the collection</typeparam>
    /// <remarks>
    /// The precise semantics of the collection is implementation-specific. A sequence of Add and Remove
    /// calls may return items in an arbitrary sequence depending on the type collection.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IImmutableCollection<TCollection, TItem> : IEquatable<TCollection>, IEnumerable<TItem>
        where TCollection : IImmutableCollection<TCollection, TItem>
    {
        /// <summary>
        /// Returns true if the collection is empty.
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <returns>A new collection with the new item.</returns>
        TCollection Add(TItem value);
        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="value">The item removed.</param>
        /// <returns>A new collection without the item.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        TCollection Remove(out TItem value);
    }
}
