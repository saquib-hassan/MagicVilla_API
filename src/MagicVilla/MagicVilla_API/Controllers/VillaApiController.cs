﻿using MagicVilla_API.Data;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task <ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            return Ok(await _db.Villas.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                //_logger.Log("Id shouldn't be zero...","error");
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);

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
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom erorr", "Villa already exists");
                return BadRequest(ModelState);
            }


            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            //Id is not present int the model class of VillaCreateDTO
            //if (villaDTO.Id > 0)
            //{
            //    return BadRequest(StatusCodes.Status500InternalServerError);
            //}

            Villa model = new Villa()
            {
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Amenity = villaDTO.Amenity,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                ImageUrl = villaDTO.ImageUrl,
                Details = villaDTO.Details,

            };

            _db.Villas.Add(model);
           await _db.SaveChangesAsync();
            

            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
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
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if(patchDto ==  null || id == 0)
            {

            return BadRequest(); 
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            VillaUpdateDTO villaDto = new VillaUpdateDTO()
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
            await _db.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
    
