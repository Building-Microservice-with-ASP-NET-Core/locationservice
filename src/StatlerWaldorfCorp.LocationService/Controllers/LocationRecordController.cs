using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Controllers {
    
    [Route("locations/{memberId}")]
    public class LocationRecordController : Controller {

        private ILocationRecordRepository locationRepository;
        //private ILogger<LocationRecordController> logger;
        public LocationRecordController(ILocationRecordRepository repository/*,ILogger<LocationRecordController> logger*/) {
            this.locationRepository = repository;
            //this.logger = logger;
        }

        [HttpPost]
        public IActionResult AddLocation(Guid memberId, [FromBody]LocationRecord locationRecord) {
            locationRepository.Add(locationRecord);
            return this.Created($"/locations/{memberId}/{locationRecord.ID}", locationRecord);
        }

        [HttpGet]
        public IActionResult GetLocationsForMember(Guid memberId) {
            //logger.LogInformation("Get===");
            return this.Ok(locationRepository.AllForMember(memberId));
        }

        [HttpGet("latest")]
        public IActionResult GetLatestForMember(Guid memberId) {
            return this.Ok(locationRepository.GetLatestForMember(memberId));
        }
    }
}