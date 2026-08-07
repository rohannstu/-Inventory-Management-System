using FluentAssertions;
using FluentValidation.TestHelper;
using InventoryManagementSystem.Application.Auth.Commands.Register;
using Xunit;

namespace InventoryManagementSystem.Tests.Auth;

public class RegisterCommandValidatorTests
{
    [Fact]
    public void Should_Fail_When_Email_Is_Empty()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: string.Empty,
            Password: "Password123",
            FullName: "Test User",
            Role: "Staff"));

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Not_Valid()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "not-an-email",
            Password: "Password123",
            FullName: "Test User",
            Role: "Staff"));

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Empty()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: string.Empty,
            FullName: "Test User",
            Role: "Staff"));

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Less_Than_8_Characters()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: "Short1",
            FullName: "Test User",
            Role: "Staff"));

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_FullName_Is_Empty()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: "Password123",
            FullName: string.Empty,
            Role: "Staff"));

        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_FullName_Exceeds_200_Characters()
    {
        var validator = new RegisterCommandValidator();
        var longName = new string('A', 201);
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: "Password123",
            FullName: longName,
            Role: "Staff"));

        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_Role_Is_Empty()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: "Password123",
            FullName: "Test User",
            Role: string.Empty));

        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Should_Fail_When_Role_Is_Not_Valid()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: "Password123",
            FullName: "Test User",
            Role: "SuperAdmin"));

        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Should_Pass_For_Valid_Command()
    {
        var validator = new RegisterCommandValidator();
        var result = validator.TestValidate(new RegisterCommand(
            Email: "test@example.com",
            Password: "Password123",
            FullName: "Test User",
            Role: "Staff"));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_For_All_Valid_Roles()
    {
        var validator = new RegisterCommandValidator();

        foreach (var role in new[] { "Staff", "Manager", "Admin" })
        {
            var result = validator.TestValidate(new RegisterCommand(
                Email: "test@example.com",
                Password: "Password123",
                FullName: "Test User",
                Role: role));

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}