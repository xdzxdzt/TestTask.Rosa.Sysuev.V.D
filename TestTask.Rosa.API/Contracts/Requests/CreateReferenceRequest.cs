using System.ComponentModel.DataAnnotations;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на создание заявки на справку.
    /// </summary>
    /// <param name="EmployeeId">Идентификатор сотрудника, создающего заявку.</param>
    /// <param name="ReferenceType">Тип запрашиваемой справки.</param>
    /// <param name="CopiesCount">Количество экземпляров справки.</param>
    /// <param name="Reason">Причина получения справки.</param>
    public record CreateReferenceRequest(
        [Required]
        Guid EmployeeId,
        [Required]
        ReferenceType ReferenceType,
        [Required]
        [Range(1, int.MaxValue)]
        int CopiesCount,
        [Required]
        [MaxLength(Reference.REASON_MAX_LENGTH)]
        string Reason);
}
