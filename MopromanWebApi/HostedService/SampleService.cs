using Microsoft.EntityFrameworkCore;
using MopromanWebApi.Controllers;
using MopromanWebApi.Models;

namespace PeriodicBackgroundTaskSample;

class SampleService
{
    private readonly ILogger<SampleService> _logger;
    private readonly MopromanDbContext _context;
    private const int MAX_POCET_ZAZNAMOV = 1000000;
    private const int ZMAZANE_ZAZNAMY = 10;
    private static int zmazane_zaznamy_celkom = 0;

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

        if (pocet_zaznamov > MAX_POCET_ZAZNAMOV)
        {
            List<Record> MazaneZaznamy = await _context.Records.Take(ZMAZANE_ZAZNAMY/*pocet_zaznamov - MAX_POCET_ZAZNAMOV*/).ToListAsync();
            _context.Records.RemoveRange((IEnumerable<Record>)MazaneZaznamy);
            //_ = _context.SaveChangesAsync();
            await _context.SaveChangesAsync();
            zmazane_zaznamy_celkom += 10;
        } 

        _logger.LogInformation(
            "\n-----------------\n" +
            "Pocet ZMAZANYCH ZAZNAMOV:"+ (ZMAZANE_ZAZNAMY/*pocet_zaznamov - MAX_POCET_ZAZNAMOV*/).ToString()+
            "Pocet ZMAZANYCH ZAZNAMOV CELKOM:" + (zmazane_zaznamy_celkom).ToString() +
            "\n-----------------");

        //System.Windows.MessageBox.Show("Nepodarilo sa pripojiť k DB z nasledujúceho dôvodu!" + e.Message + "\n\nAplikácia bude skončená!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);

    }
}
