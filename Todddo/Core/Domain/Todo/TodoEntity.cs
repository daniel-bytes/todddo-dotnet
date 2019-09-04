using System;

namespace Todddo.Core.Domain.Todo
{
    public class TodoEntity: IEntity<TodoId>
    {
        public TodoId Id { get; }
        public string Task { get; }

        public TodoEntity(TodoId id, string task)
        {
            Id = id;
            Task = task;
        }
    }
}
