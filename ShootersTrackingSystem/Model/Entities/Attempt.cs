using System.Text.Json.Serialization;

namespace ShootersTrackingSystem.Model.Entities;

public class Attempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int WeaponId { get; set; }
    public DateTime DateTime { get; set; }
    public uint Score { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
    [JsonIgnore]
    public Weapon Weapon { get; set; }
}