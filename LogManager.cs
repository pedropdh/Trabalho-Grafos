public static class LogManager
{
    
    public class ResultadosFormatados
    {
        public string RespostaI { get; set; }
        public string RespostaII { get; set; } 
        public string RespostaIII { get; set; }
        public string RespostaIV { get; set; }
        public string RespostaV { get; set; }
    }

    public static void GerarLog(string nomeGrafo, ResultadosFormatados resultados)
    {
        string path = $"Log_{nomeGrafo}.txt";

        List<string> linhasDoLog = new List<string>();

        linhasDoLog.Add($"# Log de Execução para {nomeGrafo} - PUC Minas");
        
        linhasDoLog.Add(resultados.RespostaI);
        linhasDoLog.Add("");
        linhasDoLog.Add(resultados.RespostaII);
        linhasDoLog.Add("");
        linhasDoLog.Add(resultados.RespostaIII);
        linhasDoLog.Add("");
        linhasDoLog.Add(resultados.RespostaIV);
        linhasDoLog.Add("");
        linhasDoLog.Add(resultados.RespostaV);

        try
        {
            File.WriteAllLines(path, linhasDoLog);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gerar o log para {nomeGrafo}: {ex.Message}");
        }
    }
}