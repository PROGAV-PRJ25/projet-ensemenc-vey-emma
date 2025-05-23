using System;
using System.Collections.Generic;
using System.Threading;   //🌱 🥬🪴🌿🌼🌸🥀🍓🧺🍅🟫🌾🌽🍇🍎🥒🍉🌳🍂

public class GestionJeu
{
    //Propriétés publiques en lecture seule (sauf visuelCase)
    public Terrain TerrainActuel { get; private set; }
    public string SaisonActuelle { get; private set; }
    public int SemaineActuelle { get; private set; }
    public int AnneeActuelle { get; private set; }
    public Dictionary<string, int> Inventaire { get; private set; }
    public int PointsExperience { get; private set; }
    public string NomJoueur { get; private set; }
    public string visuelCase = "░░";

    //pour les saisons et intemperies
    public Economie Economie { get; private set; }
    private Meteo.TypeIntemperie derniereIntemperie = Meteo.TypeIntemperie.Normale;

    private readonly string[] Saisons = { "Printemps", "Ete", "Automne", "Hiver" };
    private readonly int SemainesParSaison = 13; //~3 mois

    //Constructeur
    public GestionJeu(string nomJoueur)
    {
        NomJoueur = nomJoueur;
        Inventaire = new Dictionary<string, int>();
        PointsExperience = 0;
        SemaineActuelle = 1;
        AnneeActuelle = 1;
        SaisonActuelle = "Printemps"; //On commence au printemps
        //Initialisation de l'économie
        Economie = new Economie();
        //Initialisation par défaut avec un terrain de taille moyenne
        InitialiserTerrain("Mon Potager", "Centre", "Terre", 10, 5);

        //Garantir que TerrainActuel n'est jamais null
        if (TerrainActuel == null)
        {
            TerrainActuel = new TerrainArgileux("Mon Potager", "Centre", 0.05, 10, 5);
        }
        Inventaire["Rose (graine)"] = 2;
        Inventaire["Tomate (graine)"] = 3;
        Inventaire["Carotte (graine)"] = 2;
        StatistiquesAnimaux = new Dictionary<string, int>
        {
            { "Abeilles", 0 },
            { "Coccinelles", 0 },
            { "Escargots", 0 },
            { "Taupes", 0 }
        };
    }

    //Initialise un nouveau terrain avec les paramètres spécifiés
    public void InitialiserTerrain(string nom, string region, string typeTerrain, int largeur, int hauteur)
    {
        switch (typeTerrain.ToLower())
        {
            case "sable":
                TerrainActuel = new TerrainSable(nom, region, largeur * hauteur * 0.01, largeur, hauteur); //0.01 hectare par parcelle
                break;
            case "argile":
                TerrainActuel = new TerrainArgileux(nom, region, largeur * hauteur * 0.01, largeur, hauteur);
                break;
            case "marécage":
            case "marecage":
                TerrainActuel = new TerrainMarecageux(nom, region, largeur * hauteur * 0.01, largeur, hauteur);
                break;
            default:
                //Terrain par défaut - on pourrait créer une classe TerrainTerre si nécessaire
                TerrainActuel = new TerrainArgileux(nom, region, largeur * hauteur * 0.01, largeur, hauteur);
                break;
        }
    }

    //Plante une plante à une position spécifique
    public bool PlanterPlante(Plante plante, int x, int y)
    {
        if (TerrainActuel.PlanterPlante(plante, x, y))
        {
            PointsExperience += 5; //Gain d'XP pour chaque plantation réussie
            return true;
        }
        return false;
    }

