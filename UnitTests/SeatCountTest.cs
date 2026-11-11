namespace UnitTests;

[TestClass]
public sealed class SeatCountTests
{
    [DataTestMethod]
    [DataRow(-1)]
    public void TestSeatCountLow(int amount)
    {
        bool result = SeatCount.WithinRange(amount);

        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow(7)]
    public void TestSeatCountHigh(int amount)
    {
        bool result = SeatCount.WithinRange(amount);
        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow(1)]
    public void TestSeatCountLowEdgeCase(int amount)
    {
        bool result = SeatCount.WithinRange(amount);
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow(6)]
    public void TestSeatCountHighEdgeCase(int amount)
    {
        bool result = SeatCount.WithinRange(amount);
        Assert.IsTrue(result);
    }
}