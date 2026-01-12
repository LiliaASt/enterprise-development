using Bogus;
using CarRentalService.Application.Contracts.Grpc;

namespace CarRentalService.Generator.Grpc.Host.Generator;

/// <summary>
/// Static generator for creating rental data using Bogus library
/// </summary>
public static class RentalGenerator
{
    private static readonly Faker _faker = new("ru");

    private static readonly int[] _carIds =
        Enumerable.Range(1, 15).ToArray();

    private static readonly int[] _customerIds =
        Enumerable.Range(1, 15).ToArray();

    /// <summary>
    /// Generates a list of rental contracts
    /// </summary>
    /// <param name="count">Number of contracts to generate</param>
    /// <returns>List of generated rental contracts</returns>
    public static IList<RentalContractMessage> Generate(int count)
    {
        var list = new List<RentalContractMessage>(count);

        for (var i = 0; i < count; i++)
        {
            list.Add(new RentalContractMessage
            {
                Id = Guid.NewGuid().ToString(),
                CarId = _faker.PickRandom(_carIds),
                CustomerId = _faker.PickRandom(_customerIds),
                DurationHours = _faker.Random.Double(0.5, 720),
                StartTime = _faker.Date.Recent(30).ToString("o")
            });
        }

        return list;
    }
}
