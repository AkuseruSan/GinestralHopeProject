﻿using UnityEngine;
using System.Collections;

public class MainCamera
{
    public GameObject cameraObj;
    public CharacterController controller;


    // Camera movement
    public Quaternion rotation;
    public Vector3 offset;
    public Vector3 direction;

    public float objectivePosition;
    public float interpolatedPosition;

    public float angleSight;


    // Camera raycast
    public RaycastHit raycast;


    // Camera sensitivity
    public float mouseSensitivityX = 5;
    public float mouseSensitivityY = 5;
    public float rotationSensitivity = 0.2f;

    // Movement limits
    public float minAngleSight = -90;
    public float maxAngleSight = 90;
    public float distance = 3.5f;


    public MainCamera()
    {
        Init();
    }


    private void Init()
    {
        //+ Main camera creation
        cameraObj = new GameObject();

        cameraObj.name = "MainCamera";
        cameraObj.tag = "MainCamera";

        // Camera components creation
        cameraObj.AddComponent<Camera>();
        controller = cameraObj.AddComponent<CharacterController>();

        cameraObj.AddComponent("FlareLayer");
    }
}
