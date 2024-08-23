using AutoMapper;
using Azure;
using MagicVilla_API.Data;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.InteropServices.Marshalling;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaNumberApi")]
    [ApiController]
    public class VillaNumberApiController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;

        public VillaNumberApiController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _dbVilla = dbVilla;
            _response = new();
            
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {

                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    //_logger.Log("Id shouldn't be zero...","error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _dbVillaNumber.GetAsync(x => x.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }
        //[Route("api/VillaApi")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomErorr", "Villa Number already exists");
                    return BadRequest(ModelState);
                }

                if(await _dbVilla.GetAsync(u=>u.Id == createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa Id is invalid");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                //Id is not present int the model class of VillaCreateDTO
                //if (villaDTO.Id > 0)
                //{
                //    return BadRequest(StatusCodes.Status500InternalServerError);
                //}
                var villaNumber = _mapper.Map<VillaNumber>(createDTO);

                //Villa model = new Villa()
                //{
                //    Name = createDTO.Name,
                //    Occupancy = createDTO.Occupancy,
                //    Amenity = createDTO.Amenity,
                //    Sqft = createDTO.Sqft,
                //    Rate = createDTO.Rate,
                //    ImageUrl = createDTO.ImageUrl,
                //    Details = createDTO.Details,

                //};

                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _dbVillaNumber.GetAsync(x => x.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                await _dbVillaNumber.RemoveAsync(villaNumber);


                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {

                if (id == 0 || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }

                if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa Id is invalid");
                    return BadRequest(ModelState);
                }
                var model = _mapper.Map<VillaNumber>(updateDTO);
                //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
                //villa.Id = villaDTO.Id;
                //villa.Name = villaDTO.Name;
                //villa.Ocuppancy = villaDTO.Occupancy;
                //villa.Sqft = villaDTO.Sqft;
                //Villa model = new Villa()
                //{
                //    Name = updateDTO.Name,
                //    Id = updateDTO.Id,
                //    Occupancy = updateDTO.Occupancy,
                //    Amenity = updateDTO.Amenity,
                //    Sqft = updateDTO.Sqft,
                //    Rate = updateDTO.Rate,
                //    ImageUrl = updateDTO.ImageUrl,
                //    Details = updateDTO.Details,

                //};

                await _dbVillaNumber.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        //[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]

        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        //{
        //    if (patchDto == null || id == 0)
        //    {

        //        return BadRequest();
        //    }

        //    var villa = await _dbVilla.GetAsync(x => x.Id == id, tracked: false);

        //    VillaUpdateDTO villaDto = _mapper.Map<VillaUpdateDTO>(villa);

        //    //VillaUpdateDTO villaDto = new VillaUpdateDTO()
        //    //{
        //    //    Name = villa.Name,
        //    //    Id = villa.Id,
        //    //    Occupancy = villa.Occupancy,
        //    //    Sqft = villa.Sqft,
        //    //    Rate = villa.Rate,
        //    //    ImageUrl = villa.ImageUrl,
        //    //    Details = villa.Details,
        //    //    Amenity = villa.Amenity

        //    //};

        //    if (villa == null)
        //    {
        //        return BadRequest();
        //    }

        //    patchDto.ApplyTo(villaDto, ModelState);
        //    var model = _mapper.Map<Villa>(villaDto);

        //    //Villa model = new Villa()
        //    //{
        //    //    Name = villaDto.Name,
        //    //    Id = villaDto.Id,
        //    //    Occupancy = villaDto.Occupancy,
        //    //    Amenity = villaDto.Amenity,
        //    //    Sqft = villaDto.Sqft,
        //    //    Rate = villaDto.Rate,
        //    //    ImageUrl = villaDto.ImageUrl,
        //    //    Details = villaDto.Details,

        //    //};

        //    await _dbVilla.UpdateAsync(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return NoContent();
        //}
    }
}

