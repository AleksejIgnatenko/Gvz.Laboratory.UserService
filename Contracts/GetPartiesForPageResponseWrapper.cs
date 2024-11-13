namespace Gvz.Laboratory.UserService.Contracts
{
    public record GetPartiesForPageResponseWrapper(
            List<GetPartiesResponse> Parties,
            int numberParties
            );
}
