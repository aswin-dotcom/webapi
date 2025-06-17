using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using webapi.Data;
using webapi.Loging;
using webapi.Models;
using webapi.Models.DTO;
using webapi.Repository.IRepository;

namespace webapi.Controllers
{
    [Route("api/LearningAPI")]
    [ApiController]
    public class LearningAPIController:ControllerBase
    {
        private readonly ILogging _logger;
        protected APIresponsecs _response;

        //private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IRecordRepository _dbRecords;
        public LearningAPIController(ILogging logger, IRecordRepository dbRecords, IMapper mapper)
        {
            _logger = logger;
            _dbRecords = dbRecords;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<APIresponsecs>> GetRecords()
            {
            _logger.Log("Records got","");
            IEnumerable<Record> recordlist = await _dbRecords.GetAll();

            _response.StatusCode =  System.Net.HttpStatusCode.OK;
            _response.Result = _mapper.Map<List<RecordDTO>>(recordlist);
            return Ok(_response);
            }


           [HttpGet("{id:int}",Name = "GetRecords")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIresponsecs>> GetRecord(int id)
           {
            if (id == 0) {
                _logger.Log("Invalid number", "error");
                return BadRequest();
            }
             var record  = await  _dbRecords.Get(r => r.Id == id,tracked:false);
            if (record == null)
            {
                return NotFound();
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = _mapper.Map<RecordDTO>(record);


            return Ok(_response);
           }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIresponsecs>> PostRecord ([FromBody]RecordDTO record)

        {
            if(await _dbRecords.Get(u=>u.Name.ToLower()==record.Name.ToLower(),tracked:false) != null)
            {
                ModelState.AddModelError("CustomError", "Record already exists!");
                return BadRequest(ModelState);
            }
            if(record == null)
            {
                return BadRequest();
            }

            //record.Id =  _dbRecords.get(OrderByDescending(u => u.Id).FirstOrDefault().Id+1);
            var rec   =  _mapper.Map<Record>(record);
            //Record rec = new Record()
            //{
            //    Id = record.Id,
            //    Name = record.Name,
            //    Standard = record.Standard,
            //    City = record.City,
            //    percentage = record.percentage
            //};


           await _dbRecords.Create(rec);
          await   _dbRecords.save();
            _response.StatusCode = System.Net.HttpStatusCode.Created;
            _response.Result = _mapper.Map<RecordDTO>(rec);
            return CreatedAtRoute("GetRecords", new {id = rec.Id },_response);
            
        }
        [HttpDelete("{id:int}", Name = "DeleteRecord")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIresponsecs>> DeleteRecord(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var record =await  _dbRecords.Get(u => u.Id == id);
            if (record == null)
            {
                return NotFound();
            }
             await _dbRecords.Remove(record);
            _response.StatusCode = System.Net.HttpStatusCode.NoContent;
            //_response.Result = _mapper.Map<RecordDTO>(record);
            _response.IsSuccess = true;    
            //await _dbRecords.save();
            return Ok(_response);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateRecord")]
        public async Task<ActionResult<APIresponsecs>> UpdateRecord(int id, [FromBody] RecordDTO record)
        {
            if (record == null || id != record.Id)
            {
                return BadRequest();
            }
            //var existingRecord = await  _db.Records.FirstOrDefaultAsync(u => u.Id == id);
            //if (existingRecord == null)
            //{
            //    return NotFound();
            //}
            var updaterecord = _mapper.Map<Record>(record);
            //existingRecord.Name = record.Name;
            //existingRecord.City = record.City;
            //existingRecord.Standard = record.Standard;
          await  _dbRecords.Update(updaterecord);
            _response.StatusCode = System.Net.HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            //await _dbRecords.save();
            //return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialRecord")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRecordProperty(int id, JsonPatchDocument<RecordDTO> jsonPatchDocument)
        {
            if (id == 0 || jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var record = await _dbRecords.Get(u => u.Id == id); // tracked by default

            if (record == null)
            {
                return NotFound();
            }

            var temp = _mapper.Map<RecordDTO>(record);

            jsonPatchDocument.ApplyTo(temp, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map updated DTO values back to the tracked entity
            _mapper.Map(temp, record); // This updates the existing tracked entity

            await _dbRecords.save(); // No need to call Update()

            return NoContent();
        }


    }
}
