﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MopromanWebApi.Models;

public partial class Record
{
    public int Id { get; set; }

    public string PecId { get; set; } = null!;

    public float? Napatie { get; set; }

    public float? Prud { get; set; }

    public float? SobertVstup { get; set; }

    public float? TVodaVstup { get; set; }

    public float? TVodaVystup { get; set; }

    public float? Vykon { get; set; }

    public float? RzPribenie { get; set; }

    public DateTime DateTime { get; set; }

    public float? SobertVykon { get; set; }

    public float? Tlak { get; set; }

    public string Zmena { get; set; } = null!;

    public float? Frekvencia { get; set; }

    public float? TeplotaP1 { get; set; }

    public float? TeplotaP2 { get; set; }

    public float? TeplotaOkruh { get; set; }

    public float? PrietokVody { get; set; }

    //necita sa z DB; sluzi na ulozenie prepocitanych hodnot klientskej aplikacii; prepocty robi API
    [NotMapped]
    public float OkamzitaSpotreba { get; set; }

}
