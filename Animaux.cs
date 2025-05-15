using System;
using System.Collections.Generic;
using System.Linq;

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

    protected List<(int, int)> CasesAdjacentes(int x, int y)
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

    protected List<(int, int)> CasesAdjacentesVides(int x, int y)
    {
        return CasesAdjacentes(x, y)
            .Where(p => Terrain.Grille[p.Item1, p.Item2].EstVide() && Terrain.Grille[p.Item1, p.Item2].AnimalCourant == null)
            .ToList();
    }

    protected (int, int) PositionAleatoireVide()
    {
        var vides = new List<(int, int)>();
        for (int i = 0; i < Terrain.Largeur; i++)
            for (int j = 0; j < Terrain.Hauteur; j++)
                if (Terrain.Grille[i, j].EstVide() && Terrain.Grille[i, j].AnimalCourant == null)
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
        (X, Y) = PositionAleatoireVide();
        Terrain.Grille[X, Y].AnimalCourant = this;
    }

    public override void Agir()
    {
        for (int i = 0; i < distanceMax; i++)
        {
            Console.WriteLine($"Abeille à {X},{Y} => {Terrain.Grille[X, Y].AnimalCourant?.Nom ?? "null"}");

            var vides = CasesAdjacentesVides(X, Y);
            if (vides.Count == 0) break;

            if (rnd.NextDouble() < 0.4)
                Terrain.Grille[X, Y].PlanterPlante(new Rose());

            Terrain.Grille[X, Y].AnimalCourant = null;
            (X, Y) = vides[rnd.Next(vides.Count)];
            Terrain.Grille[X, Y].AnimalCourant = this;

            // Affichage beau terrain
            Jeu.AfficherTerrainConsole();
            Thread.Sleep(400);
        }
    }

}


public class Taupe : Animal
{
    public Taupe(Terrain terrain, GestionJeu jeu)
        : base("Taupe", "Fouisseur", "Mangeuse", "🕳️", terrain, jeu)
    {
        (X, Y) = PositionAleatoireVide();
        Terrain.Grille[X, Y].AnimalCourant = this;
    }

    public override void Agir()
    {
        var autour = CasesAdjacentes(X, Y);
        foreach ((int nx, int ny) in autour)
        {
            if (!Terrain.Grille[nx, ny].EstVide())
            {
                Terrain.Grille[nx, ny].EnleverPlante();
                Terrain.Grille[X, Y].AnimalCourant = null;
                return;
            }
        }

        Terrain.Grille[X, Y].AnimalCourant = null;
    }
}

