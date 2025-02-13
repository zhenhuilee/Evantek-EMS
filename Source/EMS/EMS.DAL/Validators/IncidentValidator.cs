using EMS.DataAccess.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Validators
{
    public class IncidentValidator : AbstractValidator<Incident>
    {
        public IncidentValidator() 
        {
            RuleFor(incident => incident.CustomerPhone)
            .NotEmpty()
            .WithMessage("The name is required. {PropertyName}")
            .MaximumLength(50);

        }
    }
}
