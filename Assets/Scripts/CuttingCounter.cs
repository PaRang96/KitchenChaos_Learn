using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenOjbect

            if (player.HasKitchenObject())
            {
                // Player is carrying KitchenOjbect
                player.GetKitchenObject().SetKitchenOjbectParent(this);
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // There is KitchenOjbect
            if (player.HasKitchenObject())
            {
                // Player is carrying something
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenOjbectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            // Cut existing KitchenObject
            GetKitchenObject().DestorySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
