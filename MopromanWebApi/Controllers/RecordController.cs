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
        private const int MAX_POCET_ZAZNAMOV = 1000000;


        public RecordController(MopromanDbContext context)
        {
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
        public async Task<ActionResult> GetRecordsInPeriod(int recordID, DateTime dateStart, DateTime dateEnd, string pecId, String? zmena)
        {

            DateTime OD = new DateTime(2023, 10, 13, 12, 0, 0);

            try
            {
                var orders = await _context.Records.Where(reccord => reccord.DateTime <= dateEnd && reccord.DateTime >= dateStart && reccord.PecId == pecId && ((zmena == null || zmena == "VSETKY") ? true : zmena == reccord.Zmena)).ToListAsync();
                //IList<Record> orders = await _context.Records.Where(record => record.DateTime > OD).ToListAsync<Record>(); //GetPeriodOrdersAsy; nc(productCode, dateStart.ToUniversalTime(), dateEnd.ToUniversalTime());               
                orders = Helpers.RedukujPocetHodnot(orders);                
                return Ok(orders);

            }

            catch (Exception ex)
            {
                return NotFound();
            }
        }


        //"BUSINISS LOGIC fro SampleService"
        private async Task<ActionResult> DeleteOlderRecords() {
            
            int pocet_zaznamov = _context.Records.Count();
            
            if (pocet_zaznamov > MAX_POCET_ZAZNAMOV) {
               List<Record> MazaneZaznamy = await _context.Records.Take(pocet_zaznamov - MAX_POCET_ZAZNAMOV).ToListAsync();
                _context.Records.RemoveRange((IEnumerable<Record>)MazaneZaznamy);
            }
            
            return Ok();
        }


    }
}