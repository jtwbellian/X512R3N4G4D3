using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Our Representation of an InventoryItem
[CreateAssetMenu(fileName = "Item", menuName = "Furniture", order = 0)]
public class ItemVariant : ScriptableObject
{

    public GameObject item;
    public string name;
 
}
 