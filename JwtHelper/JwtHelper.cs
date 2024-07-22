using System.Security.Claims;

namespace MedwiseBackend.JwtHelper
{
    public class JwtHelper
    {
        public static string GetUserIdFromToken(ClaimsPrincipal user)
        {
            var email = user.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            if (email == null)
            {
                 throw new UnauthorizedAccessException("UnAuthorized Access");
                /*foreach (var claim in user.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }*/
            }
            return email.Value;
        }
    }
}