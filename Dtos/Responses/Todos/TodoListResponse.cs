using System.Collections.Generic;
using ApiCoreNHibernateCrudPagination.Dtos.Responses.Shared;
using ApiCoreNHibernateCrudPagination.Entities;
using ApiCrudCore.Models;

namespace ApiCoreNHibernateCrudPagination.Dtos.Responses.Todos
{
    public class TodoListResponse : PagedDto
    {
        public IEnumerable<TodoDto> Todos { get; set; }
//    public int SortBy {get; set;}


        public static TodoListResponse Build(List<Todo> todos, string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            var todoDtos = new List<TodoDto>(todos.Count);

            foreach (var todo in todos)
                todoDtos.Add(TodoDto.Build(todo));


            return new TodoListResponse
            {
                PageMeta = new PageMeta(todos.Count, basePath, currentPage, pageSize,
                    totalItemCount),
                Todos = todoDtos
            };
        }
    }
}