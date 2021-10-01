using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class GraphData
{
    private List<float> FitnessValues;
    [SerializeField] private Sprite nodeSprite;
    private readonly RectTransform Graph_Container;
    private readonly Color color;
    private readonly int thickness;
    private readonly string name;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private int YMaximum;
    private List<GameObject> Objects;
    

    public GraphData(Color newcolor, int thicc, string newname, RectTransform container)
    {
        Graph_Container = container;
        color = newcolor;
        thickness = thicc;
        name = newname;
        FitnessValues = new List<float>() {0};
        YMaximum = 0;
        Objects = new List<GameObject>();
        nodeSprite = Resources.Load<Sprite>("node");
        
        labelTemplateX = Graph_Container.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = Graph_Container.Find("labelTemplateY").GetComponent<RectTransform>();
    }
    
    public void InsertNewValue(float newValue, int newMax)
    {
        FitnessValues.Add(newValue);
        YMaximum = newMax;
    }
    
    private GameObject CreateNode(Vector2 anchoredPosition)
    {
        GameObject go = new GameObject(name+" node", typeof(Image));
        Objects.Add(go);
        go.transform.SetParent(Graph_Container, false);
        go.GetComponent<Image>().sprite = nodeSprite;
        go.GetComponent<Image>().color = color;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(thickness, thickness);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return go;
    }

    private void LinkNodes(Vector2 PrevNode, Vector2 NextNode)
    {
        GameObject go = new GameObject(name+" bar", typeof(Image));
        Objects.Add(go);
        go.transform.SetParent(Graph_Container, false);
        go.GetComponent<Image>().color = color;
        RectTransform rectTransform = go.GetComponent<RectTransform>();

        Vector2 norm = (NextNode - PrevNode).normalized;
        float distance = Vector2.Distance(PrevNode, NextNode);

        rectTransform.sizeDelta = new Vector2(distance, thickness);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = PrevNode + (norm * (distance * .5f));
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg);
    }
    
    
    // Func<int, int> getAxisLabelx = null, Func<int, int> getAxisLabely = null
    // ReSharper disable Unity.PerformanceAnalysis
    public void ShowGraph()
    {
        ResetGraph();
        
        Vector2 GraphSize = Graph_Container.sizeDelta;
        float graphHeight = GraphSize.y;
        float MaxY = YMaximum*1.1f;
        float graphWidth = GraphSize.x;
        float SizeX = graphWidth / (FitnessValues.Count-1);
        
        GameObject lastNode = null;
        for (int i = 0; i < FitnessValues.Count; i++)
        {
            float PosX = i * SizeX;
            float PosY = (FitnessValues[i] / MaxY) * graphHeight;
            GameObject newNode = CreateNode(new Vector2(PosX, PosY));
            if (lastNode != null)
            {
                LinkNodes(lastNode.GetComponent<RectTransform>().anchoredPosition,
                    newNode.GetComponent<RectTransform>().anchoredPosition);
            }

            lastNode = newNode;


            if (FitnessValues.Count < 10)
            {
                RectTransform labelX = UnityEngine.Object.Instantiate
                    (labelTemplateX, Graph_Container, false);
                Objects.Add(labelX.gameObject);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(PosX, -20f);
                labelX.GetComponent<Text>().text = i.ToString();
            }
            
            if (MaxY < 10)
            {
                RectTransform labelY = UnityEngine.Object.Instantiate
                    (labelTemplateY, Graph_Container, false);
                Objects.Add(labelY.gameObject);
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(-20f, PosY);
                labelY.GetComponent<Text>().text = ((int)FitnessValues[i]).ToString();
            }
        }

        if (FitnessValues.Count >= 10)
        {
            int X_Label_Count = 10;
            for (int i = 0; i < X_Label_Count; i++)
            {
                RectTransform labelX = UnityEngine.Object.Instantiate
                    (labelTemplateX, Graph_Container, false);
                Objects.Add(labelX.gameObject);
                labelX.gameObject.SetActive(true);
                float normalizedValue = (i * 1f) / (X_Label_Count - 1);
                labelX.anchoredPosition = new Vector2(normalizedValue * graphWidth, -20f);
                labelX.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * (FitnessValues.Count-1)).ToString();
            }
        }
        
        if (MaxY >= 10)
        {
            int Y_Label_Count = 10;
            for (int i = 0; i < Y_Label_Count; i++)
            {
                RectTransform labelY = UnityEngine.Object.Instantiate
                    (labelTemplateY, Graph_Container, false);
                Objects.Add(labelY.gameObject);
                labelY.gameObject.SetActive(true);
                float normalizedValue = (i * 1f) / (Y_Label_Count - 1);
                labelY.anchoredPosition = new Vector2(-20f, normalizedValue * graphHeight);
                labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * (MaxY - 1)).ToString();
            }
        }
    }

    private void ResetGraph()
    {
        foreach (GameObject go in Objects)
        {
            UnityEngine.Object.Destroy(go);
        }
        Objects.Clear();
    }

}