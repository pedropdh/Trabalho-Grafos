using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP.Grafos
{
    internal class LeitorDIMACS
    {
        public static IGrafos Carregar(string caminhoArquivo)
        {
            var linhas = File.ReadAllLines(caminhoArquivo);

            var cabecalho = linhas[0].Split();
            int vertices = int.Parse(cabecalho[0]);
            int arestas = int.Parse(cabecalho[1]);

            double densidade = (double)arestas / (vertices * vertices);

            IGrafos grafo;

            if (densidade < 0.25)
            {
                grafo = new G_Lista(vertices);
                Console.WriteLine("Estrutura: Lista de Adjacência (Grafo Esparso).");
            }
            else
            {
                grafo = new G_Matriz(vertices);
                Console.WriteLine("Estrutura: Matriz de Adjacência (Grafo Denso).");
            }

            for (int i = 1; i < linhas.Length; i++)
            {
                var partes = linhas[i].Split();

                int _origem = int.Parse(partes[0]);
                int _destino = int.Parse(partes[1]);
                double custo = double.Parse(partes[2]);
                double capacidade = double.Parse(partes[3]);

                grafo.AdicionarAresta(_origem, _destino, custo, capacidade);
            }

            return grafo;
        }
    }
}
