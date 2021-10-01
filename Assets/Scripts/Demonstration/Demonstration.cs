using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Environment = Academy.Environment;
using Random = UnityEngine.Random;


public class Demonstration : MonoBehaviour
{
    private GameObject UEnv;
    private UGrid ugrid;
    private Environment environment;
    private string strategy;
    private float epsilon;

    private GameObject DemoObject;
    public void Setup(float ep, float NPE, string strat)
    {
        DemoObject = GameObject.Find("DemoActivator");

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
        environment.Step();
        ugrid.UpdateGrid(environment.GetAgentRow(), environment.GetAgentCol());
    }


    public void Quit()
    {
        ugrid.ClearGrid();
        environment.EnvReset();
        Destroy(UEnv);
        DemoObject.SetActive(false);
    }
}
