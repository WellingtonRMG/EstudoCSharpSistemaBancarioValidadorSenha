using System.Globalization;
using System.Numerics;
using System.Transactions;

class Program
{
    static void Main()
    {
        double saldoAtual = 00.00;

        // Loop principal para o validador de senha de acesso inicial
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t----- SAQUE BANCÁRIO -----");
            Console.ResetColor();

            Console.Write("Digite a senha de acesso: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string senha = Console.ReadLine();
            Console.ResetColor();

            bool tamSenha = false;
            if (!string.IsNullOrEmpty(senha))
            {
                if (tamSenha = senha.Length >= 8) // Verifica se o tamanho mínimo de 8 caracteres foi atingido
                {
                    bool temNumero = false, temMaius = false, temEspecial = false;
                    foreach (char c in senha) // Varre caractere por caractere da senha digitada
                    {
                        if (char.IsDigit(c)) // Valida se há números
                        {
                            temNumero = true;
                        }
                        else if (char.IsUpper(c)) // Valida se há letras maiúsculas
                        {
                            temMaius = true;
                        }
                        else if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) // Valida caracteres especiais
                        {
                            temEspecial |= true;
                        }
                    }

                    // Se todos os requisitos de segurança forem atendidos, libera o acesso
                    if (temNumero && tamSenha && temEspecial && temMaius)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\n\t   === ACESSO LIBERADO ===");
                        Console.ResetColor();
                        break; // Sai do loop da senha e vai para o menu bancário
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        if (!temMaius)
                        {
                            Console.WriteLine("\n\t[ERRO] A senha precisa conter pelo menos uma letra maíuscula!");
                        }
                        if (!temNumero)
                        {
                            Console.WriteLine("\t[ERRO] A senha precisa conter pelo menos um número!");
                        }
                        if (!temEspecial)
                        {
                            Console.WriteLine("\t[ERRO] A senha precisa conter pelo menos um caracter especial!");
                        }
                        Console.ResetColor();
                        VoltarAoMenuSenha();
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n\t[ERRO] A senha precisa conter pelo menos 8 caracteres!");
                    Console.ResetColor();
                    VoltarAoMenuSenha();
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\t[ERRO] Campo inválido!");
                Console.ResetColor();
                VoltarAoMenuSenha();
            }
        }

        // Loop do Menu Principal do Banco
        while (true) // O código volta diretamente para cá quando termina a operação em uma das opções
        {
            int escolha = ValidarEscolha(); // Puxa o subprograma que valida o número digitado no menu

            if (escolha == 4) // Se escolher 4, encerra o programa por este IF
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t===ATÉ A PRÓXIMA!===");
                Console.ResetColor();
                break;
            }

            ProcessarEscolha(escolha, ref saldoAtual); // Processa a opção escolhida e atualiza o saldo por referência
        }
    }

    static int ValidarEscolha() // Subprograma para validar se a opção escolhida no menu é realmente de 1 a 4
    {
        while (true)
        {
            try
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\tESCOLHA UMA DAS OPÇOES");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\n\n1 - ");
                Console.ResetColor();
                Console.WriteLine("VER SALDO");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\n\n2 - ");
                Console.ResetColor();
                Console.WriteLine("DEPOSITAR");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\n\n3 - ");
                Console.ResetColor();
                Console.WriteLine("SACAR");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\n\n4 - ");
                Console.ResetColor();
                Console.WriteLine("SAIR");

                Console.WriteLine("     ");
                Console.Write("OPÇÃO: ");

                int escolha = Convert.ToInt32(Console.ReadLine()); // Converte e valida o número digitado, capturando erros de formato

                if (escolha < 1 || escolha > 4)
                {
                    throw new ArgumentOutOfRangeException(); // Força exceção caso o número esteja fora do intervalo do menu
                }
                return escolha;
            }
            catch (FormatException) // Mostra erro caso o usuário digite letras em vez de números
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\n\n[ERRO] Digite um número válido!");
                Console.ResetColor();
                VoltarAoMenu();
            }
            catch (ArgumentOutOfRangeException) // Mostra erro caso o número seja menor que 1 ou maior que 4
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\n\n[ERRO] Digite um número de 1 a 4.");
                Console.ResetColor();
                VoltarAoMenu();
            }
        }
    }

    static void ProcessarEscolha(int escolha, ref double valor) // Processa a opção escolhida usando switch para direcionar as operações
    {
        Console.Clear();
        switch (escolha)
        {
            case 1: // Consultar saldo
                Console.Write("\tSALDO DISPONÍVEL: R$ ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(valor.ToString("F2", CultureInfo.InvariantCulture)); // Formata com 2 casas decimais e ponto no lugar da vírgula
                Console.ResetColor();
                VoltarAoMenu();
                break;

            case 3: // Realizar Saque
                double valorSaque = ValidarSaque(valor); // Puxa o subprograma de validação de saque
                if (valorSaque == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tOPERAÇÃO CANCELADA!");
                    Console.ResetColor();
                    VoltarAoMenu();
                    break;
                }
                Console.Clear();
                double resultado = CalculoSaldo(valor, valorSaque); // Envia os valores para o subprograma de cálculo
                valor = resultado;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\tSAQUE REALIZADO COM SUCESSO!");
                Console.WriteLine($"\tNovo saldo: R$ {resultado.ToString("F2", CultureInfo.InvariantCulture)}");
                Console.ResetColor();
                VoltarAoMenu();
                break;

            case 2: // Realizar Depósito
                Console.Clear();
                double valorDeposito = ValidarDeposito(valor); // Puxa o subprograma de validação de depósito
                if (valorDeposito == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tOPERAÇÃO CANCELADA!");
                    Console.ResetColor();
                    VoltarAoMenu();
                    break;
                }
                double resultadoDeposito = CalculoSaldoDeposito(valor, valorDeposito); // Envia os valores para o cálculo de depósito
                valor = resultadoDeposito;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\t DEPOSITO REALIZADO COM SUCESSO!");
                Console.WriteLine($"\t Novo saldo: R$ {resultadoDeposito.ToString("F2", CultureInfo.InvariantCulture)}");
                Console.ResetColor();
                VoltarAoMenu();
                break;
        }
    }

    static double ValidarSaque(double saldoDisponivel) // Valida se o valor do saque existe e se está dentro do limite do saldo atual
    {
        while (true)
        {
            try
            {
                Console.Write("\tQUAL VALOR DESEJA SACAR (ou digite 0 para voltar): R$ ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                double saque = Convert.ToDouble(Console.ReadLine());
                Console.ResetColor();

                if (saque == 0) // Retorna ao menu principal se o usuário digitar 0
                {
                    return 0;
                }

                if (saque < 0 || saque > saldoDisponivel) // Impede saques negativos ou maiores que o saldo em conta
                {
                    throw new ArgumentOutOfRangeException();
                }
                return saque;
            }
            catch (FormatException)
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\n\n[ERRO] Digite um número válido!");
                Console.ResetColor();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\n\n[ERRO] Valor inválido! Verifique o saldo disponível!");
                Console.ResetColor();
            }
        }
    }

    static double ValidarDeposito(double saldoDisponivel) // Valida se o depósito inserido é válido e maior que zero
    {
        while (true)
        {
            try
            {
                Console.Write("\tQUAL VALOR DESEJA DEPOSITAR (ou digite 0 para voltar): R$ ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                double deposito = Convert.ToDouble(Console.ReadLine());
                Console.ResetColor();

                if (deposito == 0) // Retorna ao menu principal se digitar 0
                {
                    return 0;
                }
                if (deposito < 0) // Impede valores negativos no depósito
                {
                    throw new ArgumentOutOfRangeException();
                }
                return deposito;
            }
            catch (FormatException)
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\n\n [ERRO] Digite um número válido!");
                Console.ResetColor();
                VoltarAoMenu();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\n\n[ERRO] Valor inválido! Digite um valor maior que zero!");
                Console.ResetColor();
                VoltarAoMenu();
            }
        }
    }

    static double CalculoSaldo(double saldoAtual, double saque) // Faz o cálculo subtraindo o valor do saque do saldo atual
    {
        return saldoAtual - saque;
    }

    static double CalculoSaldoDeposito(double saldoAtual, double deposito) // Faz o cálculo somando o depósito ao saldo atual
    {
        return saldoAtual + deposito;
    }

    static void VoltarAoMenu() // Pausa a tela, aguarda o usuário apertar qualquer tecla e limpa o console para retornar ao menu
    {
        Console.WriteLine("\t\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
        Console.Clear();
    }

    static void VoltarAoMenuSenha() // Pausa a tela especificamente para o erro de digitação de senha
    {
        Console.WriteLine("\t\nPressione qualquer tecla para digitar uma nova senha...");
        Console.ReadKey();
        Console.Clear();
    }
}