using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IPartyRepository
    {
        Task<Guid> CreatePartyAsync(PartyDto party);
        Task DeletePartiesAsync(List<Guid> ids);
        Task<(List<PartyModel> parties, int numberParties)> GetUserPartiesForPageAsync(Guid userId, int pageNumber);
        Task<Guid> UpdatePartyAsync(PartyDto party);
    }
}