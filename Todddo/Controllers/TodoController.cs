using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Todddo.Core.Domain;
using Todddo.Core.Domain.Todo;
using Todddo.Models.Todo;

namespace Todddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository repository;

        public TodoController(ITodoRepository repository)
        {
            this.repository = repository;
        }

        // GET api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoApiModel>>> Get()
        {
            var result = await repository.List();

            return result.Match(
                good => Ok(good.Select(x => new TodoApiModel
                                                {
                                                    Id = x.Id.Value,
                                                    Task = x.Task
                                                })),
                bad => this.Error(bad)
            );
        }
        

        // GET api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoApiModel>> Get(string id)
        {
            return Result(
                await repository.Get(new TodoId(id))
            );
        }
        
        // POST api/todo
        [HttpPost]
        public async Task<ActionResult<TodoApiModel>> Post([FromBody] TodoRequestApiModel value)
        {
            var todo = new TodoEntity(TodoId.NewId(), value.Task);

            return Result(
                await repository.Create(todo)
            );
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoApiModel>> Put(string id, [FromBody] TodoRequestApiModel value)
        {
            var todo = new TodoEntity(new TodoId(id), value.Task);

            return Result(
                await repository.Update(todo)
            );
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoApiModel>> Delete(string id)
        {
            return Result(
                await repository.Delete(new TodoId(id))
            );
        }

        private ActionResult<TodoApiModel> Result(Either<DomainError, TodoEntity> result)
        {
            return result.Match(
                good => Ok(new TodoApiModel
                                {
                                    Id = good.Id.Value,
                                    Task = good.Task
                                }),
                bad => this.Error(bad)
            );
        }
    }
}
