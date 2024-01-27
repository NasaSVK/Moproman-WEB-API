using Microsoft.EntityFrameworkCore;
using MopromanWebApi.Controllers;
using MopromanWebApi.Models;

namespace PeriodicBackgroundTaskSample;

class SampleService
{
    private readonly ILogger<SampleService> _logger;
    private readonly MopromanDbContext _context;
    private const int MAX_POCET_ZAZNAMOV = 1000000;
    private const int ZMAZANE_ZAZNAMY = 1;
    private static int zmazane_zaznamy_celkom = 0;
    private const int INTERVAL_MESIACE = 1; 

     //objekt implementujuci rozhranie ILogger sa dosadi automaticky
    public SampleService(ILogger<SampleService> logger, MopromanDbContext context)
    {
        _logger = logger;        
        _context = context;        
    }

    public async Task DoSomethingAsync()
    {
        //await Task.Delay(100);
        int pocet_zaznamov = _context.Records.Count();

        //if (pocet_zaznamov > MAX_POCET_ZAZNAMOV)
        //{
            _logger.LogInformation("\n ########################### \n MAZANIE ZAZNAMOV STARSICH AKO: " + DateTime.Now.AddMonths(-INTERVAL_MESIACE) + "\n ############################");
            List<Record> MazaneZaznamy = await _context.Records.Where(MZ => MZ.DateTime <  (DateTime.Now.AddMonths(-INTERVAL_MESIACE))).ToListAsync();
            //List<Record> MazaneZaznamy = await _context.Records.Take(ZMAZANE_ZAZNAMY/*pocet_zaznamov - MAX_POCET_ZAZNAMOV*/).ToListAsync();
            _context.Records.RemoveRange((IEnumerable<Record>)MazaneZaznamy);
            if (MazaneZaznamy.Count>0)
                await _context.SaveChangesAsync();
            zmazane_zaznamy_celkom += MazaneZaznamy.Count;
        //} 

        _logger.LogInformation(
            "\n-------------------------------------\n" +
            "Pocet ZMAZANYCH ZAZNAMOV: "+ (MazaneZaznamy.Count /*ZMAZANE_ZAZNAMY*//*pocet_zaznamov - MAX_POCET_ZAZNAMOV*/).ToString()+"\n"+
            "Pocet ZMAZANYCH ZAZNAMOV CELKOM: " + (zmazane_zaznamy_celkom).ToString() +
            "\n--------------------------------------");            
        //System.Windows.MessageBox.Show("Nepodarilo sa pripojiť k DB z nasledujúceho dôvodu!" + e.Message + "\n\nAplikácia bude skončená!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);

    }
}
