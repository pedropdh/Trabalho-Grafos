using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{


   /* I.Cada vértice representa um Centro de Distribuição(Hub) ou Ponto de Entrega da
        empresa.
      II.Cada aresta representa uma Rota Rodoviária ou Ligação Viária entre dois hubs.
      III.O peso da aresta indica o custo financeiro (em R$) para transportar uma unidade de
        carga por aquela rota — considerando distância, pedágio, combustível, tempo, entre
        outros fatores.
      IV.A capacidade da aresta representa o limite máximo diário (em toneladas) que pode
      ser transportado pela rota, levando em conta restrições de infraestrutura e tráfego*/
    internal class Program
    {
       
        static void Main(string[] args)
        {
            IGrafos grafo = LeitorDIMACS.Carregar("grafos/grafo02.dimacs");

            Console.WriteLine("Grafo montado!");
            Console.WriteLine($"Vértices: {grafo._quantVertices}");
            Console.WriteLine($"Arestas: {grafo._quantArestas}" );
        } 
      
    }
}
