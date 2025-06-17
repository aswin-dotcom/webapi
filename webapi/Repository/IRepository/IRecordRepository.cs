using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using webapi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapi.Repository.IRepository
{
    public interface IRecordRepository : IRepository<Record>
    {


        Task Update(Record entity);





        
    }
}
