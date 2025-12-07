using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal class G_Lista : IGrafos
    {
        private List<Aresta>[] adjacentes;

        public int QuantVertices { get; set; }
        public int QuantArestas { get; set; }

        public G_Lista(int totalVertices)
        {
            QuantVertices = totalVertices;
            adjacentes = new List<Aresta>[totalVertices + 1];

            for (int i = 1; i <= totalVertices; i++)
                adjacentes[i] = new List<Aresta>();
        }

        public void AdicionarAresta(int origem, int destino, double custo, double capacidade)
        {
            adjacentes[origem].Add(new Aresta(origem, destino, custo, capacidade));
            QuantArestas++;
        }

        public List<Aresta> ObterAdjacentes(int v) => adjacentes[v];

        public bool ExisteAresta(int origem, int destino)
            => adjacentes[origem].Any(a => a.Destino == destino);

        public double ObterCusto(int origem, int destino)
        {
            var aresta = adjacentes[origem].FirstOrDefault(a => a.Destino == destino);
            if (aresta == null)
                return double.PositiveInfinity;
            return aresta.Custo;
        }
        public double ObterCapacidade(int origem, int destino)
        {
            var aresta = adjacentes[origem].FirstOrDefault(a => a.Destino == destino);
            if (aresta == null)
                return 0;
            return aresta.Capacidade;
        }
    }
}
