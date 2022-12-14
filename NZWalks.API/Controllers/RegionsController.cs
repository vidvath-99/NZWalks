using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

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
            this.regionRepository = regionRepository;
            this.mapper = mapper;


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

    }
}
