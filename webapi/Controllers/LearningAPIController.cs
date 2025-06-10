using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;
using webapi.Models.DTO;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningAPIController:ControllerBase
    {
            [HttpGet]
            public IEnumerable<RecordDTO> GetRecords()
            {
                return RecordStore.records;
            }



    }
}
