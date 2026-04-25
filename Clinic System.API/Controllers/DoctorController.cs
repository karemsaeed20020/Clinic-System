using Clinic_System.API.Bases;
using Clinic_System.Application.Features.Doctors.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.API.Controllers
{
    // [Authorize]
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : AppControllerBase
    {
        public DoctorController(IMediator mediator) : base(mediator)
        {
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            command.BaseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        //[Authorize(Roles = "Admin,Doctor")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Doctor ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeleteDoctor(int id)
        {
            var response = await mediator.Send(new SoftDeleteDoctorCommand
            {
                Id = id
            });
            return NewResult(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}/hard")]
        public async Task<IActionResult> HardDeleteDoctor(int id)
        {
            var response = await mediator.Send(new HardDeleteDoctorCommand
            {
                Id = id
            });
            return NewResult(response);
        }
    }
}