    //Avance le temps d'une semaine
    public void PasserSemaine()
    {
        //Progression du temps
        SemaineActuelle++;
        if (SemaineActuelle > SemainesParSaison)
        {
            SemaineActuelle = 1;
            ChangerSaison();
        }
        //Génération et application d'intempérie
        derniereIntemperie = Meteo.GenererIntemperie(SaisonActuelle);
        if (derniereIntemperie != Meteo.TypeIntemperie.Normale)
        {
            Console.WriteLine("\n🌤️ === BULLETIN MÉTÉO ===");
            Meteo.AppliquerIntemperie(TerrainActuel, derniereIntemperie);
            Thread.Sleep(2000);
        }
        //Application de la progression des plantes
        TerrainActuel.ProgresserSemaine(SaisonActuelle);

        //Apparition aléatoire d'animaux avec nouvelles espèces
        Random rnd = new Random();

        //Abeille (1 chance sur 6)
        if (rnd.Next(1, 7) == 1)
        {
            StatistiquesAnimaux["Abeilles"]++;
            var abeille = new Abeille(TerrainActuel, this);
            abeille.Agir();
        }

        //Coccinelle (1 chance sur 6) - bénéfique
        if (rnd.Next(1, 7) == 2)
        {
            StatistiquesAnimaux["Coccinelle"]++;
            var coccinelle = new Coccinelle(TerrainActuel, this);
            coccinelle.Agir();
        }

        //Escargot (1 chance sur 6)
        if (rnd.Next(1, 7) == 3)
        {
            StatistiquesAnimaux["Escargot"]++; 
            var escargot = new Escargot(TerrainActuel, this);
            escargot.Agir();
        }

        //Taupe (1 chance sur 6)
        if (rnd.Next(1, 7) == 4)
        {
            StatistiquesAnimaux["Taupe"]++;
            var taupe = new Taupe(TerrainActuel, this);
            taupe.Agir();
        }
    }

    //Change la saison actuelle
    private void ChangerSaison()
    {
        int indexActuel = Array.IndexOf(Saisons, SaisonActuelle);
        indexActuel = (indexActuel + 1) % Saisons.Length;
        SaisonActuelle = Saisons[indexActuel];

        //Si on revient au printemps, nouvelle année
        if (indexActuel == 0)
        {
            AnneeActuelle++;
        }
    }

    //Actions sur les parcelles

    //Arrose une parcelle
    public void ArroserParcelle(int x, int y)
    {
        TerrainActuel.ArroserParcelle(x, y);
        PointsExperience += 1;
    }

    //Soigne une plante malade
    public void SoignerPlante(int x, int y)
    {
        TerrainActuel.SoignerPlante(x, y);
        PointsExperience += 3;
    }

    //Récolte les produits d'une parcelle
    public int RecolterParcelle(int x, int y)
    {
        int recolte = TerrainActuel.RecolterParcelle(x, y);
        if (recolte > 0)
        {
            //nom de la plante pour la vente
            string nomPlante = "Produit"; //défau
            if (!TerrainActuel.Grille[x, y].EstVide())
            {
                nomPlante = TerrainActuel.Grille[x, y].PlanteCourante?.Nom ?? "Produit";
            }

            //ajout à l'inventaire
            if (!Inventaire.ContainsKey(nomPlante))
                Inventaire[nomPlante] = 0;

            Inventaire[nomPlante] += recolte;
            PointsExperience += recolte * 2;

            Console.WriteLine($"💰 Vous avez récolté {recolte} {nomPlante}(s) !");
        }
        return recolte;
    }


    //Enlève une plante
    public bool EnleverPlante(int x, int y)
    {
        return TerrainActuel.EnleverPlante(x, y);
    }
    //Nouvelle méthode pour vendre des produits
    public void VendreProduits()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("💰 === VENTE DE PRODUITS ===");
        Console.WriteLine($"Argent actuel: {Economie.ArgentJoueur}€");
        Console.WriteLine("\nVotre inventaire:");

        if (Inventaire.Count == 0)
        {
            Console.WriteLine("Inventaire vide !");
            Thread.Sleep(1500);
            return;
        }

        foreach (var item in Inventaire)
        {
            if (item.Value > 0)
            {
                int prixUnitaire = Economie.PrixVente.ContainsKey(item.Key) ? Economie.PrixVente[item.Key] : 2;
                Console.WriteLine($"{GetEmojiPlante(item.Key)} {item.Key}: {item.Value} unités ({prixUnitaire}€/unité)");
            }
        }

        Console.Write("\nQue voulez-vous vendre (nom de la plante) ? ");
        string? choix = Console.ReadLine();

