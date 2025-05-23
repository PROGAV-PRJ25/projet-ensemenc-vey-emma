public class Economie
{
    public int ArgentJoueur { get; private set; }
    public Dictionary<string, int> PrixVente { get; private set; }
    public Dictionary<string, int> PrixAchat { get; private set; }
    
    public Economie()
    {
        ArgentJoueur = 100; // Argent de départ
        
        PrixVente = new Dictionary<string, int>
        {
            { "Tomate", 5 },
            { "Carotte", 3 },
            { "Basilic", 4 },
            { "Rose", 8 },
            { "Tournesol", 6 }
        };
        
        PrixAchat = new Dictionary<string, int>
        {
            { "Tomate", 2 },
            { "Carotte", 1 },
            { "Basilic", 2 },
            { "Rose", 3 },
            { "Tournesol", 3 }
        };
    }
    
    public bool AcheterGraine(string typePlante)
    {
        if (PrixAchat.ContainsKey(typePlante) && ArgentJoueur >= PrixAchat[typePlante])
        {
            ArgentJoueur -= PrixAchat[typePlante];
            return true;
        }
        return false;
    }
    
    public int VendreProduit(string typePlante, int quantite)
    {
        if (PrixVente.ContainsKey(typePlante))
        {
            int gain = PrixVente[typePlante] * quantite;
            ArgentJoueur += gain;
            return gain;
        }
        return 0;
    }
}