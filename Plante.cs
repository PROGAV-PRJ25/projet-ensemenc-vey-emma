public class Plante:Terrain
{
    public string Nom {get; protected set;}
    public string Nature {get;protected set;}
    public string SaisonOptimale {get;protected set;}
    public string TypePref {get; protected set;}
    public double EspacementEntre2 {get;protected set;}
    public double PlaceNecessaire {get;protected set;}
    public double VitessePousse {get;protected set;}
    public double EauNecessaire {get; protected set;}
    public double LumiereNecessaire {get; protected set;}
    public double TemperaturePref {get; protected set;}
    public string MaladiePotentielle {get; protected set;}
    public double EsperanceVie {get; protected set;}
    public double NombreProduction {get; protected set;}
    public char Visuel {get; set;}
    public double NiveauHumiditePref{get; protected set;}

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
        double PourcentageConditions=0;
        if (TypeTerrain==TypePref)
        {
            PourcentageConditions+=100;
        }
        double differenceHumidite=NiveauHumidite-NiveauHumiditePref
        PourcentageConditions-=differenceHumidite;
        double differenceTemperature=Temperature-TemperaturePref
        PourcentageConditions-=differenceTemperature;


    }
}