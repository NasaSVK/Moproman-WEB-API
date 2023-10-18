using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MopromanWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace MopromanWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecordController : ControllerBase
    {

        private readonly MopromanDbContext _context;


        public RecordController(MopromanDbContext context) { 
            _context = context; 
        }

        [HttpGet]
        public IEnumerable<Record> Get()
        {
            //using (var context = new MopromanDbContext()) {      
                return _context.Records.ToList();                    
            //}
        }

            [HttpGet, Route("interval")]
            public async Task<ActionResult> GetRecordsInPeriod(int recordID, DateTime dateStart, DateTime dateEnd)
            {

                DateTime OD = new DateTime(2023, 10, 13, 12, 0, 0);
                
                try
                {
                    var orders = await _context.Records.Where(reccord => reccord.DateTime <= dateEnd && reccord.DateTime >= dateStart).ToListAsync();
                    //IList<Record> orders = await _context.Records.Where(record => record.DateTime > OD).ToListAsync<Record>(); //GetPeriodOrdersAsync(productCode, dateStart.ToUniversalTime(), dateEnd.ToUniversalTime());               
                    return Ok(orders);
              
                }
                
                catch (Exception ex)
                {
                    return NotFound();
                }
            } 

    }
}