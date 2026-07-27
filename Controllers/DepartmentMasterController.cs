using Employee.API.Data;
using Employee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// TEST URL: hostAddress/api/DepartmentMaster/GetAllDepartments
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var deptList = await _context.Departments.ToListAsync();
                return Ok(deptList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
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
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            //
            //if (_context.Departments.Any(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower()))
            //    return Conflict("A department with that name already exists.");

            var name = department?.DepartmentName?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("DepartmentName is required.");
            }


            if (await _context.Departments.AnyAsync(d => d.DepartmentName.ToLower() == name.ToLower()))
            {
                return Conflict("A Departmen with that name already exists.");
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return Created("Department Added Successfully", department);
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
        public async Task<IActionResult> UpdateDepartment([FromBody] Department department)
        {
            try
            {
                var existingDepartment = await _context.Departments.FindAsync(department.DepartmentId);

                if (existingDepartment == null)
                {
                    return NotFound("Department not found");
                }

                existingDepartment.DepartmentId = department.DepartmentId;
                existingDepartment.DepartmentName = department.DepartmentName;
                existingDepartment.IsActive = department.IsActive;

                _context.SaveChanges();
                return Created("Department Updated Successfully", department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
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
        public async  Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var existingDepartment = await _context.Departments.FindAsync(id);
                if (existingDepartment == null)
                {
                    return NotFound("Department not Found");
                }

                _context.Departments.Remove(existingDepartment);
                _context.SaveChanges();
                return Created("Department Deleted Successfully", existingDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {message = "An error occured.", detail = $"{ex.Message}" });
            }
        }
        #endregion

    }
}
