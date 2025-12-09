using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Grafos;

namespace TP.Grafos
{
    internal class ExercicioII
    {

        public static void GerarLoga(string nomeGrafo, ResultadosFormatados resultados)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log_{nomeGrafo}.txt");

            List<string> linhasDoLog = new List<string>
                {
                    $"# Log de Execução para {nomeGrafo} - PUC Minas",
                    "", 
                    resultados.RespostaI,
                    "",
                    resultados.RespostaII,
                    "",
                    resultados.RespostaIII,
                    "",
                    resultados.RespostaIV,
                    "",
                    resultados.RespostaV
                };

            try
            {
                File.WriteAllLines(path, linhasDoLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Erro ao gerar o log para {nomeGrafo}: {ex.Message}");
                Console.WriteLine($"Caminho de tentativa: {path}");
            }
        }
    }
}
