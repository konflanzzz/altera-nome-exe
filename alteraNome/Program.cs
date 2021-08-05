using System;
using System.IO;
using System.Text.RegularExpressions;

namespace alteraNome
{
    class Program
    {
        static void Main(string[] args)
        {
            // chamada da funcao principal
            string novoNome = "";

            // array que vai armazenar os diretorios
            string[] linhas = File.ReadAllLines(@"./config.txt");

            // variaveis recebem o diretorio da pasta Temp e da Pasta Remessas do Client Suite
            string caminhoTemp = linhas[0];
            string caminhoRemessas = linhas[1];

            // verifica se o diretorio da pasta Temp existe
            if (Directory.Exists(caminhoTemp) == false)
            {
                Console.WriteLine("Nao foi possivel encontrar o diretorio Temp.");
            }

            // verifica se o diretorio da pasta Remessas está acessivel / existe 
            if (Directory.Exists(caminhoRemessas) == false)
            {
                Console.WriteLine("Nao foi possivel encontrar o diretorio Remessas.");
            }

            if (Directory.Exists(caminhoTemp))
            {
                // armazena os arquivos em um array
                string[] arquivos = Directory.GetFiles(caminhoTemp);

                foreach (string n in arquivos)
                {
                    // obtem o nome de cada um dos arquivos capturados
                    string nomeArquivo = Path.GetFileNameWithoutExtension(n);

                    // caso seja um arquivo de emissao, altera o nome mas mantem a numeracao informada
                    if (Regex.IsMatch(nomeArquivo, @"^[0-9]+$"))
                    {
                        novoNome = "NFEEMISSAO_" + nomeArquivo + ".txt";
                    }

                    else
                    {
                        switch (nomeArquivo)
                        {
                            // altera o nome para cancelamento
                            case "CANC":
                                novoNome = "NFECANC_.txt";
                                break;

                            // altera o nome para carta de correcao
                            case "CCE":
                                novoNome = "NFECCE_.txt";
                                break;

                            // altera o nome para consulta de situacao da NFe
                            case "SITU":
                                novoNome = "NFESITUACAO_.txt";
                                break;

                            // altera o nome para agendamento de envio de email da NFe
                            case "MAIL":
                                novoNome = "NFEREENVIOEMAIL_.txt";
                                break;

                            // altera o nome para reimpressao da NFe
                            case "RIMP":
                                novoNome = "NFEREIMPRIME_.txt";
                                break;

                            // altera o nome para reimpressao de evento da NFe
                            case "REVE":
                                novoNome = "NFEREIMPRIMEEVENTO_.txt";
                                break;

                            // altera o nome para previa da DANFE
                            case "PREV":
                                novoNome = "NFEPREVIA_.txt";
                                break;

                        }
                    }

                    // para cada um dos arquivos do array, move para a pasta Remessas, alterando o nome
                    string nomeFinalArquivo = Path.Combine(caminhoRemessas, novoNome);
                    File.Move(n, nomeFinalArquivo, true);
                }
            }
            else
            {
                Console.WriteLine("Houve um problema!");
            }
        }
    }
}
