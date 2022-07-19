using System;

/// <summary>
/// A data structure that uses binary tree for optimized sorting.
/// Used to make pathfinding more optimized.
/// </summary>
/// <typeparam name="T">The type that the DripHead will store.</typeparam>
public class Heap<T> where T : IHeapItem<T>
{
    private T[] _items; //array containing all items inside of the heap
    private int _currentItemCount; //the count of the heap

    public int Count => _currentItemCount;

    public Heap(int maxHeapSize) //constructor that sets the size of the heap
    {
        _items = new T[maxHeapSize];
    }

    /// <summary>
    /// Add an item to the heap and sort it up.
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount; //set the index the be the size of the heap which is also the last position.
        _items[_currentItemCount] = item; //set the slot at the last index to be the item.
        SortUp(item); //sort the item up the binary tree.
        _currentItemCount++;
    }

    /// <summary>
    /// Removes the current first item and sorts the heap
    /// </summary>
    /// <returns>The new first item after sorting.</returns>
    public T RemoveFirst()
    {
        T firstItem = _items[0]; //saves the first item
        _currentItemCount--;

        //if first item is removed, we need to take the last item and bring it to the first slot and sort again
        _items[0] = _items[_currentItemCount]; //make the first slot the last item
        _items[0].HeapIndex = 0; //set the previously last item, now first to its heap index of 0
        SortDown(_items[0]); //sort the first item down because their value might not be suitable at their current, first position in the heap
        return firstItem;
    }

    /// <summary>
    /// Whether the heap contains a specific item or not.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True when the item is in the list and vice versa.</returns>
    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item); //returns true if the item and its supposedly index matches the item in the heap with the passed in items index
    }

    /// <summary>
    /// Sort the item passed in downward to find its children and proper position.
    /// Only use for items that are not last in the heap.
    /// </summary>
    /// <param name="item">The item to be sorted.</param>
    private void SortDown(T item)
    {
        //we are sorting down so we need to look at the item's potential children.
        while (true)
        {
            //based on how a binary tree or this data structure is made, all child items should be twice of its parent index with a positive offset from 1-2
            int childIndexLeft = item.HeapIndex * 2 + 1; //index of the child of the left
            int childIndexRight = item.HeapIndex * 2 + 2; //index of the child on the right
            int swapIndex = 0;

            if (childIndexLeft < _currentItemCount) //if the left child index is not the last index
            {
                //finding the better child to try swap positions to
                swapIndex = childIndexLeft;

                if (childIndexRight < _currentItemCount) //if the right child index is also not the last index
                {
                    //CompareTo() returns 1 if the item has higher priority than the parent
                    //returns 0 if priority is the same
                    //returns -1 if the item has lower priority than the parent
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                    {
                        //if the child index on the left has less priority (higher fCost) than the right
                        swapIndex = childIndexRight;
                    }
                }

                //check if the chosen child is suitable to swap position with 
                if (item.CompareTo(_items[swapIndex]) < 0)
                {
                    //if the parent has higher fCost that the child, swap it down.
                    Swap(item, _items[swapIndex]);
                }
                else
                {
                    return; //if the parent has lower fCost (higher priority) than both children.
                }
            }
            else
            {
                return; //if the item does not have any children
            }
        }
    }

    /// <summary>
    /// Sort the item passed in upward to find its parent and proper position.
    /// Only use for items that are not first in the heap.
    /// </summary>
    /// <param name="item">The item to be sorted.</param>
    private void SortUp(T item)
    {
        //we are sorting up so we need to look at the item's potential parents.
        //based on how a binary tree or this data structure is made, all child items should be twice of its parent index with a positive offset from 1-2
        int parentIndex = (item.HeapIndex - 1) / 2; //find the parent index by using the opposite way

        //while loop to find suitable parent and position
        while (true)
        {
            T parentItem = _items[parentIndex]; //get parent item from index

            //CompareTo() returns 1 if the item has higher priority than the parent
            //returns 0 if priority is the same
            //returns -1 if the item has lower priority than the parent
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item,
                    parentItem); //swap the position of the item and its parent if item has higher priority (lower fCost)
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2; //recalculate the parent index
        }
    }

    /// <summary>
    /// Swaps the position of 2 items.
    /// </summary>
    /// <param name="item1">1st item.</param>
    /// <param name="item2">2nd item.</param>
    private void Swap(T item1, T item2)
    {
        //swap the 2 items to their opposite positions in the heap
        _items[item1.HeapIndex] = item2;
        _items[item2.HeapIndex] = item1;

        //now we need to change the items' own heap index value

        //use tuples to swap the 2 items heap index without having to use temp variable
        (item1.HeapIndex, item2.HeapIndex) = (item2.HeapIndex, item1.HeapIndex);

        // int item1Index = item1.HeapIndex; //saves the heap index of item 1
        // item1.HeapIndex = item2.HeapIndex; //set item 1's index to the those of item 2
        // item2.HeapIndex = item1Index; //set the item 2's index to be of the temporary int we just made
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex { get; set; }
}