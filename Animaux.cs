using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public abstract class Animal
{
    public string Nom { get; protected set; }
    public string Type { get; protected set; }
    public string Comportement { get; protected set; }
    public string Visuel { get; protected set; }

    public int X { get; protected set; }
    public int Y { get; protected set; }

    protected Terrain Terrain;
    protected static Random rnd = new Random();
    protected GestionJeu Jeu;


    protected Animal(string nom, string type, string comportement, string visuel, Terrain terrain, GestionJeu jeu)
    {
        Nom = nom;
        Type = type;
        Comportement = comportement;
        Visuel = visuel;
        Terrain = terrain;
        Jeu=jeu;
    }

    public abstract void Agir();

    protected List<(int, int)> VerifierCasesAdjacentes(int x, int y)
    {
        var adj = new List<(int, int)>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < Terrain.Largeur && ny >= 0 && ny < Terrain.Hauteur)
                    adj.Add((nx, ny));
            }
        }
        return adj;
    }

    protected List<(int, int)> VerifierVerifierCasesAdjacentesVides(int x, int y)
    {
        return VerifierCasesAdjacentes(x, y)
            .Where(p => Terrain.Grille[p.Item1, p.Item2].VerifierEstVide() && Terrain.Grille[p.Item1, p.Item2].AnimalCourant == null)
            .ToList();
    }

    protected (int, int) FairePositionAleatoireVide()
    {
        var vides = new List<(int, int)>();
        for (int i = 0; i < Terrain.Largeur; i++)
            for (int j = 0; j < Terrain.Hauteur; j++)
                if (Terrain.Grille[i, j].VerifierEstVide() && Terrain.Grille[i, j].AnimalCourant == null)
                    vides.Add((i, j));

        if (vides.Count == 0)
            throw new Exception("Pas de case vide pour l'animal !");
        return vides[rnd.Next(vides.Count)];
    }

    public virtual string ObtenirVisuel() => Visuel;
}

public class Abeille : Animal
{
    private int distanceMax = 5;

    public Abeille(Terrain terrain, GestionJeu jeu)
        : base("Abeille", "Insecte", "Pollinisateur", "🐝", terrain, jeu)
    {
        (X, Y) = FairePositionAleatoireVide();
        Terrain.Grille[X, Y].AnimalCourant = this;
    }

    public override void Agir()
    {
        Console.WriteLine($"\n🐝 - UNE ABEILLE APPARAÎT -");
        Console.WriteLine($"Position: ({X},{Y})");
        int rosesPlantees = 0;
        for (int i = 0; i < distanceMax; i++)
        {

            var vides = VerifierVerifierCasesAdjacentesVides(X, Y);
            if (vides.Count == 0) break;

            if (rnd.NextDouble() < 0.4)
            {
                Terrain.Grille[X, Y].PlanterPlante(new Rose());
                rosesPlantees++;
                Console.WriteLine($"  🌹 Rose plantée en ({X},{Y}) !");
            }

            Terrain.Grille[X, Y].AnimalCourant = null;
            (X, Y) = vides[rnd.Next(vides.Count)];
            Terrain.Grille[X, Y].AnimalCourant = this;

            //Affichage beau terrain
            Jeu.AfficherTerrainConsole();
            Thread.Sleep(400);
        }



        Console.WriteLine($"💝 Impact de l'abeille : {rosesPlantees} roses plantées !");
        Terrain.Grille[X, Y].AnimalCourant = null;
        Thread.Sleep(1500);
    }
}

public class Taupe : Animal
{
    public Taupe(Terrain terrain, GestionJeu jeu)
        : base("Taupe", "Fouisseur", "Mangeuse", "🕳️", terrain, jeu)
    {
        (X, Y) = FairePositionAleatoireVide();
        Terrain.Grille[X, Y].AnimalCourant = this;
    }

