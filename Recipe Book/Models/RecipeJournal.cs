using System;
using System.Collections.Generic;
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
                entries[size] = newEntry;
                size++;
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
            return false;
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
            return null;
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
