using AutoMapper;
using webapi.Models;
using webapi.Models.DTO;

namespace webapi
{
    public class MapConfig :Profile
    {
        public MapConfig()
        {
                CreateMap<Record,RecordDTO>().ReverseMap();
        }
    }
}
