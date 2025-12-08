using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal class GrafoLogistico
    {
        private readonly int numVertices;
        private readonly int[,] capacidadeOriginal;
        private int[,] capacidadeResidual;
        private int[] parent;

        public GrafoLogistico(int n, int[,] capacidades)
        {
            if (n <= 0)
            {
                throw new ArgumentException("O número de vértices deve ser positivo.", nameof(n));
            }
            if (capacidades == null || capacidades.GetLength(0) != n + 1 || capacidades.GetLength(1) != n + 1)
            {
                throw new ArgumentException("A matriz de capacidades deve ter dimensão [n+1, n+1].", nameof(capacidades));
            }

            this.numVertices = n;
            this.capacidadeOriginal = capacidades;
        }
        private bool BFS_EncontrarCaminhoLivre(int S, int T)
        {
           
            bool[] visitado = new bool[numVertices + 1];
            parent = new int[numVertices + 1];

            Queue<int> fila = new Queue<int>();
            fila.Enqueue(S);
            visitado[S] = true;
            parent[S] = -1;

            while (fila.Count != 0)
            {
                int u = fila.Dequeue();

                for (int v = 1; v <= numVertices; v++)
                {
                    if (!visitado[v] && capacidadeResidual[u, v] > 0)
                    {
                        fila.Enqueue(v);
                        visitado[v] = true;
                        parent[v] = u;

                        if (v == T)
                            return true;
                    }
                }
            }
            return false;
        }

        public (int fluxoMaximo, List<(int u, int v)> corteMinimo) CalcularFluxoMaximo(int S, int T)
        {
            if (S < 1 || S > numVertices || T < 1 || T > numVertices)
            {
                throw new ArgumentOutOfRangeException("Os hubs S e T devem estar no intervalo [1, numVertices].");
            }
            capacidadeResidual = (int[,])capacidadeOriginal.Clone();
            int maxFlow = 0;

            while (BFS_EncontrarCaminhoLivre(S, T))
            {
                int fluxoDoCaminho = int.MaxValue;
                int v = T;
                while (v != S)
                {
                    int u = parent[v];
                    fluxoDoCaminho = Math.Min(fluxoDoCaminho, capacidadeResidual[u, v]);
                    v = u;
                }

                v = T;
                while (v != S)
                {
                    int u = parent[v];
                    capacidadeResidual[u, v] -= fluxoDoCaminho;
                    capacidadeResidual[v, u] += fluxoDoCaminho;
                    v = u;
                }

                maxFlow += fluxoDoCaminho;
            }

            List<(int u, int v)> corteMinimo = EncontrarCorteMinimo(S);

            return (maxFlow, corteMinimo);
        }
        private List<(int u, int v)> EncontrarCorteMinimo(int S)
        {
            List<(int u, int v)> arestasDoCorte = new List<(int u, int v)>();

            bool[] alcançavel = new bool[numVertices + 1]; 

            BuscaApenasAlcancaveis(S, alcançavel);

            for (int u = 1; u <= numVertices; u++)
            {
                for (int v = 1; v <= numVertices; v++)
                {
                    if (alcançavel[u] && !alcançavel[v] && capacidadeOriginal[u, v] > 0)
                    {
                        arestasDoCorte.Add((u, v));
                    }
                }
            }
            return arestasDoCorte;
        }

        private void BuscaApenasAlcancaveis(int S, bool[] alcançavel)
        {
            Queue<int> fila = new Queue<int>();
            fila.Enqueue(S);
            alcançavel[S] = true;

            while (fila.Count != 0)
            {
                int u = fila.Dequeue();
                for (int v = 1; v <= numVertices; v++)
                {
                    
                    if (!alcançavel[v] && capacidadeResidual[u, v] > 0)
                    {
                        alcançavel[v] = true;
                        fila.Enqueue(v);
                    }
                }
            }
        }

        public string FormatarResultadoII(int S, int T, int fluxoMaximo, List<(int u, int v)> corteMinimo)
        {
            string resultadoFinal = "";

            resultadoFinal += "--- 2. Capacidade Máxima de Escoamento (Hub " + S + " -> Hub " + T + ") ---\n";

            resultadoFinal += "Fluxo Máximo (toneladas): " + fluxoMaximo + "\n";

            string cutString = "";

            foreach ((int u, int v) aresta in corteMinimo)
            {
                cutString += "(" + aresta.u + ", " + aresta.v + "), ";
            }

            if (corteMinimo.Count > 0)
            {
                cutString = cutString.Substring(0, cutString.Length - 2);
            }

            resultadoFinal += "Corte Mínimo (rotas críticas): " + cutString + "\n";

            return resultadoFinal;
        }
    }
    public class ResultadosFormatados
    {
        public string RespostaI { get; set; }
        public string RespostaII { get; set; }
        public string RespostaIII { get; set; }
        public string RespostaIV { get; set; }
        public string RespostaV { get; set; }
    }
    public static class LogManager
    {
        public static void GerarLog(string nomeGrafo, ResultadosFormatados resultados)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log_{nomeGrafo}.txt");

            List<string> linhasDoLog = new List<string>
        {
            $"# Log de Execução para {nomeGrafo} - PUC Minas",
            "",
            resultados.RespostaI,
            "",
            resultados.RespostaII,
            "",
            resultados.RespostaIII,
            "",
            resultados.RespostaIV,
            "",
            resultados.RespostaV
        };

            try
            {
                File.WriteAllLines(path, linhasDoLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Erro ao gerar o log para {nomeGrafo}: {ex.Message}");
                Console.WriteLine($"Caminho de tentativa: {path}");
            }
        }
    }
}
