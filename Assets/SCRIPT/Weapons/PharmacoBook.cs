using System;
using Unity.VisualScripting;
using UnityEngine;

public class PharmacoBook : Item
{
    [SerializeField] PlayerController player;
    private float tempecoule = 40;

    private void Update()
    {
        tempecoule += Time.deltaTime;
    }
    public override void Use()
    {
        if (itemScriptableObject.cooldown < tempecoule)
        {
            player.SetActualHealth(Convert.ToInt32(itemScriptableObject.damage)); //heal le player
            tempecoule = 0;
        }
        else
        {
            Debug.Log("Pharmaco Book is reloading"+ tempecoule );
        }
    }
}
