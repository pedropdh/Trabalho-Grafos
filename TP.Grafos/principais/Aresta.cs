using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal class Aresta
    {
        
        public int Origem { get; }
        public int Destino { get; }
        public double Custo { get; }
        public double Capacidade { get; set; }

        public Aresta(int origem, int destino, double custo, double capacidade)
        {
            Origem = origem;
            Destino = destino;
            Custo = custo;
            Capacidade = capacidade;
        }
    
    }
}
