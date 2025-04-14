public abstract class Terrain {
    public string Nom {get; protected set;} //"chez" Didier, Gérard, Claude, Lucette, Jeanine, Suzanne, Michel 
    public string Region {get; protected set;}  //nord sud est ouest
    public double Surface {get; protected set;} // en hectare
    public string TypeTerrain {get; protected set;} //Sable, Terre, Marécage, Argile, 
    public double NiveauHumidite {get; protected set;} //Pourcentage donc entre 0 et 100
    public double Temperature {get; protected set;} //en degré celsius
    public double PH {get; protected set;} // compris entre 0 et 14 
    public double NiveauSoleil {get ; protected set ;} //Pourcentage ensoleillement entre 0 et 100
    public List<Plante> Plantes {get; protected set;} //accès à la classe plante définié ailleurs
    public int random Longueur {get; protected set;}
    public int random Largeur {get; protected set ;}

    public ParcelleTerrain[,] Grille { get; protected set; }
    public int Largeur { get; protected set; }
    public int Hauteur { get; protected set; }

protected Terrain(string nom, double surface)
    {
        Nom = nom;
        Surface = surface;
        Largeur = largeur;
        Longueur= longueur;
        Grille = new CelluleTerrain[largeur, hauteur]; //initialise la grille donc le terrain globalement
        for (int i = 0; i < largeur; i++)
        {
            for (int j = 0; j < hauteur; j++)
            {
                Grille[i, j] = new ParcelleTerrain(this);
            }
        }
                    
    
        //Par défaut
        NiveauHumidite = 50;
        Temperature = 20;
        ExpositionSoleil = 50;
    }
public virtual bool PlanterUnePlante(Plante plante, int i, int j)
        {
            if (i < 0 || i >= Largeur || j < 0 || j >= Hauteur) //vérifie si case dans le terrain, return false si hors des lim
                return false;
            if (Grille[i,j]=! ".")
                return false;
                
            Grille[i,j]=visuel.Plante ;
            return true;

            
        }
}

