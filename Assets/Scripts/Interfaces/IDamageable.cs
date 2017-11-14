/// <summary>
/// Attach to any script that you want to the player to be able to shoot. Make sure to set the layermask on the gameobject to either Shootable or Environment
/// </summary>
public interface IDamageable {
    /// <summary>
    /// Returns whether or not the object took fatal damage. Takes in the amount of damage to inflict.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    bool TakeDamage(int amount);	
}
