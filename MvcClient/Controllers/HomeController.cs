using Flurl;
using Flurl.Http;
using IdentityModel.Client;
using Landstar.Cloud.Clients.UMS2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MvcClient.Controllers;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
/// <remarks>
/// Initializes a new instance of the <see cref="HomeController"/> class.
/// </remarks>
/// <param name="configuration">The configuration.</param>
/// <param name="httpClient">The HTTP client.</param>
[Route("/")]
public class HomeController(IConfiguration configuration, HttpClient httpClient) : Controller
{
  private const string ACCESS_TOKEN = "access_token";

  /// <summary>
  /// Indexes this instance.
  /// </summary>
  /// <returns></returns>
  public IActionResult Index()
  {
    return View();
  }
  /// <summary>
  /// Secures this instance.
  /// </summary>
  /// <returns></returns>
  [Authorize]
  public IActionResult Secure()
  {
    ViewData["Message"] = "Secure page.";

    return View();
  }
  /// <summary>
  /// Logouts the asynchronous.
  /// </summary>
  /// <returns></returns>
  [HttpGet("Logout")]
  public async Task<IActionResult> LogoutAsync()
  {
    var location = $"{Request.Scheme}://{Request.Host}";

    var url = configuration["oidc:sign_out_url"].SetQueryParam("post_logout_redirect_uri", location);


    await HttpContext.SignOutAsync();
    await HttpContext.SignOutAsync("oidc");
    await HttpContext.SignOutAsync("Cookies");

    return Redirect(url);
  }
  /// <summary>
  /// Errors this instance.
  /// </summary>
  /// <returns></returns>
  public IActionResult Error()
  {
    return View();

  }
  /// <summary>
  /// Settingses this instance.
  /// </summary>
  /// <returns></returns>
  public IActionResult Settings()
  {

    return Json(new
    {
      authority = configuration["oidc:authority"],
      client_id = configuration["oidc:client_id"],
      client_secret = configuration["oidc:client_secret"],
      scope = configuration["oidc:scope"]
    });
  }

  /// <summary>
  /// Calls the get user information asynchronous.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns></returns>
  [HttpGet("CallGetUserInfo")]
  public async Task<IActionResult> CallGetUserInfoAsync(System.Threading.CancellationToken cancellationToken = default)
  {


    var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);


    httpClient.SetBearerToken(accessToken);

    var content = await httpClient.GetStringAsync(configuration["oidc:authority"].AppendPathSegment("connect/userinfo"), cancellationToken);

    ViewBag.Json = JObject.Parse(content).ToString();
    return View("Json");
  }

  /// <summary>
  /// Calls the identity API asynchronous.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns></returns>
  [HttpGet("CallIdentityApi")]
  public async Task<IActionResult> CallIdentityApiAsync(CancellationToken cancellationToken = default)
  {

    var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);

    httpClient.SetBearerToken(accessToken);

    var content = await httpClient.GetStringAsync(configuration["identityapibase"].AppendPathSegment("api/v1/admin/User"), cancellationToken);

    ViewBag.Json = JArray.Parse(content).ToString();
    return View("Json");
  }

  /// <summary>
  /// Calls the WCF endpoint asynchronous.
  /// </summary>
  /// <returns></returns>
  [HttpGet("CallWCFEndpoint")]
  public async Task<IActionResult> CallWcfEndpointAsync()
  {
    using var umsClient = new UMS2Client(UMS2Client.EndpointConfiguration.BasicHttpBinding_UMS2, configuration["EndPoints:UMS2Api"] ?? "http://localhost:5100/lsol/UMS2/UMS2.svc");
    var userResources = await umsClient.GetAllResourcesForLSUniqueIDAsync(new AllResourcesForLSUniqueIDRequest
    {
      LSUniqueID = int.Parse(HttpContext.User.FindFirst("ls_unique_id")?.Value ?? "0")
    });


    ViewBag.Json = userResources;
    return View("Json");
  }

  /// <summary>
  /// Calls the API using user access token asynchronous.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns></returns>
  [HttpGet("CallApiUsingUserAccessToken")]
  public async Task<IActionResult> CallApiUsingUserAccessTokenAsync(CancellationToken cancellationToken = default)
  {
    var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);
    httpClient.SetBearerToken(accessToken);

    var content = await httpClient.GetStringAsync(configuration["apiurl"].AppendPathSegment("identity"), cancellationToken);

    ViewBag.Json = JArray.Parse(content).ToString();
    return View("Json");
  }

  /// <summary>
  /// Calls the search available loads asynchronous.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns></returns>
  [HttpGet("CallSearchAvailableLoads")]
  public async Task<IActionResult> CallSearchAvailableLoadsAsync(CancellationToken cancellationToken = default)
  {
    const string jObject = @"{""MaximumRows"":299,""StartRowIndex"":1,""RatePerMile"":{""SortPriority"":0,""SortType"":1},""OriginStates"":{""Filters"":[""IL"",""IN"",""IA"",""KS"",""MI"",""MN"",""MO"",""NE"",""ND"",""OH"",""SD"",""WI""],""FilterOperators"":0},""IncludeWithOutDimensions"":{""FilterValue"":true,""FilterOperators"":0}}";

    var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);



    httpClient.SetBearerToken(accessToken);
    var uri = configuration["WebApi"].AppendPathSegment("/secured/lsol/AvailableLoads/RPC/SearchAvailableLoads");
    using var sc = new StringContent(jObject, System.Text.Encoding.UTF8, "text/json");
    var result = await httpClient.PostAsync(uri, sc, cancellationToken);

    ViewBag.Json = await result.Content.ReadAsStringAsync(cancellationToken);
    return View("Json");
  }

  /// <summary>
  /// Calls the API get states asynchronous.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns></returns>
  [HttpGet("CallApiGetStates")]
  public async Task<IActionResult> CallApiGetStatesAsync(CancellationToken cancellationToken = default)
  {


    var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);

    httpClient.SetBearerToken(accessToken);
    using var fc = new FlurlClient(httpClient);

    var request = fc.Request(configuration["WebApi"])
                    .AppendPathSegment("secured/lsol/api/Lookup/api/GetStates")
                    .SetQueryParam("CountryCode", "USA");

    var result = await request.GetAsync(cancellationToken: cancellationToken);

    ViewBag.Json = JObject.Parse(await result.GetStringAsync()).ToString();

    return View("Json");
  }
  /// <summary>
  /// Calls the API address search asynchronous.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns></returns>
  [HttpGet("CallApiAddressSearch")]
  public async Task<IActionResult> CallApiAddressSearchAsync(CancellationToken cancellationToken = default)
  {
    try
    {

      var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);

      httpClient.SetBearerToken(accessToken);
      var uri = configuration["WebApi"].AppendPathSegment("secured/GeoCode/address").AppendPathSegment("123 Main Street");
      var content = await httpClient.GetStringAsync(uri, cancellationToken);

      ViewBag.Json = JArray.Parse(content).ToString();
      return View("Json");

    }
    catch (System.Exception e)
    {
      Console.WriteLine(e.ToString());
      throw;
    }
  }
}