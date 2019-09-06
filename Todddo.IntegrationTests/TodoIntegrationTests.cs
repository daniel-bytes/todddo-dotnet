using System;
using System.Linq;
using Xunit;
using Todddo.ApiClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace Todddo.IntegrationTests
{
    /// <summary>
    /// Integration tests for /api/todo.
    /// Assumes application is running via `docker-compose up`.
    /// </summary>
    public class TodoIntegrationTests
    {
        [Fact]
        public async Task Test_Todo_Crud_Workflow()
        {
            var client = new TodoApiClient(new HttpClient());

            var newTodo = await TestCreate(client, "Task 123");
            var updatedTodo = await TestUpdate(client, newTodo, "Updated Task 123!");
            await TestDelete(client, updatedTodo);
        }

        private async Task<TodoApiModel> TestCreate(TodoApiClient client, string task)
        {
            var createRequest = new TodoRequestApiModel { Task = task };
            var newTodo = await client.PostAsync(createRequest);
            await TestGet(client, newTodo);

            return newTodo;
        }

        private async Task<TodoApiModel> TestUpdate(TodoApiClient client, TodoApiModel model, string task)
        {
            var updateRequest = new TodoRequestApiModel { Task = task };
            var updatedTodo = await client.PutAsync(model.Id, updateRequest);
            await TestGet(client, updatedTodo);

            return updatedTodo;
        }

        private async Task TestGet(TodoApiClient client, TodoApiModel model)
        {
            var single = await client.GetAsync(model.Id);
            Assert.Equal(model.Task, single.Task);

            var all = await client.GetAllAsync();
            Assert.NotEmpty(all.Where(x => x.Id == model.Id && x.Task == model.Task));
        }

        private async Task TestDelete(TodoApiClient client, TodoApiModel model)
        {
            var deletedTodo = await client.DeleteAsync(model.Id);
            await Assert.ThrowsAsync<ApiException>(async () => await client.GetAsync(model.Id));

            var all = await client.GetAllAsync();
            Assert.Empty(all.Where(x => x.Id == model.Id));
        }
    }
}
