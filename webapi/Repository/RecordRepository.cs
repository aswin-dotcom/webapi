using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using webapi.Data;
using webapi.Models;
using webapi.Repository.IRepository;

namespace webapi.Repository
{
    public class RecordRepository : Repository<Record> , IRecordRepository
    {

        private readonly ApplicationDbContext _db;
        public RecordRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
                
        }

     
        public async Task Update(Record entity)
        {
           _db.Records.Update(entity);
           await _db.SaveChangesAsync();

        }
    }
}
