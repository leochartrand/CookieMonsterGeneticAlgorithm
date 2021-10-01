using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class GraphSD
    {
        private List<float> Top;
        private List<float> Bottom;
        [SerializeField] private Sprite nodeSprite;
        private readonly RectTransform Graph_Container;
        private readonly Color color;
        private readonly int thickness;
        private string name;
        private int YMaximum;
        private List<GameObject> Objects;
        private readonly Vector2 GraphSize;
        private Material material;
    

        public GraphSD(Color newcolor, int thicc, string newname, RectTransform container)
        {
            Graph_Container = container;
            GraphSize = Graph_Container.sizeDelta;
            color = newcolor;
            name = newname;
            Top = new List<float>() {0};
            thickness = thicc;
            Bottom = new List<float>() {0};
            YMaximum = 0;
            Objects = new List<GameObject>();
            material = Resources.Load<Material>("SDMat");
            nodeSprite = Resources.Load<Sprite>("node");
        }
    
        public void InsertNewValue(float newhigh, float newLow, int newMax)
        {
            Top.Add(newhigh);
            Bottom.Add(newLow);
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

        private void MakeMesh(Vector2 lastHigh, Vector2 lastLow, Vector2 newHigh, Vector2 newLow)
        {
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            vertices[0] = lastHigh;
            vertices[1] = lastLow;
            vertices[2] = newHigh;
            vertices[3] = newLow;

            uv[0] = lastHigh;
            uv[1] = lastLow;
            uv[2] = newHigh;
            uv[3] = newLow;
        
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 1;
            triangles[4] = 2;
            triangles[5] = 3;

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            GameObject mesh_go = new GameObject(name+" mesh", typeof(MeshFilter), typeof(MeshRenderer));
            Objects.Add(mesh_go);
            mesh_go.transform.SetParent(Graph_Container, false);
            mesh_go.GetComponent<MeshFilter>().mesh = mesh;
        
            CanvasRenderer cr = mesh_go.AddComponent<CanvasRenderer>();
            cr.SetMesh(mesh);
            mesh_go.GetComponent<CanvasRenderer>().SetMaterial(material, null);

            RectTransform rt = mesh_go.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0, 0);
        }
    
    
        // ReSharper disable Unity.PerformanceAnalysis
        public void ShowGraph()
        {
            ResetGraph();

            float graphHeight = GraphSize.y;
            float MaxY = YMaximum*1.1f;
            float graphWidth = GraphSize.x;
            float SizeX = graphWidth / (Top.Count-1);
        
            GameObject lastLow = null;
            GameObject lastHigh = null;
            for (int i = 0; i < Top.Count; i++)
            {
                float H_PosX = i * SizeX;
                float L_PosX = i * SizeX;
                float H_PosY = (Top[i] / MaxY) * graphHeight;
                float L_PosY = (Bottom[i] / MaxY) * graphHeight;
                GameObject newHigh = CreateNode(new Vector2(H_PosX, H_PosY));
                GameObject newLow = CreateNode(new Vector2(L_PosX, L_PosY));
                if (lastLow != null)
                {
                    MakeMesh(lastHigh.GetComponent<RectTransform>().anchoredPosition, 
                        lastLow.GetComponent<RectTransform>().anchoredPosition, 
                        newHigh.GetComponent<RectTransform>().anchoredPosition, 
                        newLow.GetComponent<RectTransform>().anchoredPosition);
                }
                lastHigh = newHigh;
                lastLow = newLow;
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
}
