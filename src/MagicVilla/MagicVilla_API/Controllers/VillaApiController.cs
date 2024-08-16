using MagicVilla_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        [Route("api/VillaApi")]
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>()
            {
                new Villa() {Id=1, Name="Beach view"},
                new Villa() {Id=2, Name="Pool view" }
            };
        }

    }
}
    
