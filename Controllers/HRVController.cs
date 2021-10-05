using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRV.Data;
using HRV.Data.Db;
using HRV.Data.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRV.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HRVController : ControllerBase
    {
        private readonly HRVDb _db;
        private readonly UserManager<ApplicationUser> _user;
        private readonly ILogger<HRVController> _logger; 
        public HRVController(HRVDb db, UserManager<ApplicationUser> userManager, ILogger<HRVController> logger)
        {
            _db = db;
            _user = userManager;
            _logger = logger;
        }

        private async Task<string> GetSid()
        {
            var user = HttpContext.User;
            var nameId = user.Claims.FirstOrDefault(o => o.Type == "sid");

            var userInDb = await _user.FindByNameAsync(user.Identity.Name);

            if (userInDb == null) throw new UserNotFoundInDatabase();

            return nameId.Value;
        }

        // GET: api/HRV
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var sid = await GetSid();

                var ranges = _db.Ranges.ToList();


                var records = _db.HRVs.Where(h => h.UserSid == sid)
                                    .OrderByDescending(o => o.Date)
                                    .Select(h => new HRVList() { Date = h.Date, HRV = h.HRV, Id = h.Id })
                                    .ToList();

                records.ToList().ForEach(h =>
                {
                    h.Range = ranges.FirstOrDefault(r => r.Low <= h.HRV && r.High >= h.HRV)?.Name;
                });

                return Ok(records);
            }
            catch(UserNotFoundInDatabase)
            {
                return Unauthorized("Token is invalid"); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/HRV/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var sid = await GetSid();

                var record = _db.HRVs.FirstOrDefault(h => h.Id == id && h.UserSid == sid);

                if (record == null) return NotFound();
                return Ok(record);
            }
            catch(UserNotFoundInDatabase)
            {
                return Unauthorized("Token is invalid"); 
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/HRV
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HRVInput value)
        {
            try
            {
     
                var sid = await GetSid();

                var rec = new HRVRecord()
                {
                    Date = value.Date,
                    HRV = value.HRV,
                    UserSid = sid
                };

                _db.HRVs.Add(rec);
                await _db.SaveChangesAsync();

                var output = new HRVOutput()
                {
                    Id = rec.Id,
                    Date = rec.Date,
                    HRV = rec.HRV
                };

                return Created($"/api/HRV/{rec.Id}", output);
            }
            catch(UserNotFoundInDatabase)
            {
                _logger.LogError("Token is invalid");
                return Unauthorized("Token is invalid"); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return StatusCode(500, new { message = ex.Message });
            }

        }

        // PUT: api/HRV/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] HRVRecord value)
        {
            try
            {
                var sid = await GetSid();
                var rec = _db.HRVs.FirstOrDefault(h => h.Id == id && h.UserSid == sid);

                if (rec == null) return NotFound();
                else
                {
                    rec.Date = value.Date;
                    rec.HRV = value.HRV;

                    await _db.SaveChangesAsync();
                }

                return Accepted(rec);
            }
            catch(UserNotFoundInDatabase)
            {
                _logger.LogError("Token is invalid");
                return Unauthorized("Token is invalid"); 
            }
        }

        // DELETE: api/HRV/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sid = await GetSid();
                var rec = _db.HRVs.FirstOrDefault(h => h.Id == id && h.UserSid == sid);

                if (rec == null) return NotFound();
                else
                {
                    _db.HRVs.Remove(rec);
                    await _db.SaveChangesAsync();

                    return Accepted(rec);
                }
            }
            catch(UserNotFoundInDatabase)
            {
                _logger.LogError("Token is invalid"); 
                return Unauthorized("Token is invalid"); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("bulkLoad")]
        public async Task<IActionResult> BulkLoad([FromBody]List<HRVInput> records)
        {
            try
            {
                var sid = await GetSid();

                foreach (var r in records)
                {
                    HRVRecord h = new HRVRecord()
                    {
                        Date = r.Date,
                        UserSid = sid,
                        HRV = r.HRV
                    };

                    await _db.HRVs.AddAsync(h);
                }

                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
