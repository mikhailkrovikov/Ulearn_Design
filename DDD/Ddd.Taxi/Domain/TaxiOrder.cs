using Ddd.Taxi.Infrastructure;
using System.Globalization;
using System.Net.Http.Headers;

namespace Ddd.Taxi.Domain;

// In real aplication it whould be the place where database is used to find driver by its Id.
// But in this exercise it is just a mock to simulate database
public class DriversRepository
{
    public Driver FillDriverOrder(int id)
    {
        if (id == 15)
        {
            var driverName = new PersonName("Drive", "Driverson");
            var car = new Car("Lada sedan", "Baklazhan", "A123BT 66");
            return new Driver(15, driverName, car);
        }
        throw new InvalidDataException("Unknown driver id " + id);
    }
}

public class TaxiApi : ITaxiApi<TaxiOrder>
{
    private readonly Func<DateTime> currentTime;
    private int idCounter;

    public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
    {
        this.currentTime = currentTime;
    }

    public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
    {
        var order = new TaxiOrder(idCounter++);
        return order.CreateOrderWithoutDestination(firstName, lastName, street, building, currentTime);
    }

    public void UpdateDestination(TaxiOrder order, string street, string building)
    {
        var destination = new Address(street, building);
        order.UpdateDestination(destination);
    }

    public void AssignDriver(TaxiOrder order, int driverId)
    {
        order.AssignDriver(driverId, currentTime);
    }

    public void UnassignDriver(TaxiOrder order)
    {
        order.UnassignDriver();
    }

    public string GetDriverFullInfo(TaxiOrder order)
    {
        return order.GetDriverFullInfo();
    }

    public string GetShortOrderInfo(TaxiOrder order)
    {
        return order.GetShortOrderInfo();
    }

    public void Cancel(TaxiOrder order)
    {
        order.Cancel(currentTime);
    }

    public void StartRide(TaxiOrder order)
    {
        order.StartRide(currentTime);
    }

    public void FinishRide(TaxiOrder order)
    {
        order.FinishRide(currentTime);
    }
}

public class TaxiOrder : Entity<int>
{
    public TaxiOrder(int id) : base(id)
    {
    }

    public PersonName? ClientName { get; private set; }

    public Address? Start { get; private set; }

    public Address? Destination { get; private set; }

    public Driver? Driver { get; private set; }

    public TaxiOrderStatus Status { get; private set; }

    public DateTime CreationTime { get; private set; }

    public DateTime DriverAssignmentTime { get; private set; }

    public DateTime CancelTime { get; private set; }

    public DateTime StartRideTime { get; private set; }

    public DateTime FinishRideTime { get; private set; }


    public TaxiOrder CreateOrderWithoutDestination(
        string firstName,
        string lastName,
        string street,
        string building,
        Func<DateTime> currentTime)
    {
        return
            new TaxiOrder(Id)
            {
                ClientName = new PersonName(firstName, lastName),
                Start = new Address(street, building),
                CreationTime = currentTime(),
            };
    }

    public void UpdateDestination(Address destination)
    {
        Destination = new Address(destination.Street, destination.Building);
    }

    public void AssignDriver(int driverId, Func<DateTime> currentTime)
    {
        if (Driver != null)
        {
            throw new InvalidOperationException($"Unable to assign driver. Already assigned {Driver}");
        }
        var repository = new DriversRepository();
        Driver = repository.FillDriverOrder(driverId);
        DriverAssignmentTime = currentTime();
        Status = TaxiOrderStatus.WaitingCarArrival;
    }

    public void UnassignDriver()
    {
        if (Driver == null)
        {
            throw new InvalidOperationException($"{Status}: unable to unassign driver");
        }
        if (Status != TaxiOrderStatus.WaitingCarArrival)
        {
            throw new InvalidOperationException($"{Status}: unable to unassign driver while in progress");
        }
        Driver = null;
        Status = TaxiOrderStatus.WaitingForDriver;
    }

    public string GetDriverFullInfo()
    {
        if (Status == TaxiOrderStatus.WaitingForDriver) return null;
        return string.Join(" ",
            "Id: " + Driver?.Id,
            "DriverName: " + FormatName(Driver.DriverName),
            "Color: " + Driver.Car.CarColor,
            "CarModel: " + Driver.Car.CarModel,
            "PlateNumber: " + Driver.Car.CarPlateNumber);
    }

    public string GetShortOrderInfo()
    {
        return string.Join(" ",
            "OrderId: " + Id,
            "Status: " + Status,
            "Client: " + FormatName(ClientName),
            "Driver: " + FormatName(Driver?.DriverName),
            "From: " + FormatAddress(Start),
            "To: " + FormatAddress(Destination),
            "LastProgressTime: " + GetLastProgressTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
    }

    public void FinishRide(Func<DateTime> currentTime)
    {
        if (Driver == null)
        {
            throw new InvalidOperationException($"{Status}: unable to finish ride without driver");
        }
        if (Status != TaxiOrderStatus.InProgress)
        {
            throw new InvalidOperationException($"{Status}: unable to finish ride");
        }
        Status = TaxiOrderStatus.Finished;
        FinishRideTime = currentTime();
    }

    public void StartRide(Func<DateTime> currentTime)
    {
        if (Driver == null)
        {
            throw new InvalidOperationException($"{Status}: unable to start ride without driver");
        }
        Status = TaxiOrderStatus.InProgress;
        StartRideTime = currentTime();
    }

    public void Cancel(Func<DateTime> currentTime)
    {
        if (Status == TaxiOrderStatus.InProgress)
        {
            throw new InvalidOperationException($"{Status}: unable to cancel when riding");
        }
        Status = TaxiOrderStatus.Canceled;
        CancelTime = currentTime();
    }

    private DateTime GetLastProgressTime()
    {
        if (Status == TaxiOrderStatus.WaitingForDriver) return CreationTime;
        if (Status == TaxiOrderStatus.WaitingCarArrival) return DriverAssignmentTime;
        if (Status == TaxiOrderStatus.InProgress) return StartRideTime;
        if (Status == TaxiOrderStatus.Finished) return FinishRideTime;
        if (Status == TaxiOrderStatus.Canceled) return CancelTime;
        throw new NotSupportedException(Status.ToString());
    }

    private static string? FormatName(PersonName personName)
    {
        if (personName == null)
        {
            return "";
        }
        return string.Join(" ", new[] { personName.FirstName, personName.LastName }.Where(n => n != null));
    }

    private static string? FormatAddress(Address address)
    {
        if (address == null)
        {
            return "";
        }
        return string.Join(" ", new[] { address.Street, address.Building }.Where(n => n != null));
    }
}

public class Driver : Entity<int>
{
    public PersonName DriverName { get; }

    public Car Car { get; }

    public Driver(int id, PersonName driverName, Car car) : base(id)
    {
        DriverName = driverName;
        Car = car;
    }
}

public class Car : ValueType<Car>
{
    public string CarColor { get; }

    public string CarModel { get; }

    public string CarPlateNumber { get; }

    public Car(string carModel, string carColor, string carPlateNumber)
    {
        CarColor = carColor;
        CarModel = carModel;
        CarPlateNumber = carPlateNumber;
    }
}