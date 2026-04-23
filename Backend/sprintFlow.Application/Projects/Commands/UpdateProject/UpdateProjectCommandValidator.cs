using FluentValidation;

namespace sprintFlow.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .Length(3, 200).WithMessage("Project name must be between 3 and 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(3, 1000).WithMessage("Description must be between 3 and 1000 characters");
    }
}
