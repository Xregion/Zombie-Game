using UnityEngine;

public class AmmoPack : MonoBehaviour, IDroppable {

    int amountOfBullets;

    public void SetDropAmount(int amountToDrop)
    {
        amountOfBullets = amountToDrop;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public int GetAmountOfBullets()
    {
        return amountOfBullets;
    }
}
