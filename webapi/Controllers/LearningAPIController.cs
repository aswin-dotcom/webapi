using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/LearningAPI")]
    [ApiController]
    public class LearningAPIController:ControllerBase
    {
            [HttpGet]
            public IEnumerable<Record> GetRecords()
            {
                return new List<Record>
                {
                    new Record { Id = 1, Name = "Record 1" },
                    new Record { Id = 2, Name = "Record 2" },
                    new Record { Id = 3, Name = "Record 3" }
                };
            }



    }
}
