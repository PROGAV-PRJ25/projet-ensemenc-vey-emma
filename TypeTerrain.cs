using System;

//terrain de sable
public class TerrainSable : Terrain
{
    public TerrainSable(string nom, string region, double surface, int largeur, int hauteur) 
        : base(nom, region, surface, "Sable", largeur, hauteur)
    {
        //Caractéristiques spécifiques du terrain sablonneux
        NiveauHumidite = 30; //Plus sec par défaut c'est le désert hehe
        PH = 6.5; //pour modifier mais en soit c'est proche de 7 neutre
    }
    
    //ça ça permet de surcharger la méthode pour refléter le fait que le sable draine rapidement =>conséquence sur l'humidité
    protected override void AjusterConditionsSaisonnieres(string saison)
    {
        base.AjusterConditionsSaisonnieres(saison);
        NiveauHumidite = Math.Max(20, NiveauHumidite - 10);
    }
}

//terrain d'argile
public class TerrainArgileux : Terrain //parent
{
    public TerrainArgileux(string nom, string region, double surface, int largeur, int hauteur)
        : base(nom, region, surface, "Argile", largeur, hauteur)
    {
        NiveauHumidite = 70; //Retient plus l'eau
        PH = 7.5; //pareil proche de 7 mais pour différencier
    }
    
    //Surcharge pour montrer que ça garde l'eau l'argile
    protected override void AjusterConditionsSaisonnieres(string saison)
    {
        base.AjusterConditionsSaisonnieres(saison);
        NiveauHumidite = Math.Min(90, NiveauHumidite + 10); //on monte le niveau au moins à 90%
    }
}


//MARECAGEE
public class TerrainMarecageux : Terrain
{
    public TerrainMarecageux(string nom, string region, double surface, int largeur, int hauteur)
        : base(nom, region, surface, "Marécage", largeur, hauteur)
    {
        NiveauHumidite = 90; //Très humide c'est presque de l'eau
        PH = 5.5; //Plus acide d'après mes recherches mais c'est pas aberrant
    }
    
    //Les marécages restent toujours humides peu importe les saisons vu que cr'est de l'eau
    protected override void AjusterConditionsSaisonnieres(string saison)//override 
    {
        base.AjusterConditionsSaisonnieres(saison);
        NiveauHumidite = Math.Min(80, NiveauHumidite);
        //par contre + de maladies à cause de l'humidité
        if (new Random().NextDouble() < 0.15) //15% de chance chaque semaine
        {
            PropagerMaladie();
        }
    }
    
    //Méthode qui fait se propager une maladie en random
    private void PropagerMaladie()
    {
        //on choisit une parcelle au hasard pour commencer l'épidémie
        int x = new Random().Next(0, Largeur);
        int y = new Random().Next(0, Hauteur);
        
        if (!Grille[x, y].EstVide() && !Grille[x, y].EstMalade())//on vérifie qu'il y a une plante et qu'elle est saine
        {
            //Infecter la plante
            Grille[x, y].Infecter(); 
            PropagerMaladieAuxVoisins(x, y, 0.5); //50% de chance de propagation
        }
    }
    
    //récursivité qui permet de propager la maladie autour du plant infecter de proche en proche
    private void PropagerMaladieAuxVoisins(int x, int y, double probabilite)
    {
        //lim de récursion qui évite une propagation excessive et que toute la parcelle soit malade
        if (probabilite < 0.1) return;
        
        //les plantes directement à côté mais pas en diag (autre lim)
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, 1, 0, -1 };
        
        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx >= 0 && nx < Largeur && ny >= 0 && ny < Hauteur)//valide dans la grille
            {
                if (!Grille[nx, ny].EstVide() && !Grille[nx, ny].EstMalade())//si il y a une plante et saine
                {
                    if (new Random().NextDouble() < probabilite)//proba de contamination
                    {
                        Grille[nx, ny].Infecter(); 
                        PropagerMaladieAuxVoisins(nx, ny, probabilite * 0.5);//on réduit la proba quand on continue l'épidémie
                    }
                }
            }
        }
    }
}