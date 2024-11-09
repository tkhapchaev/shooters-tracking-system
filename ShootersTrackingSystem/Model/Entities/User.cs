using System.Text.Json.Serialization;

namespace ShootersTrackingSystem.Model.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int UserRoleId { get; set; }
    
    [JsonIgnore]
    public UserRole UserRole { get; set; }
}