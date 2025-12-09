using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos { 
    internal class FormatadoresLog
    {
        public static string FormatarResultadoI(int S, int T)
        {
            return $"--- 1. Roteamento de Menor Custo (Hub {S} -> Hub {T}) ---\n" +
                   "Caminho Mais Econômico: [IMPLEMENTAÇÃO PENDENTE - Algoritmo de Dijkstra ou Bellman-Ford]\n" +
                   "Custo Total: R$ 0.00 (Valor provisório)";
        }


        public static string FormatarResultadoIII(ResultadoIII resultado)
        {
            if (resultado == null) return "--- 3. Expansão da Rede de Comunicação (AGM) ---\nNenhum resultado disponível.";

            string resultadoFinal = "--- 3. Expansão da Rede de Comunicação (AGM) ---\n";
            resultadoFinal += $"Custo Total da Rede: R$ {resultado.CustoTotal:F2}\n";
            resultadoFinal += "Arestas da AGM:\n";

            foreach (Aresta aresta in resultado.Arestas)
            {

                char hubOrigem = (char)('A' + aresta._origem - 1);
                char hubDestino = (char)('A' + aresta._destino - 1);
                resultadoFinal += $"  Hub {hubOrigem} -> Hub {hubDestino} : R$ {aresta.Custo:F2}\n";
            }
            return resultadoFinal;
        }


        public static string FormatarResultadoIV()
        {
            return "--- 4. Agendamento de Manutenções sem Conflito ---\n" +
                   "Agendamento: [IMPLEMENTAÇÃO PENDENTE - Coloração de Arestas]\n" +
                   "Turnos Necessários: 0 (Valor provisório)";
        }


        public static string FormatarResultadoV()
        {
            return "--- 5. Rota Única de Inspeção (Euleriano/Hamiltoniano) ---\n" +
                   "Percurso: [IMPLEMENTAÇÃO PENDENTE - Busca de Ciclos]\n" +
                   "É Possível: Não avaliado (Valor provisório)";
        }
    }
}
