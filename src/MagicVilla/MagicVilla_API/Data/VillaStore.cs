using MagicVilla_API.Models.Dto;
using System.Xml.Linq;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>()
       {
           new VillaDTO() { Id = 1,Name="Pool view", Ocuppancy=4, Sqft=200 },
           new VillaDTO() {Id = 2,Name="Beach view", Ocuppancy=3, Sqft=300 }
       };
}
}
