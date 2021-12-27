using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Main;
    public GameObject Config;
    private Simulator sim;
    
    private void Start()
    {
        Main.SetActive(true);
        Config.SetActive(false);
        
        init_Pop();
        init_EPA();
        init_GPU();
        init_MR();
        init_FD();
    }
    
    public void StartSim()
    {
        SceneManager.LoadScene("Simulation");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        if (scene.name == "Simulation") {
            sim = GameObject.Find("Simulator").GetComponent<Simulator>();
            sim.InitSim(population, Env_per_Agent, Mutation_rate, density);
        }
        else if (scene.name == "Demonstration") {
            sim.SetupDemo();
        }
    }
    
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    //==================================================================================================================
    //Pop
    private int population;
    private int pop_last_input;         // variable that stores input value while GPU_Acc is enabled
    private int pop_last_dd;            // variable that stores dropdown value while GPU_Acc is disabled
    public Dropdown Pop_dd;
    public GameObject popInfo;          // text that informs the user about the population size range
    public GameObject Pop_DropDown;
    public GameObject Pop_InputField;

    private void init_Pop()
    {
        population = 256;
        pop_last_input = 256;
        pop_last_dd = 256;
        Pop_dd.GetComponent<Dropdown>();
        Pop_dd.value = 0;
    }
    
    public void SetPopulation_Input(string popsize)
    {
        int temp = int.Parse(popsize);
        if (temp < 1 || temp > 8192) {throw new ArgumentException();}
        else
        {
            pop_last_input = temp;
            population = temp;
            popInfo.SetActive(false);
            Debug.Log("Population = "+ population);
        }
    }
    
    public void SetPopulation_Dropdown(int index)
    {
        population = 1024 * (int)Math.Pow(2, index);
        pop_last_dd = population;
        Debug.Log("Population = "+ population);
    }
    
    //==================================================================================================================
    //Env per agent
    private int Env_per_Agent;
    private int EPA_last_input;
    private int EPA_last_dd;
    public Dropdown EPA_dd;
    public GameObject EPA_info;
    public GameObject EPA_DropDown;
    public GameObject EPA_InputField;

    private void init_EPA()
    {
        Env_per_Agent = 16;
        EPA_last_input = 16;
        EPA_last_dd = 16;
        EPA_dd.GetComponent<Dropdown>();
        EPA_dd.value = 4;
    }
    public void SetEPA_Input(string input)
    {
        int test = int.Parse(input);
        if (test < 1 || test > 65) {throw new ArgumentException();}
        else
        {
            EPA_last_input = test;
            Env_per_Agent = test;
            EPA_info.SetActive(false);
            Debug.Log("EPA = "+ Env_per_Agent);
        }
    }
    
    public void SetEPA_Dropdown(int index)
    {
        Env_per_Agent = (int)Math.Pow(2, index);
        EPA_last_dd = Env_per_Agent;
        Debug.Log("EPA = "+ Env_per_Agent);
    }
    
    //==================================================================================================================
    //GPU
    private bool GPU_Enabled;
    public GameObject Toggle_BG;
    public GameObject Toggle_txt;

    private void init_GPU()
    {
        GPU_Enabled = false;
        Pop_DropDown.SetActive(false);
        Pop_InputField.SetActive(true);
        Toggle_BG.SetActive(false);
        Toggle_txt.SetActive(false);
    }
    
    public void ToggleGPU(bool value)
    {
        GPU_Enabled = value;
        
        population = value ? pop_last_dd : pop_last_input;
        Debug.Log("Population = "+ population);
        
        Pop_DropDown.SetActive(value);
        Pop_InputField.SetActive(!value);
        popInfo.SetActive(!value);
        
        
        Env_per_Agent = value ? EPA_last_dd : EPA_last_input;
        Debug.Log("EPA = "+ Env_per_Agent);
        
        EPA_DropDown.SetActive(value);
        EPA_InputField.SetActive(!value);
        EPA_info.SetActive(!value);
        
        
        Toggle_BG.SetActive(value);
        Toggle_txt.SetActive(value);
    }
    
    //==================================================================================================================
    //Mutation
    private float Mutation_rate;
    public Text M_slider_Text;

    private void init_MR()
    {
        Mutation_rate = 0.05f;
        M_slider_Text.text = Mathf.RoundToInt(Mutation_rate * 100) + "%";
    }
    
    public void Set_Mutation_Rate(float value)
    {
        Mutation_rate = value;
        M_slider_Text.text = Mathf.RoundToInt(value * 100) + "%";
        Debug.Log("M_rate = "+ Mutation_rate);
    }

    //==================================================================================================================
    //Food density
    private float density;
    public Text FD_silder_Text;

    private void init_FD()
    {
        density = 0.2f;
        FD_silder_Text.text = Mathf.RoundToInt(density * 100) + "%";
    }
    
    public void Set_Density(float value)
    {
        density = value;
        FD_silder_Text.text = Mathf.RoundToInt(value * 100) + "%";
        Debug.Log("Density = "+ density);
    }
}
