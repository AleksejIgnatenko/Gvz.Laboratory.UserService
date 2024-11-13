using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IPartyService
    {
        Task<(List<PartyModel> parties, int numberParties)> GetUserPartiesForPageAsync(Guid userId, int pageNumber);
    }
}