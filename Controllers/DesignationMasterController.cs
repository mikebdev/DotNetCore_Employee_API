using Employee.API.Data;
using Employee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationMasterController : ControllerBase
    {

        private readonly EmployeeDbContext _context;

        public DesignationMasterController (EmployeeDbContext context)
        {
            _context = context;
        }


        #region Get All Designations
        // GET: api/Designation
        /// <summary>
        /// Get All Designations
        /// TEST URL: hostAddress/api/DesignationMaster/GetAllDesignations
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDesignations")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Join with departments to include department name in the result
                var designations = await (from d in _context.Designations
                                          join dept in _context.Departments on d.DepartmentId equals dept.DepartmentId
                                          select new
                                          {
                                              d.DesignationId,
                                              d.DepartmentId,
                                              d.DesignationName,
                                              DepartmentName = dept.DepartmentName
                                          }).ToListAsync();

                return Ok(designations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        #endregion


        
        // GET: api/Designation/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var designation = await _context.Designations.FindAsync(id);
                if (designation == null)
                    return NotFound(new { message = $"Designation with ID {id} not found." });

                return Ok(designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }


        #region Add Designation
        /// <summary>
        /// Add Designation
        /// TEST URL: hostAddress/api/DesignationMaster/AddDesignation
        /// </summary>
        /// <param name="designation"></param>
        /// <returns></returns>
        [HttpPost("AddDesignation")]
        public async Task<IActionResult> Create([FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var name = designation?.DesignationName?.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("DesignationName is required.");
                }

                if (await _context.Designations.AnyAsync(d => d.DesignationName.ToLower() == name.ToLower()))
                {
                    return Conflict("A designation with that name already exists.");
                }

                _context.Designations.Add(designation);
                await _context.SaveChangesAsync();

                //return CreatedAtAction(nameof(GetById), new { id = designation.DesignationId }, designation);
                return Created("Department Added Successfully", designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        #endregion


        #region Update Designation
        /// <summary>
        /// Update Designation
        /// TEST URL: hostAddress/api/DesignationMaster/UpdateDesignation
        /// </summary>
        /// <param name="designation"></param>
        /// <returns></returns>
        [HttpPut("UpdateDesignation")]
        public async Task<IActionResult> Update( [FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
  
                var existingdesignation = _context.Designations.Find(designation.DesignationId);

                if (existingdesignation == null)
                {
                    return NotFound("Designation not found");
                }

                existingdesignation.DesignationId = designation.DesignationId;
                existingdesignation.DepartmentId = designation.DepartmentId;
                existingdesignation.DesignationName = designation.DesignationName;

                _context.SaveChanges();
                return Created("Designation Updated Successfully", designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        #endregion


        #region Delete Designation
        /// <summary>
        /// Delete Designation by ID
        /// TEST URL: hostAddress/api/DesignationMaster/DeleteDesignation/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDesignation/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var designation = await _context.Designations.FindAsync(id);
                if (designation == null)
                    return NotFound(new { message = $"Designation with ID {id} not found." });

                _context.Designations.Remove(designation);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        #endregion




        // GET: api/Designation/filter?departmentId=2&name=manager
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] int? departmentId, [FromQuery] string? name)
        {
            try
            {
                var query = _context.Designations.AsQueryable();

                if (departmentId.HasValue)
                    query = query.Where(d => d.DepartmentId == departmentId.Value);

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(d => d.DesignationName.Contains(name));

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }




    }



}



