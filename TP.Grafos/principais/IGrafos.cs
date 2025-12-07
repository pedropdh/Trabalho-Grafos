using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal interface IGrafos
    {
        int QuantVertices { get; }
        int QuantArestas { get; }
        void AdicionarAresta(int origem, int destino, double custo, double capacidade);

        List<Aresta> ObterAdjacentes(int v);

        bool ExisteAresta(int origem, int destino);

        double ObterCusto(int origem, int destino);
        double ObterCapacidade(int origem, int destino);
    }
}
