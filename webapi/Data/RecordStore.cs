using webapi.Models.DTO;

namespace webapi.Data
{
    public static class RecordStore
    {
        public static List<RecordDTO> records  =  new List<RecordDTO>
        {
            new RecordDTO { Id = 1, Name = "Record 1" },
            new RecordDTO { Id = 2, Name = "Record 2" },
            new RecordDTO { Id = 3, Name = "Record 3" }
        };
    }
}
