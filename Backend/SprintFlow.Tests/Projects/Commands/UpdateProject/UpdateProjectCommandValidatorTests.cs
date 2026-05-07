using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using FluentValidation.TestHelper;

namespace SprintFlow.Tests.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationError()
    {
        //arrange
        var command = new UpdateProjectCommand()
        {
            Name = "Test Name",
            Description = "Test Description",
        };
        var validator = new UpdateProjectCommandValidator();
        //act
        var result = validator.TestValidate(command);
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact()]
    public void Validator_ForValidCommand_ShouldHaveValidationError()
    {
        //arrange
        var command = new UpdateProjectCommand()
        {
            Name = "Te",
            Description = "Te",
        };
        var validator = new UpdateProjectCommandValidator();
        //act
        var result = validator.TestValidate(command);
        //assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Description);
    }
}
