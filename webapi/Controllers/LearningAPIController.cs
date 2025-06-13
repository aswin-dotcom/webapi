﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        public LearningAPIController(ILogging logger)
        {
                _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RecordDTO>> GetRecords()
            {
            _logger.Log("Records got","");
                return Ok(RecordStore.records);
            }


           [HttpGet("{id:int}",Name = "GetRecords")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200,Type =typeof(RecordDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        public ActionResult<RecordDTO> GetRecord(int id)
           {
            if (id == 0) {
                _logger.Log("Invalid number", "error");
                return BadRequest();
            }
             var record  =  RecordStore.records.FirstOrDefault(r => r.Id == id);
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
        public ActionResult<RecordDTO> PostRecord ([FromBody]RecordDTO record)

        {
            if(RecordStore.records.FirstOrDefault(u=>u.Name.ToLower()==record.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Record already exists!");
                return BadRequest(ModelState);
            }
            if(record == null)
            {
                return BadRequest();
            }
            if(record.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            record.Id = RecordStore.records.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;
            RecordStore.records.Add(record);
            return CreatedAtRoute("GetRecords", new {id = record.Id },record);
            
        }
        [HttpDelete("{id:int}", Name = "DeleteRecord")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteRecord(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var record = RecordStore.records.FirstOrDefault(u => u.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            RecordStore.records.Remove(record);
            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateRecord")]
        public IActionResult UpdateRecord(int id, [FromBody] RecordDTO record)
        {
            if (record == null || id != record.Id)
            {
                return BadRequest();
            }
            var existingRecord = RecordStore.records.FirstOrDefault(u => u.Id == id);
            if (existingRecord == null)
            {
                return NotFound();
            }
            existingRecord.Name = record.Name;
            existingRecord.City = record.City;
            existingRecord.Standard = record.Standard;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialRecord")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateRecordProperty(int id , JsonPatchDocument<RecordDTO> jsonPatchDocument)
        {
            if(id==0 || jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var record =  RecordStore.records.FirstOrDefault(u => u.Id == id);
            if(record == null)
            {
                return NotFound();
            }
            jsonPatchDocument.ApplyTo(record, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
