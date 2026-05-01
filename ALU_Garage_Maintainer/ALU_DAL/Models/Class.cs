using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ALU_DAL.Models;

public partial class Class
{
    public string Class1 { get; set; }

    public byte MinStars { get; set; }

    public byte MaxStars { get; set; }

    public byte MinFuel { get; set; }

    public byte MaxFuel { get; set; }

    public int ValidRarity { get; set; }

    public int ValidRarityForEip { get; set; }
    [JsonIgnore]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
