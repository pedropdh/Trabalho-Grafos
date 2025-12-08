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
                "Grafo 2" +
                "Grafo 3" +
                "Grafo 4" +
                "Grafo 5" +
                "Grafo 6" +
                "Grafo 7");
            char opcao1 = char.Parse(Console.ReadLine());
            IGrafos grafo = LeitorDIMACS.Carregar($"grafos/grafo0{opcao1}.dimacs");

            Console.WriteLine("Grafo montado!");
            Console.WriteLine($"Vértices: {grafo._quantVertices}");
            Console.WriteLine($"Arestas: {grafo._quantArestas}");

            Console.WriteLine("1) Qual é o trajeto mais econômico para enviar cargas entre dois centros? (Roteamento de Menor Custo)\n" +
                "2) Qual é o limite diário de escoamento de mercadorias da empresa? (Capacidade Máxima de Escoamento)" +
                "3) Como interligar todos os centros de distribuição ao menor custo? (Expansão da Rede de Comunicação)" +
                "4) Como planejar manutenções sem conflitos de recurso? (Agendamento de Manutenções sem Conflito)" +
                "5) É possível criar um percurso único de inspeção pelas rotas e centros? (Rota Única de Inspeção)");
            char opcao = char.Parse(Console.ReadLine());
            switch (opcao)
            {
                case '1':
                    break;
                case '2':
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
