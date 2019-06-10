using ApiCoreNHibernateCrudPagination.Entities;
using FluentNHibernate.Mapping;

namespace ApiCoreNHibernateCrudPagination.Data
{
    public class Mappings
    {
        public class TodoMap : ClassMap<Todo>
        {
            public TodoMap()
            {
                Table("todos");

                Id(t => t.Id);
                Map(t => t.Title);
                Map(t => t.Description).Length(1200);
                Map(t => t.Completed);
                Map(t => t.CreatedAt);
                Map(t => t.UpdatedAt);
            }
        }
    }
}