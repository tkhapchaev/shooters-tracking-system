namespace ShootersTrackingSystem.Entities;

public class Attempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int WeaponId { get; set; }
    public Weapon Weapon { get; set; }
    public DateTime DateTime { get; set; }
    public uint Score { get; set; }
}