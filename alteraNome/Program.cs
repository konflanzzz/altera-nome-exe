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
            string caminhoTemp = "";
            string caminhoRemessas = "";
            string[] linhas = new string[2] { "", "" };

            // array que vai armazenar os diretorios
            try 
            {
                linhas = File.ReadAllLines(@"./config.txt"); 
            }

            catch (Exception ex) // e se o arquivo nao estiver junto ao .exe?
            {
                Console.WriteLine("Houve um problema ao ler o arquivo confif.txt: ");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }
            

            // variaveis recebem o diretorio da pasta Temp e da Pasta Remessas do Client Suite
            if (linhas[0] != "") 
            { 
                caminhoTemp = linhas[0];
            } 
            else
            {
                Console.WriteLine("Favor infomar o caminho do diretorio Temp no arquivo config.txt");
                Console.ReadLine();
                return;
            }

            if (linhas[1] != "")
            {
                caminhoRemessas = linhas[1];
            }
            else
            {
                Console.WriteLine("Favor infomar o caminho do diretorio Remessas no arquivo config.txt");
                Console.ReadLine();
                return;
            }

            // verifica se o diretorio da pasta Temp existe
            if (Directory.Exists(caminhoTemp) == false)
            {
                Console.WriteLine("Diretorio temporario nao foi encontrado.");

                try
                {
                    Directory.CreateDirectory(caminhoTemp);
                    Console.WriteLine("Diretorio temporario foi criado em: ", caminhoTemp);
                }
                catch(Exception ex) // caso o arquivo config.txt nao esteja presente junto ao .exe
                {
                    Console.WriteLine("Houve um problema ao criar o diretorio Temp: ");
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    return;
                }

            }

            // verifica se o diretorio da pasta Remessas está acessivel / existe 
            if (Directory.Exists(caminhoRemessas) == false)
            {
                Console.WriteLine("Diretorio Remessas nao foi encontrado.");
            }

            if (Directory.Exists(caminhoTemp))
            {
                // armazena os arquivos em um array
                string[] arquivos = Directory.GetFiles(caminhoTemp);

                // percorre todos os nomes dos arquivos, armazenados no array, obtidos no diretorio
                foreach (string n in arquivos)
                {
                    // obtem o nome de cada um dos arquivos capturados
                    // armazena o nome do arquivo com extensao
                    string nomeArquivo = Path.GetFileName(n);

                    if (nomeArquivo.EndsWith(".txt"))
                    {
                        // armazena o nome do arquivo sem extensao
                        nomeArquivo = Path.GetFileNameWithoutExtension(n);

                        // caso seja um arquivo de emissao, altera o nome mas mantem a numeracao informada
                        if (Regex.IsMatch(nomeArquivo, @"^[0-9]+$"))
                        {
                            // caso seja um arquivo de emissao, mantem a numeracao e adiciona a nomenclatura
                            novoNome = "NFEEMISSAO_" + nomeArquivo + ".txt";
                        }

                        else
                        {
                            switch (nomeArquivo.ToUpper())
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

                        // para cada um dos arquivos do array, altera o nome final do arquivo
                        string nomeFinalArquivo = Path.Combine(caminhoRemessas, novoNome);
                        
                        try
                        {   
                            // para cada um dos arquivos do array, move para a pasta remessas
                            File.Move(n, nomeFinalArquivo, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Houve um problema ao mover o arquivo: ");
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                            return;
                        }

                    }
                }
            }
            else
            {
                Console.WriteLine("Houve um problema!");
            }
        }
    }
}
