using System.ComponentModel.DataAnnotations;

namespace webapi.Models.DTO
{
    public class RecordDTO
    {

        public int Id { get; set; }
        [Required]
        //[MaxLength(10)]
        public string Name { get; set; }
        public string? percentage { get; set; }

        public string Standard { get; set; }

        public string City { get; set; }
    }
}
