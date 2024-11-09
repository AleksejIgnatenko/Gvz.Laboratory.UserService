namespace Gvz.Laboratory.UserService.Contracts
{
    public record GetUsersForPageResponseWrapper(
        List<GetUsersForPageResponse> Users,
        int NumberUsers
    );
}
