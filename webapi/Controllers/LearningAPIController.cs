using Microsoft.AspNetCore.Mvc;
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
                return new List<RecordDTO>
                {
                    new RecordDTO { Id = 1, Name = "Record 1" },
                    new RecordDTO { Id = 2, Name = "Record 2" },
                    new RecordDTO { Id = 3, Name = "Record 3" }
                };
            }



    }
}
