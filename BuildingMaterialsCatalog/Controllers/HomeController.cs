using Microsoft.AspNetCore.Mvc;

namespace BuildingMaterialsCatalog.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Building Materials Catalog API",
            endpoints = new[]
            {
                "GET /api/materials",
                "GET /api/materials/{id}",
                "POST /api/materials",
                "DELETE /api/materials/{id}"
            }
        });
    }
}