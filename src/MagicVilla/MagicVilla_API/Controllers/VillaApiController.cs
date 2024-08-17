using MagicVilla_API.Data;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net.WebSockets;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VillaApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                //_logger.Log("Id shouldn't be zero...","error");
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

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
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom erorr", "Villa already exists");
                return BadRequest(ModelState);
            }


            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return BadRequest(StatusCodes.Status500InternalServerError);
            }

            Villa model = new Villa()
            {
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Occupancy = villaDTO.Occupancy,
                Amenity = villaDTO.Amenity,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                ImageUrl = villaDTO.ImageUrl,
                Details = villaDTO.Details,

            };

            _db.Villas.Add(model);
            _db.SaveChanges();
            

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            
            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (id == 0 || id != villaDTO.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            //villa.Id = villaDTO.Id;
            //villa.Name = villaDTO.Name;
            //villa.Ocuppancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;
            Villa model = new Villa()
            {
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Occupancy = villaDTO.Occupancy,
                Amenity = villaDTO.Amenity,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                ImageUrl = villaDTO.ImageUrl,
                Details = villaDTO.Details,

            };

            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDto)
        {
            if(patchDto ==  null || id == 0)
            {

            return BadRequest(); 
            }

            var villa = _db.Villas.FirstOrDefault(x=>x.Id == id);

            VillaDTO villaDto = new VillaDTO()
            {
                Name = villa.Name,
                Id = villa.Id,
                Occupancy = villa.Occupancy,
                Sqft = villa.Sqft,
                Rate = villa.Rate,
                ImageUrl = villa.ImageUrl,
                Details = villa.Details,
                Amenity = villa.Amenity

            };

            if(villa == null)
            {
                return BadRequest();
            }

            patchDto.ApplyTo(villaDto,ModelState);

            Villa model = new Villa()
            {
                Name = villaDto.Name,
                Id = villaDto.Id,
                Occupancy = villaDto.Occupancy,
                Amenity = villaDto.Amenity,
                Sqft = villaDto.Sqft,
                Rate = villaDto.Rate,
                ImageUrl = villaDto.ImageUrl,
                Details = villaDto.Details,

            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
    
