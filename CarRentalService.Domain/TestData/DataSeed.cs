using CarRentalService.Domain.Models;

namespace CarRentalService.Domain.TestData;
/// <summary>
/// Test data generator for car rental service
/// </summary>
public class TestData
{
    /// <summary>
    /// List of car models
    /// </summary>
    public List<CarModel> CarModels { get; }
    /// <summary>
    /// List of customers
    /// </summary>
    public List<Customer> Customers { get; }
    /// <summary>
    /// List of car model generations
    /// </summary>
    public List<CarModelGeneration> CarModelGenerations { get; }
    /// <summary>
    /// List of cars
    /// </summary>
    public List<Car> Cars { get; }
    /// <summary>
    /// List of rental transactions
    /// </summary>
    public List<Rent> Rents { get; }
    /// <summary>
    /// Constructor for data initialization
    /// </summary>
    public TestData()
    {
        CarModels =
        [
            new()
            {
                Id = 1,
                Name = "Toyota Camry",
                DriveType = "Front",
                SeatCount = 5,
                BodyType = "Sedan",
                CarClass = "Business"
            },
            new()
            {
                Id = 2,
                Name = "BMW X5",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "SUV",
                CarClass = "Premium"
            },
            new()
            {
                Id = 3,
                Name = "Volkswagen Golf",
                DriveType = "Front",
                SeatCount = 5,
                BodyType = "Hatchback",
                CarClass = "Economy"
            },
            new()
            {
                Id = 4,
                Name = "Mercedes-Benz E-Class",
                DriveType = "Rear",
                SeatCount = 5,
                BodyType = "Sedan",
                CarClass = "Business"
            },
            new()
            {
                Id = 5,
                Name = "Audi A6",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "Sedan",
                CarClass = "Premium"
            },
            new()
            {
                Id = 6,
                Name = "Hyundai Solaris",
                DriveType = "Front",
                SeatCount = 5,
                BodyType = "Sedan",
                CarClass = "Economy"
            },
            new()
            {
                Id = 7,
                Name = "Kia Sportage",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "SUV",
                CarClass = "Standard"
            },
            new()
            {
                Id = 8,
                Name = "Ford Focus",
                DriveType = "Front",
                SeatCount = 5,
                BodyType = "Hatchback",
                CarClass = "Standard"
            },
            new()
            {
                Id = 9,
                Name = "Skoda Octavia",
                DriveType = "Front",
                SeatCount = 5,
                BodyType = "Liftback",
                CarClass = "Standard"
            },
            new()
            {
                Id = 10,
                Name = "Lexus RX",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "SUV",
                CarClass = "Premium"
            },
            new()
            {
                Id = 11,
                Name = "Volvo XC90",
                DriveType = "All",
                SeatCount = 7,
                BodyType = "SUV",
                CarClass = "Premium"
            },
            new()
            {
                Id = 12,
                Name = "Renault Logan",
                DriveType = "Front",
                SeatCount = 5,
                BodyType = "Sedan",
                CarClass = "Economy"
            },
            new()
            {
                Id = 13,
                Name = "Tesla Model 3",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "Sedan",
                CarClass = "Premium"
            },
            new()
            {
                Id = 14,
                Name = "Honda CR-V",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "SUV",
                CarClass = "Standard"
            },
            new()
            {
                Id = 15,
                Name = "Mazda CX-5",
                DriveType = "All",
                SeatCount = 5,
                BodyType = "SUV",
                CarClass = "Standard"
            }
        ];

        CarModelGenerations =
        [
            new()
            {
                Id = 1,
                CarModelId = 1,
                CarModel = CarModels[0],
                ProductionYear = 2020,
                EngineVolume = 2.5,
                TransmissionType = "Automatic",
                RentalCostPerHour = 1500
            },
            new()
            {
                Id = 2,
                CarModelId = 1,
                CarModel = CarModels[0],
                ProductionYear = 2022,
                EngineVolume = 2.5,
                TransmissionType = "CVT",
                RentalCostPerHour = 1700
            },
            new()
            {
                Id = 3,
                CarModelId = 2,
                CarModel = CarModels[1],
                ProductionYear = 2021,
                EngineVolume = 3.0,
                TransmissionType = "Automatic",
                RentalCostPerHour = 2500
            },
            new()
            {
                Id = 4,
                CarModelId = 2,
                CarModel = CarModels[1],
                ProductionYear = 2023,
                EngineVolume = 3.0,
                TransmissionType = "Automatic",
                RentalCostPerHour = 2800
            },
            new()
            {
                Id = 5,
                CarModelId = 3,
                CarModel = CarModels[2],
                ProductionYear = 2020,
                EngineVolume = 1.4,
                TransmissionType = "Manual",
                RentalCostPerHour = 800
            },
            new()
            {
                Id = 6,
                CarModelId = 3,
                CarModel = CarModels[2],
                ProductionYear = 2022,
                EngineVolume = 1.4,
                TransmissionType = "Automatic",
                RentalCostPerHour = 900
            },
            new()
            {
                Id = 7,
                CarModelId = 4,
                CarModel = CarModels[3],
                ProductionYear = 2021,
                EngineVolume = 2.0,
                TransmissionType = "Automatic",
                RentalCostPerHour = 2200
            },
            new()
            {
                Id = 8,
                CarModelId = 5,
                CarModel = CarModels[4],
                ProductionYear = 2022,
                EngineVolume = 2.0,
                TransmissionType = "Automatic",
                RentalCostPerHour = 2400
            },
            new()
            {
                Id = 9,
                CarModelId = 6,
                CarModel = CarModels[5],
                ProductionYear = 2021,
                EngineVolume = 1.6,
                TransmissionType = "Automatic",
                RentalCostPerHour = 750
            },
            new()
            {
                Id = 10,
                CarModelId = 7,
                CarModel = CarModels[6],
                ProductionYear = 2022,
                EngineVolume = 2.0,
                TransmissionType = "Automatic",
                RentalCostPerHour = 1200
            },
            new()
            {
                Id = 11,
                CarModelId = 8,
                CarModel = CarModels[7],
                ProductionYear = 2020,
                EngineVolume = 1.6,
                TransmissionType = "Manual",
                RentalCostPerHour = 850
            },
            new()
            {
                Id = 12,
                CarModelId = 9,
                CarModel = CarModels[8],
                ProductionYear = 2021,
                EngineVolume = 1.8,
                TransmissionType = "Automatic",
                RentalCostPerHour = 950
            },
            new()
            {
                Id = 13,
                CarModelId = 10,
                CarModel = CarModels[9],
                ProductionYear = 2023,
                EngineVolume = 3.5,
                TransmissionType = "Automatic",
                RentalCostPerHour = 3000
            },
            new()
            {
                Id = 14,
                CarModelId = 11,
                CarModel = CarModels[10],
                ProductionYear = 2022,
                EngineVolume = 2.0,
                TransmissionType = "Automatic",
                RentalCostPerHour = 2700
            },
            new()
            {
                Id = 15,
                CarModelId = 12,
                CarModel = CarModels[11],
                ProductionYear = 2020,
                EngineVolume = 1.6,
                TransmissionType = "Manual",
                RentalCostPerHour = 700
            }
        ];

        Cars =
        [
            new()
            {
                Id = 1,
                LicensePlate = "A123BC777",
                Color = "Black",
                CarModelGenerationId = 1,
                CarModelGeneration = CarModelGenerations[0]
            },
            new()
            {
                Id = 2,
                LicensePlate = "B234CD777",
                Color = "White",
                CarModelGenerationId = 1,
                CarModelGeneration = CarModelGenerations[0]
            },
            new()
            {
                Id = 3,
                LicensePlate = "C345DE777",
                Color = "Silver",
                CarModelGenerationId = 3,
                CarModelGeneration = CarModelGenerations[2]
            },
            new()
            {
                Id = 4,
                LicensePlate = "D456EF777",
                Color = "Blue",
                CarModelGenerationId = 3,
                CarModelGeneration = CarModelGenerations[2]
            },
            new()
            {
                Id = 5,
                LicensePlate = "E567FG777",
                Color = "Red",
                CarModelGenerationId = 5,
                CarModelGeneration = CarModelGenerations[4]
            },
            new()
            {
                Id = 6,
                LicensePlate = "F678GH777",
                Color = "Gray",
                CarModelGenerationId = 5,
                CarModelGeneration = CarModelGenerations[4]
            },
            new()
            {
                Id = 7,
                LicensePlate = "G789HI777",
                Color = "Black",
                CarModelGenerationId = 7,
                CarModelGeneration = CarModelGenerations[6]
            },
            new()
            {
                Id = 8,
                LicensePlate = "H890IJ777",
                Color = "White",
                CarModelGenerationId = 7,
                CarModelGeneration = CarModelGenerations[6]
            },
            new()
            {
                Id = 9,
                LicensePlate = "I901JK777",
                Color = "Blue",
                CarModelGenerationId = 9,
                CarModelGeneration = CarModelGenerations[8]
            },
            new()
            {
                Id = 10,
                LicensePlate = "J012KL777",
                Color = "Silver",
                CarModelGenerationId = 9,
                CarModelGeneration = CarModelGenerations[8]
            },
            new()
            {
                Id = 11,
                LicensePlate = "K123LM777",
                Color = "Red",
                CarModelGenerationId = 11,
                CarModelGeneration = CarModelGenerations[10]
            },
            new()
            {
                Id = 12,
                LicensePlate = "L234MN777",
                Color = "Black",
                CarModelGenerationId = 11,
                CarModelGeneration = CarModelGenerations[10]
            },
            new()
            {
                Id = 13,
                LicensePlate = "M345NO777",
                Color = "White",
                CarModelGenerationId = 13,
                CarModelGeneration = CarModelGenerations[12]
            },
            new()
            {
                Id = 14,
                LicensePlate = "N456OP777",
                Color = "Gray",
                CarModelGenerationId = 13,
                CarModelGeneration = CarModelGenerations[12]
            },
            new()
            {
                Id = 15,
                LicensePlate = "O567PQ777",
                Color = "Blue",
                CarModelGenerationId = 15,
                CarModelGeneration = CarModelGenerations[14]
            }
        ];

        Customers =
        [
            new()
            {
                Id = 1,
                DriverLicenseNumber = "77AA123456",
                FullName = "Ivanov Ivan Ivanovich",
                DateOfBirth = new DateTime(1990, 5, 15)
            },
            new()
            {
                Id = 2,
                DriverLicenseNumber = "77BB234567",
                FullName = "Petrov Petr Petrovich",
                DateOfBirth = new DateTime(1985, 8, 22)
            },
            new()
            {
                Id = 3,
                DriverLicenseNumber = "77CC345678",
                FullName = "Sidorova Anna Sergeevna",
                DateOfBirth = new DateTime(1992, 3, 10)
            },
            new()
            {
                Id = 4,
                DriverLicenseNumber = "77DD456789",
                FullName = "Kuznetsov Alexey Vladimirovich",
                DateOfBirth = new DateTime(1988, 11, 5)
            },
            new()
            {
                Id = 5,
                DriverLicenseNumber = "77EE567890",
                FullName = "Smirnova Ekaterina Dmitrievna",
                DateOfBirth = new DateTime(1995, 7, 18)
            },
            new()
            {
                Id = 6,
                DriverLicenseNumber = "77FF678901",
                FullName = "Vasiliev Dmitry Andreevich",
                DateOfBirth = new DateTime(1991, 2, 28)
            },
            new()
            {
                Id = 7,
                DriverLicenseNumber = "77GG789012",
                FullName = "Nikolaeva Olga Igorevna",
                DateOfBirth = new DateTime(1987, 9, 12)
            },
            new()
            {
                Id = 8,
                DriverLicenseNumber = "77HH890123",
                FullName = "Morozov Sergey Viktorovich",
                DateOfBirth = new DateTime(1993, 4, 25)
            },
            new()
            {
                Id = 9,
                DriverLicenseNumber = "77II901234",
                FullName = "Pavlova Maria Alexandrovna",
                DateOfBirth = new DateTime(1994, 6, 30)
            },
            new()
            {
                Id = 10,
                DriverLicenseNumber = "77JJ012345",
                FullName = "Fedorov Andrey Nikolaevich",
                DateOfBirth = new DateTime(1989, 12, 8)
            },
            new()
            {
                Id = 11,
                DriverLicenseNumber = "77KK123456",
                FullName = "Lebedeva Tatyana Petrovna",
                DateOfBirth = new DateTime(1996, 1, 14)
            },
            new()
            {
                Id = 12,
                DriverLicenseNumber = "77LL234567",
                FullName = "Sokolov Igor Borisovich",
                DateOfBirth = new DateTime(1990, 10, 20)
            },
            new()
            {
                Id = 13,
                DriverLicenseNumber = "77MM345678",
                FullName = "Romanova Elena Andreevna",
                DateOfBirth = new DateTime(1986, 7, 3)
            },
            new()
            {
                Id = 14,
                DriverLicenseNumber = "77NN456789",
                FullName = "Kazakov Mikhail Sergeevich",
                DateOfBirth = new DateTime(1997, 11, 15)
            },
            new()
            {
                Id = 15,
                DriverLicenseNumber = "77OO567890",
                FullName = "Tikhonova Anastasia Pavlovna",
                DateOfBirth = new DateTime(1998, 3, 22)
            }
        ];

        Rents =
        [
            new()
            {
                Id = 1,
                CarId = 1,
                Car = Cars[0],
                CustomerId = 1,
                Customer = Customers[0],
                StartTime = new DateTime(2024, 3, 1, 10, 0, 0),
                Duration = 48
            },
            new()
            {
                Id = 2,
                CarId = 1,
                Car = Cars[0],
                CustomerId = 3,
                Customer = Customers[2],
                StartTime = new DateTime(2024, 2, 25, 14, 30, 0),
                Duration = 72
            },
            new()
            {
                Id = 3,
                CarId = 1,
                Car = Cars[0],
                CustomerId = 5,
                Customer = Customers[4],
                StartTime = new DateTime(2024, 2, 20, 9, 15, 0),
                Duration = 24
            },
            new()
            {
                Id = 4,
                CarId = 2,
                Car = Cars[1],
                CustomerId = 2,
                Customer = Customers[1],
                StartTime = new DateTime(2024, 2, 27, 11, 45, 0),
                Duration = 96
            },
            new()
            {
                Id = 5,
                CarId = 2,
                Car = Cars[1],
                CustomerId = 4,
                Customer = Customers[3],
                StartTime = new DateTime(2024, 2, 25, 16, 0, 0),
                Duration = 120
            },
            new()
            {
                Id = 6,
                CarId = 3,
                Car = Cars[2],
                CustomerId = 6,
                Customer = Customers[5],
                StartTime = new DateTime(2024, 2, 23, 13, 20, 0),
                Duration = 72
            },
            new()
            {
                Id = 7,
                CarId = 3,
                Car = Cars[2],
                CustomerId = 8,
                Customer = Customers[7],
                StartTime = new DateTime(2024, 2, 18, 10, 10, 0),
                Duration = 48
            },
            new()
            {
                Id = 8,
                CarId = 4,
                Car = Cars[3],
                CustomerId = 7,
                Customer = Customers[6],
                StartTime = new DateTime(2024, 2, 28, 8, 30, 0),
                Duration = 36
            },
            new()
            {
                Id = 9,
                CarId = 5,
                Car = Cars[4],
                CustomerId = 9,
                Customer = Customers[8],
                StartTime = new DateTime(2024, 2, 15, 12, 0, 0),
                Duration = 96
            },
            new()
            {
                Id = 10,
                CarId = 6,
                Car = Cars[5],
                CustomerId = 10,
                Customer = Customers[9],
                StartTime = new DateTime(2024, 2, 28, 7, 0, 0),
                Duration = 168
            },
            new()
            {
                Id = 11,
                CarId = 7,
                Car = Cars[6],
                CustomerId = 11,
                Customer = Customers[10],
                StartTime = new DateTime(2024, 2, 22, 15, 45, 0),
                Duration = 72
            },
            new()
            {
                Id = 12,
                CarId = 8,
                Car = Cars[7],
                CustomerId = 12,
                Customer = Customers[11],
                StartTime = new DateTime(2024, 2, 26, 9, 20, 0),
                Duration = 48
            },
            new()
            {
                Id = 13,
                CarId = 9,
                Car = Cars[8],
                CustomerId = 13,
                Customer = Customers[12],
                StartTime = new DateTime(2024, 2, 29, 22, 0, 0),
                Duration = 60
            },
            new()
            {
                Id = 14,
                CarId = 10,
                Car = Cars[9],
                CustomerId = 14,
                Customer = Customers[13],
                StartTime = new DateTime(2024, 2, 24, 11, 30, 0),
                Duration = 96
            },
            new()
            {
                Id = 15,
                CarId = 11,
                Car = Cars[10],
                CustomerId = 15,
                Customer = Customers[14],
                StartTime = new DateTime(2024, 2, 10, 14, 15, 0),
                Duration = 120
            },
            new()
            {
                Id = 16,
                CarId = 12,
                Car = Cars[11],
                CustomerId = 1,
                Customer = Customers[0],
                StartTime = new DateTime(2024, 2, 29, 14, 0, 0),
                Duration = 48
            },
            new()
            {
                Id = 17,
                CarId = 13,
                Car = Cars[12],
                CustomerId = 2,
                Customer = Customers[1],
                StartTime = new DateTime(2024, 2, 5, 16, 45, 0),
                Duration = 72
            },
            new()
            {
                Id = 18,
                CarId = 14,
                Car = Cars[13],
                CustomerId = 3,
                Customer = Customers[2],
                StartTime = new DateTime(2024, 2, 12, 10, 10, 0),
                Duration = 36
            },
            new()
            {
                Id = 19,
                CarId = 15,
                Car = Cars[14],
                CustomerId = 4,
                Customer = Customers[3],
                StartTime = new DateTime(2024, 2, 16, 13, 30, 0),
                Duration = 84
            },
            new()
            {
                Id = 20,
                CarId = 2,
                Car = Cars[1],
                CustomerId = 6,
                Customer = Customers[5],
                StartTime = new DateTime(2024, 3, 2, 9, 0, 0),
                Duration = 24
            }
        ];
    }
}
