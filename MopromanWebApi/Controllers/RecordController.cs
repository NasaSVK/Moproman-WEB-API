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
                orders = this.dajOkamzituSpotrebu(orders);
                orders = Helpers.RedukujPocetHodnot(orders);                
                return Ok(orders);

            }

            catch (Exception ex)
            {
                return NotFound();
            }
        }

        private List<Record> dajOkamzituSpotrebu(List<Record> pList) {

            int DLZKA = pList.Count;
            double? Pi=0,Pii = 0;
            List <Record> result = pList;
            if (DLZKA == 0) return result;
            double dii = 5000 / 1000; //prevod z ms na sekundy
            double? p0 = pList[0].Vykon;
            result[0].OkamzitaSpotreba = (float)(1000*p0*dii);
            
            for (int i = 1; i < DLZKA-1; i++) {
                DateTime dti = pList[i].DateTime;
                DateTime dtii = pList[i+1].DateTime;
                dii = (dtii - dti).TotalMilliseconds/1000; //prevod z ms na sekundy
                Pi = pList[i].Vykon;
                Pii = pList[i + 1].Vykon;
                result[i].OkamzitaSpotreba = (float)(1000*(Pi+Pii)*dii/2);
            }

            return result;
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