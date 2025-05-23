public abstract class Plante
{
    public string Nom { get; protected set; } //nom de la plante (carotte/cosmos/tournesol...)
    public string Nature { get; protected set; } //plante vivace, decorative, consommable...
    public string SaisonOptimale { get; protected set; } //saison optimale pour se developper (ete, automne, hiver, printemps)
    public string TypePref { get; protected set; } //type de terrain prefere pour se developper
    public double EspacementEntre2 { get; protected set; } //nombre de parcelles necessaire entre la plante et la suivante
    public double PlaceNecessaire { get; protected set; } //place necessaire de parcelle pour que la plante se developpe au max
    public double VitessePousse { get; protected set; } //cm par jour ou semaine que la plante prends avec les meilleures conditions
    public double EauNecessaire { get; protected set; } //litres d'eau necessaire par jour/semaine pour la plante
    public double EnsoleillementNecessaire { get; protected set; } //pourcentage de lumiere necessaire optimal
    public double TemperaturePref { get; protected set; } //temperature prefere de la plante
    public string MaladiePotentielle { get; protected set; } //maladies potentielles qu'elle peut avoir (probabilité gerer aleatoirement)
    public double EsperanceVie { get; protected set; } //nombre de semaine ou année de vie de la plante
    public double NombreProduction { get; protected set; } //nombre de fruit ou fleur par pied qu'elle peut produire au max en 1 saison
    public string Visuel { get; set; } //emoji de la plante
    public double NiveauHumiditePref { get; protected set; } //pourcentage d'humidite prefere de la plante pour se developper au max
    
    //paramètres d'état descriptifs
    public double TailleActuelle { get; protected set; } //taille actuelle en cm
    public double SanteActuelle { get; set; } //pourcentage de santé entre 0 et 100
    public bool VerifierEstMalade { get; set; } //indique si la plante est malade - changé de protected set à public set
    public int Age { get; protected set; } //âge en semaines
    public int ProductionActuelle { get; protected set; } //nombre de fruits/fleurs actuellement produits

    public DateTime DatePlantation { get; protected set; }

    public Plante(string nom, string nature, string saisonOptimale, string typePref, double espacementEntre2, double placeNecessaire,
    double vitessePousse, double eauNecessaire, double lumiereNecessaire, double temperaturePref, string maladiePotentielle,
    double esperanceVie, double nombreProduction, string visuel, double niveauHumiditePref)
    {
        Nom = nom;
        Nature = nature;
        SaisonOptimale = saisonOptimale;
        TypePref = typePref;
        EspacementEntre2 = espacementEntre2;
        PlaceNecessaire = placeNecessaire;
        VitessePousse = vitessePousse;
        EauNecessaire = eauNecessaire;
        EnsoleillementNecessaire = lumiereNecessaire;
        TemperaturePref = temperaturePref;
        MaladiePotentielle = maladiePotentielle;
        EsperanceVie = esperanceVie;
        NombreProduction = nombreProduction;
        Visuel = visuel;
        NiveauHumiditePref = niveauHumiditePref;
        
        //initialis de l'état
        TailleActuelle = 0;
        SanteActuelle = 100;
        VerifierEstMalade = false;
        Age = 0;
        ProductionActuelle = 0;

    }

    //méthode qui calcule un coeff de développement en fct du respect des conditions de pousse
    public double CalculerCoefficientDeveloppement(string typeTerrain, double niveauHumidite, double niveauSoleil, double temperature, double espaceDisponible)
    {
        double pourcentageConditions = 0;
        if (typeTerrain == TypePref)//si son terrain est son terrain pref
        {
            pourcentageConditions += 20; //20% du coefficient 0 sinon 
        }
        double differenceHumidite = Math.Abs(niveauHumidite - NiveauHumiditePref); //humidité
        pourcentageConditions += 20 * (1 - differenceHumidite / 100); //20% max 
        double differenceEnsoleillement = Math.Abs(EnsoleillementNecessaire - niveauSoleil); //soleil
        pourcentageConditions += 20 * (1 - differenceEnsoleillement / 100);//pareil que humidité 20%
        if (!VerifierEstMalade)
        {
            pourcentageConditions += 10; //si saine impact de 10% sur le coeff
        }
        double differenceTemperature = Math.Abs(temperature - TemperaturePref);//température impact 15%
        if (differenceTemperature <= 10)
        {
            pourcentageConditions += 15 * (1 - differenceTemperature / 10);
        }
        pourcentageConditions += 15 * Math.Min(espaceDisponible / PlaceNecessaire, 1); //espace impact 15%
        return Math.Max(0, Math.Min(100, pourcentageConditions));//si jamais ça dépasse 100? on s'assure que c'est entre 0 et 100
    }

    //Mméthode qui avance le temps et fait progresser d'une semaine
    public virtual void Progresser(string typeTerrain, double niveauHumidite, double niveauSoleil, double temperature, double espaceDisponible)
    {
        double coefficient = CalculerCoefficientDeveloppement(typeTerrain, niveauHumidite, niveauSoleil, temperature, espaceDisponible);//remet coeff
        
        //si le coeff de dév est inférieur à 50%, la plante perd des points de santé
        if (coefficient < 50)
        {
            SanteActuelle -= (50 - coefficient) / 5;
            if (SanteActuelle <= 0)
            {
                SanteActuelle = 0; //triste, la plante est morte 
                return;
            }
        }
        else
        {
            //la plante pousse proportionnellement au coefficient, donc plus le coeff est bien plus elle pousse vite 
            TailleActuelle += VitessePousse * (coefficient / 100);
        }
        
        //proba de tomber malade
        if (!VerifierEstMalade && new Random().NextDouble() < 0.05) //5% de chance par semaine
        {
            VerifierEstMalade = true;
        }
        
        //vieillissement de la plante
        Age++;
        if (Age > EsperanceVie)
        {
            SanteActuelle -= 5; //Déclin de santé avec l'âge
        }

        // Mise à jour production simple
        if (Age > 2 && ProductionActuelle < NombreProduction)
        {
            ProductionActuelle++;
        }
    }

    //méthode récolter des produits
    public virtual int Recolter()
    {
        int recolte = ProductionActuelle;
        ProductionActuelle = 0;
        return recolte;
    }

    //méthode arrosage
    public virtual void Arroser()
    {
        SanteActuelle = Math.Min(SanteActuelle + 5, 100);
    }

    //méthode soin
    public virtual void Soigner()
    {
        if (VerifierEstMalade)
        {
            VerifierEstMalade = false;
            SanteActuelle = Math.Min(SanteActuelle + 15, 100);
        }
    }


    public virtual string ObtenirVisuel()
    {
        return "🌱"; // Par défaut
    }

    public override string ToString()
    {
        return ObtenirVisuel();
    }

}


