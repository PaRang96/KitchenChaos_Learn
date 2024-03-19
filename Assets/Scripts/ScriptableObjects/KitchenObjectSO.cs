using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string objectName;

    public Transform GetPrefab()
    {
        return prefab;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
    public string ObjectName()
    {
        return objectName;
    }
}
