using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class NomGrid
{

    private bool[,] Grid;
    private static int columns, rows;
    private int nomCount;

    public NomGrid()
    {
        columns = 10;
        rows = 10;
        Grid = new bool[rows, columns];
        nomCount = 0;
    }

    public void initGrid(float NPE)
    {
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (rnd.Next(0, 100) < NPE*100)
                {
                    Grid[i, j] = true;
                    nomCount++;
                }
                else
                {
                    Grid[i, j] = false;
                }
            }
        }
    }

    private byte getValueHere(int row, int col)
    {
        bool value;
        
        try
        {
            value = Grid[row, col];
        }
        catch (IndexOutOfRangeException)
        {
            return 2;
        }

        if (value) return 1;
        
        else return 0;
    }

    public byte getState(int row, int col)
    {
        byte state = 0;
        //here
        state += (byte)(getValueHere(row, col) * 81);
        //up
        state += (byte)(getValueHere(row + 1, col) * 27);
        //right
        state += (byte)(getValueHere(row, col + 1) * 9);
        //down
        state += (byte)(getValueHere(row - 1, col) * 3);
        //left
        state += getValueHere(row, col - 1);
        
        return state;
    }
   
    public int getNomCount()
    {
        return nomCount;
    }

    public bool[,] getGrid()
    {
        return Grid;
    }
    
    public void eatMe(int row, int col)
    {
        Grid[row, col] = false;
        nomCount--;
    }
}
