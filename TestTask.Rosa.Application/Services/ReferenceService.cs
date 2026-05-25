using CSharpFunctionalExtensions;
using TestTask.Rosa.Application.Interfaces.Services;
using TestTask.Rosa.Application.Models;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Interfaces.Repositories;
using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.Application.Services
{
    /// <summary>
    /// Сервис для работы с заявками на справки.
    /// </summary>
    public class ReferenceService : IReferenceService
    {
        private readonly IReferenceRepository _referenceRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса заявок на справки.
        /// </summary>
        /// <param name="referenceRepository">Репозиторий заявок на справки.</param>
        /// <param name="userRepository">Репозиторий пользователей.</param>
        public ReferenceService(
            IReferenceRepository referenceRepository,
            IUserRepository userRepository)
        {
            _referenceRepository = referenceRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Создает новую заявку на справку от имени сотрудника.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <param name="type">Тип запрашиваемой справки.</param>
        /// <param name="copiesCount">Количество экземпляров справки.</param>
        /// <param name="reason">Причина получения справки.</param>
        /// <returns>Результат с идентификатором созданной заявки или текстом ошибки.</returns>
        public async Task<Result<Guid>> CreateReference(Guid employeeId, ReferenceType type, int copiesCount, string reason)
        {
            if (employeeId == Guid.Empty)
            {
                return Result.Failure<Guid>($"{nameof(employeeId)} не может быть пустым");
            }

            var isEmployee = await _userRepository.ExistsWithRole(employeeId, UserRole.Employee);

            if (!isEmployee)
            {
                return Result.Failure<Guid>("Создавать заявки может только сотрудник");
            }

            if (await _referenceRepository.ExistsActive(employeeId, type))
            {
                return Result.Failure<Guid>("Справка такого типа уже в работе");
            }

            var referenceResult = Reference.Create(
                Guid.NewGuid(),
                employeeId,
                type,
                copiesCount,
                reason);

            if (referenceResult.IsFailure)
            {
                return Result.Failure<Guid>(referenceResult.Error);
            }

            await _referenceRepository.Create(referenceResult.Value);
            return Result.Success(referenceResult.Value.Id);
        }

        /// <summary>
        /// Получает заявку на справку по идентификатору вместе с данными сотрудника.
        /// </summary>
        /// <param name="id">Идентификатор заявки.</param>
        /// <returns>Результат с найденной заявкой и сотрудником или текстом ошибки.</returns>
        public async Task<Result<ReferenceDetails>> GetReferenceById(Guid id)
        {
            var reference = await _referenceRepository.GetById(id);

            if (reference is null)
            {
                return Result.Failure<ReferenceDetails>("Справка не найдена");
            }

            var employee = await _userRepository.GetById(reference.EmployeeId);

            if (employee is null)
            {
                return Result.Failure<ReferenceDetails>("Сотрудник не найден");
            }

            return Result.Success(new ReferenceDetails(reference, employee));
        }

        /// <summary>
        /// Получает все заявки конкретного сотрудника вместе с его данными.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <returns>Результат со списком заявок сотрудника или текстом ошибки.</returns>
        public async Task<Result<List<ReferenceDetails>>> GetAllReferencesByEmployeeId(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                return Result.Failure<List<ReferenceDetails>>($"{nameof(employeeId)} не найден");
            }

            var references = await _referenceRepository.GetByEmployeeId(employeeId);

            return await AddEmployees(references);
        }

        /// <summary>
        /// Получает все заявки на справки вместе с данными сотрудников.
        /// </summary>
        /// <returns>Результат со списком всех заявок.</returns>
        public async Task<Result<List<ReferenceDetails>>> GetAllReferences()
        {
            var references = await _referenceRepository.GetAll();

            return await AddEmployees(references);
        }

        /// <summary>
        /// Обновляет статус заявки на справку от имени бухгалтера.
        /// </summary>
        /// <param name="accountantId">Идентификатор бухгалтера.</param>
        /// <param name="referenceId">Идентификатор заявки.</param>
        /// <param name="status">Новый статус заявки.</param>
        /// <returns>Результат операции обновления.</returns>
        public async Task<Result> UpdateReferenceStatus(Guid accountantId, Guid referenceId, ReferenceStatus status)
        {
            if (accountantId == Guid.Empty)
            {
                return Result.Failure("Идентификатор бухгалтера не может быть пустым");
            }

            var isAccountant = await _userRepository.ExistsWithRole(
                accountantId,
                UserRole.Accountant);

            if (!isAccountant)
            {
                return Result.Failure("Изменять статус может только бухгалтер");
            }

            if (referenceId == Guid.Empty)
            {
                return Result.Failure($"{nameof(referenceId)} не может быть пустым");
            }

            var reference = await _referenceRepository.GetById(referenceId);

            if (reference is null)
            {
                return Result.Failure($"{nameof(reference)} не найден");
            }

            var updateResult = reference.UpdateStatus(status);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            var isUpdated = await _referenceRepository.Update(reference);

            if (!isUpdated)
            {
                return Result.Failure("Не удалось обновить справку");
            }

            return Result.Success();
        }

        /// <summary>
        /// Добавляет к списку заявок данные сотрудников, которые создали эти заявки.
        /// </summary>
        /// <param name="references">Список заявок на справки.</param>
        /// <returns>
        /// Результат со списком заявок, дополненных данными сотрудников,
        /// или ошибка, если один или несколько сотрудников не найдены.
        /// </returns>
        private async Task<Result<List<ReferenceDetails>>> AddEmployees(List<Reference> references)
        {
            var employeeIds = references
                .Select(x => x.EmployeeId)
                .Distinct()
                .ToList();

            var employees = await _userRepository.GetByIds(employeeIds);
            var employeesById = employees.ToDictionary(x => x.Id);

            if (employeesById.Count != employeeIds.Count)
            {
                return Result.Failure<List<ReferenceDetails>>("Один или несколько сотрудников не найдены");
            }

            var result = references
                .Select(x => new ReferenceDetails(x, employeesById[x.EmployeeId]))
                .ToList();

            return Result.Success(result);
        }
    }
}