        if (!string.IsNullOrEmpty(choix) && Inventaire.ContainsKey(choix) && Inventaire[choix] > 0)
        {
            Console.Write($"Combien de {choix} voulez-vous vendre ? (max: {Inventaire[choix]}) ");
            if (int.TryParse(Console.ReadLine(), out int quantite) && quantite > 0 && quantite <= Inventaire[choix])
            {
                int gain = Economie.VendreProduit(choix, quantite);
                Inventaire[choix] -= quantite;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Vendu {quantite} {choix}(s) pour {gain}€ !");
                PointsExperience += gain / 2;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Quantité invalide !");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Produit non trouvé ou stock épuisé !");
        }

        Console.ResetColor();
        Thread.Sleep(2000);
    }

    //Nouvelle méthode pour le magasin
    public void AfficherMagasin()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("🏪 === MAGASIN ===");
        Console.WriteLine($"💰 Votre argent: {Economie.ArgentJoueur}€");
        Console.WriteLine("\n📦 GRAINES DISPONIBLES:");

        foreach (var item in Economie.PrixAchat)
        {
            Console.WriteLine($"  {GetEmojiPlante(item.Key)} {item.Key}: {item.Value}€");
        }

        Console.Write("\nQuelle graine voulez-vous acheter ? (nom complet ou 'q' pour quitter) ");
        string? choix = Console.ReadLine();

        if (choix?.ToLower() == "q") return;

        if (!string.IsNullOrEmpty(choix) && Economie.AcheterGraine(choix))
        {
            //Ajouter la graine à l'inventaire
            string nomGraine = choix + " (graine)";
            if (!Inventaire.ContainsKey(nomGraine))
                Inventaire[nomGraine] = 0;
            Inventaire[nomGraine]++;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ Graine de {choix} achetée ! Argent restant: {Economie.ArgentJoueur}€");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Achat impossible ! (argent insuffisant ou plante inconnue)");
        }

        Console.ResetColor();
        Thread.Sleep(2000);
    }
    //Renvoie un résumé de l'état du jeu
    public string ObtenirResume()
    {
        string resume = $"👤 {NomJoueur} - Année {AnneeActuelle}, {SaisonActuelle} (Semaine {SemaineActuelle})\n";
        resume += "═══════════════════════════════════════════════════════════\n";
        resume += TerrainActuel.ObtenirResume();
        resume += "\n🎒 Inventaire:\n";
        resume += "\n🐾 Statistiques animaux:\n";
        foreach (var stat in StatistiquesAnimaux)
        {
            resume += $"   {GetEmojiAnimal(stat.Key)} {stat.Key}: {stat.Value} apparitions\n";
        }

        if (Inventaire.Count == 0)
        {
            resume += "   Vide\n";
        }
        else
        {
            foreach (var item in Inventaire)
            {
                resume += $"   📦 {item.Key}: {item.Value}\n";
            }
        }

        resume += $"\n⭐ Expérience: {PointsExperience} points\n";
        resume += $"💰 Argent: {Economie.ArgentJoueur}€\n";

        return resume;
    }

    //Ajoute une nouvelle fonctionnalité pour afficher la grille du terrain en mode console
    public void AfficherTerrainConsole()
    {
        Console.Clear();

        //Affiche le titre avec un style plus esthétique
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                              🌱 Potager et Cie 🌱                             ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════╝");

        //Informations du jeu
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"👤 Jardinier: {NomJoueur}");
        Console.WriteLine($"📅 Année {AnneeActuelle} - {GetSaisonEmoji(SaisonActuelle)} {SaisonActuelle} (Semaine {SemaineActuelle})");
        Console.WriteLine($"🏞️  Terrain: {TerrainActuel.Nom} - {GetTerrainEmoji(TerrainActuel.TypeTerrain)}Type: {TerrainActuel.TypeTerrain}");
        Console.WriteLine($"🌡️  {TerrainActuel.Temperature}°C | 💧 {TerrainActuel.NiveauHumidite}% | ☀️  {TerrainActuel.NiveauSoleil}%");
        Console.WriteLine();
        if (derniereIntemperie != Meteo.TypeIntemperie.Normale)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"🌤️  Météo: {GetIntempErieEmoji(derniereIntemperie)} {derniereIntemperie}");
        }
        //Affiche la grille avec des bordures
        Console.ForegroundColor = ConsoleColor.White;

        //Indique les coordonnées X en haut
        Console.Write("    ");
        for (int x = 0; x < TerrainActuel.Largeur; x++)
        {
            Console.Write($"{x:D2} ");
        }
        Console.WriteLine();

        //Bordure supérieure
        Console.Write("   ╔");
        for (int x = 0; x < TerrainActuel.Largeur; x++)
        {
            Console.Write("══");
            if (x < TerrainActuel.Largeur - 1) Console.Write("╤");
        }
        Console.WriteLine("╗");

        //Affiche la grille du terrain
        for (int y = 0; y < TerrainActuel.Hauteur; y++)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{y:D2} ║");

            for (int x = 0; x < TerrainActuel.Largeur; x++)
            {
                if (TerrainActuel.Grille[x, y].EstVide())
                {
                    // Vérifier s'il y a un animal sur une case vide
                    if (TerrainActuel.Grille[x, y].AnimalCourant != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Couleur vive pour les animaux
                        Console.Write($"{TerrainActuel.Grille[x, y].AnimalCourant.ObtenirVisuel()}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("░░");
                    }
                }
                else
                {
                    // Priorité aux animaux même sur une plante
                    if (TerrainActuel.Grille[x, y].AnimalCourant != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan; // Couleur spéciale animal sur plante
                        Console.Write($"{TerrainActuel.Grille[x, y].AnimalCourant.ObtenirVisuel()}");
                    }
                    else
                    {
                        // Code existant pour les plantes...
                        double sante = TerrainActuel.Grille[x, y].ObtenirSante();

                        if (TerrainActuel.Grille[x, y].EstMalade())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (sante > 75)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (sante > 50)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else if (sante > 25)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }

                        Console.Write($"{TerrainActuel.Grille[x, y].ObtenirVisuel()}");
                    }
                }

                if (x < TerrainActuel.Largeur - 1)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("│");
                }

            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("║");

            //Ligne séparatrice entre les lignes
            if (y < TerrainActuel.Hauteur - 1)
            {
                Console.Write("   ╟");
                for (int x = 0; x < TerrainActuel.Largeur; x++)
                {
                    Console.Write("──");
                    if (x < TerrainActuel.Largeur - 1) Console.Write("┼");
                }
                Console.WriteLine("╢");
            }
        }

        //Bordure inférieure
        Console.Write("   ╚");
        for (int x = 0; x < TerrainActuel.Largeur; x++)
        {
            Console.Write("══");
            if (x < TerrainActuel.Largeur - 1) Console.Write("╧");
        }
        Console.WriteLine("╝");

        //Légende
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n📊 État des plantes:");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("🟢 Bonne santé  ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("🟡 Moyenne  ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("🟠 Faible  ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("🔴 Malade");

        //Menu des commandes avec emojis
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n🎮 COMMANDES:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("💧 (A)rroser    💊 (S)oigner    🌾 (R)écolter    ❌ (E)nlever");
        Console.WriteLine("🌱 (P)lanter    ⏰ (T)emps +1    🏪 (M)agasin    💰 (V)endre");
        Console.WriteLine("👋 (Q)uitter");
        Console.WriteLine();

        //Afficher l'inventaire et l'XP
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"🎒 Inventaire: {GetInventaireResume()}");
        Console.WriteLine($"⭐ Expérience: {PointsExperience} points | 💰 Argent: {Economie.ArgentJoueur}€");
    }

    //Méthodes auxiliaires pour les emojis
    private string GetSaisonEmoji(string saison)
    {
        switch (saison.ToLower())
        {
            case "printemps": return "🌸";
            case "ete": return "☀️";
            case "automne": return "🍂";
            case "hiver": return "❄️";
            default: return "🌱";
        }
    }

    private string GetTerrainEmoji(string typeTerrain)
    {
        switch (typeTerrain.ToLower())
        {
            case "sable": return "🏜️ ";
            case "argile": return "🏺 ";
            case "marécage": return "🌿 ";
            case "marecage": return "🌿 ";
            case "terre": return "🌾 ";
            default: return "🌍 ";
        }
    }

    private string GetInventaireResume()
    {
        if (Inventaire.Count == 0)
            return "Vide";

        string resume = "";
        int count = 0;
        foreach (var item in Inventaire)
        {
            if (item.Value > 0 && count < 3) //on limite l'affichage
            {
                resume += $"{item.Key}:{item.Value} ";
                count++;
            }
        }
        if (Inventaire.Count > 3) resume += "...";
        return resume;
    }
    //méthode qui permet de gérer l'interface utilisateur en mode console
    public void LancerInterfaceConsole()
    {
        bool continuer = true;

        while (continuer)
        {
            AfficherTerrainConsole();

            Console.Write("\nAction > ");
            string? commande = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrEmpty(commande))
            {
                continue;
            }

            switch (commande)
            {
                case "A": //Arroser
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("💧 Arrosage...");
                    Thread.Sleep(500);
                    ExecuterActionSurParcelle("Arroser");
                    break;

                case "S": //Soigner
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("💊 Soin des plantes...");
                    Thread.Sleep(500);
                    ExecuterActionSurParcelle("Soigner");
                    break;

                case "R": //Récolter
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("🌾 Récolte...");
                    Thread.Sleep(500);
                    ExecuterActionSurParcelle("Récolter");
                    break;

                case "E": //Enlever
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Enlèvement de plante...");
                    Thread.Sleep(500);
                    ExecuterActionSurParcelle("Enlever");
                    break;

                case "P": //Planter
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("🌱 Plantation...");
                    Thread.Sleep(500);
                    ExecuterActionSurParcelle("Planter");
                    break;

                case "T": //Avancer le temps
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("⏰ Le temps passe...");
                    Thread.Sleep(500);
                    PasserSemaine();
                    Console.WriteLine($"📅 Nous sommes maintenant en {GetSaisonEmoji(SaisonActuelle)} {SaisonActuelle}, semaine {SemaineActuelle}");
                    Thread.Sleep(1000);
                    break;

                case "M": //Magasin
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("🏪 Accès au magasin...");
                    Thread.Sleep(500);
                    AfficherMagasin();
                    break;

                case "V": //Vendre
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("💰 Vente de produits...");
                    Thread.Sleep(500);
                    VendreProduits();
                    break;

                case "Q": //Quitter
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("👋 Au revoir!");
                    Thread.Sleep(500);
                    continuer = false;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Commande non reconnue.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    //Méthode auxiliaire pour exécuter une action sur une parcelle spécifique
    private void ExecuterActionSurParcelle(string action)
    {
        try
        {
            Console.Write("Position X (0-" + (TerrainActuel.Largeur - 1) + "): ");
            string? xInput = Console.ReadLine();
            if (string.IsNullOrEmpty(xInput))
            {
                Console.WriteLine("Position invalide!");
                Thread.Sleep(1000);
                return;
            }
            int x = int.Parse(xInput);

            Console.Write("Position Y (0-" + (TerrainActuel.Hauteur - 1) + "): ");
            string? yInput = Console.ReadLine();
            if (string.IsNullOrEmpty(yInput))
            {
                Console.WriteLine("Position invalide!");
                Thread.Sleep(1000);
                return;
            }
            int y = int.Parse(yInput);

            if (x < 0 || x >= TerrainActuel.Largeur || y < 0 || y >= TerrainActuel.Hauteur)
            {
                Console.WriteLine("Position invalide!");
                Thread.Sleep(1000);
                return;
            }

            string message = "";

            switch (action)
            {
                case "Planter":
                    bool selectionValide = false;
                    Plante planteChoisie = null;
                    string nomPlanteChoisie = "";

                    while (!selectionValide)
                    {
                        Console.WriteLine("Plantes disponibles:");
                        Console.WriteLine("🌹 (R)ose    🍅 (T)omate    🥕 (C)arotte");
                        Console.WriteLine("🌻 (S)oleil  🌿 (B)asilic");
                        Console.Write("Quelle plante voulez-vous planter ? ");
                        string? planteChoisi = Console.ReadLine()?.ToUpper();

                        if (string.IsNullOrEmpty(planteChoisi)) continue;

                        switch (planteChoisi)
                        {
                            case "R":
                                nomPlanteChoisie = "Rose";
                                break;
                            case "T":
                                nomPlanteChoisie = "Tomate";
                                break;
                            case "C":
                                nomPlanteChoisie = "Carotte";
                                break;
                            case "S":
                                nomPlanteChoisie = "Tournesol";
                                break;
                            case "B":
                                nomPlanteChoisie = "Basilic";
                                break;
                            default:
                                Console.WriteLine("❌ Plante non reconnue.");
                                continue;
                        }

                        //Vérifier si on a la graine dans l'inventaire
                        string nomGraine = nomPlanteChoisie + " (graine)";
                        if (Inventaire.ContainsKey(nomGraine) && Inventaire[nomGraine] > 0)
                        {
                            //On a la graine, on peut planter
                            planteChoisie = nomPlanteChoisie switch
                            {
                                "Rose" => new Rose(),
                                "Tomate" => new Tomate(),
                                "Carotte" => new Carotte(),
                                "Tournesol" => new Tournesol(),
                                "Basilic" => new Basilic(),
                                _ => null
                            };
                            selectionValide = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"❌ Vous n'avez pas de graine de {nomPlanteChoisie} !");
                            Console.WriteLine("💡 Rendez-vous au magasin (M) pour acheter des graines.");
                            Console.ForegroundColor = ConsoleColor.White;
                            Thread.Sleep(2000);
                            return; //Sortir de la méthode
                        }
                    }

                    if (planteChoisie != null)
                    {
                        bool plantee = PlanterPlante(planteChoisie, x, y);
                        if (plantee)
                        {
                            //Consommer la graine de l'inventaire
                            string nomGraine = nomPlanteChoisie + " (graine)";
                            Inventaire[nomGraine]--;
                            message = $"✅ {nomPlanteChoisie} plantée ! (Graine consommée)";
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            message = "❌ Échec de la plantation.";
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                    break;


                case "Arroser":
                    ArroserParcelle(x, y);
                    message = "✅ Parcelle arrosée! 💧";
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case "Soigner":
                    SoignerPlante(x, y);
                    message = "✅ Plante soignée! 💊";
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "Récolter":
                    int recolte = RecolterParcelle(x, y);
                    if (recolte > 0)
                    {
                        message = $"✅ Récolte: {recolte} produits! 🌾";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        message = "❌ Rien à récolter.";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    break;

                case "Enlever":
                    bool enleve = EnleverPlante(x, y);
                    if (enleve)
                    {
                        message = "✅ Plante enlevée! ❌";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        message = "❌ Aucune plante à enlever.";
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    break;
            }

            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);
        }
        catch (FormatException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Veuillez entrer un nombre valide.");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);
        }
    }
    private string GetIntempErieEmoji(Meteo.TypeIntemperie intemperie)
    {
        return intemperie switch
        {
            Meteo.TypeIntemperie.Pluie => "🌧️",
            Meteo.TypeIntemperie.Secheresse => "☀️",
            Meteo.TypeIntemperie.Grele => "🧊",
            Meteo.TypeIntemperie.Gel => "❄️",
            Meteo.TypeIntemperie.Canicule => "🔥",
            _ => "☁️"
        };
    }

    private string GetEmojiPlante(string nom)
    {
        return nom switch
        {
            "Tomate" => "🍅",
            "Rose" => "🌹",
            "Carotte" => "🥕",
            "Tournesol" => "🌻",
            "Basilic" => "🌿",
            _ => "🌱"
        };
    }
    private string GetEmojiAnimal(string nom)
    {
        return nom switch
        {
            "Abeilles" => "🐝",
            "Coccinelles" => "🐞",
            "Escargots" => "🐌",
            "Taupes" => "🕳️",
            _ => "🐾"
        };
    }
}