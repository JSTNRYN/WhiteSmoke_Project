namespace UnitTests;

[TestClass]
public sealed class ReservationTests
{
    [DataTestMethod]
    [DataRow("01-01-2012")]
    public void TestDateIsInPast(string date)
    {
        bool validation = ReservationLogic.IsValidateDate(date);

        Assert.IsFalse(validation);
    }

    [DataTestMethod]
    public void TestDateIsInFuture()
    {
        DateTime tomorrow = DateTime.Today.AddDays(1);
        string parsedDate = tomorrow.ToString();

        bool validation = ReservationLogic.IsValidateDate(parsedDate);

        Assert.IsTrue(validation);
    }

    // Test if the given date format is in the Dutch (nl-NL) style format
    [DataTestMethod]
    [DataRow("2000-01-20", "2000-20-01", "01-20-2000", "20-01-2000")]
    public void TestDateFormat(string date0, string date1, string date2, string date3)
    {
        bool date0Validation = ReservationLogic.IsValidDateFormat(date0);
        bool date1Validation = ReservationLogic.IsValidDateFormat(date1);
        bool date2Validation = ReservationLogic.IsValidDateFormat(date2);
        bool date3Validation = ReservationLogic.IsValidDateFormat(date3);

        Assert.IsFalse(date0Validation);
        Assert.IsFalse(date1Validation);
        Assert.IsFalse(date2Validation);
        Assert.IsTrue(date3Validation);
    }
}