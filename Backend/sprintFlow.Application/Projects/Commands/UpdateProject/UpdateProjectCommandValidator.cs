using FluentValidation;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .Length(3, 100).WithMessage("Project name must be between 3 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(3, 300).WithMessage("Description must be between 3 and 300 characters");
    }
}
