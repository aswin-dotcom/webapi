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
            public ActionResult<IEnumerable<RecordDTO>> GetRecords()
            {
                return Ok(RecordStore.records);
            }
           [HttpGet("{id:int}")]
           public ActionResult<RecordDTO> GetRecord(int id)
           {
            if (id == 0) { 
                return BadRequest();
            }
             var record  =  RecordStore.records.FirstOrDefault(r => r.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
           }



    }
}
