using System.Collections;
using UnityEngine;

namespace Interface
{
    public class GraphManager : MonoBehaviour
    {
        public RectTransform Graph_Container;
        private GraphData Mean_Fitness;
        private GraphData Min_Fitness;
        private GraphData Max_Fitness;
        private GraphSD SD_Fitness;
        private int MaxValueY;

        public void InitGM()
        {
            init_Graphs();
            MaxValueY = 0;
        }

        private void init_Graphs()
        {
            Mean_Fitness = new GraphData(new Color(0.9f, 0.55f, 0.1f), 7, "Mean", Graph_Container);
            Min_Fitness = new GraphData(new Color(0.8f, 0.45f, 0.1f, 0.5f), 3, "Min", Graph_Container);
            Max_Fitness = new GraphData(new Color(0.8f, 0.45f, 0.1f, 0.5f), 3, "Max", Graph_Container);
            SD_Fitness = new GraphSD(new Color(0.6f, 0.44f, 0.08f, 0.1f), 1, "SD", Graph_Container);
        }

        private void UpdateGraphs()
        {
            SD_Fitness.ShowGraph();
            Max_Fitness.ShowGraph();
            Min_Fitness.ShowGraph();
            Mean_Fitness.ShowGraph();
        }

        public void InsertNewValues(float max, float min, float mean, float sdlow, float sdhigh)
        {
            if (max > MaxValueY) MaxValueY = Mathf.FloorToInt(max)+1;
        
            Max_Fitness.InsertNewValue(max,MaxValueY);
            Min_Fitness.InsertNewValue(min, MaxValueY);
            Mean_Fitness.InsertNewValue(mean, MaxValueY);
            SD_Fitness.InsertNewValue(sdhigh, sdlow, MaxValueY);
        
        
            UpdateGraphs();
        }
    
        public IEnumerator graphtest()
        {
            int top = 2;
            for (int i = 0; i < 50; i++)
            {
                int newnum = UnityEngine.Random.Range(Mathf.FloorToInt(top*0.5f), top);
                InsertNewValues(Mathf.FloorToInt(newnum*1.2f),Mathf.FloorToInt(newnum*0.8f),newnum, Mathf.FloorToInt(newnum*1.1f),Mathf.FloorToInt(newnum*0.9f));
                yield return new WaitForSecondsRealtime(1);
                top = Mathf.FloorToInt(top*1.5f);
            }

        }

    }
}
