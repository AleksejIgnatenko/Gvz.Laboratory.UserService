using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;
        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }
        [HttpGet]
        [Authorize]
        [Route("getUserPartiesForPage")]
        public async Task<ActionResult> GetUserPartiesForPageAsync(Guid userId, int pageNumber)
        {
            var (parties, numberParties) = await _partyService.GetUserPartiesForPageAsync(userId, pageNumber);
            var response = parties.Select(p => new GetPartiesResponse(p.Id,
                p.BatchNumber,
                p.DateOfManufacture,
                p.ProductName,
                p.SupplierName,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.User.Surname,
                p.Note)).ToList();

            var responseWrapper = new GetPartiesForPageResponseWrapper(response, numberParties);

            return Ok(responseWrapper);
        }
    }
}
