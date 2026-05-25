using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.API.Contracts.Response
{
    /// <summary>
    /// Ответ с данными заявки на справку.
    /// </summary>
    /// <param name="Id">Идентификатор заявки.</param>
    /// <param name="EmployeeId">Идентификатор сотрудника.</param>
    /// <param name="FirstNameEmployee">Имя сотрудника.</param>
    /// <param name="LastNameEmployee">Фамилия сотрудника.</param>
    /// <param name="Type">Тип справки.</param>
    /// <param name="CopiesCount">Количество экземпляров справки.</param>
    /// <param name="Reason">Причина получения справки.</param>
    /// <param name="Status">Текущий статус заявки.</param>
    /// <param name="CreatedAt">Дата и время создания заявки.</param>
    /// <param name="UpdatedAt">Дата и время последнего обновления заявки.</param>
    /// <param name="ClosedAt">Дата и время закрытия заявки.</param>
    public record ReferenceResponse(
        Guid Id,
        Guid EmployeeId,
        string FirstNameEmployee,
        string LastNameEmployee,
        ReferenceType Type,
        int CopiesCount,
        string Reason,
        ReferenceStatus Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        DateTime? ClosedAt);
}
