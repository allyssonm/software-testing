using System;
using Features.Core;
using FluentValidation;

namespace Features.Clients
{
    public class Client : Entity
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public DateTime BirthDate { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Email { get; private set; }
        public bool Active { get; private set; }

        protected Client()
        {
        }

        public Client(Guid id, string name, string surname, DateTime birthDate, string email, bool active,
            DateTime registerDate)
        {
            Id = id;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Email = email;
            Active = active;
            RegisterDate = registerDate;
        }

        public string FullName()
        {
            return $"{Name} {Surname}";
        }

        public bool IsSpecial()
        {
            return RegisterDate < DateTime.Now.AddYears(-3) && Active;
        }

        public void Inactivate()
        {
            Active = false;
        }

        public override bool IsValid()
        {
            ValidationResult = new ClientValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ClientValidation : AbstractValidator<Client>
    {
        public ClientValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please, check if you informed a name")
                .Length(2, 150).WithMessage("The name must have between 2 and 150 characters");

            RuleFor(c => c.Surname)
                .NotEmpty().WithMessage("Please, check if you informed a surname")
                .Length(2, 150).WithMessage("The surname must have between 2 and 150 characters");

            RuleFor(c => c.BirthDate)
                .NotEmpty()
                .Must(HaveMinimumAge)
                .WithMessage("Client must be 18 yo or older");

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        public static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }
    }
}