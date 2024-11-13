using Microsoft.AspNetCore.Mvc;
using TSchedule.Persistence.Extensions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Api.Controllers;

[ApiController]
public class LicenseController(ILicensesService licenseService) : ControllerBase
{
    public record LicenseResult(string Message, bool IsValid);

    [HttpGet("[controller]/verify/{licenseKey}")]
    public async Task<ActionResult<LicenseResult>> VerifyLicense(string licenseKey)
    {
        var license = await licenseService.GetLicenseByKey(licenseKey);

        if (license is null)
            return NotFound(new LicenseResult("Лицензия не найдена.", false));

        if (!LicenseHelper.IsLicenseValid(license))
            return BadRequest(new LicenseResult("Лицензия просрочена.", false));

        return Ok(new LicenseResult("Лицензия верна.", true));
    }
}
