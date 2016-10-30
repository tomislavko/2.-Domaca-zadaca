using System;
using System.Collections.Generic;
using System.Linq;
using Exceptions;
using Interfaces;
using Models;


namespace Exceptions
{
    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(string mess) : base(mess)
        {
        }
    }
}

namespace Interfaces
{
    public interface ITodoRepository
    {
        /// <summary >
        /// Gets TodoItem for a given id
        /// </summary >
        /// <returns > TodoItem if found , null otherwise </returns >
        TodoItem Get(Guid todoId);

        /// <summary >
        /// Adds new TodoItem object in database .
        /// If object with the same id already exists ,
        /// method should throw DuplicateTodoItemException with the message " duplicate id: {id }".
        /// </summary >
        void Add(TodoItem todoItem);

        /// <summary >
        /// Tries to remove a TodoItem with given id from the database .
        /// </summary >
        /// <returns > True if success , false otherwise </returns >
        bool Remove(Guid todoId);

        /// <summary >
        /// Updates given TodoItem in database .
        /// If TodoItem does not exist , method will add one .
        /// </summary >
        void Update(TodoItem todoItem);

        /// <summary >
        /// Tries to mark a TodoItem as completed in database .
        /// </summary >
        /// <returns > True if success , false otherwise </returns >
        bool MarkAsCompleted(Guid todoId);

        /// <summary >
        /// Gets all TodoItem objects in database , sorted by date created ( descending )
        /// </summary >
        List<TodoItem> GetAll();

        /// <summary >
        /// Gets all incomplete TodoItem objects in database
        /// </summary >
        List<TodoItem> GetActive();

        /// <summary >
        /// Gets all completed TodoItem objects in database
        /// </summary >
        List<TodoItem> GetCompleted();

        /// <summary >
        /// Gets all TodoItem objects in database that apply to the filter
        /// </summary >
        List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction);
    }
}

namespace Repositories
{
    /// <summary >
    /// Class that encapsulates all the logic for accessing TodoTtems .
    /// </summary >
    public class TodoRepository : ITodoRepository
    {
        /// <summary >
        /// Repository does not fetch todoItems from the actual database ,
        /// it uses in memory storage for this excersise .
        /// </summary >
        private readonly List<TodoItem> _inMemoryTodoDatabase;

        public TodoRepository(List<TodoItem> initialDbState = null)
        {
            if (initialDbState != null)
            {
                _inMemoryTodoDatabase = initialDbState;
            }
            else
            {
                _inMemoryTodoDatabase = new List<TodoItem>();
            }
            // Shorter way to write this in C# using ?? operator :
            // _inMemoryTodoDatabase = initialDbState ?? new List < TodoItem >() ;
            // x ?? y -> if x is not null , expression returns x. Else y.
        }

        public TodoItem Get(Guid todoId)
        {
            var query = from item in _inMemoryTodoDatabase
                where item.Id == todoId
                select item;
            var ret = query.FirstOrDefault();
            return ret;
        }

        public void Add(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                throw new ArgumentNullException();
            }
            if (!_inMemoryTodoDatabase.Exists(p => p == todoItem))
            {
                _inMemoryTodoDatabase.Add(todoItem);
            }
            else
            {
                throw new DuplicateTodoItemException("Duplicate id: " + todoItem.Id);
            }
        }

        public bool Remove(Guid todoId)
        {
            if (todoId.Equals(null))
            {
                return false;
            }
            if (!_inMemoryTodoDatabase.Exists(p => p.Id == todoId))
            {
                return false;
            }
            _inMemoryTodoDatabase.Remove(_inMemoryTodoDatabase.First(p => p.Id == todoId));
            return true;
        }

        public void Update(TodoItem todoItem)
        {
            if (todoItem.Equals(null))
            {
                return;
            }
            Remove(todoItem.Id);
            Add(todoItem);
        }

        public bool MarkAsCompleted(Guid todoId)
        {
            if (_inMemoryTodoDatabase.Exists(p => p.Id == todoId))
            {
                _inMemoryTodoDatabase.First(p => p.Id == todoId).MarkAsCompleted();
                return true;
            }
            return false;
        }

        public List<TodoItem> GetAll()
        {
            return _inMemoryTodoDatabase.Where(p => p != null).ToList();
        }

        public List<TodoItem> GetActive()
        {
            return _inMemoryTodoDatabase.Where(p => !p.IsCompleted).ToList();
        }

        public List<TodoItem> GetCompleted()
        {
            return _inMemoryTodoDatabase.Where(p => p.IsCompleted).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction)
        {
            return _inMemoryTodoDatabase.Where(filterFunction).ToList();
        }
    }

    // implement ITodoRepository
}

namespace Models
{
    public class TodoItem
    {
        public TodoItem(string text)
        {
            Id = Guid.NewGuid(); // Generates new unique identifier
            Text = text;
            IsCompleted = false;
            DateCreated = DateTime.Now; // Set creation date as current time
        }

        public void MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                DateCompleted = DateTime.Now;
            }
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}