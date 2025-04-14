public class Plante:Terrain
{
    public string Nom {get; protected set;} // nom de la plante (carotte/cosmos/tournesol...)
    public string Nature {get;protected set;} // plante vivace, decorative, consommable...
    public string SaisonOptimale {get;protected set;} //saison optimale pour se developper (ete, automne, hiver, printemps)
    public string TypePref {get; protected set;} //type de terrain prefere pour se developper
    public double EspacementEntre2 {get;protected set;} //nombre de parcelles necessaire entre la plante et la suivante
    public double PlaceNecessaire {get;protected set;} //place necessaire de parcelle pour que la plante se developpe au max
    public double VitessePousse {get;protected set;} //cm par jour ou semaine que la plante prends avec les meilleures conditions
    public double EauNecessaire {get; protected set;} //litres d'eau necessaire par jour/semaine pour la plante
    public double LumiereNecessaire {get; protected set;} //pourcentage de lumiere necessaire optimal
    public double TemperaturePref {get; protected set;} //temperature prefere de la plante
    public string MaladiePotentielle {get; protected set;} //maladies potentielles qu'elle peut avoir (probabilité gerer aleatoirement)
    public double EsperanceVie {get; protected set;} //nombre de semaine ou année de vie de la plante
    public double NombreProduction {get; protected set;} //nombre de fruit ou fleur par pied qu'elle peut produire au max en 1 saison
    public char Visuel {get; set;} //emoji de la plante
    public double NiveauHumiditePref{get; protected set;} //pourcentage d'humidite prefere de la plnate pour se developper au max

    public Plante(string Nom, char Nature, string SaisonOptimale, string TypePref, double EspacementEntre2, double PlaceNecessaire,
    double VitessePousse, double EauNecessaire, double LumiereNecessaire, double TemperaturePref, string MaladiePotentielle,
    double EsperanceVie, double NombreProduction, char Visuel, double NiveauHumiditePref)
    {
        this.Nom=Nom;
        this.Nature=Nature;
        this.SaisonOptimale=SaisonOptimale;
        this.TypePref=TypePref;
        this.EspacementEntre2=EspacementEntre2;
        this.PlaceNecessaire=PlaceNecessaire;
        this.VitessePousse=VitessePousse;
        this.EauNecessaire=EauNecessaire;
        this.LumiereNecessaire=LumiereNecessaire;
        this.TemperaturePref=TemperaturePref;
        this.MaladiePotentielle=MaladiePotentielle;
        this.EsperanceVie=EsperanceVie;
        this.NombreProduction=NombreProduction;
        this.Visuel=Visuel;
        this.NiveauHumiditePref=NiveauHumiditePref;
    }

    public void RespecterConditions()
    {
        double PourcentageConditions=0; //pourcentage pour evaluer le respect des condition, agit comme un coeficient de pousse
        // si le terrain de la plante est le meme que son terrain de predilection, augmenter le pourcentage
        if (TypeTerrain==TypePref)
        {
            PourcentageConditions+=100;
        }
        //ajouter la difference de pourcentage entre l'humidité reel et celle desirée
        double differenceHumidite=NiveauHumidite-NiveauHumiditePref;
        PourcentageConditions-=differenceHumidite;
        //ajouter la difference de pourcentage entre la temperature reel et celle desirée
        double differenceTemperature=Temperature-TemperaturePref; //voir comment faire pour que tt soit en %
        PourcentageConditions-=differenceTemperature;


    }
}