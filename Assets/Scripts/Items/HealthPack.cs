using UnityEngine;

public class HealthPack : MonoBehaviour, IDroppable
{

    int amountOfHealth;

    public void SetDropAmount(int amountToDrop)
    {
        amountOfHealth = amountToDrop;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public int GetAmountOfHealth()
    {
        return amountOfHealth;
    }
}
