using System;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Potager et Cie";
// Augmenter la taille du buffer de la console hauteur à 50lignes ou plus évite les bugs
if (OperatingSystem.IsWindows())
{
    Console.SetBufferSize(Console.WindowWidth, 50); 
}
 
Console.Clear(); 
DessinerTitre();
DessinerPlantes();

Console.SetCursorPosition(0, 25);
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Appuyez sur une touche pour démarrer votre aventure...");
Console.ReadKey();

// Démarrer le jeu avec un nom de joueur
Console.Clear();
DessinerTitre();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.SetCursorPosition(0, 12);
Console.WriteLine("\nBienvenue dans Potager et Cie !");
Console.Write("\nEntrez votre nom de jardinier : ");
string? nomJoueur = Console.ReadLine();
if (string.IsNullOrWhiteSpace(nomJoueur))
{
    nomJoueur = "Jardinier";
}

// Création et lancement de l'interface de jeu
GestionJeu jeu = new GestionJeu(nomJoueur);
jeu.LancerInterfaceConsole();

// Quand le joueur quitte le jeu
Console.Clear();
DessinerTitre();
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"\nMerci d'avoir joué, {nomJoueur}!");
Console.WriteLine("\nRésumé de votre partie :");
Console.WriteLine(jeu.ObtenirResume());
Console.WriteLine("\nAppuyez sur une touche pour quitter...");
Console.ReadKey();


static void DessinerTitre()
{
    string[] titre = {
        @" _____      _                                 _      _____ _      ",
        @"|  __ \    | |                               | |    / ____(_)     ",
        @"| |__) |__ | |_ __ _  __ _  ___ _ __     ___ | |_  | |     _  ___ ",
        @"|  ___/ _ \| __/ _` |/ _` |/ _ \ '__|   / _ \| __| | |    | |/ _ \",
        @"| |  | (_) | || (_| | (_| |  __/ |     |  __/| |_  | |____| |  __/",
        @"|_|   \___/ \__\__,_|\__, |\___|_|      \___| \__|  \_____|_|\___|",
        @"                      __/ |                                       ",
        @"                     |___/                                        "
    };

    Console.ForegroundColor = ConsoleColor.Green;
    
    for (int i = 0; i < titre.Length; i++)
    {
        Console.SetCursorPosition((Console.WindowWidth - titre[i].Length) / 2, i + 2);
        Console.WriteLine(titre[i]);
        Thread.Sleep(100); 
    }
}

static void DessinerPlantes()
{
    DessinerTournesol(15, 12);
    DessinerTournesol(90,12);
    DessinerRose(30, 12);
    DessinerRose(105,12);
    DessinerTulipe(45, 12);
    DessinerTulipe(120,12);
    DessinerMarguerite(60, 12);
    DessinerArbre(75, 12);
    DessinerSol();
    
    // Message
    Console.ForegroundColor = ConsoleColor.Cyan;
    string message = " Bienvenue dans votre potager virtuel !";
    Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, 20);
    Console.WriteLine(message);
}

static void DessinerTournesol(int x, int y)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.SetCursorPosition(x-1, y-1);
    Console.Write("\\|/");
    Console.SetCursorPosition(x-1, y);
    Console.Write("-O-");
    Console.SetCursorPosition(x-1, y+1);
    Console.Write("/|\\");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(x, y+2);
    Console.Write("|");
    Console.SetCursorPosition(x, y+3);
    Console.Write("|");
    Console.SetCursorPosition(x-1, y+4);
    Console.Write("\\|/");
}

static void DessinerRose(int x, int y)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.SetCursorPosition(x-1, y-1);
    Console.Write("@@@");
    Console.SetCursorPosition(x-1, y);
    Console.Write("@@@");
    Console.SetCursorPosition(x-1, y+1);
    Console.Write("@@@");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(x, y+2);
    Console.Write("|");
    Console.SetCursorPosition(x-1, y+2);
    Console.Write("/");
    Console.SetCursorPosition(x, y+3);
    Console.Write("|");
    Console.SetCursorPosition(x-1, y+4);
    Console.Write("\\|/");
}

static void DessinerTulipe(int x, int y)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.SetCursorPosition(x-1, y-1);
    Console.Write("/ \\");
    Console.SetCursorPosition(x-1, y);
    Console.Write("| |");
    Console.SetCursorPosition(x-1, y+1);
    Console.Write("\\_/");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(x, y+2);
    Console.Write("|");
    Console.SetCursorPosition(x, y+3);
    Console.Write("|");
    Console.SetCursorPosition(x-1, y+4);
    Console.Write("\\|/");
}

static void DessinerMarguerite(int x, int y)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.SetCursorPosition(x-1, y-1);
    Console.Write("* *");
    Console.SetCursorPosition(x-1, y);
    Console.Write(" O ");
    Console.SetCursorPosition(x-1, y+1);
    Console.Write("* *");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.SetCursorPosition(x, y+2);
    Console.Write("|");
    Console.SetCursorPosition(x, y+3);
    Console.Write("|");
    Console.SetCursorPosition(x-1, y+4);
    Console.Write("\\|/");
}

static void DessinerArbre(int x, int y)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.SetCursorPosition(x-2, y-1);
    Console.Write("/  \\");
    Console.SetCursorPosition(x-3, y);
    Console.Write("/    \\");
    Console.SetCursorPosition(x-4, y+1);
    Console.Write("/      \\");
    Console.SetCursorPosition(x-3, y+2);
    Console.Write("\\______/");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.SetCursorPosition(x, y+3);
    Console.Write("||");
    Console.SetCursorPosition(x, y+4);
    Console.Write("||");
    Console.SetCursorPosition(x-1, y+5);
    Console.Write("/||\\");
}

static void DessinerSol()
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    for (int i = 10; i < Console.WindowWidth - 10; i += 2)
    {
        Console.SetCursorPosition(i, 17);
        Console.Write("ω");
    }
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    for (int i = 5; i < Console.WindowWidth - 5; i++)
    {
        Console.SetCursorPosition(i, 18);
        Console.Write("▀");
    }
    
    Console.ForegroundColor = ConsoleColor.DarkGray;
    for (int i = 5; i < Console.WindowWidth - 5; i++)
    {
        Console.SetCursorPosition(i, 19);
        Console.Write("▄");
    }
}
