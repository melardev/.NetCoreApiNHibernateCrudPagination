using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCoreNHibernateCrudPagination.Data;
using ApiCoreNHibernateCrudPagination.Entities;
using ApiCoreNHibernateCrudPagination.Enums;
using NHibernate.Linq;

namespace ApiCoreNHibernateCrudPagination.Infrastructure.Services
{
    public class TodoService
    {
        private readonly ISessionFactoryBuilder _sessionFactoryBuilder;

        public TodoService(ISessionFactoryBuilder sessionFactoryBuilder)
        {
            _sessionFactoryBuilder = sessionFactoryBuilder;
        }

        public async Task<Tuple<int, List<Todo>>> FetchMany(int page = 1, int pageSize = 5,
            TodoShow show = TodoShow.All)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                var offset = (page - 1) * pageSize;
                IQueryable<Todo> queryable = null;

                if (show == TodoShow.Completed)
                    queryable = session.Query<Todo>().Where(t => t.Completed);
                else if (show == TodoShow.Pending)
                    queryable = session.Query<Todo>().Where(t => !t.Completed);


                int totalCount;
                List<Todo> todos;

                if (queryable != null)
                {
                    totalCount = await queryable.CountAsync();
                    todos = await queryable.Skip(offset).Take(pageSize).Select(t => new Todo
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Completed = t.Completed,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    }).ToListAsync();
                }
                else
                {
                    totalCount = await session.Query<Todo>().CountAsync();
                    todos = await session.Query<Todo>().Skip(offset).Take(pageSize).Select(t => new Todo
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Completed = t.Completed,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    }).ToListAsync();
                }

                return Tuple.Create(totalCount, todos);
            }
        }


        /// <summary>
        ///     Return a To do object
        /// </summary>
        /// <param name="todoId"></param>
        /// <returns></returns>
        public async Task<Todo> Get(int todoId)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                return await session.Query<Todo>().FirstAsync(t => t.Id == todoId);
            }
        }

        public async Task CreateTodo(Todo todo)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.SaveAsync(todo);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<Todo> Update(int id, Todo todoFromUser)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todoFromDb = await session.Query<Todo>().FirstAsync(t => t.Id == id);
                    todoFromDb.Title = todoFromUser.Title;

                    if (todoFromUser.Description != null)
                        todoFromDb.Description = todoFromUser.Description;

                    todoFromDb.Completed = todoFromUser.Completed;

                    await session.UpdateAsync(todoFromDb);

                    await transaction.CommitAsync();
                    return todoFromDb;
                }
            }
        }


        public async Task<Todo> Update(Todo currentTodo, Todo todoFromUser)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    currentTodo.Title = todoFromUser.Title;

                    if (todoFromUser.Description != null)
                        currentTodo.Description = todoFromUser.Description;

                    currentTodo.Completed = todoFromUser.Completed;

                    await session.UpdateAsync(currentTodo);
                    await transaction.CommitAsync();
                    return currentTodo;
                }
            }
        }

        /// <summary>
        ///     Deletes a To do
        /// </summary>
        /// <param name="todoId"></param>
        /// <returns></returns>
        public async void Delete(int todoId)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todo = await session.Query<Todo>().FirstAsync(t => t.Id == todoId);
                    await session.DeleteAsync(todo);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteAll()
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // Approach 1
                    // session.Query<Todo>().Delete();

                    // Approach 2
                    await session.DeleteAsync("from Todo t");
                    await session.FlushAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<Todo> GetById(int id)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                return await session.Query<Todo>().FirstOrDefaultAsync(t => t.Id == id);
            }
        }
    }
}