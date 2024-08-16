using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        [Route("api/VillaApi")]
        [HttpGet]
        public IEnumerable<VillaDTO> GetVillas()
        {
            return new List<VillaDTO>()
            {
                new VillaDTO() {Id=1, Name="Beach view"},
                new VillaDTO() {Id=2, Name="Pool view" }
            };
        }

    }
}
    
