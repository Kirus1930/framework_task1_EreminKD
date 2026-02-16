namespace BuildingMaterialsCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly InMemoryStorageService _storageService;
        private readonly BuildingMaterialValidator _validator;
        private readonly ILogger<MaterialsController> _logger;

        public MaterialsController(
            InMemoryStorageService storageService,
            BuildingMaterialValidator validator,
            ILogger<MaterialsController> logger)
        {
            _storageService = storageService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var materials = _storageService.GetAll();
            return Ok(materials);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var material = _storageService.GetById(id);
            if (material == null)
            {
                throw new KeyNotFoundException($"Material with id {id} not found");
            }
            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BuildingMaterialCreateDto dto)
        {
            _validator.ValidateCreateDto(dto);

            var material = await _storageService.CreateAsync(dto);

            _logger.LogInformation("Created material with ID: {MaterialId}", material.Id);

            return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
        }
    }
}