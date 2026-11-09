namespace UnitTests;

[TestClass]
public sealed class CustomerManageTest
{
    [DataTestMethod]
    [DataRow("John@gmail.com")]
    public void TestCustomerEmailValidation(string email)
    {
        // Arrange
        UserLogic userLogic = new UserLogic();
        UserAccess userAccess = new UserAccess();

        // Act
        bool emailIsValid = UserLogic.ValidateEmail(email);
        // Assert
        Assert.IsTrue(emailIsValid, "Email address should be valid.");
    }

    [DataTestMethod]
    [DataRow("johndoegmail.com")]
    public void TestCustomerEmailValidation_InvalidEmail(string invalidEmail)
    {
        // Arrange

        // Act
        bool emailIsValid = UserLogic.ValidateEmail(invalidEmail);

        // Assert
        Assert.IsFalse(emailIsValid, "Email address should be invalid.");
    }

    [DataTestMethod]
    [DataRow("")]
    public void TestCustomerNameValidation_EmptyName(string emptyName)
    {
        // Arrange

        // Act
        bool nameIsValid = UserLogic.ValidateName(emptyName);

        // Assert
        Assert.IsFalse(nameIsValid, "Name should be invalid.");
    }
}

