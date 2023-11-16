
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers.RestApi;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ICapPublisher _capPublisher;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ICapPublisher capPublisher)
    {
        _logger = logger;
        _capPublisher = capPublisher;
    }

    [HttpGet("/send_site")]
    public IActionResult SendMessage()
    {
        Dictionary<string, object> dataDetail = new()
        {
            {"company_code", "F100 test publish"},
            {"company_description", "PT Foods Beverages Indonesia Update"},
            {"company_address", "Ged. Kawan Lama Jl. Puri Kencana No.1 RT 005/002 Kembangan Selatan Kembangan Jakarta Barat"},
            {"sales_organization_code", "F100"},
            {"sales_organization_description", "Food Beverage Indo"},
            {"sales_office_code", "F794"},
            {"sales_office_description", "ST CT SPBU KALIABANG"},
            {"site_code", "F794"},
            {"search_term1", "F7W"},
            {"search_term2", "CHATIME"},
            {"site_description", "ST CT SPBU KALIABANG Update"},
            {"site_address1", "JL. KALIABANG NO.67 RT001 RW006"},
            {"site_address2", "PERWIRA}, KEC.BEKASI UTARA}, KOTA BEKASI"},
            {"site_phone", ""},
            {"site_fax", ""},
            {"site_email", ""},
            {"tax_number", "029635331086000"},
            {"tax_number1", ""},
            {"tax_number2", ""},
            {"city_description", "BEKASI"},
            {"tax_code", "B"},
            {"pricing_zone", "Z1"},
            {"site_status", ""},
            {"block_from", ""},
            {"block_to", ""}
        };

        Dictionary<string, object> data = new()
        {
            {"SyncName", "site"},
            {"DocumentNumber", "F789"},
            {"SyncData", dataDetail}
        };

        _capPublisher.Publish("site", data);

        return Ok();
    }
}
