using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        string diretorioXmls = @"D:\Downloads\CDT_DEZEMBRO";
        decimal valorTotalSomado = 0;
        int quantidadeDeArquivos = 0;
        bool encontrouArquivoComTagCompNfse = false;

        try
        {
            var arquivosXml = Directory.GetFiles(diretorioXmls, "*.xml");
            quantidadeDeArquivos = arquivosXml.Length;

            foreach (var arquivoXml in arquivosXml)
            {
                // Parte 1: Verificar a tag CompNfse
                if (VerificarXML(arquivoXml, "CompNfse"))
                {
                    Console.WriteLine($"{arquivoXml} contém a tag CompNfse.");
                    encontrouArquivoComTagCompNfse = true;
                }

                // Parte 2: Somar os valores da tag ValorTotal
                XDocument xmlDoc = XDocument.Load(arquivoXml);
                var valorTotalElement = xmlDoc.Descendants("ValorTotal").FirstOrDefault();

                if (valorTotalElement != null)
                {
                    decimal valorNota;

                    if (decimal.TryParse(valorTotalElement.Value, out valorNota))
                    {
                        valorTotalSomado += valorNota;
                    }
                    else
                    {
                        Console.WriteLine($"Valor inválido na tag <ValorTotal> no arquivo: {arquivoXml}");
                    }
                }
            }

            if (!encontrouArquivoComTagCompNfse)
            {
                Console.WriteLine("Nenhum arquivo encontrado com a tag: CompNfse ");
            }

            Console.WriteLine($"O valor total somado é: {valorTotalSomado}");
            Console.WriteLine($"Número total de arquivos XML na pasta: {quantidadeDeArquivos}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        }

        Console.ReadLine();
    }

    static bool VerificarXML(string caminhoArquivo, string tag)
    {
        try
        {
            XDocument xmlDoc = XDocument.Load(caminhoArquivo);
            var tagElement = xmlDoc.Descendants(tag).FirstOrDefault();

            return tagElement != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar o arquivo {caminhoArquivo}: {ex.Message}");
            return false;
        }
    }
}
