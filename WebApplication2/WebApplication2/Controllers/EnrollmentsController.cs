using Microsoft.AspNetCore.Mvc;
using WebApp.DPOs.Requests;
using WebApp.Services;


namespace WebApplication2.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {

            string res = _service.EnrollStudent(request);
            if (res.Contains("Failed:"))
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }


        [HttpPost("{promotions}")]
        public IActionResult PromoteStudents(PromoteStudentRequest request)
        {
            string res = _service.PromoteStudents(request);
            if (res.Contains("Failed:"))
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }

        }

    }
}