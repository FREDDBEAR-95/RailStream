using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RailStream_Server.Models;
using RailStream_Server;
using RailStream_Server_Backend.Managers;

namespace RailStream_ModularTests
{
    [TestClass]
    public class UnitTest1
    {
        private DatabaseManager GetInMemoryDatabaseManager()
        {
            var options = new DbContextOptionsBuilder<DatabaseManager>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new DatabaseManager(options);
        }

        [TestMethod]
        public void LoadWagonCombo_ShouldLoadTrainAndWagonTypes()
        {
            // Arrange
            var dbManager = GetInMemoryDatabaseManager();

            dbManager.Trains.AddRange(new List<Train>
            {
                new Train { TrainId = 1 },
                new Train { TrainId = 2 }
            });

            dbManager.WagonType.AddRange(new List<WagonType>
            {
                new WagonType { WagonTypeId = 1 },
                new WagonType { WagonTypeId = 2 }
            });

            dbManager.SaveChanges();

            var window = new WagonsEditAndAddWindow(); // Передаем null

            // Act
            window.LoadWagonCombo();

            // Assert
            Assert.AreEqual(2, dbManager.Trains.Count()); // Проверяем количество поездов в базе данных
            Assert.AreEqual(2, dbManager.WagonType.Count()); // Проверяем количество типов вагонов в базе данных
        }

    }
}