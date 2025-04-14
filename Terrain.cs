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
    
protected Terrain(string nom, double surface)
    {
        Nom = nom;
        Surface = surface;
        Plantes = new List<Plante>();
        
        //Par défaut
        NiveauHumidite = 50;
        Temperature = 20;
        ExpositionSoleil = 50;
    }
public virtual bool PlanterUnePlante(Plante.plante)
    {
        return Plantes.Add(plante);
    }
public virtual bool RetirerPlante(Plante plante)
    {
        return Plantes.Remove(plante);
    }

}

