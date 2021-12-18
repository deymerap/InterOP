using FluentValidation;
using InterOP.Core.Herpers;
using InterOP.Core.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterOP.Infrastructure.Validators
{
    class UserChangeValidator : AbstractValidator<StrsUserChangePwd>
    {
        public UserChangeValidator()
        {
			RuleFor(x => x.ContrasenaNueva).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
						.MinimumLength(8).WithMessage(MsgResources.ReadResourceString("MinimumLength"))
							.Matches("[a-z]").WithMessage(MsgResources.ReadResourceString("PasswordLowercaseLetter"))
							.Matches("[A-Z]").WithMessage(MsgResources.ReadResourceString("PasswordUppercaseLetter"))
							.Matches("[^a-zA-Z0-9]").WithMessage(MsgResources.ReadResourceString("PasswordSpecialCharacter"))
							.Matches("[0-9]").WithMessage(MsgResources.ReadResourceString("PasswordNumber"));
		}
    }
}
