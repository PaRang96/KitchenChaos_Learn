using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSO;

    private List<KitchenObjectSO> kitchenObjectSOList;

    public void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (validKitchenObjectSO.Contains(kitchenObjectSO))
        {
            // not a vliad ingredient
            return false;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        kitchenObjectSOList.Add(kitchenObjectSO);
        return true;
    }
}
