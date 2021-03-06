﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World
{
    // Chunks relative
    public IntVector3 chunkNumber = new IntVector3(8, 2, 8);                    // Initial number of chunks
    public IntVector3 chunkSize = new IntVector3(16, 16, 16);                   // Space occupied by a chunk
    public ChunkGenerator[, ,] chunk;                                           // Chunks declaration
    public List<IntVector3> chunksToReset = new List<IntVector3>();             // Control the chunks we are going to reset

    // GameObjects
    public GameObject worldObj;


    // Prefabs
    public Transform normalSlime;
    public Transform woodPieces;
    public Transform nails;


    // Control Variables
    public static bool gameLoaded = false;
    public static bool voxelsPrepared = false;
    private bool chunksCreated = false;
    private bool chunksFilled = false;

    private int chunksLoadedPerFrame = 12;
    private int chunksLoadedThisFrame = 0;

    // Materials relative
    public float textureSize;
    public int maxSediment;                                                     // Terrain height



    public World(GameObject world, Transform prefabs, Material atlas)
    {
        this.worldObj = world;
        chunk = new ChunkGenerator[chunkNumber.x, chunkNumber.y, chunkNumber.z];

        // Prefabs relative
        normalSlime = prefabs.FindChild("Normal Slime");
        woodPieces = prefabs.FindChild("Wood Pieces");
        nails = prefabs.FindChild("Nails");

        // Atlas relative
        textureSize = 128 / 1024.0f;
        maxSediment = 12;

        Init(atlas, textureSize);
    }


    public void Init(Material atlas, float textureSize)
    {
        //+ World setting
        worldObj.name = "World";

        // Set world transforms
        worldObj.transform.position = Vector3.zero;
        worldObj.transform.eulerAngles = Vector3.zero;
        worldObj.transform.localScale = Vector3.one;

        worldObj.AddComponent<GUISystem>();
        worldObj.AddComponent<HUD>();


        //+ World creation
        for(int cx = 0; cx < chunkNumber.x; cx++)
            for(int cy = 0; cy < chunkNumber.y; cy++)
                for (int cz = 0; cz < chunkNumber.z; cz++)
                    chunk[cx, cy, cz] = new ChunkGenerator(new IntVector3(cx, cy, cz), atlas);

            // Instantiates and Fills the chunks.
        for(int cx = 0; cx < chunkNumber.x; cx++)
            for(int cy = 0; cy < chunkNumber.y; cy++)
                for(int cz = 0; cz < chunkNumber.z; cz++)
                {
                    chunk[cx, cy, cz].InstantiateChunk(this);
                    chunk[cx, cy, cz].DefaultChunkFilling(this);    // Default map asigment
                }

            // Builds the vertices of the chunks if they have voxels inside, if not, set them as empty.
        for(int cx = 0; cx < chunkNumber.x; cx++)
            for(int cy = 0; cy < chunkNumber.y; cy++)
                for(int cz = 0; cz < chunkNumber.z; cz++)
                {
                    if (chunk[cx, cy, cz].empty == false)
                        chunk[cx, cy, cz].BuildChunkVertices(this);
                    else
                        chunk[cx, cy, cz].chunkObject.SetActive(false);
                }

            // Vuild the normals and the final mesh
        for(int cx = 0; cx < chunkNumber.x; cx++)
            for(int cy = 0; cy < chunkNumber.y; cy++)
                for(int cz = 0; cz < chunkNumber.z; cz++)
                    if (!chunk[cx, cy, cz].empty)
                    {
                        chunk[cx, cy, cz].BuildChunkNormals(this);
                        chunk[cx, cy, cz].BuildChunkMesh();
                    }
    }
}
