using System;
using System.Collections.Generic;
using Todddo.Core.Domain.Todo;

namespace Todddo.Core.Infra.Todo
{
    public class TodoInMemoryRepository:
        InMemoryRepository<TodoId, TodoEntity>,
        ITodoRepository
    {
        public TodoInMemoryRepository(TodoValidator validator)
            : base(validator)
        {
        }

        public TodoInMemoryRepository(TodoValidator validator, IEnumerable<KeyValuePair<TodoId, TodoEntity>> values)
            : base(validator, values)
        {
        }
    }
}
