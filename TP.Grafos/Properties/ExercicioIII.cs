using System;
using System.Collections.Generic;
using System.Linq;

namespace TP.Grafos
{
    /// <summary>
    /// Representa o resultado do cálculo da Árvore Geradora Mínima.
    /// Contém as arestas selecionadas e o custo total da árvore.
    /// </summary>
    internal class ResultadoIII
    {
        /// <summary>
        /// Lista contendo as arestas que compõem a AGM.
        /// </summary>
        public IReadOnlyList<Aresta> Arestas { get; }

        /// <summary>
        /// Custo total da AGM.
        /// </summary>
        public double CustoTotal { get; }

        /// <summary>
        /// Inicializa uma nova instância da classe ResultadoIII.
        /// </summary>
        /// <param name="arestas">Coleção de arestas que formam a AGM.</param>
        /// <param name="custoTotal">Custo total da AGM.</param>
        public ResultadoIII(IEnumerable<Aresta> arestas, double custoTotal)
        {
            Arestas = (arestas ?? Enumerable.Empty<Aresta>()).ToList().AsReadOnly();
            CustoTotal = custoTotal;
        }
    }

    /// <summary>
    /// Classe estática que fornece métodos para calcular e mostrar a AGM.
    /// Implementa uma versão do algoritmo de Prim para grafos não direcionados.
    /// </summary>
    internal static class AGM
    {
        /// <summary>
        /// Calcula a AGM do um grafo não direcionado com algoritmo de Prim.
        /// </summary>
        /// <param name="grafo">Grafo de entrada.</param>
        /// <param name="verticeRaiz">Vértice raiz para começar a construção da árvore.</param>
        /// <returns>Objeto ResultadoIII contendo as arestas da AGM e o custo total.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o grafo é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando o grafo não tem vértices.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o vértice raiz está fora do intervalo válido.</exception>
        /// <exception cref="InvalidOperationException">Lançada quando o grafo não é conexo, impossibilitando o cálculo da AGM.</exception>
        internal static ResultadoIII CalcularAGM(IGrafos grafo, int verticeRaiz = 1)
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

        /// <summary>
        /// Converte um grafo direcionado para não direcionada, duplicando as arestas em ambas as direções.
        /// </summary>
        /// <param name="grafo">Grafo que será convertido.</param>
        /// <returns>Dicionário representando o grafo não direcionado, a chave é o vértice de origem
        /// e o valor é a lista de arestas.</returns>
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

                    if (origem >= 1 && origem <= numVertices && destino >= 1 && destino <= numVertices)
                    {
                        novoGrafo[origem].Add(new Aresta(origem, destino, custo, capacidade));
                        novoGrafo[destino].Add(new Aresta(destino, origem, custo, capacidade));
                    }
                }
            }

            return novoGrafo;
        }

        /// <summary>
        /// Exibe os resultados da Árvore Geradora Mínima formatada.
        /// </summary>
        /// <param name="resultado">Os dados da AGM a serem exibidos.</param>
        /// <exception cref="ArgumentNullException">Lançada quando o resultado é nulo.</exception>
        public static void ExibirResultados(ResultadoIII resultado)
        {
            if (resultado == null) throw new ArgumentNullException(nameof(resultado));

            Console.WriteLine("===- ÁRVORE GERADORA MÍNIMA -===");
            Console.WriteLine($"Custo Total da Rede: R$ {resultado.CustoTotal:F2}");
            Console.WriteLine("Arestas da AGM:");

            foreach (Aresta aresta in resultado.Arestas)
            {
                int hubOrigem = (aresta._origem); 
                int hubDestino = (aresta._destino);
                Console.WriteLine($"  Hub {hubOrigem} -> Hub {hubDestino} : R$ {aresta.Custo:F2}");
            }
            Console.WriteLine();
        }
    }
}
