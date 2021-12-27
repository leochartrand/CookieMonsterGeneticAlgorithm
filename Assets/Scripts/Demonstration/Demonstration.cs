using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Environment = Academy.Environment;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;


public class Demonstration : MonoBehaviour
{
    private GameObject UEnv;
    private UGrid ugrid;
    private Environment environment;
    private string strategy;
    private float epsilon;

    public void Setup(float ep, float NPE, string strat)
    {
        epsilon = ep;
        strategy = strat;
        
        environment = new Environment();
        environment.EnvSet(epsilon, strategy, NPE);
         
        UEnv = new GameObject("Environment");
        ugrid = UEnv.AddComponent<UGrid>();
        ugrid.createUGrid(environment.GetNomGrid(), environment.GetAgentRow(), environment.GetAgentCol());
    }

    public void NextStep()
    {
        if (environment.isEmpty()) {
            GameObject.Find("Next Step").SetActive(false);
        }
        environment.Step();
        ugrid.UpdateGrid(environment.GetAgentRow(), environment.GetAgentCol());
    }


    public void Quit()
    {
        ugrid.ClearGrid();
        environment.EnvReset();
        Destroy(UEnv);
        SceneManager.UnloadScene("Demonstration");
        GameObject gsim = Array.Find(SceneManager.GetSceneByName("Simulation").GetRootGameObjects(), go => go.name == "Simulator");
        gsim.SetActive(true);
        gsim.GetComponent<Simulator>().StartSimulation();
    }
}
