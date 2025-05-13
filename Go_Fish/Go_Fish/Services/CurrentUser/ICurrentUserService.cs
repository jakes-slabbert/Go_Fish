namespace GoFish.Services.CurrentUser
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }

        string? UserName { get; }

        string? Email { get; }

        string? Photo { get; }

        string? DisplayName { get; }

        Task<bool> IsInRoleAsync(string role);

        Task<bool> AuthorizeAsync(string policy);
    }
}
