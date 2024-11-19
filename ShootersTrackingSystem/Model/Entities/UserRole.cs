using System.Text.Json.Serialization;

namespace ShootersTrackingSystem.Model.Entities;

public class UserRole
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    [JsonIgnore]
    public ICollection<User> Users { get; set; } = new List<User>();
}