namespace ShootersTrackingSystem.Entities;

public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WeaponTypeId { get; set; }
    public WeaponType WeaponType { get; set; }
}