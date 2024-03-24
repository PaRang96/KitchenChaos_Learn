using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

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
                // Player is carrying a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestorySelf();
                    }
                }
                // Player is carrying something that is not a plate
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestorySelf();
                        }
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenOjbectParent(player);
            }
        }
    }
}
