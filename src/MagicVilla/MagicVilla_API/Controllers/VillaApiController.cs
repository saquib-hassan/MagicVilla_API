using MagicVilla_API.Data;
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
            return VillaStore.villaList;
        }

        [HttpGet("id")]
        public VillaDTO GetById(int id)
        {
            return VillaStore.villaList.FirstOrDefault(x => x.Id == id);
        }

    }
}
    
