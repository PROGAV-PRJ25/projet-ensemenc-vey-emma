using System;
using System.Collections.Generic;

public abstract class Terrain
{
    public string Nom { get; protected set; } //"chez" Didier, Gérard, Claude, Lucette, Jeanine, Suzanne, Michel 
    public string Region { get; protected set; } //nord sud est ouest
    public double Surface { get; protected set; } //en hectare
    public string TypeTerrain { get; protected set; } //Sable, Terre, Marécage, Argile 
    public double NiveauHumidite { get; protected set; } //Pourcentage donc entre 0 et 100
    public double Temperature { get; protected set; } //en degré celsius
    public double PH { get; protected set; } //compris entre 0 et 14 
    public double NiveauSoleil { get; protected set; } //Pourcentage ensoleillement entre 0 et 100
    
    public int Largeur { get; protected set; }
    public int Hauteur { get; protected set; }
    
    //La grille représente le terrain avec les différentes parcelles
    public ParcelleTerrain[,] Grille { get; protected set; }
    
    //constructeur
    protected Terrain(string nom, string region, double surface, string typeTerrain, int largeur, int hauteur)
    {
        Nom = nom;
        Region = region;
        Surface = surface;
        TypeTerrain = typeTerrain;
        Largeur = largeur;
        Hauteur = hauteur;
        
        //initialisation
        Grille = new ParcelleTerrain[largeur, hauteur];
        for (int i = 0; i < largeur; i++)
        {
            for (int j = 0; j < hauteur; j++)
            {
                Grille[i, j] = new ParcelleTerrain(this); //création d'une nouvelle parcelle
            }
        }
        
        //par défaut
        NiveauHumidite = 50;
        Temperature = 20;
        NiveauSoleil = 50;
        PH = 7; //pH neutre 
    }
    
    //Méthode pour planter une plante sur le terrain
    public virtual bool PlanterPlante(Plante plante, int x, int y)
    {
        if (x < 0 || x >= Largeur || y < 0 || y >= Hauteur) //vérifier si la case est dans les limites
            return false;
        if (!Grille[x, y].VerifierEstVide())//vérifie si la case est bien vide
            return false;
        
        if (!VerifierEspacement(plante, x, y)) //vérifier si la plante a l'espacde nécessaire
            return false;
            
        return Grille[x, y].PlanterPlante(plante); //on plante si tout est bon
    }
    
    //méthode pour vérifier si la plante a l'espace nécessaire
    protected bool VerifierEspacement(Plante plante, int x, int y)
    {
        double espacementRequis = plante.EspacementEntre2;
        
        //On utilise le rayon nécessaire pour l'espacement requis et on parcour les parcelles
        for (int i = Math.Max(0, x - (int)Math.Ceiling(espacementRequis)); i <= Math.Min(Largeur - 1, x + (int)Math.Ceiling(espacementRequis)); i++)
        {
            for (int j = Math.Max(0, y - (int)Math.Ceiling(espacementRequis)); j <= Math.Min(Hauteur - 1, y + (int)Math.Ceiling(espacementRequis)); j++)
            {
                //Si ce n'est pas la parcelle actuelle et qu'elle n'est pas vide
                if ((i != x || j != y) && !Grille[i, j].VerifierEstVide())
                {
                    //Calculer la distance entre les parcelles
                    double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                    
                    //Si une plante est trop proche
                    if (distance < espacementRequis)
                        return false;
                }
            }
        }
        
        return true;
    }

        public double CalculerEspaceDisponible(int x, int y)
    {
        double espaceDisponible = 0;
        int rayonRecherche = 3; //rayon arbitraire pour la recherche
        
        //Parcourir les parcelles voisines
        for (int i = Math.Max(0, x - rayonRecherche); i <= Math.Min(Largeur - 1, x + rayonRecherche); i++)
        {
            for (int j = Math.Max(0, y - rayonRecherche); j <= Math.Min(Hauteur - 1, y + rayonRecherche); j++)
            {
                //Si la parcelle est vide, on l'ajoute à l'espace disponible
                if (Grille[i, j].VerifierEstVide())
                {
                    double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                    if (distance <= rayonRecherche)
                    {
                        //Chaque parcelle vide contribue de manière égale (1 point)
                        espaceDisponible += 1;
                    }
                }
            }
        }
        
        return espaceDisponible;
    }

    
    //Faire progresser le terrain d'une semaine pour avancer dans le temps du jeu
    public virtual void ProgresserSemaine(string saison)
    {
        //modifier les conditions en fonction de la saison
        AjusterConditionsSaisonnieres(saison);
        for (int i = 0; i < Largeur; i++)
        {
            for (int j = 0; j < Hauteur; j++)
            {
                if (!Grille[i, j].VerifierEstVide())
                {
                    double espaceDisponible = CalculerEspaceDisponible(i, j);
                    Grille[i, j].ProgresserSemaine(TypeTerrain, NiveauHumidite, NiveauSoleil, Temperature, espaceDisponible);
                }
            }
        }
    }
    
