using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;
using webapi.Models.DTO;

namespace webapi.Controllers
{
    [Route("api/LearningAPI")]
    [ApiController]
    public class LearningAPIController:ControllerBase
    {
            [HttpGet]
            public IEnumerable<RecordDTO> GetRecords()
            {
                return RecordStore.records;
            }
           [HttpGet("{id:int}")]
           public RecordDTO GetRecord(int id)
           {
                return RecordStore.records.FirstOrDefault(r => r.Id == id);
           }



    }
}
