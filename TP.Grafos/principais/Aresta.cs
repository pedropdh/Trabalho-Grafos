using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal class Aresta
    {
        
        public int _origem { get; }
        public int _destino { get; }
        public double Custo { get; }
        public double Capacidade { get; set; }

        public Aresta(int origem, int _destino, double custo, double capacidade)
        {
            _origem = origem;
            _destino = _destino;
            Custo = custo;
            Capacidade = capacidade;
        }
    
    }
}
