using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Recipe_Book.Models
{
    /// <summary>
    /// Class <code>RecipeJournal</code> represents a collection of
    /// <code>RecipeJournalEntry</code> objects. Entries can be added,
    /// retrieved, and removed from this collection.
    /// </summary>
    public class RecipeJournal : INotifyCollectionChanged, IList
    {

        private const int DEFAULT_SIZE = 10;

        private int size; // number of items
        private RecipeJournalEntry[] entries; // internal list of entries

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                return getSize();
            }
        }

        public bool IsSynchronized => false;

        public object SyncRoot => throw new NotImplementedException();

        public object this[int index]
        {
            get
            {
                return get(index);
            }
            set
            {
                set(index, (RecipeJournalEntry)value);
            }
        }

        /// <summary>
        /// The default constructor, creates an empty recipe journal.
        /// </summary>
        public RecipeJournal()
        {
            size = 0;
            entries = new RecipeJournalEntry[DEFAULT_SIZE];
        }

        // Ensures the sizxe of the internal array
        private void ensureSize()
        {
            if (size >= entries.Length)
            {
                RecipeJournalEntry[] temp = new RecipeJournalEntry[size * 2];
                for (int i = 0; i < entries.Length; i++)
                {
                    temp[i] = entries[i];
                }
                entries = temp;
            }
        }

        /// <summary>
        /// Removes all journal entries from the journal.
        /// </summary>
        public void Clear()
        {
            size = 0;
        }

        /// <summary>
        /// Removes the recipe at the given index. Will throw an 
        /// IllegalArgumentException if the index is greater than the
        /// current journal size.
        /// </summary>
        /// <param name="index">
        /// The index of the entry to remove.
        /// </param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException("Index must be " +
                    "between 0 and the current journal size: " + index);
            }
            RecipeJournalEntry toRemove = entries[index];
            size--;
            for (int i = index; i < size; i++)
            {
                entries[index] = entries[index + 1];
            }
            if (CollectionChanged != null)
            {
                NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Remove;
                NotifyCollectionChangedEventArgs args =
                    new NotifyCollectionChangedEventArgs(action, toRemove, index);
                CollectionChanged(this, args);
            }
        }

        /// <summary>
        /// Removes the given entry from the journal if it exists.
        /// </summary>
        public void Remove(object value)
        {
            RecipeJournalEntry toRemove = (RecipeJournalEntry)value;
            if (toRemove == null)
            {
                throw new ArgumentNullException("The entry to remove " +
                    "must not be null.");
            }
            int index = IndexOf(toRemove);
            RemoveAt(index);
        }

        /// <summary>
        /// Sets the entry at the given index to the given new entry.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newEntry"></param>
        public void set(int index, RecipeJournalEntry newEntry)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException("Index must be " +
                    "between 0 and the current journal size: " + index);
            }
            entries[index] = newEntry;
        }

        /// <summary>
        /// Gets whether the journal has entries in it or not.
        /// </summary>
        /// <returns>
        /// True if the journal is empty. False otherwise.
        /// </returns>
        public bool isEmpty()
        {
            return size == 0;
        }

        /// <summary>
        /// Gets the index of the given recipe journal entry.
        /// </summary>
        /// <param name="toFind">
        /// The journal entry to find in the journal.
        /// </param>
        /// <returns>
        /// The index of the journal entry in the journal. -1 if the
        /// entry is not in the journal.
        /// </returns>
        public int IndexOf(object value)
        {
            RecipeJournalEntry toFind = (RecipeJournalEntry)value;
            for (int i = 0; i < size; i++)
            {
                if (entries[i] == toFind)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets whether the given journal entry is in the journal.
        /// </summary>
        /// <param name="toFind">
        /// The entry to find in the journal.
        /// </param>
        /// <returns>
        /// True if the journal entry is in the journal. False 
        /// otherwise.
        /// </returns>
        public bool Contains(object value)
        {
            RecipeJournalEntry toFind = (RecipeJournalEntry)value;
            return IndexOf(toFind) >= 0;
        }

        /// <summary>
        /// Gets the journal entry at the given index in the journal.
        /// If the index is negative or higher than the total number
        /// of entries in the journal, will throw an IllegalArgumentException.
        /// </summary>
        /// <param name="index">
        /// The index of the journal entry to find in the journal.
        /// </param>
        /// <returns>
        /// The entry if it is found. Null otherwise.
        /// </returns>
        public RecipeJournalEntry get(int index)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException("Index must be " +
                    "between 0 and the current journal size: " + index);
            }
            return entries[index];
        }

        /// <summary>
        /// Gets the number of entries in the journal.
        /// </summary>
        /// <returns></returns>
        public int getSize()
        {
            return size;
        }

        /// <summary>
        /// Gets the most recent journal entry.
        /// </summary>
        /// <returns>
        /// The most recent entry in teh journal or null if journal
        /// is empty.
        /// </returns>
        public RecipeJournalEntry getRecentEntry()
        {
            return null;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Adds the given recipe journal entry to the recipe journal.
        /// </summary>
        /// <param name="newEntry">
        /// The entry to add to the recipe journal.
        /// </param>
        public int Add(object value)
        {
            RecipeJournalEntry newEntry = (RecipeJournalEntry)value;
            if (newEntry != null)
            {
                ensureSize();
                entries[size] = newEntry;
                size++;
                int index = size - 1;
                if (CollectionChanged != null)
                {
                    NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add;
                    NotifyCollectionChangedEventArgs args = 
                        new NotifyCollectionChangedEventArgs(action, newEntry, index);
                    CollectionChanged(this, args);
                }
                return index;
            }
            return -1;
        }

        public static void sort(RecipeJournal data)
        {
            if (data.getSize() > 1)
            {
                int midPoint = data.getSize() / 2;
                RecipeJournal half1 = new RecipeJournal();
                RecipeJournal half2 = new RecipeJournal();
                for (int i = 0; i < midPoint; i++)
                {
                    half1.Add(data.get(i));
                }
                for (int i = midPoint; i < data.getSize(); i++)
                {
                    half2.Add(data.get(i));
                }
                sort(half1);
                sort(half2);
                putTogether(data, half1, half2);
            }
        }

        private static void putTogether(RecipeJournal result,
            RecipeJournal half1, RecipeJournal half2)
        {
            result.Clear();
            int index1 = 0;
            int index2 = 0;
            while (index1 < half1.getSize() &&
                index2 < half2.getSize())
            {
                RecipeJournalEntry item1 = half1.get(index1);
                RecipeJournalEntry item2 = half2.get(index2);
                if (item1.EntryDate <= item2.EntryDate)
                {
                    result.Add(item1);
                    index1++;
                }
                else
                {
                    result.Add(item2);
                    index2++;
                }
            }
            while (index1 < half1.getSize())
            {
                RecipeJournalEntry item1 = half1.get(index1);
                result.Add(item1);
                index1++;
            }
            while (index2 < half2.getSize())
            {
                RecipeJournalEntry item2 = half2.get(index2);
                result.Add(item2);
                index2++;
            }
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
