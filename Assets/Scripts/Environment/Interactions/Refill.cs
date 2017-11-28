using UnityEngine;

public class Refill : InspectableObject {

    public enum RefillType { HEALTH, AMMO };

    public RefillType type;
    public int refillAmount;

    protected override void Interact()
    {
        GameObject player = LoadManager.instance.GetPlayer();
        if (type == RefillType.HEALTH)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController.GetCurrentHealth() != playerController.totalHealth)
                player.GetComponent<PlayerController>().Heal(refillAmount);
            else
                inspectionText = "You are too full to eat anymore.";
        }
        else if (type == RefillType.AMMO)
        {
            player.GetComponent<GunController>().totalBulletsRemaining += refillAmount;
        }
        base.Interact();
    }
}
