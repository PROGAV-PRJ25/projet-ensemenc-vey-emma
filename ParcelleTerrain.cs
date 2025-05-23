using System;
public class ParcelleTerrain
{
    private Terrain TerrainParent { get; set; } //classe parent
    protected internal Plante? PlanteCourante { get; set; } // Changé de private à protected internal
    public Animal? AnimalCourant { get; set; } = null;
    // Constructeur 
    public ParcelleTerrain(Terrain terrainParent)
    {
        TerrainParent = terrainParent;
        PlanteCourante = null; // initialise les parcelles à vide
    }

    public bool VerifierEstVide() // doit être ici, en dehors du constructeur
    {
        return PlanteCourante == null;
    }
    
    public bool PlanterPlante(Plante plante)//plante une plante
    {
        if (VerifierEstVide())
        {
            PlanteCourante = plante;
            return true;
        }
        return false;
    }
    
    //fct pour avancer dans le temps et faire progresser la parcelle d'une semaine
    public void ProgresserSemaine(string typeTerrain, double niveauHumidite, double niveauSoleil, double temperature, double espaceDisponible)
    {
        if (!VerifierEstVide() && PlanteCourante != null)
        {
            PlanteCourante.Progresser(typeTerrain, niveauHumidite, niveauSoleil, temperature, espaceDisponible);
        }
    }
    
    //avoir le score santé d'une plante entre 0 et 100
    public double ObtenirSante()
    {
        return VerifierEstVide() || PlanteCourante == null ? 0 : PlanteCourante.SanteActuelle;
    }
    public double ObtenirTaille()//permet d'avoir la taille de la plante
    {
        return VerifierEstVide() || PlanteCourante == null ? 0 : PlanteCourante.TailleActuelle;
    }
    public bool VerifierEstMalade() //si la plante est malade 
    {
        return !VerifierEstVide() && PlanteCourante != null && PlanteCourante.VerifierEstMalade;
    }

    //arrosage
    public void Arroser()
    {
        if (!VerifierEstVide() && PlanteCourante != null)
        {
            PlanteCourante.Arroser();
        }
    }
    
    //Soigner la plante
    public void Soigner()
    {
        if (!VerifierEstVide() && PlanteCourante != null)
        {
            PlanteCourante.Soigner();
        }
    }
    
    //Récolter les produits de la plante si fruits légumes par ex
    public int Recolter()
    {
        if (!VerifierEstVide() && PlanteCourante != null)
        {
            return PlanteCourante.Recolter();
        }
        return 0;
    }
    
    //désherbe
    public bool EnleverPlante()
    {
        if (!VerifierEstVide())
        {
            PlanteCourante = null;
            return true;
        }
        return false;
    }
    
    //voir le visu de la plante ou de l'animal
    public string ObtenirVisuel()
    {
        if (AnimalCourant != null)
            return AnimalCourant.ObtenirVisuel();

        if (PlanteCourante != null)
            return PlanteCourante.ObtenirVisuel();

        return "."; // Case vide
    }


    public string ObtenirInfoPlante()//donne les infos
    {
        if (VerifierEstVide() || PlanteCourante == null)
            return "Parcelle vide";
            
        return $"{PlanteCourante.Nom} - Santé: {PlanteCourante.SanteActuelle}% - Taille: {PlanteCourante.TailleActuelle}cm" + 
               (PlanteCourante.VerifierEstMalade ? " (Malade)" : "") + 
               $" - Production: {PlanteCourante.ProductionActuelle}/{PlanteCourante.NombreProduction}";
    }
    
    // Nouvelle méthode pour infecter une plante
    public void Infecter()
    {
        if (!VerifierEstVide() && PlanteCourante != null)
        {
            PlanteCourante.VerifierEstMalade = true;
        }
    }
}
