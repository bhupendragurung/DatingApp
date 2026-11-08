using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

 namespace Api.Features.Members.UpdateProfile;
    public class UpdateProfileCommandValidator:AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x=> x.City).NotEmpty().MaximumLength(100);
            RuleFor(x=> x.Country).NotEmpty().MaximumLength(100);
            RuleFor(x=> x.Introduction).MaximumLength(2000);
            RuleFor(x=> x.LookingFor).MaximumLength(2000);
            RuleFor(x=> x.Interests).MaximumLength(500);
        }
    }
