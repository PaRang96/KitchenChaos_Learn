using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private bool isCounterFull = false;

    public void Interact()
    {
        Debug.Log("Interact!");

        if (!isCounterFull)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.GetPrefab(), counterTopPoint);
            kitchenObjectTransform.localPosition = Vector3.zero;
            isCounterFull = true;
            Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO());
        }
        else
        {
            Debug.Log("Counter already full, cannot grab a new kitchenObjectSO");
        }
    }
}
