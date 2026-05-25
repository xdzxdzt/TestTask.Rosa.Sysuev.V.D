using CSharpFunctionalExtensions;
using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.Core.Models
{
    /// <summary>
    /// Представляет пользователя системы.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Максимальная допустимая длина имени пользователя.
        /// </summary>
        public const int MAX_LENGTH_FIRSTNAME = 120;

        /// <summary>
        /// Максимальная допустимая длина фамилии пользователя.
        /// </summary>
        public const int MAX_LENGTH_LASTNAME = 120;

        private User(Guid id, string firstName, string lastName, UserRole role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Роль пользователя в системе.
        /// </summary>
        public UserRole Role { get; private set; }

        /// <summary>
        /// Создает пользователя после проверки бизнес-правил.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="firstName">Имя пользователя.</param>
        /// <param name="lastName">Фамилия пользователя.</param>
        /// <param name="role">Роль пользователя.</param>
        /// <returns>Результат с созданным пользователем или текстом ошибки.</returns>
        public static Result<User> Create(Guid id, string firstName, string lastName, UserRole role)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<User>($"{nameof(id)} не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > MAX_LENGTH_FIRSTNAME)
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
