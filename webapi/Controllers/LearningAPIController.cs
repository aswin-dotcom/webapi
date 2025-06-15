using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using webapi.Data;
using webapi.Loging;
using webapi.Models;
using webapi.Models.DTO;

namespace webapi.Controllers
{
    [Route("api/LearningAPI")]
    [ApiController]
    public class LearningAPIController:ControllerBase
    {
        private readonly ILogging _logger;
        private readonly ApplicationDbContext _db;
        public LearningAPIController(ILogging logger,ApplicationDbContext db)
        {
                _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<IEnumerable<RecordDTO>>> GetRecords()
            {
            _logger.Log("Records got","");
                return Ok(await _db.Records.ToListAsync());
            }


           [HttpGet("{id:int}",Name = "GetRecords")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<RecordDTO>> GetRecord(int id)
           {
            if (id == 0) {
                _logger.Log("Invalid number", "error");
                return BadRequest();
            }
             var record  = await  _db.Records.FirstOrDefaultAsync(r => r.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
           }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecordDTO>> PostRecord ([FromBody]RecordDTO record)

        {
            if(await _db.Records.FirstOrDefaultAsync(u=>u.Name.ToLower()==record.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Record already exists!");
                return BadRequest(ModelState);
            }
            if(record == null)
            {
                return BadRequest();
            }

            record.Id =  _db.Records.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;

            Record rec = new Record()
            {
                Id = record.Id,
                Name = record.Name,
                Standard = record.Standard,
                City = record.City,
                percentage = record.percentage
            };


           await  _db.Records.AddAsync(rec);
          await   _db.SaveChangesAsync();
            return CreatedAtRoute("GetRecords", new {id = record.Id },record);
            
        }
        [HttpDelete("{id:int}", Name = "DeleteRecord")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var record =await  _db.Records.FirstOrDefaultAsync(u => u.Id == id);
            if (record == null)
            {
                return NotFound();
            }
             _db.Records.Remove(record);
            await  _db.SaveChangesAsync();
            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateRecord")]
        public async Task<IActionResult> UpdateRecord(int id, [FromBody] RecordDTO record)
        {
            if (record == null || id != record.Id)
            {
                return BadRequest();
            }
            var existingRecord = await  _db.Records.FirstOrDefaultAsync(u => u.Id == id);
            if (existingRecord == null)
            {
                return NotFound();
            }
            existingRecord.Name = record.Name;
            existingRecord.City = record.City;
            existingRecord.Standard = record.Standard;
            _db.Records.Update(existingRecord);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialRecord")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRecordProperty(int id , JsonPatchDocument<RecordDTO> jsonPatchDocument)
        {
            if(id==0 || jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var record = await  _db.Records.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if(record == null)
            {
                return NotFound();
            }
            RecordDTO temp = new RecordDTO()
            {
                Id =  record.Id,
                Name = record.Name,
                Standard = record.Standard,
                City = record.City,
                percentage =  record.percentage

            };

            jsonPatchDocument.ApplyTo(temp, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Record patch = new() { 
                Id  = temp.Id,
                Name = temp.Name,
                Standard = temp.Standard,
                City= temp.City,
                percentage =  temp.percentage
                
            };

            _db.Records.Update(patch);
          await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
