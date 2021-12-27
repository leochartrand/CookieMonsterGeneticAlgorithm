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
        GenerateRandomStrategy();
        fitness = 0;
    }

    public Species Reproduce() 
    {
        Species clone = new Species(this.MutationRate);
        clone.SetFitness(this.fitness);
        clone.SetStrategy(this.genome);
        clone.Mutate();
        return clone;
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

    // Private Methods
    private void Mutate()
    {
        StringBuilder mutatedGenome = new StringBuilder(genome);

        // int rate = (int) (Mathf.Floor((Mathf.Tan((-Mathf.PI/2) * (Random.Range(0.0f, 1.0f) + 1.1f))/4 + 1.1f)*MutationRate*genome.Length));
        int rate = (int) (Mathf.Floor(MutationRate*genome.Length));

        for (int i = 0; i < rate; i++)
        {
            int index = Random.Range(0, genome.Length);
            mutatedGenome[index] = Convert.ToChar(Random.Range(1, 7) + 48);
        }

        genome = mutatedGenome.ToString();
    }

    private void GenerateRandomStrategy()
    {
        for (int i = 0; i < 162; i++)
        {
            genome += Random.Range(1, 7).ToString();
        }
    }
    
}