    //ajuster les conditions selon la saison dans laquelle on est
    protected virtual void AjusterConditionsSaisonnieres(string saison)
    {
        switch (saison.ToLower())
        {
            case "printemps":
                Temperature = 14 + new Random().Next(-6, 7);  //8 à 20°C
                NiveauHumidite = 50 + new Random().Next(-5, 16);  //45 à 75%
                NiveauSoleil = 60 + new Random().Next(-15, 16);  //45 à 75%
                break;
            case "ete":
                Temperature = 25 + new Random().Next(-5, 8);  //20 à 32°C
                NiveauHumidite = 40 + new Random().Next(-20, 6);  //20 à 45%
                NiveauSoleil = 80 + new Random().Next(-10, 16);  //70 à 95%
                break;
            case "automne":
                Temperature = 12 + new Random().Next(-2, 6);  //10 à 17°C
                NiveauHumidite = 70 + new Random().Next(-10, 11);  //60 à 80%
                NiveauSoleil = 40 + new Random().Next(-10, 11);  //30 à 50%
                break;
            case "hiver":
                Temperature = 3 + new Random().Next(-8, 8);  //-5 à 10°C
                NiveauHumidite = 75 + new Random().Next(-10, 11);  //65 à 85%
                NiveauSoleil = 20 + new Random().Next(-10, 11);  //10 à 30%
                break;
            default:
                //conditions moyennes pour default
                Temperature = 15 + new Random().Next(-5, 6);//10 à 20°C
                NiveauHumidite = 60 + new Random().Next(-10, 11); //50 à 70%
                NiveauSoleil = 50 + new Random().Next(-10, 11);//40 à 60%
                break;
        }
    }
    
    //méthode arroser une parcelle 
    public virtual void ArroserParcelle(int x, int y)
    {
        if (x >= 0 && x < Largeur && y >= 0 && y < Hauteur && !Grille[x, y].VerifierEstVide())
        {
            Grille[x, y].Arroser();
        }
    }
    
    //méthode récolter une parcelle
    public virtual int RecolterParcelle(int x, int y)
    {
        if (x >= 0 && x < Largeur && y >= 0 && y < Hauteur && !Grille[x, y].VerifierEstVide())
        {
            return Grille[x, y].Recolter();
        }
        return 0;
    }
    
    //méthode soigner une plante
    public virtual void SoignerPlante(int x, int y)
    {
        if (x >= 0 && x < Largeur && y >= 0 && y < Hauteur && !Grille[x, y].VerifierEstVide())
        {
            Grille[x, y].Soigner();
        }
    }
    
    //méthode enlever une plante 
    public virtual bool EnleverPlante(int x, int y)
    {
        if (x >= 0 && x < Largeur && y >= 0 && y < Hauteur && !Grille[x, y].VerifierEstVide())
        {
            return Grille[x, y].EnleverPlante();
        }
        return false;
    }
    
    //méthode pour avoir un résumé du terrain et des caractéristiques
    public virtual string ObtenirResume()
    {
        string resume = $"Terrain: {Nom} ({Region})\n";
        resume += $"Type: {TypeTerrain}, Surface: {Surface} hectares\n";
        resume += $"Conditions actuelles: {Temperature}°C, {NiveauHumidite}% humidité, {NiveauSoleil}% soleil, pH {PH}\n";
        
        int plantesTotal = 0;
        int plantesMalades = 0;
        
        for (int i = 0; i < Largeur; i++)
        {
            for (int j = 0; j < Hauteur; j++)
            {
                if (!Grille[i, j].VerifierEstVide())
                {
                    plantesTotal++;
                    if (Grille[i, j].VerifierEstMalade())
                    {
                        plantesMalades++;
                    }
                }
            }
        }
        
        resume += $"Nombre de plantes: {plantesTotal}\n";
        resume += $"Plantes malades: {plantesMalades}\n";
        
        return resume;
    }
}