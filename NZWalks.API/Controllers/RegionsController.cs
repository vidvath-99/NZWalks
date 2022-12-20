using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RegionsController : Controller
    {
       
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;


        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
         
            this.mapper = mapper;
            this.regionRepository = regionRepository;

        }



        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            ////return DTO region
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region();
            //    {





            //    };
            //    regionsDTO.Add(regionDTO);
            //});



            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]

        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var RegionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(RegionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //validate the request

            if (!validateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            //request dto to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };


            //pass details to repository

            region = await regionRepository.AddAsync(region);

            //convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                ID = region.ID,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,

            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.ID }, regionDTO);



        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {

            //get region from database
            var region = await regionRepository.DeleteAsync(id);

            //if null not found
            if (region == null)
            {
                return NotFound();

            }
            //convert region back to DTO
            var regionDTO = new Models.DTO.Region
            {
                ID = region.ID,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,

            };


            //return ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //validate incoming request
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };

            //Update region using respository

            region = await regionRepository.UpdateAsync(id, region);

            //If null not found
            if (region == null)
            {
                return NotFound();
            }

            //Convert domain to dto
            var regionDTO = new Models.DTO.Region
            {
                ID = region.ID,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,

            };

            //Return ok response
            return Ok(regionDTO);
        }

        #region Private methods

        private bool validateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                    $"Add region data is required.");
                return false;

            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space.");
            }

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                   $"{nameof(addRegionRequest.Area)} cannot be less than or eqaul to zero.");

            }

           
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                   $"{nameof(addRegionRequest.Population)} cannot be less than zero.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }


            return true;


        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"Add region data is required.");
                return false;

            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null or empty or white space.");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                   $"{nameof(updateRegionRequest.Area)} cannot be less than or eqaul to zero.");

            }

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                   $"{nameof(updateRegionRequest.Population)} cannot be less than zero.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }


            return true;


        }
        #endregion

    }
}
