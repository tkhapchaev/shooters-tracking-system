using System.Text.Json.Serialization;

namespace ShootersTrackingSystem.Model.Entities;

public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WeaponTypeId { get; set; }
    
    [JsonIgnore]
    public WeaponType WeaponType { get; set; }
}