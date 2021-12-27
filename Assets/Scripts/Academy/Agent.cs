using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Random = System.Random;

public class Agent
{
    private int posRow;
    private int posCol;
    private int fitness;
    private readonly string strategy;

    public Agent(string newstrat)
    {
        strategy = newstrat;
        fitness = 0;
    }

    public void initPos()
    {
        Random rnd = new Random();
        posCol = rnd.Next(0, 10);
        posRow = rnd.Next(0, 10);
    }


    public void action(byte state, NomGrid nomGrid, float epsilon)
    {
        //Stochastic process to encourage exploration and test robustness
        //Epsilon starts at 0.01
        Random rnd = new Random();
        float r = (float)rnd.NextDouble();
        if (r < epsilon)
        {
            MoveRandom(state);
            return;
        }

        int i = state;
        string j = strategy;
        
        switch (Convert.ToByte(strategy[state]) - 48)
        {
            case 1:
                eat(state, nomGrid);
                break;
            
            case 2:
                MoveUp(state);
                break;
            
            case 3:
                MoveRight(state);
                break;
            
            case 4:
                MoveDown(state);
                break;
            
            case 5:
                MoveLeft(state);
                break;
            
            case 6:
                //Do nothing
                //Ideally natural selection should eliminate this gene variation
                //fitness--;
                break;
        }
    }

    private void eat(byte state, NomGrid nomGrid)
    {
        if (state >= 162)
        {
            Debug.LogError("Grid cell state error");
        }
        if (state >= 81)
        {
            nomGrid.eatMe(posRow, posCol);
            fitness += 500;
        }
        else
        {
            fitness -= 1;
        }
    }

    private void wallCrash()
    {
        fitness -= 5;
    }

    private void MoveUp(byte state)
    {
        if (state%81 >= 54)
        {
            wallCrash();
        }
        else
        {
            posRow++;
            //fitness--;
        }
    }
    
    private void MoveRight(byte state)
    {
        if (state%27 >= 18)
        {
            wallCrash();
        }
        else
        {
            posCol++;
            //fitness--;
        }
    }
    
    private void MoveDown(byte state)
    {
        if (state%9 >= 6)
        {
            wallCrash();
        }
        else
        {
            posRow--;
            //fitness--;
        }
    }
    
    private void MoveLeft(byte state)
    {
        if (state % 3 == 2) 
        {
            wallCrash();
        }
        else
        {
            posCol--;
            //fitness--;
        }
    }

    private void MoveRandom(byte state)
    {
        Random rnd = new Random();
        switch (rnd.Next(1, 5))
        {
            case 1:
                MoveUp(state);
                break;
            
            case 2:
                MoveRight(state);
                break;
            
            case 3:
                MoveDown(state);
                break;
            
            case 4:
                MoveLeft(state);
                break;
        }
    }

    public int GetRow()
    {
        return posRow;
    }
    
    public int GetCol()
    {
        return posCol;
    }

    public int GetFitness()
    {
        if (fitness < 0) fitness = 1;
        return fitness;
    }
}
