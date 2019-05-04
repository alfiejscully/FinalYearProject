using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIdx 
	{
		get;
		set;
	}
}

public class Heap<T> where T : IHeapItem<T> 
{
	T[] items;
	int currItemCount;

	public int Count
	{
		get
		{
			return currItemCount;
		}
	}

	public Heap(int maxHeapSize)
	{
		items = new T[maxHeapSize]; 

	}

	public void Add(T item)
	{
		item.HeapIdx = currItemCount;	
		items[currItemCount] = item;
		SortUpHandler(item);
		currItemCount++;
	}

	public T RemoveFirst()
	{
		T firstItem = items[0];
		currItemCount--;
		items[0] = items[currItemCount];
		items[0].HeapIdx = 0;
		SortDownHandler(items[0]);
		return firstItem;
	}

	public bool Contains(T item)
	{
		return Equals(items[item.HeapIdx], item);
	}

	void SortDownHandler(T item)
	{
		while (true) 
		{
			int childIdxLeft = item.HeapIdx * 2 + 1;
			int childIdxRight = item.HeapIdx * 2 + 2;
			int swapIdx = 0;

			if (childIdxLeft < currItemCount) 
			{
				swapIdx = childIdxLeft;

				if (childIdxRight < currItemCount) {
					if (items[childIdxLeft].CompareTo (items[childIdxRight]) < 0) {
						swapIdx = childIdxRight;
					}
				}

				if (item.CompareTo (items[swapIdx]) < 0) {
					Swap (item, items[swapIdx]);
				} else {
					return;
				}
			} 
			else 
			{
				return;
			}
		}
	}

	void SortUpHandler(T item)
	{
		int parentIdx = (item.HeapIdx - 1) / 2;

		while (true) 
		{
			T parentItem = items [parentIdx];
			if (item.CompareTo(parentItem) > 0) {
				Swap(item, parentItem);
			} 
			else 
			{
				break;
			}
			parentIdx = (item.HeapIdx - 1) / 2;
		}
	}

	void Swap(T itemA, T itemB)
	{
		items[itemA.HeapIdx] = itemB;
		items[itemB.HeapIdx] = itemA;

		int itemAIdx = itemA.HeapIdx;
		itemA.HeapIdx = itemB.HeapIdx;
		itemB.HeapIdx = itemAIdx;
	}
}

//	public void UpdateItem(T item)
//	{
//		SortUpHandler(item);
//	}
//