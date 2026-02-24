using Microsoft.AspNetCore.Mvc;
using BuildingMaterialsCatalog.Models;
using BuildingMaterialsCatalog.Services;
using BuildingMaterialsCatalog.Validators;

namespace BuildingMaterialsCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly IStorageService _storage;
    private readonly BuildingMaterialValidator _validator;
    private readonly ILogger<MaterialsController> _logger;

    public MaterialsController(IStorageService storage, BuildingMaterialValidator validator, ILogger<MaterialsController> logger)
    {
        _storage = storage;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var materials = _storage.GetAll();
        return Ok(materials);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var material = _storage.GetById(id);
        if (material == null)
            throw new KeyNotFoundException($"Material with id '{id}' not found.");
        return Ok(material);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BuildingMaterialCreateDto dto)
    {
        _validator.ValidateCreateDto(dto);
        var material = await _storage.CreateAsync(dto);
        _logger.LogInformation("Created material {MaterialId}", material.Id);
        return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _storage.Delete(id);
        if (!deleted)
            throw new KeyNotFoundException($"Material with id '{id}' not found.");
    
        _logger.LogInformation("Deleted material {MaterialId}", id);
        return NoContent(); 
}
}