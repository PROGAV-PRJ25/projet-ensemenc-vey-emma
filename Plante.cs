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
    public char Visuel { get; set; } //emoji de la plante
    public double NiveauHumiditePref { get; protected set; } //pourcentage d'humidite prefere de la plante pour se developper au max
    
    //paramètres d'état descriptifs
    public double TailleActuelle { get; protected set; } //taille actuelle en cm
    public double SanteActuelle { get; protected set; } //pourcentage de santé entre 0 et 100
    public bool EstMalade { get; protected set; } //indique si la plante est malade
    public int Age { get; protected set; } //âge en semaines
    public int ProductionActuelle { get; protected set; } //nombre de fruits/fleurs actuellement produits

    public Plante(string nom, string nature, string saisonOptimale, string typePref, double espacementEntre2, double placeNecessaire,
    double vitessePousse, double eauNecessaire, double lumiereNecessaire, double temperaturePref, string maladiePotentielle,
    double esperanceVie, double nombreProduction, char visuel, double niveauHumiditePref)
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
        EstMalade = false;
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
        if (!EstMalade)
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
            
        //     //Augmentation potentielle de la production si la plante est mature
        //     if (TailleActuelle >= PlaceNecessaire * 50) //50 cm par unité de place comme exemple
        //     {
        //         double facteurProduction = (coefficient / 100) * (SanteActuelle / 100);
        //         ProductionActuelle = (int)Math.Min(NombreProduction, ProductionActuelle + (NombreProduction * facteurProduction / 10));
        //     }
            
        //     //Chance de guérison si la plante est malade 
        //     if (EstMalade && new Random().NextDouble() < (coefficient / 200))
        //     {
        //         EstMalade = false;
        //         SanteActuelle = Math.Min(SanteActuelle + 10, 100);
        //     }
        // }
        
        //proba de tomber malade
        if (!EstMalade && new Random().NextDouble() < 0.05) //5% de chance par semaine
        {
            EstMalade = true;
        }
        
        //vieillissement de la plante
        Age++;
        if (Age > EsperanceVie)
        {
            SanteActuelle -= 5; //Déclin de santé avec l'âge
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
        if (EstMalade)
        {
            EstMalade = false;
            SanteActuelle = Math.Min(SanteActuelle + 15, 100);
        }
    }
}
