using System.Threading.Tasks;
using UnityEngine;

namespace Academy
{
    public class Environment
    {
    
        private NomGrid nomGrid;
        private Agent agent;
        private float epsilon;
        private bool enable;
        private float NOMS_PER_ENV;
    

        public void EnvSet(float ep, string strategy, float NPE)
        {
            epsilon = ep;
            NOMS_PER_ENV = NPE;

            nomGrid = new NomGrid();
            nomGrid.initGrid(NOMS_PER_ENV);

            agent = new Agent(strategy);
            agent.initPos();
        }

        public void EnvReset()
        {
            nomGrid = null;
            agent = null;
        }

        public int RunEnv()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (isEmpty()) break;
                
                Task action = Task.Factory.StartNew(() => Step());
                    
                action.Wait();
            }
            
            return agent.GetFitness();
        }

        public void Step()
        {
            agent.action(nomGrid.getState(agent.GetRow(), agent.GetCol()), nomGrid, epsilon);
        }

        public bool[,] GetNomGrid()
        {
            return nomGrid.getGrid();
        }

        public int GetAgentRow()
        {
            return agent.GetRow();
        }

        public int GetAgentCol()
        {
            return agent.GetCol();
        }

        public bool isEmpty() 
        {
            return (nomGrid.getNomCount() <= 0);
        } 
    }
}
