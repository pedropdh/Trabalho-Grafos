using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Grafos;

namespace TP.Grafos
{

    internal class Program
    {
        /* I.Cada vértice representa um Centro de Distribuição(Hub) ou Ponto de Entrega da
         empresa.
       II.Cada aresta representa uma Rota Rodoviária ou Ligação Viária entre dois hubs.
       III.O peso da aresta indica o custo financeiro (em R$) para transportar uma unidade de
         carga por aquela rota — considerando distância, pedágio, combustível, tempo, entre
         outros fatores.
       IV.A capacidade da aresta representa o limite máximo diário (em toneladas) que pode
       ser transportado pela rota, levando em conta restrições de infraestrutura e tráfego*/
        static void Main(string[] args)
        {
            Console.WriteLine("Grafo 1\n" +
                "Grafo 2\n" +
                "Grafo 3\n" +
                "Grafo 4\n" +
                "Grafo 5\n" +
                "Grafo 6\n" +
                "Grafo 7");
            char opcao1 = char.Parse(Console.ReadLine());
            IGrafos grafo = LeitorDIMACS.Carregar($"grafos/grafo0{opcao1}.dimacs");

            Console.WriteLine("Grafo montado!");
            Console.WriteLine($"Vértices: {grafo._quantVertices}");
            Console.WriteLine($"Arestas: {grafo._quantArestas}");

            Console.WriteLine("1) Qual é o trajeto mais econômico para enviar cargas entre dois centros? (Roteamento de Menor Custo)\n" +
                "2) Qual é o limite diário de escoamento de mercadorias da empresa? (Capacidade Máxima de Escoamento)\n" +
                "3) Como interligar todos os centros de distribuição ao menor custo? (Expansão da Rede de Comunicação)\n" +
                "4) Como planejar manutenções sem conflitos de recurso? (Agendamento de Manutenções sem Conflito)\n" +
                "5) É possível criar um percurso único de inspeção pelas rotas e centros? (Rota Única de Inspeção)\n");
            char opcao = char.Parse(Console.ReadLine());
            switch (opcao)
            {
                case '1':
                    break;
                case '2':
                    int numVertices = grafo._quantVertices;
                    int[,] capacidadesInt = new int[numVertices + 1, numVertices + 1];

                    for (int u = 1; u <= numVertices; u++)
                    {
                        for (int v = 1; v <= numVertices; v++)
                        {
                            capacidadesInt[u, v] = (int)grafo.ObterCapacidade(u, v);
                        }
                    }
                    int fonteS = 1;
                    int sumidouroT = numVertices;

                    GrafoLogistico logistico = new GrafoLogistico(numVertices, capacidadesInt);

                    (int fluxoMaximo, List<(int u, int v)> corteMinimo) = logistico.CalcularFluxoMaximo(fonteS, sumidouroT);

                    Console.WriteLine(logistico.FormatarResultadoII(fonteS, sumidouroT, fluxoMaximo, corteMinimo));
                    break;
                case '3':
                    AGM.ExibirResultados(AGM.CalcularAGM(grafo));
                    break;
                case '4':
                    AgendamentoManutencao.ExibirResultados(AgendamentoManutencao.CalcularAgendamento(grafo));
                    break;
                case '5':
                    break;
            }
        }

    }
}
