namespace Gvz.Laboratory.UserService.Contracts
{
    public record GetUsersForPageResponseWrapper(
        List<GetUsersResponse> Users,
        int NumberUsers
    );
}
