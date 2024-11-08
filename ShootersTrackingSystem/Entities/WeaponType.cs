using System.Text.Json.Serialization;

namespace ShootersTrackingSystem.Entities;

public class WeaponType
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    [JsonIgnore]
    public ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
}