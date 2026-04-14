using System.Security.Claims;

namespace Course_Selling_System.Helper
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var s = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if(s is null || !int.TryParse(s, out var id))
            {
                throw new InvalidOperationException("Invalid user is claim");
            }
            return id;
        }
    }
}
