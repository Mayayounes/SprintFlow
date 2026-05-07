using FluentValidation.TestHelper;
using sprintFlow.Application.Projects.Commands.CreateProject;
using System.Xml.Linq;

namespace SprintFlow.Tests.Projects.Commands.CreateProject;

public class CreateProjectCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationError()
    {
        //arrange
        var command = new CreateProjectCommand()
        {
            Name = "Test Name",
            Description = "Test Description",
        };
        var validator = new CreateProjectCommandValidator();
        //act
        var result = validator.TestValidate(command);
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact()]
    public void Validator_ForValidCommand_ShouldHaveValidationError()
    {
        //arrange
        var command = new CreateProjectCommand()
        {
            Name = "Te",
            Description = "Te",
        };
        var validator = new CreateProjectCommandValidator();
        //act
        var result = validator.TestValidate(command);
        //assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Description);
    }
}
