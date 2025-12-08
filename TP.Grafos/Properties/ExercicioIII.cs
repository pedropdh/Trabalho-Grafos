
using System;
using System.Collections.Generic;
using System.Linq;

namespace TP.Grafos
{
    internal class ResultadoIII
    {
        public IReadOnlyList<Aresta> Arestas { get; }
        public double CustoTotal { get; }

        public ResultadoIII(IEnumerable<Aresta> arestas, double custoTotal)
        {
            Arestas = (arestas ?? Enumerable.Empty<Aresta>()).ToList().AsReadOnly();
            CustoTotal = custoTotal;
        }
    }

    internal static class AGM
    {
        public static ResultadoIII CalcularAGM(IGrafos grafo, int verticeRaiz = 1)
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));
            int numVertices = grafo._quantVertices;
            if (numVertices <= 0) throw new ArgumentException("O grafo deve ter pelo menos um vértice.", nameof(grafo));
            if (verticeRaiz < 1 || verticeRaiz > numVertices) throw new ArgumentOutOfRangeException(nameof(verticeRaiz));

            Dictionary<int, List<Aresta>> novoGrafo = ConverterNaoDirecionado(grafo);

            bool[] inTree = new bool[numVertices + 1];
            List<Aresta> arvore = new List<Aresta>();
            double custoTotal = 0;

            inTree[verticeRaiz] = true;
            int adicionados = 1;

            while (adicionados < numVertices)
            {
                double menorCusto = double.PositiveInfinity;
                Aresta melhorAresta = null;

                for (int v = 1; v <= numVertices; v++)
                {
                    if (!inTree[v]) continue;
                    List<Aresta> vizinhos;
                    if (!novoGrafo.TryGetValue(v, out vizinhos)) continue;

                    foreach (Aresta a in vizinhos)
                    {
                        if (a == null) continue;
                        int destino = a._destino;
                        if (destino < 1 || destino > numVertices) continue;

                        if (!inTree[destino] && a.Custo < menorCusto)
                        {
                            menorCusto = a.Custo;
                            melhorAresta = a;
                        }
                    }
                }

                if (melhorAresta == null)
                    throw new InvalidOperationException("O grafo não é conexo.");

                inTree[melhorAresta._destino] = true;
                arvore.Add(melhorAresta);
                custoTotal += menorCusto;
                adicionados++;
            }

            return new ResultadoIII(arvore, custoTotal);
        }

        private static Dictionary<int, List<Aresta>> ConverterNaoDirecionado(IGrafos grafo)
        {
            Dictionary<int, List<Aresta>> novoGrafo = new Dictionary<int, List<Aresta>>();
            int numVertices = grafo._quantVertices;

            for (int i = 1; i <= numVertices; i++)
                novoGrafo[i] = new List<Aresta>();

            for (int origem = 1; origem <= numVertices; origem++)
            {
                List<Aresta> vizinhos;
                try
                {
                    vizinhos = grafo.ObterAdjacentes(origem);
                }
                catch
                {
                    continue;
                }

                if (vizinhos == null) continue;

                foreach (Aresta aresta in vizinhos)
                {
                    if (aresta == null) continue;
                    int destino = aresta._destino;
                    double custo = aresta.Custo;
                    double capacidade = aresta.Capacidade;

                    if (origem < destino)
                    {
                        novoGrafo[origem].Add(new Aresta(origem, destino, custo, capacidade));
                        novoGrafo[destino].Add(new Aresta(destino, origem, custo, capacidade));
                    }
                }
            }

            return novoGrafo;
        }

        public static void ExibirResultados(ResultadoIII resultado)
        {
            if (resultado == null) throw new ArgumentNullException(nameof(resultado));

            Console.WriteLine("===- ÁRVORE GERADORA MÍNIMA -===");
            Console.WriteLine($"Custo Total da Rede: R$ {resultado.CustoTotal:F2}");
            Console.WriteLine("Arestas da AGM:");

            foreach (Aresta aresta in resultado.Arestas)
            {
                char hubOrigem = (char)('A' + aresta._origem - 1); 
                char hubDestino = (char)('A' + aresta._destino - 1);
                Console.WriteLine($"  Hub {hubOrigem} -> Hub {hubDestino} : R$ {aresta.Custo:F2}");
            }
            Console.WriteLine();
        }
    }
}
