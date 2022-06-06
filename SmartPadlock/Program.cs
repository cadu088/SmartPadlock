using SmartPadlock.Classes;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

string name_user = "";
DateTime nascimento_user;
string email_user = "";
string password_user = "";
Login login = new Login();
User user = new User();
createUser createUser = new createUser();
viewAplication view = new viewAplication();
List<Aplication> aplications = new List<Aplication>();



Console.WriteLine("|-| Bem vindo ao SmartPadlock");
Console.WriteLine("|-|");
while (!login.status)
{
    string escolha = "";
    while (escolha != "L" && escolha != "S")
    {
        Console.WriteLine("|-| Escolha uma opção:");
        Console.WriteLine("|-| -> 'L' para uma conta existente.");
        Console.WriteLine("|-| -> 'S' para uma nova conta.");
        Console.WriteLine("|-|");
        Console.Write("|-| ");
        escolha = Console.ReadLine();
    }


    if (escolha == "L")
    {
        //Login
        var aplicationControler = new aplicationControler();
        Console.Write("|-| Digite o nome completo sem abreviações: ");
        name_user = Console.ReadLine();
        bool verificaEmail = false;
        while (!verificaEmail)
        {
            Console.Write("|-| Digite seu email: ");
            email_user = Console.ReadLine();
            verificaEmail = email_user.Contains("@");
        }
        Console.Write("|-| Digite sua data de aniversario. Ex: 28/08/2002: ");
        string birth = Console.ReadLine();
        nascimento_user = new DateTime(int.Parse(birth.Split('/')[2]), int.Parse(birth.Split('/')[1]), int.Parse(birth.Split('/')[0]));
        user.Create(name_user, nascimento_user, email_user, password_user);




        if (user.BuscarUser(false))
        {
            Console.Clear();
            Console.WriteLine("| Usuario encontrado com sucesso!");
            Console.WriteLine("|-|-----------------------------|-|");
            Console.WriteLine("|-|");
            if (await login.CreateAsync(user))
            {
                Console.WriteLine("|-| Login efetuado com sucesso!");
                aplicationControler.login = true;
                login.status = true;
            }
            else
            {
                Console.WriteLine("|-| Falha ao efetuar login!");
                aplicationControler.login = false;
                login.status = false;
            };
        }
        else
        {
            Console.Clear();
            Console.WriteLine("|-| Usuario não encontrado!");
        }
    }
    else
    {
        //Sign In
        var aplicationControler = new aplicationControler();
        Console.Write("|-| Digite o nome completo sem abreviações: ");
        name_user = Console.ReadLine();
        bool verificaEmail = false;
        while (!verificaEmail)
        {
            Console.Write("|-| Digite seu email: ");
            email_user = Console.ReadLine();
            verificaEmail = email_user.Contains("@");
        }
        Console.Write("|-| Digite sua data de aniversario. Ex: 28/08/2002: ");
        string birth = Console.ReadLine();
        nascimento_user = new DateTime(int.Parse(birth.Split('/')[2]), int.Parse(birth.Split('/')[1]), int.Parse(birth.Split('/')[0]));
        user.Create(name_user, nascimento_user, email_user, password_user);

        if (!user.BuscarUser(false))
        {
            if (createUser.Create(user))
            {
                Console.Clear();
                Console.WriteLine("| Usuario criado com sucesso! ");
                Console.WriteLine("|-|-------------------------|-|");
                aplicationControler.login = true;
                login.status = true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("|-| Falha ao criar usuario, tente novamente mais tarde...");
                Console.WriteLine("|-| :(");
                aplicationControler.login = false;
                login.status = false;
            };
        }
        else
        {
            Console.Clear();
            Console.WriteLine("|-| Usuario já existente!");
            Console.WriteLine("|-| Efetue login!");
        }

    }
}

Console.Clear();
await view.createViewAsync(login, user);
Console.WriteLine("|-| Bem vindo " + user.NAME_USER.Split(' ')[0] + "!");

//1 - visualizar 
//2 - adicionar
//3 - sair

int menu = 0;
while (menu != 3)
{
    Console.WriteLine("|-|_________________________________________________________________________________|-|");
    Console.WriteLine("|-|Para visualizar opção: |1| Para adicionar nova senha opção: |2| Para sair opção: |3|");
    Console.Write("--> ");
    menu = int.Parse(Console.ReadLine());
    switch (menu)
    {
        case 1:
            {
                aplications = view.view(login);
                if (aplications == null)
                {
                    Console.WriteLine("|-| Nenhuma aplicação localizada!");
                }
                else
                {
                    Console.WriteLine("|-|");
                    string vizualiza = "";
                    while (vizualiza != "cancelar")
                    {
                        Console.WriteLine("|-| Escolha qual senha deseja visualizar: ");

                        int indexApps = 1;
                        foreach (var apps in aplications)
                        {
                            Console.WriteLine("|-| " + indexApps + " | " + apps.APP_ID);
                            indexApps++;
                        }
                        Console.WriteLine("|-| Digite o numero para visualizar ou '-1' para sair.");
                        Console.Write("--> ");
                        int escolha = int.Parse(Console.ReadLine()) - 1;
                        if (escolha < aplications.Count() && escolha != -2)
                        {
                            Console.WriteLine("|-| Escolha um elemento.");
                        }
                        if (escolha == -2)
                        {
                            vizualiza = "cancelar";
                        }
                        else
                        {

                            aplications[escolha].ReadingCliente();
                            string configs = "";
                            while (configs != "E" && configs != "S")
                            {
                                Console.WriteLine("|-|");
                                Console.WriteLine("|-|__________________________________________|-|");
                                Console.WriteLine("|-| Para exluir o registro 'E' ou 'S' para sair.");
                                Console.Write("--> ");
                                configs = Console.ReadLine();
                            }

                            switch (configs)
                            {
                                case "E":
                                    aplications = view.removeAplication(login, aplications[escolha]);
                                    Console.WriteLine("|-| Senha exluida!");
                                    break;
                                case "S":
                                    Console.Clear();
                                    break;
                                default:
                                    Console.WriteLine("|-| Desculpe não entendi! :( ");
                                    break;
                            }
                        }
                    }
                }
            }
            break;
        case 2:
            {
                Console.WriteLine("|-| Para adicionar uma nova senha siga os passos:");
                Console.Write("|-| Qual o nome da aplicação? ");
                Console.Write("--> ");
                string nameApp = Console.ReadLine();
                Console.Write("|-| Agora adicione a URL:");
                Console.Write("--> ");
                string urlApp = Console.ReadLine();
                Console.Write("|-| Para finalizar digite a senha da aplicação: ");
                Console.Write("--> ");
                string senhaApp = Console.ReadLine();
                Aplication novaApp = new Aplication();
                novaApp.AplicationCreate(nameApp, urlApp, senhaApp);
                aplications = view.addAplication(login, novaApp);
                Console.WriteLine("|-|");
                Console.WriteLine("|-| Nova aplicação adicionada com sucesso!");
            }
            break;
        case 3:
            {
                string confirm = "";
                while (confirm != "S" && confirm != "N")
                {
                    Console.Clear();
                    Console.WriteLine("|-| Deseja mesmo sair? Caso isso ocorra as alterações serão salvas em seu perfil!");
                    confirm = Console.ReadLine();
                    if (confirm == "S")
                    {
                        view.saveAplication(login, user);
                    }
                }
            }
            break;
        default :
            Console.WriteLine("|-|");
            Console.WriteLine("|-| Não entendi, poderia repetir? :)");
            break;
    }

}









//aplicationControler.CriptografarUser(new Contract(usuario, false), new Contract(usuario, true), ""