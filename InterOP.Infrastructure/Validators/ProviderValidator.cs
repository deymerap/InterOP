using FluentValidation;
using InterOP.Core.DTOs;
using InterOP.Core.Herpers;
using System;

namespace InterOP.Infrastructure.Validators
{
    public class ProviderValidator : AbstractValidator<ProveedoresDto>
    {
        const string cRegexUrl = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$";
        public ProviderValidator()
        {
            RuleFor(x => x.Nit).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                            .Length(9, 9).WithMessage(MsgResources.ReadResourceString("ExactLength"))
                            .Matches("[0-9]").WithMessage(MsgResources.ReadResourceString("OnlyNumber"));

            RuleFor(x => x.RazonSocial).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"));
            
            RuleFor(x => x.Email).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                                .EmailAddress().WithMessage(MsgResources.ReadResourceString("InvalidMail"));

            RuleFor(x => x.Pwd).NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                            .MinimumLength(8).WithMessage(MsgResources.ReadResourceString("MinimumLength"))
                            .Matches("[a-z]").WithMessage(MsgResources.ReadResourceString("PasswordLowercaseLetter"))
                            .Matches("[A-Z]").WithMessage(MsgResources.ReadResourceString("PasswordUppercaseLetter"))
                            .Matches("[^a-zA-Z0-9]").WithMessage(MsgResources.ReadResourceString("PasswordSpecialCharacter"))
                            .Matches("[0-9]").WithMessage(MsgResources.ReadResourceString("PasswordNumber"));


            //RuleFor(x => x.UserUrlApiProv)
            //           .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"));

            //RuleFor(x => x.PwdUrlApiProv)
            //           .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"));

            RuleFor(x => x.Url1ApiProv)
                       .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                       .Matches(cRegexUrl).WithMessage(MsgResources.ReadResourceString("InvalidUrl"));

            RuleFor(x => x.Url2ApiProv)
                       .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                       .Matches(cRegexUrl).WithMessage(MsgResources.ReadResourceString("InvalidUrl"));

            RuleFor(x => x.Url3ApiProv)
                       .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                       .Matches(cRegexUrl).WithMessage(MsgResources.ReadResourceString("InvalidUrl"));

            RuleFor(x => x.Url4ApiProv)
                       .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                       .Matches(cRegexUrl).WithMessage(MsgResources.ReadResourceString("InvalidUrl"));

            RuleFor(x => x.UrlSftpProv)
                       .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"))
                       .Matches(cRegexUrl).WithMessage(MsgResources.ReadResourceString("InvalidUrl"));

            //RuleFor(x => x.UsuSftpProv)
            //           .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"));

            //RuleFor(x => x.PwdSftpProv)
            //           .NotEmpty().WithMessage(MsgResources.ReadResourceString("EmptyField"));
        }

    }
}
