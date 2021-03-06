using System;
using System.IO;
using System.Linq;
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
                Console.WriteLine("Houve um problema ao ler o arquivo config.txt: ");
                Console.WriteLine(ex.Message);
                gravarLinhaLog("Houve um problema ao ler o arquivo config.txt: " + ex.Message);
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
                gravarLinhaLog("Nao foi informado caminho temp no arquivo config.txt");
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
                gravarLinhaLog("Nao foi possivel acessar diretorio Remessas");
                Console.ReadLine();
                return;
            }

            // verifica se o diretorio da pasta Temp existe
            if (Directory.Exists(caminhoTemp) == false)
            {
                Console.WriteLine("Diretorio temporario nao foi encontrado.");
                gravarLinhaLog("Diretorio temporario nao foi encontrado.");

                try
                {
                    Directory.CreateDirectory(caminhoTemp);
                    Console.WriteLine("Diretorio temporario foi criado em: ", caminhoTemp);
                    gravarLinhaLog("Diretorio temporario foi criado em: " + caminhoTemp);
                }

                catch (Exception ex) // caso o arquivo config.txt nao esteja presente junto ao .exe
                {
                    Console.WriteLine("Houve um problema ao criar o diretorio Temp: ");
                    Console.WriteLine(ex.Message);
                    gravarLinhaLog("Houve um problema ao criar o diretorio Temp: " + ex.Message);
                    Console.ReadLine();
                    return;
                }
            }

            // verifica se o diretorio da pasta Remessas está acessivel / existe 
            if (Directory.Exists(caminhoRemessas) == false)
            {
                Console.WriteLine("Diretorio Remessas nao foi encontrado.");
                gravarLinhaLog("Diretorio Remessas nao foi encontrado.");
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

                        else if (nomeArquivo.ToUpper().Contains("NOTA") && !nomeArquivo.All(char.IsDigit))
                        {
                            novoNome = "NFEEMISSAO_" + nomeArquivo.ToUpper().Replace("NOTA", "") + ".txt";
                        }

                        else
                        {
                            switch (nomeArquivo)
                            {
                                // altera o nome para cancelamento
                                case var target when nomeArquivo.ToUpper().Contains("CANC"):
                                    novoNome = "NFECANC_" + nomeArquivo.ToUpper().Replace("CANC", "") + ".txt";
                                    break;

                                // altera o nome para carta de correcao
                                case var target when nomeArquivo.ToUpper().Contains("CCE"):
                                    novoNome = "NFECCE_" + nomeArquivo.ToUpper().Replace("CCE", "") + ".txt";
                                    break;

                                // altera o nome para consulta de situacao da NFe
                                case var target when nomeArquivo.ToUpper().Contains("SITU"):
                                    novoNome = "NFESITUACAO_" + nomeArquivo.ToUpper().Replace("SITU", "") + ".txt";
                                    break;

                                // altera o nome para agendamento de envio de email da NFe
                                case var target when nomeArquivo.ToUpper().Contains("MAIL"):
                                    novoNome = "NFEREENVIOEMAIL_" + nomeArquivo.ToUpper().Replace("MAIL", "") + ".txt";
                                    break;

                                // altera o nome para reimpressao da NFe
                                case var target when nomeArquivo.ToUpper().Contains("RIMP"):
                                    novoNome = "NFEREIMPRIME_" + nomeArquivo.ToUpper().Replace("RIMP", "") + ".txt";
                                    break;

                                // altera o nome para reimpressao de evento da NFe
                                case var target when nomeArquivo.ToUpper().Contains("REVE"):
                                    novoNome = "NFEREIMPRIMEEVENTO_" + nomeArquivo.ToUpper().Replace("REVE", "") + ".txt";
                                    break;

                                // altera o nome para previa da DANFE
                                case var target when nomeArquivo.ToUpper().Contains("PREV"):
                                    novoNome = "NFEPREVIA_" + nomeArquivo.ToUpper().Replace("PREV", "") + ".txt";
                                    break;

                                // altera o nome para carta de correcao
                                case var target when nomeArquivo.ToUpper().Contains("INUT"):
                                    novoNome = "NFEINUT_" + nomeArquivo.ToUpper().Replace("INUT", "") + ".txt";
                                    break;
                            }
                        }

                        // para cada um dos arquivos do array, altera o nome final do arquivo
                        string nomeFinalArquivo = Path.Combine(caminhoRemessas, novoNome);

                        try
                        {
                            if (File.Exists(Path.Combine(n,nomeFinalArquivo)))
                            {
                                // caso o arquivo já existe, substitui
                                File.Delete(Path.Combine(caminhoRemessas, novoNome));
                                gravarLinhaLog("Substituindo arquivo já existente...");
                                File.Move(n, nomeFinalArquivo);
                                gravarLinhaLog(nomeArquivo + " -> " + novoNome);
                            }

                            else 
                            {
                                // para cada um dos arquivos do array, move para a pasta remessas
                                File.Move(n, nomeFinalArquivo);
                                gravarLinhaLog(nomeArquivo + " -> " + novoNome);
                            }
                            
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine("Houve um problema ao mover o arquivo: ");
                            Console.WriteLine(ex.Message);
                            gravarLinhaLog("Houve um problema ao mover o arquivo: " + ex.Message);
                            Console.ReadLine();
                            return;
                        }
                    }
                    else
                    {
                        gravarLinhaLog(nomeArquivo + " nao foi movido pois não é um arquivo .txt");
                    }
                }
            }

            else
            {
                Console.WriteLine("Houve um problema! Por favor verifique os logs.");
            }
        }

        public static void gravarLinhaLog(string registro)
        {
            string caminhoLogs = @".\logs\";

            if (!Directory.Exists(caminhoLogs))Directory.CreateDirectory(caminhoLogs);

            string data = DateTime.Now.ToShortDateString();
            string hora = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");
            string nomeLog = data.Replace("/", "");

            using (StreamWriter outputFile = new StreamWriter(caminhoLogs + nomeLog + ".txt", true))
            {
                outputFile.WriteLine(hora + " - " + registro);
            }
        }
    }
}
