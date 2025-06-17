using System.Linq.Expressions;
using webapi.Models;

namespace webapi.Repository.IRepository
{
    public interface IRepository <T>  where T : class
    {
        //        Part Meaning
        //Record  ->  The input type → each item in the table/collection.
        //bool -> The return type → filter condition must return true/false.
        //Func<Record, bool> -> A delegate (function pointer) that takes a Record and returns a bool.
        //Expression<Func<...>> -> A LINQ expression (not just a function, but a tree structure).

        //tracked = true (default):
        //EF tracks the entity.
        //tracked = false:
        //EF does not track the entity.

        Task Remove(T entity);

        Task<List<T>> GetAll(Expression<Func<T, bool>>? Filter = null);
        Task<T> Get(Expression<Func<T, bool>>? Filter = null, bool tracked = true);
        Task Create(T entity);
        Task save();
    }
}