    public override void Agir()
    {
        Console.WriteLine($"\n🕳️ -UNE TAUPE APPARAÎT-");
        Console.WriteLine($"Position: ({X},{Y}) - Attention c'est un danger pour les cultures !");
        bool degats = false;
        var autour = VerifierCasesAdjacentes(X, Y);
        foreach ((int nx, int ny) in autour)
        {
            if (!Terrain.Grille[nx, ny].VerifierEstVide())
            {
                string nomPlante = Terrain.Grille[nx, ny].PlanteCourante?.Nom ?? "Plante";
                Terrain.Grille[nx, ny].EnleverPlante();
                Console.WriteLine($"  💥 {nomPlante} détruite en ({nx},{ny}) !");
                degats = true;
                break;
            }
        }

        if (!degats)
        {
            Console.WriteLine("  🎯 Aucune plante à proximité - La taupe repart bredouille");
        }
        else
        {
            Console.WriteLine("💀 Impact de la taupe : 1 plante détruite !");
        }

        Terrain.Grille[X, Y].AnimalCourant = null;
        Thread.Sleep(2000);
    } }

public class Coccinelle : Animal
{
    public Coccinelle(Terrain terrain, GestionJeu jeu)
        : base("Coccinelle", "Insecte", "Protecteur", "🐞", terrain, jeu)
    {
        (X, Y) = FairePositionAleatoireVide();
        Terrain.Grille[X, Y].AnimalCourant = this;
    }

    public override void Agir()
    {
        Console.WriteLine($"🐞 Une coccinelle apparaît en ({X},{Y}) et soigne les plantes malades !");
        int plantesSoignees = 0;

        //la cocci soigne toutes les plantes malades dans un rayon de 2 cases
        for (int i = Math.Max(0, X - 2); i <= Math.Min(Terrain.Largeur - 1, X + 2); i++)
        {
            for (int j = Math.Max(0, Y - 2); j <= Math.Min(Terrain.Hauteur - 1, Y + 2); j++)
            {
                if (!Terrain.Grille[i, j].VerifierEstVide() && Terrain.Grille[i, j].VerifierEstMalade())
                {
                    Terrain.Grille[i, j].Soigner();
                    Console.WriteLine($"  ✨ Plante en ({i},{j}) soignée !");
                }
            }
        }
        if (plantesSoignees == 0)
        {
            Console.WriteLine("  ✅ Aucune plante malade trouvée - Tout va bien !");
        }
        else
        {
            Console.WriteLine($"💚 Impact de la coccinelle : {plantesSoignees} plantes soignées !");
        }

        Thread.Sleep(2000);
        Terrain.Grille[X, Y].AnimalCourant = null;
    }
}

public class Escargot : Animal
{
    public Escargot(Terrain terrain, GestionJeu jeu)
        : base("Escargot", "Mollusque", "Grignoteur", "🐌", terrain, jeu)
    {
        (X, Y) = FairePositionAleatoireVide();
        Terrain.Grille[X, Y].AnimalCourant = this;
    }

    public override void Agir()
    {
        Console.WriteLine($"🐌 Un escargot apparaît en ({X},{Y}) et grignote les feuilles...");
        
        //l'escargot réduit la santé des plantes adjacentes de 10%
        var adjacentes = VerifierCasesAdjacentes(X, Y);
        int plantesGrignotees = 0;
        foreach ((int nx, int ny) in adjacentes)
        {
            if (!Terrain.Grille[nx, ny].VerifierEstVide())
        {
            var parcelle = Terrain.Grille[nx, ny];
            if (parcelle.PlanteCourante != null)
            {
                
                double santePrecedente = parcelle.PlanteCourante.SanteActuelle;
                parcelle.PlanteCourante.SanteActuelle = Math.Max(0, santePrecedente - 10);
                
                string nomPlante = parcelle.PlanteCourante.Nom;
                Console.WriteLine($"  🍃 {nomPlante} en ({nx},{ny}) grignotée ! (Santé: {santePrecedente:F0}% → {parcelle.PlanteCourante.SanteActuelle:F0}%)");
                plantesGrignotees++;
            }
        }
        }
        if (plantesGrignotees == 0)
        {
            Console.WriteLine("  😋 Aucune plante à grignoter - L'escargot repart affamé");
        }
        else
        {
            Console.WriteLine($"🐌 Impact de l'escargot : {plantesGrignotees} plantes endommagées !");
        }
    
        Thread.Sleep(800);
        Terrain.Grille[X, Y].AnimalCourant = null; //Disparaît
    }
}
