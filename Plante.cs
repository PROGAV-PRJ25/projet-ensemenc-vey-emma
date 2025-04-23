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
    public double EnsoleillementNecessaire {get; protected set;} //pourcentage de lumiere necessaire optimal
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
        double differenceHumidite= Math.Abs(NiveauHumidite-NiveauHumiditePref);
        PourcentageConditions-=differenceHumidite;

        //ajouter la difference de pourcentage entre l'ensoleillement reel et celle desirée
        double differenceEnsoleillement= Math.Abs(EnsoleillementNecessaire-NiveauSoleil); 
        PourcentageConditions-=differenceEnsoleillement;

        // si la plante n'est pas malade, son coefficient de developpement augmente
        if (Maladie==0)
        {
            PourcentageConditions+=100;
        }

        // si la temperature est de +/-10°C, le coef de dev diminue de 100%, 
        //sinon on prends la diff de temp entre la reel et la voulue puis on la multiplie par 10 pour avoir un pourcentage valide et on l'enleve du coef de dev
        double differenceTemperature = Math.Abs(Temperature-TemperaturePref);
        if (differenceTemperature<=10)
        {
            PourcentageConditions-=100;
        }
        else
        {
            PourcentageConditions-=(differenceTemperature*10);
        }

        //compter combien de case auutour de la plantes sont disponibles
        double espaceAutourDispo = EspaceAutourDispo(i, j);
        //s'il y a plus de place dispo que necessaire, augmenter le coef de dev de 100%
        if (espaceAutourDispo==PlaceNecessaire)
        {
            PourcentageConditions+=100;
        }
        //s'il y a moins de place que necessaire, alors faire le rapport de ce qu'il ya sur ce qu'il faut et le mettre en pourcentage
        else
        {
            PourcentageConditions-= ((espaceAutourDispo/PlaceNecessaire)*100);
        }

        //diviser le coef de dev par le nombre de conditions pour avoir un resultat entre 0 et 100
        PourcentageConditions=(PourcentageConditions/6);

    }

    public int EspaceAutourDispo(int i, int j)
    {
        double espaceAutourDispo=0;

        // verifier qu'on est bien dans la limite du terrain -> i=colonne et j=ligne
        if (i < 0 || i >= Largeur || j < 0 || j >= Hauteur)
        {
            // parcourir les places autour de la plantes et si elles sont vide (=0) alors rajouter 1 au compteur de case dispo autour
            for (int m = i - 1; m <= i + 1; m++)
            {
                for (int n = i - 1; n <= i + 1; n++)
                {
                    if (Grille[m,n]==0)
                    {
                        espaceAutourDispo+=1;
                    }
                }
            }
        }

        //renvoyer le compteur pour le comparer a la valeur voulue
        return espaceAutourDispo;
    }
}

test