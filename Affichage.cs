using System;
using System.Threading;

//Classe responsable de l'affichage visuel du terrain et des éléments du jeu
public static class AffichageTerrain
{ 
    //Couleurs pour différents types de terrain
    private static readonly ConsoleColor CouleurSable = ConsoleColor.Yellow;
    private static readonly ConsoleColor CouleurArgile = ConsoleColor.DarkYellow;
    private static readonly ConsoleColor CouleurMarecage = ConsoleColor.DarkCyan;
    
    //Affiche le terrain avec les plantes
    public static void AfficherTerrain(Terrain terrain)
    {
        Console.Clear();
        AfficherEntete(terrain);
        
        //Affiche un cadre pour le terrain
        AfficherCadreTerrain(terrain.Largeur, terrain.Hauteur);
        
        //Affiche les parcelles avec leurs plantes
        for (int y = 0; y < terrain.Hauteur; y++)
        {
            Console.SetCursorPosition(2, y + 5);
            for (int x = 0; x < terrain.Largeur; x++)
            {
                // Détermine la couleur de fond selon le type de terrain
                DefiniCouleurTerrain(terrain.TypeTerrain);

                string visuel = terrain.Grille[x, y].ObtenirVisuel();

                // 👇 Affichage priorité animal
                if (terrain.Grille[x, y].AnimalCourant != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; // 🐝 visible
                }
                else if (!terrain.Grille[x, y].VerifierEstVide())
                {
                    if (terrain.Grille[x, y].VerifierEstMalade())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        double sante = terrain.Grille[x, y].ObtenirSante();
                        if (sante > 75)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (sante > 50)
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        else if (sante > 25)
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray; // Parcelle vide
                }

                Console.Write(visuel + " ");
            }
            Console.ResetColor();
        }
        
        //Affiche les informations sur les conditions actuelles
        AfficherConditionsActuelles(terrain);
    }
    
    //Affiche l'entête avec le nom du terrain
    private static void AfficherEntete(Terrain terrain)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        string titre = $"Terrain: {terrain.Nom} ({terrain.Region}) - {terrain.TypeTerrain}";
        Console.SetCursorPosition((Console.WindowWidth - titre.Length) / 2, 1);
        Console.WriteLine(titre);
        Console.ResetColor();
    }
    
    //Affiche un cadre autour du terrain
    private static void AfficherCadreTerrain(int largeur, int hauteur)
    {
        Console.ForegroundColor = ConsoleColor.White;
        
        //Ligne du haut
        Console.SetCursorPosition(1, 4);
        Console.Write("┌");
        for (int i = 0; i < largeur * 2; i++)
        {
            Console.Write("─");
        }
        Console.Write("┐");
        
        //Lignes verticales
        for (int y = 0; y < hauteur; y++)
        {
            Console.SetCursorPosition(1, y + 5);
            Console.Write("│");
            Console.SetCursorPosition(2 + largeur * 2, y + 5);
            Console.Write("│");
        }
        
        //Ligne du bas
        Console.SetCursorPosition(1, hauteur + 5);
        Console.Write("└");
        for (int i = 0; i < largeur * 2; i++)
        {
            Console.Write("─");
        }
        Console.Write("┘");
        
        Console.ResetColor();
    }
    
    //Définit la couleur de fond selon le type de terrain
    private static void DefiniCouleurTerrain(string typeTerrain)
    {
        switch (typeTerrain.ToLower())
        {
            case "sable":
                Console.BackgroundColor = CouleurSable;
                break;
            case "argile":
                Console.BackgroundColor = CouleurArgile;
                break;
            case "marécage":
                Console.BackgroundColor = CouleurMarecage;
                break;
            default:
                Console.BackgroundColor = ConsoleColor.DarkGray; //Par défaut pour les autres types
                break;
        }
    }
    
    //Affiche les informations sur les conditions actuelles du terrain
    private static void AfficherConditionsActuelles(Terrain terrain)
    {
        int ligneDepart = terrain.Hauteur + 7;
        
        Console.SetCursorPosition(2, ligneDepart);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Conditions actuelles:");
        
        Console.SetCursorPosition(2, ligneDepart + 1);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"Température: ");
        
        //Affichage avec couleur selon la température
        if (terrain.Temperature < 5)
            Console.ForegroundColor = ConsoleColor.Blue;
        else if (terrain.Temperature < 15)
            Console.ForegroundColor = ConsoleColor.Cyan;
        else if (terrain.Temperature < 25)
            Console.ForegroundColor = ConsoleColor.Green;
        else if (terrain.Temperature < 30)
            Console.ForegroundColor = ConsoleColor.Yellow;
        else
            Console.ForegroundColor = ConsoleColor.Red;
            
        Console.WriteLine($"{terrain.Temperature:F1}°C");
        
        Console.SetCursorPosition(2, ligneDepart + 2);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"Humidité: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{terrain.NiveauHumidite:F1}%");
        
        Console.SetCursorPosition(2, ligneDepart + 3);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"Ensoleillement: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{terrain.NiveauSoleil:F1}%");
        
        Console.SetCursorPosition(2, ligneDepart + 4);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"pH du sol: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{terrain.PH:F1}");

        Console.ResetColor();
    }
    
    //Afficher les détails d'une plante sélectionnée
    public static void AfficherDetailPlante(Terrain terrain, int x, int y)
    {
        if (x < 0 || x >= terrain.Largeur || y < 0 || y >= terrain.Hauteur)
            return;
            
        int ligneDepart = terrain.Hauteur + 12;
        
        Console.SetCursorPosition(2, ligneDepart);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Détails de la parcelle:");
        
        Console.SetCursorPosition(2, ligneDepart + 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(terrain.Grille[x, y].ObtenirInfoPlante());
        
        Console.ResetColor();
    }
    
    //Affiche le menu principal du jeu
    public static void AfficherMenuPrincipal()
    {
        int menuX = Console.WindowWidth - 40;
        int menuY = 5;
        
        Console.SetCursorPosition(menuX, menuY);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("╔════════════════════════════════╗");
        Console.SetCursorPosition(menuX, menuY + 1);
        Console.WriteLine("║          MENU PRINCIPAL        ║");
        Console.SetCursorPosition(menuX, menuY + 2);
        Console.WriteLine("╠════════════════════════════════╣");
        Console.SetCursorPosition(menuX, menuY + 3);
        Console.WriteLine("║ 1. Planter une plante          ║");
        Console.SetCursorPosition(menuX, menuY + 4);
        Console.WriteLine("║ 2. Arroser une parcelle        ║");
        Console.SetCursorPosition(menuX, menuY + 5);
        Console.WriteLine("║ 3. Soigner une plante          ║");
        Console.SetCursorPosition(menuX, menuY + 6);
        Console.WriteLine("║ 4. Récolter une plante         ║");
        Console.SetCursorPosition(menuX, menuY + 7);
        Console.WriteLine("║ 5. Enlever une plante          ║");
        Console.SetCursorPosition(menuX, menuY + 8);
        Console.WriteLine("║ 6. Progression d'une semaine   ║");
        Console.SetCursorPosition(menuX, menuY + 9);
        Console.WriteLine("║ 7. Changer de saison           ║");
        Console.SetCursorPosition(menuX, menuY + 10);
        Console.WriteLine("║ 0. Quitter                     ║");
        Console.SetCursorPosition(menuX, menuY + 11);
        Console.WriteLine("╚════════════════════════════════╝");
        
        Console.ResetColor();
    }
}