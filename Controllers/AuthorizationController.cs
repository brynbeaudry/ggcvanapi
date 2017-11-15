using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using ggcvan.Models;
using ggcvan.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OpenIddict.Core;


public class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public AuthorizationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AuthorizationController> logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _config = configuration;
        }
    [HttpPost("/connect/token"), Produces("application/json")]
    public async Task<IActionResult> ExchangeAsync(OpenIdConnectRequest request)
    {
        // Reject the token request if client_id or client_secret is missing.
            /* if (string.IsNullOrEmpty(request.ClientId) || string.IsNullOrEmpty(request.ClientSecret))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidRequest,
                    ErrorDescription = "Missing credentials: ensure that your credentials were correctly " +
                                       "flowed in the request body or in the authorization header"
                });
            } */

            // Retrieve the application details from the database.
            /* var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);

            if (application == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidClient,
                    ErrorDescription = "The credentials is invalid."
                });
            } */

            if (request.IsPasswordGrantType())
            {
                /* if (!string.Equals(application.Type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.Ordinal))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidClient,
                        ErrorDescription = "Only confidential clients are allowed to use password grant type."
                    });
                } */

                var user = await _userManager.FindByNameAsync(request.Username);

                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }
                var canSignIn = await _signInManager.CanSignInAsync(user);
                // Ensure the user is allowed to sign in.
                if (!canSignIn)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                // Reject the token request if two-factor authentication has been enabled by the user.
                if (_userManager.SupportsUserTwoFactor && await _userManager.GetTwoFactorEnabledAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                // Ensure the user is not already locked out.
                if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Ensure the password is valid.
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    if (_userManager.SupportsUserLockout)
                    {
                        await _userManager.AccessFailedAsync(user);
                    }

                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                if (_userManager.SupportsUserLockout)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            if (request.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the authorization code.
                var info = await HttpContext.AuthenticateAsync(
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the authorization code.
                // Note: if you want to automatically invalidate the authorization code
                // when the user password/roles change, use the following line instead:
                var user = await _signInManager.ValidateSecurityStampAsync(info.Principal);
                // var user = await _userManager.GetUserAsync(info.Principal);

                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The authorization code is no longer valid."
                    });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the authorization code, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user, info.Properties);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                var user = await _signInManager.ValidateSecurityStampAsync(info.Principal);
                //var user = await _userManager.GetUserAsync(info.Principal);

                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The refresh token is no longer valid."
                    });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user, info.Properties);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
    }

        //[Authorize]
        //[ValidateAntiForgeryToken]
        //[FormValueRequired("Authorize")]
        [HttpPost("connect/authorize")]
        public async Task<IActionResult> Accept(OpenIdConnectRequest request)
        {
            // Retrieve the profile of the logged in user.
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new
                {
                    Error = OpenIdConnectConstants.Errors.ServerError,
                    ErrorDescription = "An internal error has occurred - User Related"
                });
            }

            // Create a new authentication ticket.
            var ticket = await CreateTicketAsync(request, user);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        //likely won't be used for ios
        [HttpGet("/connect/authorize")]
        public async Task<IActionResult> Authorize(OpenIdConnectRequest request)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // If the client application request promptless authentication,
                // return an error indicating that the user is not logged in.
                if (request.HasPrompt(OpenIdConnectConstants.Prompts.None))
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIdConnectConstants.Properties.Error] = OpenIdConnectConstants.Errors.LoginRequired,
                        [OpenIdConnectConstants.Properties.ErrorDescription] = "The user is not logged in."
                    });

                    // Ask OpenIddict to return a login_required error to the client application.
                    return Forbid(properties, OpenIdConnectServerDefaults.AuthenticationScheme);
                }

                return Challenge();
            }

            // Retrieve the profile of the logged in user.
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new
                {
                    Error = OpenIdConnectConstants.Errors.ServerError,
                    ErrorDescription = "An internal error has occurred"
                });
            }

            // Create a new authentication ticket.
            var ticket = await CreateTicketAsync(request, user);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

    [Authorize]
    [HttpPost("/connect/logout")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
        {
            // Ask ASP.NET Core Identity to delete the local and external cookies created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            await _signInManager.SignOutAsync();

            // Returning a SignOutResult will ask OpenIddict to redirect the user agent
            // to the post_logout_redirect_uri specified by the client application.
            return SignOut(OpenIdConnectServerDefaults.AuthenticationScheme);
        }

        [HttpGet("/api/userinfo")]
        //[Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Userinfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user profile is no longer available."
                });
            }

            var claims = new JObject
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [OpenIdConnectConstants.Claims.Subject] = user.Id,
                //[OpenIdConnectConstants.Claims.Nickname] = user.Nickname
            };

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Email))
            {
                claims[OpenIdConnectConstants.Claims.Email] = user.Email;
                //claims[OpenIdConnectConstants.Claims.EmailVerified] = user.EmailConfirmed;
            }

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Phone))
            {
                claims[OpenIdConnectConstants.Claims.PhoneNumber] = user.PhoneNumber;
                claims[OpenIdConnectConstants.Claims.PhoneNumberVerified] = user.PhoneNumberConfirmed;
            }

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIddictConstants.Scopes.Roles))
            {
                //laims["roles"] = JArray.FromObject(user.ROles);
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Json(claims);
        }

    private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, ApplicationUser user, AuthenticationProperties properties = null)
    {
        // Create a new ClaimsPrincipal containing the claims that will be used to create an id_token, a token or a code.
        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        // Create a new authentication ticket holding the user identity.
        var ticket = new AuthenticationTicket(principal, properties, OpenIdConnectServerDefaults.AuthenticationScheme);

        if (!request.IsRefreshTokenGrantType())
        {
            // Set the list of scopes granted to the client application.
            // Note: the offline_access scope must be granted to allow OpenIddict to return a refresh token.
            ticket.SetScopes(new[]
            {
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Email,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()));
            
        }
        ticket.SetResources("RouteManagerAPI");

        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them to a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        foreach (var claim in ticket.Principal.Claims)
        {
            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            if (claim.Type == "AspNet.Identity.SecurityStamp")
            {
                continue;
            }
            var destinations = new List<string>
            {
                OpenIdConnectConstants.Destinations.AccessToken
            };
            // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
            // The other claims will only be added to the access_token, which is encrypted when using the default format.
            if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
            {
                destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
            }
            claim.SetDestinations(destinations);
        }
        return ticket;
    }

        /* [Authorize]
        //[ValidateAntiForgeryToken]
        //[FormValueRequired("Deny")]
        [HttpPost("~/Connect/Authorize")]
        public IActionResult Deny()
        {
            // Notify OpenIddict that the authorization grant has been denied by the resource owner
            // to redirect the user agent to the client application using the appropriate response_mode.
            return Forbid(OpenIdConnectServerDefaults.AuthenticationScheme);
        }
 */

        /* [HttpGet]
        public IActionResult Logout(OpenIdConnectRequest request)
        {
            // Flow the request_id to allow OpenIddict to restore
            // the original logout request from the distributed cache.
            return View(new LogoutViewModel
            {
                RequestId = request.RequestId,
            });
        } */
}