public class Tomate : Plante
{
    public Tomate() : base(
        nom: "Tomate",
        nature: "Consommable",
        saisonOptimale: "Été",
        typePref: "Terre meuble",
        espacementEntre2: 1,
        placeNecessaire: 1,
        vitessePousse: 2,
        eauNecessaire: 1.5,
        lumiereNecessaire: 80,
        temperaturePref: 25,
        maladiePotentielle: "Mildiou",
        esperanceVie: 12,
        nombreProduction: 5,
        visuel: "🍅",
        niveauHumiditePref: 60)
    { }

    public override string ObtenirVisuel()
    {
        return "🍅"; // Par exemple pour Tomate
    }

}

public class Rose : Plante
{
    public Rose() : base(
        nom: "Rose",
        nature: "Ornementale",
        saisonOptimale: "Printemps",
        typePref: "Terre fertile",
        espacementEntre2: 2,
        placeNecessaire: 1,
        vitessePousse: 3,
        eauNecessaire: 1.2,
        lumiereNecessaire: 70,
        temperaturePref: 20,
        maladiePotentielle: "Oïdium",
        esperanceVie: 24,
        nombreProduction: 1, // Si elle ne produit qu'une fleur visible
        visuel: "🌹",
        niveauHumiditePref: 65)
    { }

    public override string ObtenirVisuel()
    {
        return "🌹";
    }
}
public class Carotte : Plante
{
    public Carotte() : base(
        nom: "Carotte",
        nature: "Consommable",
        saisonOptimale: "Automne",
        typePref: "Sable",
        espacementEntre2: 0.5,
        placeNecessaire: 0.5,
        vitessePousse: 1.5,
        eauNecessaire: 1,
        lumiereNecessaire: 60,
        temperaturePref: 15,
        maladiePotentielle: "Mouche de la carotte",
        esperanceVie: 16,
        nombreProduction: 1,
        visuel: "🥕",
        niveauHumiditePref: 50)
    { }

    public override string ObtenirVisuel()
    {
        return "🥕";
    }
}

public class Tournesol : Plante
{
    public Tournesol() : base(
        nom: "Tournesol",
        nature: "Ornementale",
        saisonOptimale: "Été",
        typePref: "Terre fertile",
        espacementEntre2: 2,
        placeNecessaire: 2,
        vitessePousse: 4,
        eauNecessaire: 2,
        lumiereNecessaire: 90,
        temperaturePref: 28,
        maladiePotentielle: "Sclérotinia",
        esperanceVie: 20,
        nombreProduction: 1,
        visuel: "🌻",
        niveauHumiditePref: 55)
    { }

    public override string ObtenirVisuel()
    {
        return "🌻";
    }
}

public class Basilic : Plante
{
    public Basilic() : base(
        nom: "Basilic",
        nature: "Aromatique",
        saisonOptimale: "Été",
        typePref: "Terre meuble",
        espacementEntre2: 1,
        placeNecessaire: 1,
        vitessePousse: 2.5,
        eauNecessaire: 1.8,
        lumiereNecessaire: 75,
        temperaturePref: 24,
        maladiePotentielle: "Fusariose",
        esperanceVie: 15,
        nombreProduction: 3,
        visuel: "🌿",
        niveauHumiditePref: 65)
    { }

    public override string ObtenirVisuel()
    {
        return "🌿";
    }
}


