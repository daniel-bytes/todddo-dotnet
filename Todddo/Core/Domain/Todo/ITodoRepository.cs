using System;

namespace Todddo.Core.Domain.Todo
{
    public interface ITodoRepository: IRepository<TodoId, TodoEntity>
    {
    }
}
