using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            IGrafos grafo = LeitorDIMACS.Carregar("grafos/grafo02.dimacs");

            Console.WriteLine("Grafo montado!");
            Console.WriteLine($"Vértices: {grafo.QuantVertices}");
            Console.WriteLine($"Arestas: {grafo.QuantArestas}");
        }
      
    }
}
