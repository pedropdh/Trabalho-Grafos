using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal interface IGrafos
    {
        int _quantVertices { get; }
        int _quantArestas { get; }
        void AdicionarAresta(int _origem, int _destino, double custo, double capacidade);

        List<Aresta> ObterAdjacentes(int v);

        bool ExisteAresta(int _origem, int _destino);

        double ObterCusto(int _origem, int _destino);
        double ObterCapacidade(int _origem, int _destino);
    }
}
