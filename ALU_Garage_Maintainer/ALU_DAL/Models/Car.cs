using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ALU_DAL.Models;

public partial class Car
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Class { get; set; }

    public byte Fuel { get; set; }

    public string Rarity { get; set; }

    public decimal TopSpeed { get; set; }

    public decimal Acceleration { get; set; }

    public decimal Handling { get; set; }

    public decimal Nitro { get; set; }

    public byte MaxStars { get; set; }

    public decimal MaxRank { get; set; }

    public bool HasEips { get; set; }

    public byte? NoEips { get; set; }

    public bool RequiresKey { get; set; }

    public int? Bp1sCount { get; set; }

    public int Bp2sCount { get; set; }

    public int Bp3sCount { get; set; }

    public int? Bp4sCount { get; set; }

    public int? Bp5sCount { get; set; }

    public int? Bp6sCount { get; set; }

    [JsonIgnore]
    public virtual Class ClassNavigation { get; set; }
}
