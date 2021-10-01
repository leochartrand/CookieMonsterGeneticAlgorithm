﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

// ReSharper disable SuggestVarOrType_BuiltInTypes
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Academy
{
    public class Academy
    {
        private List<Environment> environments;
        private List<Species> agents;
        private float epsilon;
        private float maxFitness;
        private float minFitness;
        private float meanFitness;
        private float SDLowFitness;
        private float SDHighFitness;
        private readonly int POPULATION;
        private readonly float NOMS_PER_ENV;
        private readonly int ENVS_PER_AGENT;
        private readonly float MUTATION_RATE;

        public Academy(int pop, int epa, float mr, float d)
        {
            POPULATION = pop;
            ENVS_PER_AGENT = epa;
            MUTATION_RATE = mr;
            NOMS_PER_ENV = d;
        }

        public void Setup()
        {
            epsilon = (float)0.01;
            
            agents = new List<Species>();
            environments = new List<Environment>();
            for (int i = 0; i < POPULATION; i++)
            {
                agents.Add(new Species(MUTATION_RATE));
                agents[i].GenerateRandomStrategy();
                agents[i].SetFitness(0);
                
                environments.Add(new Environment());
            }
            
        }
        
        public void NextGen()
        {
            Debug.Log("Best Fitness: " + agents[0].GetFitness() + "\n                Mean Fitness: " + meanFitness);
        
            NaturalSelection();
        
            Reproduce();
            
            epsilon -= (float)0.0001;
        }

        public void AgentTrialCPU(int agIndex)
        {
            float agentMeanFitness = 0;

            for (int i = 0; i < ENVS_PER_AGENT; i++)
            {
                environments[i].EnvSet(epsilon, agents[agIndex].GetStrategy(), NOMS_PER_ENV);          
                int score = environments[i].RunEnv();
                agentMeanFitness += score;
                environments[i].EnvReset();
            }

            agentMeanFitness /= ENVS_PER_AGENT;

            agents[agIndex].SetFitness(Mathf.RoundToInt(agentMeanFitness));
        }

        private void NaturalSelection()
        {
            /*
            int cptr = 0;
        
            while (cptr < POPULATION / 2)
            {
                int max = POPULATION - cptr;
            
                float a = Random.Range(0.0f, 1.0f);
                float b = (Mathf.Pow(Random.Range(-1.0f, 1.0f), 3 ) + 1)/2;
                if (a > b)
                {
                    int index = (int) Mathf.Floor(a * max);
                    agents.RemoveAt(index);
                    cptr++;
                }
            }*/
            agents.RemoveRange(POPULATION/2, POPULATION/2);
        }
    
        private void Reproduce()
        {
            List<Species> babies = agents;

            foreach (Species bb in babies)
            {
                bb.Mutate();
            }
        
            agents.AddRange(babies);
        }

        public void ExtractData()
        {
            agents.Sort((x,y) => y.GetFitness().CompareTo(x.GetFitness()));

            float mean = 0;
            foreach (Species ag in agents)
            {
                mean += ag.GetFitness();
            }
            meanFitness = mean/(POPULATION);

            maxFitness = agents[0].GetFitness();

            minFitness = agents[POPULATION-1].GetFitness();

            float sd = StandardDev(meanFitness);

            SDHighFitness = meanFitness + sd;

            SDLowFitness = meanFitness - sd;
        }

        private float StandardDev(float mean)
        {
            float sd = 0;

            foreach (Species ag in agents)
            {
                sd += (float)Math.Pow((ag.GetFitness() - mean), 2);
            }

            return (float)Math.Sqrt(sd / POPULATION);
        }

        public float GetMaxFit()
        {
            return maxFitness;
        }

        public float GetMinFit()
        {
            return minFitness;
        }
    
        public float GetMeanFit()
        {
            return meanFitness;
        }
    
        public float GetLowSD()
        {
            return SDLowFitness;
        }
    
        public float GetHighSD()
        {
            return SDHighFitness;
        }

        public string GetBestStrat()
        {
            return agents[0].GetStrategy();
        }

    }
}
