using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace BuildingMaterialsCatalog.Tests
{
    public class MaterialsControllerTests
    {
        [Fact]
        public void GetAll_ReturnsAllMaterials()
        {
            // Arrange
            var storageMock = new Mock<InMemoryStorageService>();
            var materials = new List<BuildingMaterial>
            {
                new() { Id = Guid.NewGuid(), Name = "Test1" },
                new() { Id = Guid.NewGuid(), Name = "Test2" }
            };

            storageMock.Setup(s => s.GetAll()).Returns(materials);

            var validator = new BuildingMaterialValidator();
            var loggerMock = new Mock<ILogger<MaterialsController>>();

            var controller = new MaterialsController(
                storageMock.Object, validator, loggerMock.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMaterials = Assert.IsType<List<BuildingMaterial>>(okResult.Value);
            Assert.Equal(2, returnedMaterials.Count);
        }

        [Fact]
        public void GetById_WithValidId_ReturnsMaterial()
        {
            // Arrange
            var materialId = Guid.NewGuid();
            var material = new BuildingMaterial { Id = materialId, Name = "Test" };

            var storageMock = new Mock<InMemoryStorageService>();
            storageMock.Setup(s => s.GetById(materialId)).Returns(material);

            var validator = new BuildingMaterialValidator();
            var loggerMock = new Mock<ILogger<MaterialsController>>();

            var controller = new MaterialsController(
                storageMock.Object, validator, loggerMock.Object);

            // Act
            var result = controller.GetById(materialId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMaterial = Assert.IsType<BuildingMaterial>(okResult.Value);
            Assert.Equal(materialId, returnedMaterial.Id);
        }

        [Fact]
        public void GetById_WithInvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var storageMock = new Mock<InMemoryStorageService>();
            storageMock.Setup(s => s.GetById(invalidId)).Returns((BuildingMaterial?)null);

            var validator = new BuildingMaterialValidator();
            var loggerMock = new Mock<ILogger<MaterialsController>>();

            var controller = new MaterialsController(
                storageMock.Object, validator, loggerMock.Object);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => controller.GetById(invalidId));
        }

        [Theory]
        [InlineData("", "kg", 50, 100, "Name is required")]
        [InlineData("Test", "invalid", 50, 100, "Unit of measure must be one of")]
        [InlineData("Test", "kg", -10, 100, "Price per unit must be positive")]
        [InlineData("T", "kg", 50, 100, "Name must be at least 2 characters long")]
        public void Create_WithInvalidData_ThrowsValidationException(
            string name, string unit, decimal price, int quantity, string expectedError)
        {
            // Arrange
            var dto = new BuildingMaterialCreateDto
            {
                Name = name,
                UnitOfMeasure = unit,
                PricePerUnit = price,
                QuantityInStock = quantity
            };

            var storageMock = new Mock<InMemoryStorageService>();
            var validator = new BuildingMaterialValidator();
            var loggerMock = new Mock<ILogger<MaterialsController>>();

            var controller = new MaterialsController(
                storageMock.Object, validator, loggerMock.Object);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
                controller.Create(dto).GetAwaiter().GetResult());
            Assert.Contains(expectedError, exception.Message);
        }
    }
}