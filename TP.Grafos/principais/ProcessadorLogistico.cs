using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos.principais
{
    internal class ProcessadorLogistico
    {
       
        public static void ProcessarTodasRespostasELog(IGrafos grafo, string nomeGrafo)
        {
            if (grafo == null) return;

            ResultadosFormatados resultados = new ResultadosFormatados();
            int n = grafo._quantVertices;
            int s = 1;
            int t = n;

            
            resultados.RespostaI = FormatadoresLog.FormatarResultadoI(s, t);

            
            int[,] capacidadesIntLog = new int[n + 1, n + 1];
            for (int u = 1; u <= n; u++)
                for (int v = 1; v <= n; v++)
                   
                    capacidadesIntLog[u, v] = (int)grafo.ObterCapacidade(u, v);

            GrafoLogistico logisticoLog = new GrafoLogistico(n, capacidadesIntLog);
            (int fluxoMaximoLog, List<(int u, int v)> corteMinimoLog) = logisticoLog.CalcularFluxoMaximo(s, t);
            resultados.RespostaII = logisticoLog.FormatarResultadoII(s, t, fluxoMaximoLog, corteMinimoLog);

            
            ResultadoIII resultadoAGM = AGM.CalcularAGM(grafo);
            resultados.RespostaIII = FormatadoresLog.FormatarResultadoIII(resultadoAGM);

            
            resultados.RespostaIV = FormatadoresLog.FormatarResultadoIV();

            
            resultados.RespostaV = FormatadoresLog.FormatarResultadoV();

           
            LogManager.GerarLog(nomeGrafo, resultados);
        }
    }
}
