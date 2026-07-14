using Employee.API.Data;
using Employee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public EmployeeMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee?pageNumber=1&pageSize=10&sortBy=Name&sortDesc=false
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "employeeId",
            bool sortDesc = false)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                    return BadRequest("pageNumber and pageSize must be greater than 0.");

                var query = _context.Employees.AsQueryable();

                query = sortBy.ToLower() switch
                {
                    "name" => sortDesc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
                    "email" => sortDesc ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                    "city" => sortDesc ? query.OrderByDescending(e => e.City) : query.OrderBy(e => e.City),
                    "createddate" => sortDesc ? query.OrderByDescending(e => e.CreatedDate) : query.OrderBy(e => e.CreatedDate),
                    _ => sortDesc ? query.OrderByDescending(e => e.EmployeeId) : query.OrderBy(e => e.EmployeeId),
                };

                var total = await query.CountAsync();
                var employees = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    totalRecords = total,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)total / pageSize),
                    data = employees
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching employees.", error = ex.Message });
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return NotFound(new { message = $"Employee with ID {id} not found." });

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the employee.", error = ex.Message });
            }
        }

        // GET: api/Employee/filter?name=john&city=delhi&designationId=2
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
            string? name,
            string? city,
            string? state,
            int? designationId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                    return BadRequest("pageNumber and pageSize must be greater than 0.");

                var query = _context.Employees.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(e => e.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(city))
                    query = query.Where(e => e.City.Contains(city));

                if (!string.IsNullOrWhiteSpace(state))
                    query = query.Where(e => e.State.Contains(state));

                if (designationId.HasValue)
                    query = query.Where(e => e.DesignationId == designationId.Value);

                var total = await query.CountAsync();
                var employees = await query
                    .OrderBy(e => e.EmployeeId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    totalRecords = total,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)total / pageSize),
                    data = employees
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while filtering employees.", error = ex.Message });
            }
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeL employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                employee.CreatedDate = DateTime.UtcNow;
                employee.ModifiedDate = DateTime.UtcNow;

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the employee.", error = ex.Message });
            }
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeL employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != employee.EmployeeId)
                    return BadRequest(new { message = "ID in URL does not match ID in body." });

                var existing = await _context.Employees.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Employee with ID {id} not found." });

                existing.Name = employee.Name;
                existing.Phone = employee.Phone;
                existing.MobilePhone = employee.MobilePhone;
                existing.Email = employee.Email;
                existing.Address = employee.Address;
                existing.City = employee.City;
                existing.State = employee.State;
                existing.Zipcode = employee.Zipcode;
                existing.DesignationId = employee.DesignationId;
                existing.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(existing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the employee.", error = ex.Message });
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return NotFound(new { message = $"Employee with ID {id} not found." });

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the employee.", error = ex.Message });
            }
        }




        #region Login
        /// <summary>
        /// POST: api/Auth/login
        /// TEST URL: 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _context.Employees.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                //if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                //    return Unauthorized(new { message = "Invalid email or password." });

                //var token = GenerateJwtToken(user);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                if (user.Password != model.Password)
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }


                //return Ok(new
                //{
                //    token,
                //    user.UserId,
                //    user.FullName,
                //    user.Email,
                //    user.Role
                //});

                return Ok(new
                {
                    Message = "Login Successful",
                    Data = new
                    {
                        user.EmployeeId,
                        user.Name,
                        user.Email,
                        user.Phone,
                        user.DesignationId, // get designationName with this ID
                        user.Role
                        //user.DesignationName
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        #endregion



    }

}
