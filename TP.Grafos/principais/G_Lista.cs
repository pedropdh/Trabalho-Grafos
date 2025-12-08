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

        public int _quantVertices { get; set; }
        public int _quantArestas { get; set; }

        public G_Lista(int totalVertices)
        {
            _quantVertices = totalVertices;
            adjacentes = new List<Aresta>[totalVertices + 1];

            for (int i = 1; i <= totalVertices; i++)
                adjacentes[i] = new List<Aresta>();
        }

        public void AdicionarAresta(int _origem, int _destino, double custo, double capacidade)
        {
            adjacentes[_origem].Add(new Aresta(_origem, _destino, custo, capacidade));
            _quantArestas++;
        }

        public List<Aresta> ObterAdjacentes(int v) => adjacentes[v];

        public bool ExisteAresta(int _origem, int _destino)
            => adjacentes[_origem].Any(a => a._destino == _destino);

        public double ObterCusto(int _origem, int _destino)
        {
            var aresta = adjacentes[_origem].FirstOrDefault(a => a._destino == _destino);
            if (aresta == null)
                return double.PositiveInfinity;
            return aresta.Custo;
        }
        public double ObterCapacidade(int _origem, int _destino)
        {
            var aresta = adjacentes[_origem].FirstOrDefault(a => a._destino == _destino);
            if (aresta == null)
                return 0;
            return aresta.Capacidade;
        }
    }
}
