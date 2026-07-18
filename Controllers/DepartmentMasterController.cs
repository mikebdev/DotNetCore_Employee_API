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


        #region Get All Departments
        /// <summary>
        /// Get All Department
        /// TEST URL: hostAddress/api/DepartmentMaster/etAllDepartments
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDepartments")]
        public IActionResult GetDepartments()
        {
            var deptList = _context.Departments.ToList();
            return Ok(deptList);
        }
        #endregion


        #region Add Department
        /// <summary>
        /// Add Department
        /// TEST URL: hostAddress/api/DepartmentMaster/AddDepartment
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department department)
        {
            //
            //if (_context.Departments.Any(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower()))
            //    return Conflict("A department with that name already exists.");

            bool exists = _context.Departments.Any(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower());

            if (exists)
            {
                return BadRequest("A department with that name already exists.");
            }


            _context.Departments.Add(department);
            _context.SaveChanges();
            return Created("Department Added Successfully", department);
            //return Ok(department); // could return inserted ID here.
        }
        #endregion


        #region Update Department
        /// <summary>
        /// Update Department
        /// TEST URL: hostAddress/api/DepartmentMaster/UpdateDepartment
        /// </summary>
        /// <param="department"></param>
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
            return Created("Department Updated Successfully", department);

        }
        #endregion


        #region Delete Department
        /// <summary>
        /// Delete Department by ID
        /// TEST URL: hostAddress/api/DepartmentMaster/id 
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
            return Created("Department Deleted Successfully", existingDepartment);
        }
        #endregion

    }
}
