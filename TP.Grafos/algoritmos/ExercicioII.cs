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
        // A classe ResultadosFormatados foi removida daqui, 
        // assumindo que agora está definida como classe pública no seu próprio arquivo.

        public static void GerarLoga(string nomeGrafo, ResultadosFormatados resultados)
        {
            // string path = $"Log_{nomeGrafo}.txt"; // Cuidado com caracteres inválidos no nome

            // Usando Path.Combine é a forma mais segura de construir caminhos:
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log_{nomeGrafo}.txt");

            List<string> linhasDoLog = new List<string>
{
    $"# Log de Execução para {nomeGrafo} - PUC Minas",
    "", // Linha em branco para separar o cabeçalho
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
                // File.WriteAllLines é a forma mais limpa de escrever uma lista de strings
                File.WriteAllLines(path, linhasDoLog);
            }
            catch (Exception ex)
            {
                // Tratamento de exceção mantido, mas idealmente seria um log de sistema.
                Console.WriteLine($"\n❌ Erro ao gerar o log para {nomeGrafo}: {ex.Message}");
                Console.WriteLine($"Caminho de tentativa: {path}");
            }
        }
    }
}
