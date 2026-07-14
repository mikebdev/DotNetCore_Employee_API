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



        // GET: api/Designation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var designations = await _context.Designations.ToListAsync();
                return Ok(designations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

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

        // POST: api/Designation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (_context.Designations.Any(d => d.DesignationName.ToLower() == d.DesignationName.ToLower()))
                    return Conflict("A designation with that name already exists.");


                _context.Designations.Add(designation);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = designation.DesignationId }, designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        // PUT: api/Designation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != designation.DesignationId)
                    return BadRequest(new { message = "ID mismatch." });

                var existing = await _context.Designations.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Designation with ID {id} not found." });

                existing.DepartmentId = designation.DepartmentId;
                existing.DesignationName = designation.DesignationName;

                await _context.SaveChangesAsync();
                return Ok(existing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }

        // DELETE: api/Designation/5
        [HttpDelete("{id}")]
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
    }



}



