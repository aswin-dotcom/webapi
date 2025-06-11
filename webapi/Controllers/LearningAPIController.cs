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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RecordDTO>> GetRecords()
            {
                return Ok(RecordStore.records);
            }
           [HttpGet("{id:int}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200,Type =typeof(RecordDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
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
