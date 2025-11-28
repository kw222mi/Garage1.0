using System;
using System.Linq;
using Garage1._0.Garage;
using Garage1._0.Vehicles;
using Xunit;

public class GarageTests
{
    // Helper method to avoid duplication when creating cars in tests.
    // Makes the tests easier to read and maintain.
    private Car CreateCar(string regNr = "ABC123")
    {
        return new Car(regNr, "Röd", 4, "Volvo V70", "bensin");
    }

    // --- Constructor tests ---

    [Fact]
    public void NewGarage_CreatesNewGarage_StartsEmpty()
    {
        // A newly created garage should start empty with the given capacity.
        var garage = new Garage<Vehicle>(10);

        Assert.Equal(10, garage.Capacity);
        Assert.Equal(0, garage.Count);
    }


    // --- TryAdd tests ---

    [Fact]
    public void TryAdd_AddsVehicle_WhenSpaceAvailable()
    {
        // Adding a vehicle to a garage with free capacity should succeed
        // and increase the Count.
        var garage = new Garage<Vehicle>(2);
        var car = CreateCar("ABC123");

        var result = garage.TryAdd(car);

        Assert.True(result);
        Assert.Equal(1, garage.Count);
        Assert.Contains(car, garage);
    }

    [Fact]
    public void TryAdd_AddsTwoVehicles_WhenSpaceAvailable()
    {
        // Multiple additions should succeed until capacity is reached.
        var garage = new Garage<Vehicle>(2);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF123");

        var result1 = garage.TryAdd(car1);
        var result2 = garage.TryAdd(car2);

        Assert.True(result1);
        Assert.True(result2);
        Assert.Equal(2, garage.Count);
        Assert.Contains(car1, garage);
        Assert.Contains(car2, garage);
    }

    [Fact]
    public void TryAdd_ReturnsFalse_WhenGarageIsFull()
    {
        // Adding a vehicle when the garage is full should fail
        // and should not affect Count.
        var garage = new Garage<Vehicle>(capacity: 1);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF123");

        garage.TryAdd(car1);

        var result = garage.TryAdd(car2);

        Assert.False(result);
        Assert.Equal(1, garage.Count);
        Assert.DoesNotContain(car2, garage);
    }

    [Fact]
    public void TryAdd_ThrowsArgumentNullException_WhenItemIsNull()
    {
        // Passing null should throw, since the API requires a valid Vehicle instance.
        var garage = new Garage<Vehicle>(capacity: 1);

        Assert.Throws<ArgumentNullException>(() => garage.TryAdd(null!));
    }


    // --- TryRemove tests ---

    [Fact]
    public void TryRemove_RemovesVehicle_WhenRegNrExists()
    {
        // Removing an existing vehicle should succeed and remove it from enumeration.
        var garage = new Garage<Vehicle>(capacity: 2);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF456");

        garage.TryAdd(car1);
        garage.TryAdd(car2);

        var result = garage.TryRemove("DEF456");

        Assert.True(result);
        Assert.Equal(1, garage.Count);
        Assert.DoesNotContain(car2, garage);
        Assert.Contains(car1, garage);
    }

    [Fact]
    public void TryRemove_DoesNotRemoveVehicle_WhenRegNrDoesNotExist()
    {
        // Removing a non-existing regnr should not change the state.
        var garage = new Garage<Vehicle>(capacity: 2);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF123");

        garage.TryAdd(car1);
        garage.TryAdd(car2);

        var result = garage.TryRemove("DEF789");

        Assert.False(result);
        Assert.Equal(2, garage.Count);
        Assert.Contains(car1, garage);
        Assert.Contains(car2, garage);
    }

    [Fact]
    public void TryRemove_RemoveVehicle_LeavesFreeParking()
    {
        // After removal, the freed slot should be reused by the next added vehicle.
        var garage = new Garage<Vehicle>(capacity: 2);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF123");
        var car3 = CreateCar("GHI789");

        garage.TryAdd(car1);
        garage.TryAdd(car2);

        var result = garage.TryRemove("ABC123");
        garage.TryAdd(car3);

        Assert.True(result);
        Assert.Equal(2, garage.Count);
        Assert.DoesNotContain(car1, garage);
        Assert.Contains(car2, garage);
        Assert.Contains(car3, garage);
    }


    // --- Enumerator tests ---

    [Fact]
    public void Enumerator_SkipsNullSlots()
    {
        // The enumerator should only expose real vehicles, not empty array slots.
        var garage = new Garage<Vehicle>(capacity: 3);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF123");

        garage.TryAdd(car1);
        garage.TryAdd(car2);

        var vehicles = garage.ToList();

        Assert.Equal(2, vehicles.Count);
        Assert.Contains(car1, vehicles);
        Assert.Contains(car2, vehicles);
    }

    [Fact]
    public void Enumerator_IteratesOnlyOverExistingVehicles_AfterRemoval()
    {
        // Ensures that removed vehicles do not appear in iteration.
        // Demonstrates correct behavior after modifying collection state.
        var garage = new Garage<Vehicle>(capacity: 3);
        var car1 = CreateCar("ABC123");
        var car2 = CreateCar("DEF456");
        var car3 = CreateCar("GHI789");

        garage.TryAdd(car1);
        garage.TryAdd(car2);
        garage.TryAdd(car3);

        garage.TryRemove("DEF456"); // remove middle vehicle

        var vehicles = garage.ToList();

        Assert.Equal(2, vehicles.Count);
        Assert.Contains(car1, vehicles);
        Assert.Contains(car3, vehicles);
        Assert.DoesNotContain(car2, vehicles);
    }
}
