namespace UnitTests;

[TestClass]
public sealed class CustomerManageTest
{
    [DataTestMethod]
    [DataRow("John@gmail.com")]
    public void TestCustomerManagement(string email)
    {
        // Arrange
        UserLogic userLogic = new UserLogic();
        UserAccess userAccess = new UserAccess();

        // Act
        bool emailIsValid = UserLogic.ValidateEmail(email);
        // Assert
        Assert.IsTrue(emailIsValid, "Email address should be valid.");

    }
}
