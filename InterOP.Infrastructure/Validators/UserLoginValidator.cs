using FluentValidation;
using InterOP.Core.Entities;
using InterOP.Core.Herpers;
using System.Reflection;
using System.Resources;

namespace InterOP.Infrastructure.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
		//rivate static ResourceManager prvShrRsRecursos = new ResourceManager("InterOP.Core.Resources.ErrorMessages", Assembly.GetExecutingAssembly());
		

		public UserLoginValidator()
		{
			RuleFor(x => x.u).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
							.Length(1, 9).WithMessage(MsgResources.ReadResourceString("LengthField"));
							//.Length(1, 9).WithMessage(MsgResources.ReadResourceString("LengthField"));

			RuleFor(x => x.p).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
							.MinimumLength(8).WithMessage(MsgResources.ReadResourceString("MinimumLength"))
							.Matches("[a-z]").WithMessage(MsgResources.ReadResourceString("PasswordLowercaseLetter"))
							.Matches("[A-Z]").WithMessage(MsgResources.ReadResourceString("PasswordUppercaseLetter"))
							.Matches("[^a-zA-Z0-9]").WithMessage(MsgResources.ReadResourceString("PasswordSpecialCharacter"))
							.Matches("[0-9]").WithMessage(MsgResources.ReadResourceString("PasswordNumber"));

		}
	}
}
