﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[System.Serializable]
public struct VoxelStruct
{
    public string name;
    public int number;
}


[System.Serializable]
public struct PlayerStruct
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public bool unlockedSkillFireBall;
}


[System.Serializable]
public struct EmitterStruct
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public float intensity;
    public float range;
    public float r;
    public float g;
    public float b;
}


[System.Serializable]
public struct EnemyStruct
{
    public string ID;
    public float positionX;
    public float positionY;
    public float positionZ;
}


[System.Serializable]
public struct GeometryStruct
{
    public string ID;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
}


[System.Serializable]
public struct InteractiveStruct
{
    public string ID;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
}


[System.Serializable]
public struct EventStruct
{
    public string ID;
    public float positionX;
    public float positionY;
    public float positionZ;
}



[System.Serializable]
public class GameSerializer
{
    private VoxelStruct voxelStructSave;
    private PlayerStruct playerStruct;
    private EmitterStruct emitterStruct;
    private GeometryStruct geometryStruct;
    private InteractiveStruct interactiveStruct;
    private EnemyStruct enemyStruct;
    private EventStruct eventStruct;


    private static List<VoxelStruct> VoxelSave;
    private static List<EmitterStruct> EmitterSave;
    private static List<GeometryStruct> GeometrySave;
    private static List<InteractiveStruct> InteractiveSave;
    private static List<EnemyStruct> EnemySave;
    private static List<EventStruct> EventSave;

    private static string actualName;
    private static int actualNumber;


    public void Save(World world, Player player, string saveName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create("Assets/Saves/" + saveName + ".save");

        actualNumber = 1;

        VoxelSave = new List<VoxelStruct>();
        EmitterSave = new List<EmitterStruct>();
        GeometrySave = new List<GeometryStruct>();
        InteractiveSave = new List<InteractiveStruct>();
        EnemySave = new List<EnemyStruct>();
        EventSave = new List<EventStruct>();

        Encryptor(world, player, bf, file);
        file.Close();
    }


