using TestTask.Rosa.Core.Enums;
using CSharpFunctionalExtensions;

namespace TestTask.Rosa.Core.Models
{
    public class User
    {
        public const int MAX_LENGTH_FIRSTNAME = 120;
        public const int MAX_LENGTH_LASTNAME = 120;
        private User(Guid id, string firstName, string lastName, UserRole role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public UserRole Role { get; private set; }

        public static Result<User> Create(Guid id, string firstName, string lastName, UserRole role)
        {
            if(id ==  Guid.Empty)
            {
                return Result.Failure<User>($"{nameof(id)} не может быть пустым");
            }
            if(string.IsNullOrWhiteSpace(firstName) || firstName.Length > MAX_LENGTH_FIRSTNAME)
            {
                return Result.Failure<User>($"{nameof(firstName)} не может быть пустым или длиннее {MAX_LENGTH_FIRSTNAME}");
            }
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > MAX_LENGTH_LASTNAME)
            {
                return Result.Failure<User>($"{nameof(lastName)} не может быть пустым или длиннее {MAX_LENGTH_LASTNAME}");
            }

            var user = new User(id, firstName, lastName, role);

            return Result.Success(user);
        }
    }
}
