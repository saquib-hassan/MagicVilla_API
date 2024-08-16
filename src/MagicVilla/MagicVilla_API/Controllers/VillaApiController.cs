using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        //[Route("api/VillaApi")]
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok (VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult <VillaDTO> GetById(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
        //[Route("api/VillaApi")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if(villaDTO.Id > 0)
            {
                return BadRequest(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla",new {id = villaDTO.Id}, villaDTO);
        }

    }
}
    
