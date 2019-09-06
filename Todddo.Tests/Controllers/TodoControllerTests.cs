using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todddo.Controllers;
using Todddo.Core.Domain.Todo;
using Todddo.Core.Infra.Todo;
using Todddo.Models;
using Todddo.Models.Todo;
using Xunit;

namespace Todddo.Tests.Controllers
{
    public class TodoControllerTests
    {
        [Fact]
        public async Task Can_Get_All_Todos()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var result = (await subject.Get()).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(
                new[] { TodoDataFixture.TodoApiModel },
                ((IEnumerable<TodoApiModel>)result.Value).ToArray(),
                new TodoApiModelComparer()
            );
        }

        [Fact]
        public async Task Can_Get_A_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var result = (await subject.Get("test-id")).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(
                TodoDataFixture.TodoApiModel,
                (TodoApiModel)result.Value,
                new TodoApiModelComparer()
            );
        }

        [Fact]
        public async Task Can_404_When_Getting_A_Missing_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var result = (await subject.Get("BADID")).Result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.IsAssignableFrom<ApiErrorModel>(result.Value);
            Assert.Equal(
                "not_found",
                ((ApiErrorModel)result.Value).Code
            );
        }

        [Fact]
        public async Task Can_Post_A_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var request = new TodoRequestApiModel { Task = "Created Value" };
            var result = (await subject.Post(request)).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.False(String.IsNullOrWhiteSpace(((TodoApiModel)result.Value).Id));
            Assert.Equal(
                request.Task,
                ((TodoApiModel)result.Value).Task
            );
            Assert.Equal(
                new[] { "Created Value", "Test Value" },
                fixture.Repository.Values.Values.Select(x => x.Task).OrderBy(x => x).ToArray()
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        public async Task Can_400_When_Posting_An_Invalid_Todo(string task)
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var request = new TodoRequestApiModel { Task = task };
            var result = (await subject.Post(request)).Result as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(
                "failed_validation",
                ((ApiErrorModel)result.Value).Code
            );
            Assert.Equal(
                new[] { "Test Value" },
                fixture.Repository.Values.Values.Select(x => x.Task).ToArray()
            );
        }

        [Fact]
        public async Task Can_Put_A_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var request = new TodoRequestApiModel { Task = "Updated Value" };
            var result = (await subject.Put("test-id", request)).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(
                TodoDataFixture.UpdatedTodoApiModel,
                (TodoApiModel)result.Value,
                new TodoApiModelComparer()
            );
            Assert.Equal(
                new[] { "Updated Value" },
                fixture.Repository.Values.Values.Select(x => x.Task).ToArray()
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        public async Task Can_400_When_Putting_An_Invalid_Todo(string task)
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var request = new TodoRequestApiModel { Task = task };
            var result = (await subject.Put("test-id", request)).Result as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(
                "failed_validation",
                ((ApiErrorModel)result.Value).Code
            );
            Assert.Equal(
                new[] { "Test Value" },
                fixture.Repository.Values.Values.Select(x => x.Task).ToArray()
            );
        }

        [Fact]
        public async Task Can_404_When_Putting_A_Missing_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var request = new TodoRequestApiModel { Task = "Updated value" };
            var result = (await subject.Put("BADID", request)).Result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.IsAssignableFrom<ApiErrorModel>(result.Value);
            Assert.Equal(
                "not_found",
                ((ApiErrorModel)result.Value).Code
            );
            Assert.Equal(
                new[] { "Test Value" },
                fixture.Repository.Values.Values.Select(x => x.Task).ToArray()
            );
        }

        [Fact]
        public async Task Can_Delete_A_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var result = (await subject.Delete("test-id")).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(
                TodoDataFixture.TodoApiModel,
                (TodoApiModel)result.Value,
                new TodoApiModelComparer()
            );
            Assert.Empty(fixture.Repository.Values);
        }

        [Fact]
        public async Task Can_404_When_Deleting_A_Missing_Todo()
        {
            var fixture = new TodoDataFixture();
            var subject = new TodoController(fixture.Repository);
            var result = (await subject.Delete("BADID")).Result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.IsAssignableFrom<ApiErrorModel>(result.Value);
            Assert.Equal(
                "not_found",
                ((ApiErrorModel)result.Value).Code
            );
            Assert.Single(fixture.Repository.Values);
        }
    }

    public class TodoDataFixture
    {
        public static TodoEntity TodoEntity { get; } =
            new TodoEntity(new TodoId("test-id"), "Test Value");

        public static TodoApiModel TodoApiModel { get; } =
            new TodoApiModel { Id = "test-id", Task = "Test Value" };

        public static TodoApiModel UpdatedTodoApiModel { get; } =
            new TodoApiModel { Id = "test-id", Task = "Updated Value" };

        public TodoInMemoryRepository Repository { get; }

        public TodoDataFixture()
        {
            Repository = new TodoInMemoryRepository(new TodoValidator(), new Dictionary<TodoId, TodoEntity>
            {
                { TodoEntity.Id, TodoEntity }
            });
        }
    }

    public class TodoApiModelComparer : IEqualityComparer<TodoApiModel>
    {
        public bool Equals(TodoApiModel x, TodoApiModel y)
        {
            if (x == null) return y == null;
            if (y == null) return false;

            return x.Id == y.Id && x.Task == y.Task;
        }

        public int GetHashCode(TodoApiModel obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}
