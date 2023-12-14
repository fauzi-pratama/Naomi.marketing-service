using Newtonsoft.Json;

namespace Naomi.marketing_service.Services.AuthorizationService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthService _authService;

        public AuthService(IAuthService authService)
        {
            _authService = authService;
        }

        
    }
}
