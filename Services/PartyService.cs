using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        public PartyService(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }
        public async Task<(List<PartyModel> parties, int numberParties)> GetUserPartiesForPageAsync(Guid userId, int pageNumber)
        {
            return await _partyRepository.GetUserPartiesForPageAsync(userId, pageNumber);
        }
    }
}
