﻿using UnityEngine;
using System.Collections;

public class DevConstructionSkillsManager
{
    // Detecting variables
    // Base detections
    public static ChunkGenerator chunk;
    public static Voxel voxel;

    // Auxiliar
    public static ChunkGenerator detChunk;
    public static Voxel detVoxel;
    public static Vector2 detVertex;

    // Multi selection tools
    public static bool selected = false;

    // Number of the vertex: 0 - backLeft, 1 - backRight, 2 - frontRight, 3 - frontLeft
    private static int voxelVertex;


    // Math variables
    private static int sedimentExcess;
    private static int sedimentPerClick = 3;

    public void Update(World world, Player player, MainCamera mainCamera)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Remove(world, player, mainCamera);
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            Place(world, player, mainCamera);
        else if (Input.GetKeyDown(KeyCode.Mouse2))
            Cancel();
        else
            Detect(world, player, mainCamera);
    }


    private void Remove(World world, Player player, MainCamera mainCamera)
    {
        if (EGameFlow.selectedTool == EGameFlow.SelectedTool.TERRAIN)
            TerrainVoxelsToolManager.Remove(world, player, mainCamera);
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.MINE)
            MineVoxelsToolManager.Remove(world, player, mainCamera);
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.GADGET)
            GadgetsToolManager.Remove(player, mainCamera);
		else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.ENEMY)
			EnemiesToolManager.Remove();
    }


    private void Place(World world, Player player, MainCamera mainCamera)
    {
        if (EGameFlow.selectedTool == EGameFlow.SelectedTool.TERRAIN)
            TerrainVoxelsToolManager.Place(world, player, mainCamera);
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.MINE)
            MineVoxelsToolManager.Place(world, player, mainCamera);
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.GADGET)
            GadgetsToolManager.Place(world, player, mainCamera);
		else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.ENEMY)
            EnemiesToolManager.Place(world, player, mainCamera);
    }


    private void Cancel()
    {
        if (EGameFlow.selectedTool == EGameFlow.SelectedTool.TERRAIN)
            TerrainVoxelsToolManager.Cancel();
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.MINE)
            MineVoxelsToolManager.Cancel();
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.GADGET)
            GadgetsToolManager.Cancel();
		else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.ENEMY)
			EnemiesToolManager.Cancel();
    }


    private void Detect(World world, Player player, MainCamera mainCamera)
    {
        if (EGameFlow.selectedTool == EGameFlow.SelectedTool.TERRAIN)
            TerrainVoxelsToolManager.Detect(world, player, mainCamera);
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.MINE)
            MineVoxelsToolManager.Detect(world, player, mainCamera);
        else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.GADGET)
            GadgetsToolManager.Detect(world, player, mainCamera);
		else if (EGameFlow.selectedTool == EGameFlow.SelectedTool.ENEMY)
            EnemiesToolManager.Detect(world, player, mainCamera);
    }
}
