﻿using UnityEngine;
using System.Collections;

public class WoodenBridge : Geometry 
{
    public override void Init(World world, Player player, MainCamera mainCamera)
    {
        this.world = world;
        this.player = player;
        this.mainCamera = mainCamera;

        placedOn = PlacedOn.FLOOR;
    }



    public override void Place(string ID, Vector3 pos, Vector3 rot, Vector3 scale, bool firstPlacing)
    {
        if (firstPlacing)
        {
            rot.y += 90;
            pos += player.playerObj.transform.forward * 2.5f;
        }

        Transform woodenBridge = Transform.Instantiate(world.geometry.FindChild("Wooden Bridge"), pos, Quaternion.Euler(rot)) as Transform;

        woodenBridge.name = "Wooden Bridge 6m";
        woodenBridge.tag = "Geometry";

        woodenBridge.transform.localScale = scale;
        woodenBridge.transform.parent = world.geometryController.transform;
    }
}