using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.Application.Models;

/// <summary>
/// Данные заявки на справку вместе с данными сотрудника.
/// </summary>
/// <param name="Reference">Заявка на справку.</param>
/// <param name="Employee">Сотрудник, создавший заявку.</param>
public record ReferenceDetails(
    Reference Reference,
    User Employee);
