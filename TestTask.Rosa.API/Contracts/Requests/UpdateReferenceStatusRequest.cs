using System.ComponentModel.DataAnnotations;
using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на изменение статуса заявки на справку.
    /// </summary>
    /// <param name="AccountantId">Идентификатор бухгалтера, изменяющего статус.</param>
    /// <param name="Status">Новый статус заявки.</param>
    public record UpdateReferenceStatusRequest(
        [Required]
        Guid AccountantId,
        [Required]
        ReferenceStatus Status);
}
