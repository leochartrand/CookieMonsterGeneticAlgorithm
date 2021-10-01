using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UGrid : MonoBehaviour
{
    private bool[,] Grid;
    private static int columns, rows;
    private Sprite GridSprite;
    private List<GameObject> NomList;
    private GameObject Agent;
    private int posRow, posCol;
    private Sprite MonsterSprite;
    private static int vertical, horizontal;
    
    
    public void createUGrid(bool[,] grid, int row, int col)
    {
        horizontal = 5; //(int)Camera.main.orthographicSize;
        vertical = 5; //(int)Camera.main.orthographicSize;

        Grid = grid;
        
        NomList = new List<GameObject>();

        rows = grid.GetLength(0);
        columns = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (Grid[i, j])
                {
                    SpawnNom(i, j);
                }
            }
        }
        
        Agent = new GameObject("Cookie Monster");
        SpawnAgent(row, col);
    }
    
    
    public void SpawnAgent(int row, int col)
    {
        Agent.transform.position = new Vector3(0.5f - horizontal + col, 0.5f - vertical + row, 2);
        var spt = Agent.AddComponent<SpriteRenderer>();
        spt.sprite = Resources.Load<Sprite>("monster");
        spt.sortingLayerName = "monster";
    }

    private void SpawnNom(int row, int col)
    {
        GameObject nom = new GameObject("[" + row + ", " + col + "]");
        NomList.Add(nom);
        nom.transform.parent = this.transform;
        nom.transform.position = new Vector3(col - (columns/2) + 0.5f, row - (rows/2) + 0.5f,  1);
        var spt = nom.AddComponent<SpriteRenderer>();
        spt.sprite = Resources.Load<Sprite>("nom");
        spt.sortingLayerName = "noms";
    }

    public void UpdateGrid(int row, int col)
    {
        Agent.transform.position = new Vector3(0.5f - horizontal + col, 0.5f - vertical + row, 2);
        turnOffNomSprite(row, col);
    }


    public void turnOffNomSprite(int row, int col)
    {
        GameObject nom = GameObject.Find("[" + row + ", " + col + "]");

        if (nom)
        {
            nom.GetComponent<SpriteRenderer>().enabled = false;   
        }
    }
    
    public void ClearNomList()
    {
        foreach (GameObject nom in NomList)
        {
            Destroy(nom);
        }
        NomList.Clear();
    }

    
    public void ClearGrid()
    {
        ClearNomList();
        
        Destroy(GameObject.Find("NomGrid"));
        
        Agent = null;
        Destroy(GameObject.Find("Cookie Monster"));
    }
}
