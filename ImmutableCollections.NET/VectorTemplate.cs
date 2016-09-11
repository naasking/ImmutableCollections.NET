using System;
using System.Collections.Generic;
using System.Collections;

namespace ImmutableCollections
{
    sealed class Vector0<T> : Vector<T>
	{
        public T[] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            node0[i & MASK] = value;
            return new Vector0<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node0 = items;
            if (node0.Length < 32)
                return new Vector0<T>
				{
				    Count = Count + 1,
					items = node0.InternalAppend(
                        value)
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
    sealed class Vector1<T> : Vector<T>
	{
        public T[][] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 5) & MASK][(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            var node1 = Arrays.InternalCopy(ref node0[(i >> 30) & MASK]);
            node1[i & MASK] = value;
            return new Vector1<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node1 = items;
            var node0 = node1[node1.Length - 1];
            if (node0.Length < 32)
                return new Vector1<T>
				{
				    Count = Count + 1,
					items = node1.InternalSet(node1.Length-1, node0.InternalAppend(
                        value))
                };
            if (node1.Length < 32)
                return new Vector1<T>
				{
				    Count = Count + 1,
					items = node1.InternalAppend(
                        new T[]{value})
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
    sealed class Vector2<T> : Vector<T>
	{
        public T[][][] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 10) & MASK][(i >> 5) & MASK][(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            var node1 = Arrays.InternalCopy(ref node0[(i >> 30) & MASK]);
            var node2 = Arrays.InternalCopy(ref node1[(i >> 25) & MASK]);
            node2[i & MASK] = value;
            return new Vector2<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node2 = items;
            var node1 = node2[node2.Length - 1];
            var node0 = node1[node1.Length - 1];
            if (node0.Length < 32)
                return new Vector2<T>
				{
				    Count = Count + 1,
					items = node2.InternalSet(node2.Length-1, node1.InternalSet(node1.Length-1, node0.InternalAppend(
                        value)))
                };
            if (node1.Length < 32)
                return new Vector2<T>
				{
				    Count = Count + 1,
					items = node2.InternalSet(node2.Length-1, node1.InternalAppend(
                        new T[]{value}))
                };
            if (node2.Length < 32)
                return new Vector2<T>
				{
				    Count = Count + 1,
					items = node2.InternalAppend(
                        new T[][]{new T[]{value}})
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
    sealed class Vector3<T> : Vector<T>
	{
        public T[][][][] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 15) & MASK][(i >> 10) & MASK][(i >> 5) & MASK][(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            var node1 = Arrays.InternalCopy(ref node0[(i >> 30) & MASK]);
            var node2 = Arrays.InternalCopy(ref node1[(i >> 25) & MASK]);
            var node3 = Arrays.InternalCopy(ref node2[(i >> 20) & MASK]);
            node3[i & MASK] = value;
            return new Vector3<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node3 = items;
            var node2 = node3[node3.Length - 1];
            var node1 = node2[node2.Length - 1];
            var node0 = node1[node1.Length - 1];
            if (node0.Length < 32)
                return new Vector3<T>
				{
				    Count = Count + 1,
					items = node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalSet(node1.Length-1, node0.InternalAppend(
                        value))))
                };
            if (node1.Length < 32)
                return new Vector3<T>
				{
				    Count = Count + 1,
					items = node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalAppend(
                        new T[]{value})))
                };
            if (node2.Length < 32)
                return new Vector3<T>
				{
				    Count = Count + 1,
					items = node3.InternalSet(node3.Length-1, node2.InternalAppend(
                        new T[][]{new T[]{value}}))
                };
            if (node3.Length < 32)
                return new Vector3<T>
				{
				    Count = Count + 1,
					items = node3.InternalAppend(
                        new T[][][]{new T[][]{new T[]{value}}})
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
    sealed class Vector4<T> : Vector<T>
	{
        public T[][][][][] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 20) & MASK][(i >> 15) & MASK][(i >> 10) & MASK][(i >> 5) & MASK][(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            var node1 = Arrays.InternalCopy(ref node0[(i >> 30) & MASK]);
            var node2 = Arrays.InternalCopy(ref node1[(i >> 25) & MASK]);
            var node3 = Arrays.InternalCopy(ref node2[(i >> 20) & MASK]);
            var node4 = Arrays.InternalCopy(ref node3[(i >> 15) & MASK]);
            node4[i & MASK] = value;
            return new Vector4<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node4 = items;
            var node3 = node4[node4.Length - 1];
            var node2 = node3[node3.Length - 1];
            var node1 = node2[node2.Length - 1];
            var node0 = node1[node1.Length - 1];
            if (node0.Length < 32)
                return new Vector4<T>
				{
				    Count = Count + 1,
					items = node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalSet(node1.Length-1, node0.InternalAppend(
                        value)))))
                };
            if (node1.Length < 32)
                return new Vector4<T>
				{
				    Count = Count + 1,
					items = node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalAppend(
                        new T[]{value}))))
                };
            if (node2.Length < 32)
                return new Vector4<T>
				{
				    Count = Count + 1,
					items = node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalAppend(
                        new T[][]{new T[]{value}})))
                };
            if (node3.Length < 32)
                return new Vector4<T>
				{
				    Count = Count + 1,
					items = node4.InternalSet(node4.Length-1, node3.InternalAppend(
                        new T[][][]{new T[][]{new T[]{value}}}))
                };
            if (node4.Length < 32)
                return new Vector4<T>
				{
				    Count = Count + 1,
					items = node4.InternalAppend(
                        new T[][][][]{new T[][][]{new T[][]{new T[]{value}}}})
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
    sealed class Vector5<T> : Vector<T>
	{
        public T[][][][][][] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 25) & MASK][(i >> 20) & MASK][(i >> 15) & MASK][(i >> 10) & MASK][(i >> 5) & MASK][(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            var node1 = Arrays.InternalCopy(ref node0[(i >> 30) & MASK]);
            var node2 = Arrays.InternalCopy(ref node1[(i >> 25) & MASK]);
            var node3 = Arrays.InternalCopy(ref node2[(i >> 20) & MASK]);
            var node4 = Arrays.InternalCopy(ref node3[(i >> 15) & MASK]);
            var node5 = Arrays.InternalCopy(ref node4[(i >> 10) & MASK]);
            node5[i & MASK] = value;
            return new Vector5<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node5 = items;
            var node4 = node5[node5.Length - 1];
            var node3 = node4[node4.Length - 1];
            var node2 = node3[node3.Length - 1];
            var node1 = node2[node2.Length - 1];
            var node0 = node1[node1.Length - 1];
            if (node0.Length < 32)
                return new Vector5<T>
				{
				    Count = Count + 1,
					items = node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalSet(node1.Length-1, node0.InternalAppend(
                        value))))))
                };
            if (node1.Length < 32)
                return new Vector5<T>
				{
				    Count = Count + 1,
					items = node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalAppend(
                        new T[]{value})))))
                };
            if (node2.Length < 32)
                return new Vector5<T>
				{
				    Count = Count + 1,
					items = node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalAppend(
                        new T[][]{new T[]{value}}))))
                };
            if (node3.Length < 32)
                return new Vector5<T>
				{
				    Count = Count + 1,
					items = node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalAppend(
                        new T[][][]{new T[][]{new T[]{value}}})))
                };
            if (node4.Length < 32)
                return new Vector5<T>
				{
				    Count = Count + 1,
					items = node5.InternalSet(node5.Length-1, node4.InternalAppend(
                        new T[][][][]{new T[][][]{new T[][]{new T[]{value}}}}))
                };
            if (node5.Length < 32)
                return new Vector5<T>
				{
				    Count = Count + 1,
					items = node5.InternalAppend(
                        new T[][][][][]{new T[][][][]{new T[][][]{new T[][]{new T[]{value}}}}})
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
    sealed class Vector6<T> : Vector<T>
	{
        public T[][][][][][][] items;
	    
        public override T this[int i]
        {
            get { return items[(i >> 30) & MASK][(i >> 25) & MASK][(i >> 20) & MASK][(i >> 15) & MASK][(i >> 10) & MASK][(i >> 5) & MASK][(i >> 0) & MASK]; }
        }

        public override Vector<T> Set(int i, T value)
        {
            var node0 = items.Copy();
            var node1 = Arrays.InternalCopy(ref node0[(i >> 30) & MASK]);
            var node2 = Arrays.InternalCopy(ref node1[(i >> 25) & MASK]);
            var node3 = Arrays.InternalCopy(ref node2[(i >> 20) & MASK]);
            var node4 = Arrays.InternalCopy(ref node3[(i >> 15) & MASK]);
            var node5 = Arrays.InternalCopy(ref node4[(i >> 10) & MASK]);
            var node6 = Arrays.InternalCopy(ref node5[(i >> 5) & MASK]);
            node6[i & MASK] = value;
            return new Vector6<T> { Count = Count, items = node0 };
        }

        public override Vector<T> Add(T value)
        {
            var node6 = items;
            var node5 = node6[node6.Length - 1];
            var node4 = node5[node5.Length - 1];
            var node3 = node4[node4.Length - 1];
            var node2 = node3[node3.Length - 1];
            var node1 = node2[node2.Length - 1];
            var node0 = node1[node1.Length - 1];
            if (node0.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalSet(node6.Length-1, node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalSet(node1.Length-1, node0.InternalAppend(
                        value)))))))
                };
            if (node1.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalSet(node6.Length-1, node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalSet(node2.Length-1, node1.InternalAppend(
                        new T[]{value}))))))
                };
            if (node2.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalSet(node6.Length-1, node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalSet(node3.Length-1, node2.InternalAppend(
                        new T[][]{new T[]{value}})))))
                };
            if (node3.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalSet(node6.Length-1, node5.InternalSet(node5.Length-1, node4.InternalSet(node4.Length-1, node3.InternalAppend(
                        new T[][][]{new T[][]{new T[]{value}}}))))
                };
            if (node4.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalSet(node6.Length-1, node5.InternalSet(node5.Length-1, node4.InternalAppend(
                        new T[][][][]{new T[][][]{new T[][]{new T[]{value}}}})))
                };
            if (node5.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalSet(node6.Length-1, node5.InternalAppend(
                        new T[][][][][]{new T[][][][]{new T[][][]{new T[][]{new T[]{value}}}}}))
                };
            if (node6.Length < 32)
                return new Vector6<T>
				{
				    Count = Count + 1,
					items = node6.InternalAppend(
                        new T[][][][][][]{new T[][][][][]{new T[][][][]{new T[][][]{new T[][]{new T[]{value}}}}}})
                };
            throw new InvalidOperationException("Vector is full.");
        }
	}
}