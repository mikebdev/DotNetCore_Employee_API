using Employee.API.Data;
using Employee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentMasterController : ControllerBase
    {

        private readonly EmployeeDbContext _context;

        public DepartmentMasterController (EmployeeDbContext context)
        {
            _context = context;
        }



        /// <summary>
        /// TEST URL: https://localhost:7004/api/DepartmentMaster/GetAllDepartments
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet("GetAllDepartments")]
        public IActionResult GetDepartments()
        {
            var deptList = _context.Departments.ToList();
            return Ok(deptList);
        }

        /// <summary>
        /// TEST URL: https://localhost:7004/api/DepartmentMaster/    
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department department)
        {
            if (_context.Departments.Any(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower()))
                return Conflict("A department with that name already exists.");

            _context.Departments.Add(department);
            _context.SaveChanges();
            return Ok("Department Added Successfully");
            //return Ok(department); // could return inserted ID here.
        }

        /// <summary>
        /// TEST URL: https://localhost:7004/api/DepartmentMaster/
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department department)
        {
            var existingDepartment = _context.Departments.Find(department.DepartmentId);
            if (existingDepartment == null)
            {
                return NotFound("Department not found");
            }

            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.IsActive = department.IsActive;
            _context.SaveChanges();
            return Ok("Department Updated Successfully");

        }

        /// <summary>
        /// TEST URL: https://localhost:7004/api/DepartmentMaster/
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDepartment/{id}")]
        public IActionResult DeleteDepartment(int id) {
            var existingDepartment = _context.Departments.Find(id);
            if (existingDepartment == null) {
                return NotFound("Department not Found");
            }

            _context.Departments.Remove(existingDepartment);
            _context.SaveChanges();
            return Ok("Department Deleted Successfully");
        }


    }
}
