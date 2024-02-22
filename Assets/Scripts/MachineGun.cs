public class MachineGun : Weapon
{
    void Update()
    {
        if(State == EnumWeaponState.NoTarget) return;
        Rotate(TargetPosition.position);
    }
}