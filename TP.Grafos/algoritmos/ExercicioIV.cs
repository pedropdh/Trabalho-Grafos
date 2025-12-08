using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos 
{ 
    /// <summary>
    /// Representa o resultado do agendamento de manutenções para as rotas de uma rede.
    /// Contém o número de turnos necessários e a distribuição das rotas por turno.
    /// </summary>
    internal class ResultadoManutencao
    {
        /// <summary>
        /// Número mínimo de turnos necessários para realizar todas as manutenções
        /// sem conflito de vértices (hubs) em um mesmo turno.
        /// </summary>
        public int NumeroTurnos { get; }

        /// <summary>
        /// Dicionário que mapeia cada turno (índice baseado em 0) para a lista de rotas
        /// que podem ser mantidas simultaneamente naquele turno.
        /// </summary>
        public Dictionary<int, List<Aresta>> RotasPorTurno { get; }

        /// <summary>
        /// Inicializa uma nova instância da classe ResultadoManutencao.
        /// </summary>
        /// <param name="numeroTurnos">Número total de turnos necessários para o agendamento.</param>
        /// <param name="rotasPorTurno">Distribuição das rotas por turno, onde a chave é o índice do turno.</param>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o número de turnos é negativo.</exception>
        public ResultadoManutencao(int numeroTurnos, Dictionary<int, List<Aresta>> rotasPorTurno)
        {
            if (numeroTurnos < 0) throw new ArgumentOutOfRangeException(nameof(numeroTurnos));
            NumeroTurnos = numeroTurnos;
            RotasPorTurno = rotasPorTurno ?? new Dictionary<int, List<Aresta>>();
        }
    }

    /// <summary>
    /// Classe estática que fornece métodos para agendar manutenções de rotas em uma rede,
    /// garantindo que nenhum hub seja mantido mais de uma vez por turno.
    /// Implementa uma heurística de coloração de arestas baseada no grau dos vértices.
    /// </summary>
    internal static class AgendamentoManutencao
    {
        /// <summary>
        /// Calcula um agendamento ótimo de manutenções para todas as rotas da rede,
        /// minimizando o número de turnos necessários.
        /// </summary>
        /// <param name="grafo">Grafo representando a rede de hubs e rotas.</param>
        /// <returns>Objeto ResultadoManutencao contendo o agendamento calculado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o grafo é nulo.</exception>
        internal static ResultadoManutencao CalcularAgendamento(IGrafos grafo)
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));

            List<Aresta> arestas = ExtrairTodasArestas(grafo);
            if (arestas == null) arestas = new List<Aresta>();
            if (arestas.Count == 0)
            {
                Dictionary<int, List<Aresta>> vazio = new Dictionary<int, List<Aresta>>();
                return new ResultadoManutencao(0, vazio);
            }

            int numVertices = grafo._quantVertices;

            // 1) calcular grau de cada vértice 
            int[] grau = new int[numVertices + 1];
            for (int i = 0; i < arestas.Count; i++)
            {
                Aresta a = arestas[i];
                if (a == null) continue;
                if (a._origem >= 1 && a._origem <= numVertices) grau[a._origem]++;
                if (a._destino >= 1 && a._destino <= numVertices) grau[a._destino]++;
            }

            int grauMaximo = 0;
            for (int i = 1; i <= numVertices; i++)
            {
                if (grau[i] > grauMaximo) grauMaximo = grau[i];
            }

            // número de cores suficiente = Δ + 1
            int numCores = grauMaximo + 1;

            // 2) estrutura para verificar cores usadas em vértices
            bool[][] corUsadaPorVertice = new bool[numVertices + 1][];
            for (int i = 1; i <= numVertices; i++)
            {
                corUsadaPorVertice[i] = new bool[numCores];
            }

            // 3) ordenar arestas por importância
            List<Aresta> arestasOrdenadas = new List<Aresta>(arestas);
            arestasOrdenadas.Sort(delegate (Aresta x, Aresta y)
            {
                int dx = grau[x._origem] + grau[x._destino];
                int dy = grau[y._origem] + grau[y._destino];
                return dy.CompareTo(dx);
            });

            // 4) colorir procura a menor cor disponível  
            Dictionary<Aresta, int> coloracao = new Dictionary<Aresta, int>();
            int corMaxAtribuida = -1;

            for (int i = 0; i < arestasOrdenadas.Count; i++)
            {
                Aresta aresta = arestasOrdenadas[i];
                if (aresta == null) continue;

                int u = aresta._origem;
                int v = aresta._destino;

                int corEncontrada = -1;
                for (int c = 0; c < numCores; c++)
                {
                    if (!corUsadaPorVertice[u][c] && !corUsadaPorVertice[v][c])
                    {
                        corEncontrada = c;
                        break;
                    }
                }

                if (corEncontrada == -1)
                {
                    int novaNumCores = numCores + 1;
                    bool[][] novo = new bool[numVertices + 1][];
                    for (int j = 1; j <= numVertices; j++)
                    {
                        bool[] antigo = corUsadaPorVertice[j];
                        bool[] arr = new bool[novaNumCores];
                        for (int k = 0; k < antigo.Length; k++) arr[k] = antigo[k];
                        novo[j] = arr;
                    }
                    corUsadaPorVertice = novo;
                    numCores = novaNumCores;
                    corEncontrada = numCores - 1;
                }

                coloracao[aresta] = corEncontrada;
                corUsadaPorVertice[u][corEncontrada] = true;
                corUsadaPorVertice[v][corEncontrada] = true;
                if (corEncontrada > corMaxAtribuida) corMaxAtribuida = corEncontrada;
            }

            // 5) agrupar arestas por turno
            Dictionary<int, List<Aresta>> rotasPorTurno = new Dictionary<int, List<Aresta>>();
            foreach (KeyValuePair<Aresta, int> kvp in coloracao)
            {
                Aresta a = kvp.Key;
                int c = kvp.Value;
                if (!rotasPorTurno.ContainsKey(c)) rotasPorTurno[c] = new List<Aresta>();
                rotasPorTurno[c].Add(a);
            }

            int numeroTurnos = corMaxAtribuida + 1;
            return new ResultadoManutencao(numeroTurnos, rotasPorTurno);
        }

        /// <summary>
        /// Extrai todas as arestas únicas de um grafo, eliminando duplicatas (para grafos não direcionados).
        /// </summary>
        /// <param name="grafo">Grafo do qual serão extraídas as arestas.</param>
        /// <returns>Lista de arestas únicas do grafo.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o grafo é nulo.</exception>
        private static List<Aresta> ExtrairTodasArestas(IGrafos grafo)
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));

            List<Aresta> arestas = new List<Aresta>();
            HashSet<string> paresVistos = new HashSet<string>();
            int numVertices = grafo._quantVertices;

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

                for (int i = 0; i < vizinhos.Count; i++)
                {
                    Aresta a = vizinhos[i];
                    if (a == null) continue;
                    int u = a._origem < a._destino ? a._origem : a._destino;
                    int v = a._origem < a._destino ? a._destino : a._origem;
                    string chave = string.Format("{0}:{1}:{2}", u, v, a.Custo);
                    if (!paresVistos.Contains(chave))
                    {
                        paresVistos.Add(chave);
                        arestas.Add(a);
                    }
                }
            }

            return arestas;
        }

        /// <summary>
        /// Exibe os resultados do agendamento de manutenções de forma formatada no console.
        /// </summary>
        /// <param name="resultado">Objeto ResultadoManutencao contendo os dados do agendamento.</param>
        /// <exception cref="ArgumentNullException">Lançada quando o resultado é nulo.</exception>
        public static void ExibirResultados(ResultadoManutencao resultado)
        {
            if (resultado == null) throw new ArgumentNullException(nameof(resultado));
            Console.WriteLine("===- AGENDAMENTO DE MANUTENÇÕES -===");
            Console.WriteLine(string.Format("Número mínimo de turnos necessários: {0}", resultado.NumeroTurnos));
            List<int> turnos = new List<int>(resultado.RotasPorTurno.Keys);
            turnos.Sort();
            for (int i = 0; i < turnos.Count; i++)
            {
                int turno = turnos[i];
                Console.WriteLine(string.Format("Turno {0}:", turno + 1));
                List<Aresta> rotas;
                if (!resultado.RotasPorTurno.TryGetValue(turno, out rotas)) continue;
                for (int j = 0; j < rotas.Count; j++)
                {
                    Aresta rota = rotas[j];
                    int hubOrigem = (rota._origem);
                    int hubDestino = (rota._destino);
                    Console.WriteLine(string.Format("  - Hub {0} -> Hub {1} (Custo: R$ {2:F2}, Capacidade: {3}t)",
                        hubOrigem, hubDestino, rota.Custo, rota.Capacidade));
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Verifica se um agendamento de manutenções é válido, garantindo que nenhum hub
        /// apareça mais de uma vez em um mesmo turno.
        /// </summary>
        /// <param name="resultado">Objeto ResultadoManutencao a ser validado.</param>
        /// <returns>
        /// True se o agendamento é válido (nenhum hub repete em um mesmo turno),
        /// False caso contrário.
        /// </returns>
        /// <exception cref="ArgumentNullException">Lançada quando o resultado é nulo.</exception>
        public static bool VerificarAgendamentoValido(ResultadoManutencao resultado)
        {
            if (resultado == null) throw new ArgumentNullException(nameof(resultado));

            foreach (KeyValuePair<int, List<Aresta>> kvp in resultado.RotasPorTurno)
            {
                List<Aresta> lista = kvp.Value;
                if (lista == null) continue;
                HashSet<int> verticesTurno = new HashSet<int>();
                for (int i = 0; i < lista.Count; i++)
                {
                    Aresta a = lista[i];
                    if (verticesTurno.Contains(a._origem) || verticesTurno.Contains(a._destino))
                    {
                        return false;
                    }
                    verticesTurno.Add(a._origem);
                    verticesTurno.Add(a._destino);
                }
            }

            return true;
        }
    }
}