    public void Load(World world, Player player, string saveName)
    {
        if (File.Exists("Assets/Saves/" + saveName + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("Assets/Saves/" + saveName + ".save", FileMode.Open);

            actualNumber = 1;

            VoxelSave = new List<VoxelStruct>();
            EmitterSave = new List<EmitterStruct>();
            GeometrySave = new List<GeometryStruct>();
            InteractiveSave = new List<InteractiveStruct>();
            EnemySave = new List<EnemyStruct>();
            EventSave = new List<EventStruct>();

            Desencrypter(world, player, bf, file);
            file.Close();
        }
    }


    private void Encryptor(World world, Player player, BinaryFormatter bf, FileStream file)
    {
        //+ Player
        // Find all players in game
        GameObject playerOnGame = GameObject.FindGameObjectWithTag("Player");

        playerStruct.positionX = playerOnGame.transform.position.x;
        playerStruct.positionY = playerOnGame.transform.position.y + 0.05f;
        playerStruct.positionZ = playerOnGame.transform.position.z;
        playerStruct.rotationX = playerOnGame.transform.eulerAngles.x;
        playerStruct.rotationY = playerOnGame.transform.eulerAngles.y;
        playerStruct.rotationZ = playerOnGame.transform.eulerAngles.z;
        playerStruct.unlockedSkillFireBall = player.unlockedSkillFireBall;

        bf.Serialize(file, playerStruct);


        //+ Voxels
        bool firstVoxel = true;

        for (int cx = 0; cx < world.chunkNumber.x; cx++)
            for (int cy = 0; cy < world.chunkNumber.y; cy++)
                for (int cz = 0; cz < world.chunkNumber.z; cz++)
                    if (!world.chunk[cx, cy, cz].empty)
                        for (int x = 0; x < world.chunkSize.x; x++)
                            for (int y = 0; y < world.chunkSize.y; y++)
                                for (int z = 0; z < world.chunkSize.z; z++)
                                {
                                    if (firstVoxel)
                                    {
                                        firstVoxel = false;
                                        actualName = world.chunk[cx, cy, cz].voxel[x, y, z].ID;
                                    }
                                    else
                                    {
                                        if (actualName == world.chunk[cx, cy, cz].voxel[x, y, z].ID)
                                            actualNumber++;
                                        else
                                        {
                                            if (actualName == "Sand Way")
                                                actualName = "P1(0, 7)";
                                            else if (actualName == "Little Rock")
                                                actualName = "P1(0, 6)";
                                            else if (actualName == "Large Rock")
                                                actualName = "P1(1, 6)";

                                            voxelStructSave.name = actualName;
                                            voxelStructSave.number = actualNumber;
                                            VoxelSave.Add(voxelStructSave);

                                            actualNumber = 1;
                                            actualName = world.chunk[cx, cy, cz].voxel[x, y, z].ID;
                                        }
                                    }
                                }

        // The last struct is not detected in the loop
        voxelStructSave.name = actualName;
        voxelStructSave.number = actualNumber;
        VoxelSave.Add(voxelStructSave);

        // Serialization of the lists
        bf.Serialize(file, VoxelSave);
        VoxelSave.Clear();


        //+ Emitters
        // Find all emiters in game
        GameObject[] emittersInGame = GameObject.FindGameObjectsWithTag("Emiter");

        foreach (GameObject emitterObj in emittersInGame)
        {
            emitterStruct.positionX = emitterObj.transform.position.x;
            emitterStruct.positionY = emitterObj.transform.position.y;
            emitterStruct.positionZ = emitterObj.transform.position.z;
            emitterStruct.intensity = emitterObj.light.intensity;
            emitterStruct.range = emitterObj.light.range;
            emitterStruct.r = emitterObj.light.color.r;
            emitterStruct.g = emitterObj.light.color.g;
            emitterStruct.b = emitterObj.light.color.b;
            EmitterSave.Add(emitterStruct);
        }
        bf.Serialize(file, EmitterSave);
        EmitterSave.Clear();


        //+ Geometry
        // Find all geometries in game
        GameObject[] geometryInGame = GameObject.FindGameObjectsWithTag("Geometry");

        foreach (GameObject geometry in geometryInGame)
        {
            geometryStruct.ID = geometry.name;
            geometryStruct.positionX = geometry.transform.position.x;
            geometryStruct.positionY = geometry.transform.position.y;
            geometryStruct.positionZ = geometry.transform.position.z;
            geometryStruct.rotationX = geometry.transform.eulerAngles.x;
            geometryStruct.rotationY = geometry.transform.eulerAngles.y;
            geometryStruct.rotationZ = geometry.transform.eulerAngles.z;
            geometryStruct.scaleX = geometry.transform.localScale.x;
            geometryStruct.scaleY = geometry.transform.localScale.y;
            geometryStruct.scaleZ = geometry.transform.localScale.z;
            GeometrySave.Add(geometryStruct);
        }
        bf.Serialize(file, GeometrySave);
        GeometrySave.Clear();


        //+ Gadgets
        // Find all gadgets in game
        GameObject[] interactivesInGame = GameObject.FindGameObjectsWithTag("Interactive");

        foreach (GameObject interactive in interactivesInGame)
        {
            interactiveStruct.ID = interactive.name;
            interactiveStruct.positionX = interactive.transform.position.x;
            interactiveStruct.positionY = interactive.transform.position.y;
            interactiveStruct.positionZ = interactive.transform.position.z;
            interactiveStruct.rotationX = interactive.transform.eulerAngles.x;
            interactiveStruct.rotationY = interactive.transform.eulerAngles.y;
            interactiveStruct.rotationZ = interactive.transform.eulerAngles.z;
            InteractiveSave.Add(interactiveStruct);
        }
        bf.Serialize(file, InteractiveSave);
        InteractiveSave.Clear();


        //+ Enemies
        // Find all enemies in game
        GameObject[] enemiesInGame = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemiesInGame)
        {
            enemyStruct.ID = enemy.name;
            enemyStruct.positionX = enemy.transform.position.x;
            enemyStruct.positionY = enemy.transform.position.y;
            enemyStruct.positionZ = enemy.transform.position.z;
            EnemySave.Add(enemyStruct);
        }
        bf.Serialize(file, EnemySave);
        EnemySave.Clear();


        //+ Events
        // Find all events in game
        GameObject[] eventsInGame = GameObject.FindGameObjectsWithTag("Event");

        foreach (GameObject eventObj in eventsInGame)
        {
            eventStruct.ID = eventObj.name;
            eventStruct.positionX = eventObj.transform.position.x;
            eventStruct.positionY = eventObj.transform.position.y;
            eventStruct.positionZ = eventObj.transform.position.z;
            EventSave.Add(eventStruct);
        }
        bf.Serialize(file, EventSave);
        EventSave.Clear();
    }


