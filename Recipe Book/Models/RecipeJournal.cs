using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Models
{
    /// <summary>
    /// Class <code>RecipeJournal</code> represents a collection of
    /// <code>RecipeJournalEntry</code> objects. Entries can be added,
    /// retrieved, and removed from this collection.
    /// </summary>
    public class RecipeJournal : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private const int DEFAULT_SIZE = 10;

        private int size; // number of items
        private RecipeJournalEntry[] entries; // internal list of entries

        // TODO: move this up to the model?
        public ObservableCollection<RecipeJournalEntry> Entries
        {
            get
            {
                ObservableCollection<RecipeJournalEntry> result = new ObservableCollection<RecipeJournalEntry>();
                for (int i = 0; i < size; i++)
                {
                    result.Add(entries[i]);
                }
                return result;
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

        /// <summary>
        /// Adds the given recipe journal entry to the recipe journal.
        /// </summary>
        /// <param name="newEntry">
        /// The entry to add to the recipe journal.
        /// </param>
        public void add(RecipeJournalEntry newEntry)
        {
            if (newEntry != null)
            {
                ensureSize();
                entries[size] = newEntry;
                size++;
            }
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
        public void empty()
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
        public void removeAt(int index)
        {
            if (index < 0 || index > size)
            {
                throw new IndexOutOfRangeException("Index must be " +
                    "between 0 and the current journal size: " + index);
            }
            size--;
            for (int i = index; i < size; i++)
            {
                entries[index] = entries[index + 1];
            }
        }

        /// <summary>
        /// Removes the given entry from the journal if it exists.
        /// </summary>
        /// <param name="toRemove"></param>
        public void remove(RecipeJournalEntry toRemove)
        {
            if (toRemove == null)
            {
                throw new ArgumentNullException("The entry to remove " +
                    "must not be null.");
            }
            int index = indexOf(toRemove);
            removeAt(index);
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
        public int indexOf(RecipeJournalEntry toFind)
        {
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
        public bool contains(RecipeJournalEntry toFind)
        {
            return indexOf(toFind) >= 0;
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
    }
}
