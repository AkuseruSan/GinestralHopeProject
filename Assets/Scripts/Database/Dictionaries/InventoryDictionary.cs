﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryDictionary : MonoBehaviour 
{
    // Items
    Weapon weapon;
    Armor armor;
    Consumable consumable;
    GameComponent gameComponent;

    // Voxels
    Gadget gadget;
    

    void Start()
    {

    }

    void OnGUI()
    {
        // Show woodpieces
        gameComponent = GameComponentDictionary.GameComponentsDictionary["Wood Pieces"];
        GUI.Label(new Rect(Screen.width * 0.8f, Screen.height * 0.75f, 200, 50), gameComponent.nameKey + "  " + gameComponent.count);

        // Show wooden planks
        gadget = GadgetDictionary.GadgetsDictionary["Wooden Plank"];
        GUI.Label(new Rect(Screen.width * 0.8f, Screen.height * 0.80f, 200, 50), gadget.nameKey + "  " + gadget.count);
    }
}
