using Microsoft.AspNetCore.Mvc;
using TestTask.Rosa.Application.Interfaces.Services;

namespace TestTask.Rosa.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReferenceController : ControllerBase
    {
        private readonly IReferenceService _referenceService;

        public ReferenceController(IReferenceService referenceService)
        {
            _referenceService = referenceService;
        }


    }
}
