﻿using UnityEngine;
using System.Collections;

public class Torch : Gadget
{
    public override void Init(World world, Player player, MainCamera mainCamera, Gadget gadget)
    {
        this.world = world;
        this.player = player;
        this.mainCamera = mainCamera;

        ID = "Torch";

        size = new Vector3(1, 1, 1);
        count = 0;

        placedOnFloor = false;

        givesComponents = false;
        dropCount = 0;
    }


    public override void Place(string ID, Vector3 pos, Vector3 rotation)
    {
        Transform torch = world.nails;
        torch = Object.Instantiate(world.gadgets.FindChild("Torch"), pos, Quaternion.identity) as Transform;

        // Head atributes
        torch.name = "Torch";
        torch.tag = "Gadget";
        torch.transform.parent = world.gadgetsController.transform;

        torch.transform.eulerAngles = rotation;
    }
}
