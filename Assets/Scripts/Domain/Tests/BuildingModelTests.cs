using NUnit.Framework;
using Domain.Models;
using Domain.Enums;

namespace Domain.Tests
{
    [TestFixture]
    public class BuildingModelTests
    {
        [Test]
        public void Building_Upgrade_IncreasesLevel()
        {
            // Arrange
            var building = new BuildingModel(1, BuildingType.House, new GridPosition(0, 0));

            // Act
            building.Upgrade();

            // Assert
            Assert.AreEqual(2, building.Level);
        }

        [Test]
        public void Building_GetIncome_ReturnsCorrectValue()
        {
            // Arrange
            var building = new BuildingModel(1, BuildingType.Farm, new GridPosition(0, 0));

            // Act & Assert
            Assert.AreEqual(10, building.GetIncome()); // Level 1 Farm = 10
            building.Upgrade();
            Assert.AreEqual(20, building.GetIncome()); // Level 2 Farm = 20
        }
    }
}
