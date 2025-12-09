using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos.algoritmos
{
    internal class ExercicioI
    {
        public List<int> CalcularMenorCaminho(IGrafos grafo, int raiz, int destino, out double custoTotal)
        {
            int tam = grafo._quantVertices;

            double[] dist = new double[tam + 1];
            int[] pred = new int[tam + 1];
            bool[] visitado = new bool[tam + 1];

            for (int i = 1; i <= tam; i++)
            {
                dist[i] = double.PositiveInfinity;
                pred[i] = -1;
                visitado[i] = false;
            }

            dist[raiz] = 0;

            
            for (int i = 1; i <= tam; i++)
            {
                int atual = -1;
                double menorDist = double.PositiveInfinity;

                for (int j = 1; j <= tam; j++)
                {
                    if (!visitado[j] && dist[j] < menorDist)
                    {
                        menorDist = dist[j];
                        atual = j;
                    }
                }

                if (atual == -1)
                    break;

                visitado[atual] = true;

                List<Aresta> adjacentes = grafo.ObterAdjacentes(atual);

                for (int j = 0; j < adjacentes.Count; j++)
                {
                    Aresta aresta = adjacentes[j];
                    int prox = aresta._destino;
                    double custo = aresta.Custo;

                    if (dist[atual] + custo < dist[prox])
                    {
                        dist[prox] = dist[atual] + custo;
                        pred[prox] = atual;
                    }
                }
            }

            if (double.PositiveInfinity == dist[destino])
            {
                custoTotal = -1;
                return null; 
            }
            else
            {

                List<int> caminho = new List<int>();
                int v = destino;

                while (v != -1)
                {
                    caminho.Add(v);
                    v = pred[v];
                }

                caminho.Reverse();

                custoTotal = dist[destino];
                return caminho;
            }
        }
    }
}
