using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class Species
{
      
    private int fitness;
    private string genome;
    private float MutationRate;

    public Species(float mr)
    {
        MutationRate = mr;
    }

    public void Mutate()
    {
        StringBuilder mutatedGenome = new StringBuilder(genome);

        int rate = (int) (Mathf.Floor(Mathf.Tan((-Mathf.PI/2) * (Random.Range(0.0f, 1.0f) + 1.1f)) + 1.5f));

        for (int i = 0; i < 10; i++)
        {
            int index = Random.Range(0, genome.Length);
            mutatedGenome[index] = Convert.ToChar(Random.Range(1, 6) + 48);
        }

        genome = mutatedGenome.ToString();
    }

    public void GenerateRandomStrategy()
    {
        for (int i = 0; i < 162; i++)
        {
            genome += Random.Range(1, 6).ToString();
        }
    }

    public void SetStrategy(string str)
    {
        genome = str;
    }

    public void SetFitness(int newFitness)
    {
        fitness = newFitness;
    }
    
    public string GetStrategy()
    {
        return genome;
    }

    public int GetFitness()
    {
        return fitness;
    }
    
}