    private void Desencrypter(World world, Player player, BinaryFormatter bf, FileStream file)
    {
        //+ Player
        // Deserialize the players
        playerStruct = (PlayerStruct)bf.Deserialize(file);

        GameObject playerOnGame = GameObject.FindGameObjectWithTag("Player");

        playerOnGame.transform.position = new Vector3(playerStruct.positionX, playerStruct.positionY, playerStruct.positionZ);
        playerOnGame.transform.eulerAngles = new Vector3(playerStruct.rotationX, playerStruct.rotationY, playerStruct.rotationZ);
        player.unlockedSkillFireBall = playerStruct.unlockedSkillFireBall;


        //+ Voxels
        // Deserialize the emiters list
        VoxelSave = (List<VoxelStruct>)bf.Deserialize(file);

        int listPosition = -1;
        int downCounter = 0;

        for (int cx = 0; cx < world.chunkNumber.x; cx++)
            for (int cy = 0; cy < world.chunkNumber.y; cy++)
                for (int cz = 0; cz < world.chunkNumber.z; cz++)
                    if (!world.chunk[cx, cy, cz].empty)
                        for (int x = 0; x < world.chunkSize.x; x++)
                            for (int y = 0; y < world.chunkSize.y; y++)
                                for (int z = 0; z < world.chunkSize.z; z++)
                                {
                                    if (downCounter > 0)
                                    {
                                        world.chunk[cx, cy, cz].voxel[x, y, z] =
                                            new Voxel(world, new IntVector3(x, y, z), new IntVector3(cx, cy, cz), VoxelSave[listPosition].name);

                                        downCounter--;
                                    }
                                    else
                                    {
                                        listPosition++;
                                        downCounter = VoxelSave[listPosition].number - 1;

                                        world.chunk[cx, cy, cz].voxel[x, y, z] =
                                            new Voxel(world, new IntVector3(x, y, z), new IntVector3(cx, cy, cz), VoxelSave[listPosition].name);
                                    }   
                                }


        VoxelSave.Clear();

        // Reset voxels
        foreach (Chunk chunk in world.chunk)
        {
            chunk.BuildChunkVertices(world);
            chunk.BuildChunkMesh();
        }


        //+ Emitters
        // Deserialize the emiters list
        EmitterSave = (List<EmitterStruct>)bf.Deserialize(file);

        // Destroy existing emiters
        GameObject[] emitters = GameObject.FindGameObjectsWithTag("Emiter");
        foreach (GameObject emitterObj in emitters)
            GameObject.Destroy(emitterObj);

        // Load emiters
        foreach (EmitterStruct emitterObj in EmitterSave)
            Emiter.Place(world, new Vector3(emitterObj.positionX, emitterObj.positionY, emitterObj.positionZ),
                                    emitterObj.intensity, emitterObj.range, emitterObj.r, emitterObj.g, emitterObj.b);

        // Reset emiters list
        EmitterSave.Clear();


        //+ Geometry
        // Deserialize the gadgets list
        GeometrySave = (List<GeometryStruct>)bf.Deserialize(file);

        // Destroy existing gadgets
        GameObject[] geometries = GameObject.FindGameObjectsWithTag("Geometry");
        foreach (GameObject geometry in geometries)
            GameObject.Destroy(geometry);

        // Load gadgets
        foreach (GeometryStruct geometry in GeometrySave)
            Geometry.Dictionary[geometry.ID].Place(geometry.ID,
                new Vector3(geometry.positionX, geometry.positionY, geometry.positionZ), 
                new Vector3(geometry.rotationX, geometry.rotationY, geometry.rotationZ),
                new Vector3(geometry.scaleX, geometry.scaleY, geometry.scaleZ), false);

        //Reset gadgets list
        GeometrySave.Clear();


        //+ Interactives
        // Deserialize the interactives list
        InteractiveSave = (List<InteractiveStruct>)bf.Deserialize(file);

        // Destroy existing interactives
        GameObject[] interactives = GameObject.FindGameObjectsWithTag("Interactive");
        foreach (GameObject interactive in interactives)
            GameObject.Destroy(interactive);

        // Load interactives
        foreach (InteractiveStruct interactive in InteractiveSave)
        {
            Interactive.Dictionary[interactive.ID].Place(interactive.ID,
                new Vector3(interactive.positionX, interactive.positionY, interactive.positionZ), new Vector3(interactive.rotationX, interactive.rotationY, interactive.rotationZ), false);
        }

        //Reset interactives list
        InteractiveSave.Clear();


        //+ Enemies
        // Deserialize the enemies list
        EnemySave = (List<EnemyStruct>)bf.Deserialize(file);

        // Destroy existing enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);

        // Load enemies
        foreach (EnemyStruct enemy in EnemySave)
            Enemy.Dictionary[enemy.ID]
                .Place(new Vector3(enemy.positionX - 0.5f, enemy.positionY, enemy.positionZ - 0.5f));

        // Reset enemies list
        EnemySave.Clear();


        //+ Events
        // Deserialize the events list
        EventSave = (List<EventStruct>)bf.Deserialize(file);

        // Destroy existing events
        GameObject[] events = GameObject.FindGameObjectsWithTag("Event");
        foreach (GameObject eventObj in events)
            GameObject.Destroy(eventObj);

        // Load events
        foreach (EventStruct eventObj in EventSave)
            Event.Place(world, player, eventObj.ID, new Vector3(eventObj.positionX, eventObj.positionY, eventObj.positionZ));

        // Reset events list
        EventSave.Clear();
    }
}
