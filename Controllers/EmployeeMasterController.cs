using Employee.API.Data;
using Employee.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        #region Get All Employees
        /// <summary>
        /// Get All Employees
        /// TEST URL: hostAddress/api/EmployeeMaster/GetAllEmployees
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await (from emp in _context.Employees
                                       join dept in _context.Departments on emp.DepartmentId equals dept.DepartmentId
                                       join des in _context.Designations on emp.DesignationId equals des.DesignationId
                                       select new
                                       {
                                           emp.EmployeeId,
                                           emp.Name,
                                           emp.Password,
                                           emp.Phone,
                                           emp.MobilePhone,
                                           emp.Email,
                                           emp.Address,
                                           emp.City,
                                           emp.State,
                                           emp.Zipcode,
                                           emp.DesignationId,
                                           emp.DepartmentId,
                                           emp.CreatedDate,
                                           emp.ModifiedDate,
                                           emp.Role,
                                           DesignationName = des.DesignationName,
                                           DepartmentName = dept.DepartmentName
                                       }).ToListAsync();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        #endregion



        #region Get Employee by id
        /// <summary>
        /// Get Employee | GET: api/Employee/5
        /// TEST URL: hostAddress/api/EmployeeMaster/EmployeeGetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("EmployeeGetById/{id:int}")]
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
        #endregion



        #region Add Employee
        /// <summary>
        /// Add Employee
        /// TEST URL: hostAddress/api/EmployeeMaster/AddEmployee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("AddEmployee")]
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
        #endregion


        #region Update Employee
        /// <summary>
        /// Add Department
        /// TEST URL: hostAddress/api/EmployeeMaster/UpdateEmployee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeL employee)
        {
            // fix this like designations and departments.
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
        #endregion




        #region Delete Employee
        /// <summary>
        /// Delete Employee by ID
        /// TEST URL: hostAddress/api/EmployeeMaster/id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEmployee/{id}")]
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
        #endregion



        #region Login
        /// <summary>
        /// POST: api/Auth/login
        /// TEST URL: 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
         //JWT, Oauth, OpenConnect can be implemented later for security. For now, this endpoint is open for testing.
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


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



        //private string GenerateJwtToken(User user)
        //{
        //    var claims = new[]
        //    {
        //new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

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



        #region GetAllEmployees Paging
        // Might need to change this to work like #region Get All Designations in DesignationMasterController, need drop-downs and all that.
        /// <summary>
        /// TEST URL: https://localhost:7004/api/EmployeeMaster
        /// GET: api/Employee?pageNumber=1&pageSize=10&sortBy=Name&sortDesc=false
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDesc"></param>
        /// <returns></returns>
        [HttpGet("GetAllEmployeesPaging")]
        public async Task<IActionResult> GetAllPaging(
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
        #endregion


    }

}
