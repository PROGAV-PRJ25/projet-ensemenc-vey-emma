public class Plante
{
    public string Nom {get; protected set;}
    public char Nature {get;protected set;}
    public string SaisonOptimale {get;protected set;}
    public string TypePref {get; protected set;}
    public double EspacementEntre2 {get;protected set;}
    public double PlaceNecessaire {get;protected set;}
    public double VitessePousse {get;protected set;}
    public double EauNecessaire {get; protected set;}
    public double LumiereNecessaire {get; protected set;}
    public double TempPref {get; protected set;}
    public string MaladiePotentielle {get; protected set;}
    public double EsperanceVie {get; protected set;}
    public double NombreProduction {get; protected set;}

    public Plante(string Nom, char Nature, string SaisonOptimale, string TypePref, double EspacementEntre2, double PlaceNecessaire,
    double VitessePousse, double EauNecessaire, double LumiereNecessaire, double TempPref, string MaladiePotentielle,
    double EsperanceVie, double NombreProduction)
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
        this.TempPref=TempPref;
        this.MaladiePotentielle=MaladiePotentielle;
        this.EsperanceVie=EsperanceVie;
        this.NombreProduction=NombreProduction;
    }
}