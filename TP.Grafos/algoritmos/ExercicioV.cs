using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos.algoritmos
{
    internal class ExercicioV
    {
//////////////////Código para achar o Cenario B - Caminho Hamiltoniano
        public List<int> EncontrarCaminhoHamiltoniano(IGrafos grafo, int inicio)
        {
            int n = grafo._quantVertices;

            List<int> caminho = new List<int>();
            bool[] visitado = new bool[n + 1];

            caminho.Add(inicio);
            visitado[inicio] = true;

            bool achou = AcharCaminho(grafo, inicio, caminho, visitado);

            if (achou)
            {
                return caminho;
            }

            return null;
        }

        private bool AcharCaminho(IGrafos grafo, int atual, List<int> caminho, bool[] visitado)
        {
            if (caminho.Count == grafo._quantVertices)
            {
                int primeiro = caminho[0];

                if (grafo.ExisteAresta(atual, primeiro))
                {
                    return true;
                }

                return false;
            }

            List<Aresta> adj = grafo.ObterAdjacentes(atual);

            for (int i = 0; i < adj.Count; i++)
            {
                Aresta aresta = adj[i];
                int prox = aresta._destino;

                if (!visitado[prox])
                {
                    visitado[prox] = true;
                    caminho.Add(prox);

                    if (AcharCaminho(grafo, prox, caminho, visitado))
                    {
                        return true;
                    }

                    visitado[prox] = false;
                    caminho.RemoveAt(caminho.Count - 1);
                }
            }

            return false;
        }


//////////////////Código para achar o Cenario A - Ciclo Eureliano

           public bool TemCicloEuleriano(IGrafos grafo)
            {
                int n = grafo._quantVertices;

                int[] entrada = new int[n + 1];
                int[] saida = new int[n + 1];

                for (int v = 1; v <= n; v++)
                {
                    List<Aresta> lista = grafo.ObterAdjacentes(v);

                    for (int j = 0; j < lista.Count; j++)
                    {
                        Aresta a = lista[j];
                        saida[v]++;
                        entrada[a._destino]++;
                    }
                }

                for (int i = 1; i <= n; i++)
                {
                    if (entrada[i] != saida[i])
                    {
                        return false;
                    }
                }

                return true;
           }
            public List<int> EncontrarCicloEuleriano(IGrafos grafo, int inicio)
            {
                Dictionary<int, Queue<Aresta>> copia = new Dictionary<int, Queue<Aresta>>();

                for (int i = 1; i <= grafo._quantVertices; i++)
                {
                    Queue<Aresta> fila = new Queue<Aresta>(grafo.ObterAdjacentes(i));
                    copia[i] = fila;
                }

                Stack<int> pilha = new Stack<int>();
                List<int> caminho = new List<int>();

                pilha.Push(inicio);

                while (pilha.Count > 0)
                {
                    int v = pilha.Peek();

                    if (copia[v].Count > 0)
                    {
                        Aresta a = copia[v].Dequeue();
                        pilha.Push(a._destino);
                    }
                    else
                    {
                        caminho.Add(v);
                        pilha.Pop();
                    }
                }

                caminho.Reverse();
                return caminho;
            }
    }


    
}
