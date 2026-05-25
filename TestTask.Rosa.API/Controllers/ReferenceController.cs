using Microsoft.AspNetCore.Mvc;
using TestTask.Rosa.API.Contracts.Requests;
using TestTask.Rosa.API.Contracts.Response;
using TestTask.Rosa.Application.Interfaces.Services;
using TestTask.Rosa.Application.Models;

namespace TestTask.Rosa.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с заявками на справки.
    /// </summary>
    [ApiController]
    [Route("api/reference")]
    public class ReferenceController : ControllerBase
    {
        private readonly IReferenceService _referenceService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера заявок.
        /// </summary>
        /// <param name="referenceService">Сервис заявок на справки.</param>
        public ReferenceController(IReferenceService referenceService)
        {
            _referenceService = referenceService;
        }

        /// <summary>
        /// Создает новую заявку на справку.
        /// </summary>
        /// <param name="request">Данные для создания заявки.</param>
        /// <returns>Ответ с идентификатором созданной заявки.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateReferenceRequest request)
        {
            try
            {
                var result = await _referenceService.CreateReference(
                    request.EmployeeId,
                    request.ReferenceType,
                    request.CopiesCount,
                    request.Reason);

                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }

                return Ok(new { id = result.Value });
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Произошла внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получает все заявки на справки.
        /// </summary>
        /// <returns>Список заявок на справки.</returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllReferences()
        {
            try
            {
                var result = await _referenceService.GetAllReferences();

                if (result.IsFailure)
                {
                    return NotFound(result.Error);
                }

                return Ok(result.Value.Select(MapToResponse));
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Произошла внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получает заявку на справку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заявки.</param>
        /// <returns>Данные найденной заявки.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReferenceResponse>> GetReferenceById(Guid id)
        {
            try
            {
                var reference = await _referenceService.GetReferenceById(id);

                if (reference.IsFailure)
                {
                    return NotFound(reference.Error);
                }

                return Ok(MapToResponse(reference.Value));
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Произошла внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получает все заявки указанного сотрудника.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <returns>Список заявок сотрудника.</returns>
        [HttpGet("employee/{employeeId:guid}")]
        public async Task<IActionResult> GetAllReferencesByEmployeeId(Guid employeeId)
        {
            try
            {
                var result = await _referenceService.GetAllReferencesByEmployeeId(employeeId);

                if (result.IsFailure)
                {
                    return NotFound(result.Error);
                }

                return Ok(result.Value.Select(MapToResponse));
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Произошла внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Обновляет статус заявки на справку.
        /// </summary>
        /// <param name="id">Идентификатор заявки.</param>
        /// <param name="request">Данные для изменения статуса.</param>
        /// <returns>Результат операции обновления статуса.</returns>
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReferenceStatusRequest request)
        {
            try
            {
                var result = await _referenceService.UpdateReferenceStatus(
                    request.AccountantId,
                    id,
                    request.Status);

                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Произошла внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Преобразует данные заявки и сотрудника в контракт ответа API.
        /// </summary>
        /// <param name="details">Данные заявки и сотрудника.</param>
        /// <returns>Контракт ответа с данными заявки.</returns>
        private static ReferenceResponse MapToResponse(ReferenceDetails details)
        {
            var reference = details.Reference;
            var employee = details.Employee;

            return new ReferenceResponse(
                reference.Id,
                reference.EmployeeId,
                employee.FirstName,
                employee.LastName,
                reference.Type,
                reference.CopiesCount,
                reference.Reason,
                reference.Status,
                reference.CreatedAt,
                reference.UpdatedAt,
                reference.ClosedAt);
        }
    }
}
