using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace InMaPro_cse325.Services;

public class DemoAuthStateProvider : AuthenticationStateProvider
{
    private bool _isAuthenticated = false;
    private string _userName = string.Empty;

    public void Login(string username)
    {
        _isAuthenticated = true;
        _userName = username;
        
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        }, "demo");
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public void Logout()
    {
        _isAuthenticated = false;
        _userName = string.Empty;
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity;
        
        if (_isAuthenticated)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, _userName),
                new Claim(ClaimTypes.Role, "Admin")
            }, "demo");
        }
        else
        {
            identity = new ClaimsIdentity();
        }
        
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }
}