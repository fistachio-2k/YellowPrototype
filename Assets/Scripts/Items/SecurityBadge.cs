using System;
using UnityEngine;

public class SecurityBadge : Interactable, IHideable, IHideable.IWearable
{
    public override Action[] CalcInteractions()
    {
        // there's place on the inventory
        if (!GameManager.Instance.inventory.IsInInventory(ItemType.SecurityBadge) && GameManager.Instance.inventory.CanAdd())
        {
            return new Action[] {PickUp};
        }
        
        // there isn't place on the inventory
        return new Action[] {};    
    }

    public void PickUp()
    {
        Debug.Log("Pickup");
        GameManager.Instance.inventory.AddItem(this);        
    }

}
