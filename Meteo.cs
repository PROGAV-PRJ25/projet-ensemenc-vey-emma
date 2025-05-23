public static class Meteo
{
    private static Random rnd = new Random();
    
    public enum TypeIntemperie
    {
        Normale,
        Pluie,
        Secheresse,
        Grele,
        Gel,
        Canicule
    }
    
    public static TypeIntemperie GenererIntemperie(string saison)
    {
        double chance = rnd.NextDouble();
        
        return saison.ToLower() switch
        {
            "printemps" => chance < 0.15 ? TypeIntemperie.Pluie : 
                          chance < 0.25 ? TypeIntemperie.Gel : TypeIntemperie.Normale,
            "ete" => chance < 0.1 ? TypeIntemperie.Secheresse :
                    chance < 0.15 ? TypeIntemperie.Canicule :
                    chance < 0.2 ? TypeIntemperie.Grele : TypeIntemperie.Normale,
            "automne" => chance < 0.2 ? TypeIntemperie.Pluie :
                        chance < 0.1 ? TypeIntemperie.Gel : TypeIntemperie.Normale,
            "hiver" => chance < 0.25 ? TypeIntemperie.Gel :
                      chance < 0.15 ? TypeIntemperie.Pluie : TypeIntemperie.Normale,
            _ => TypeIntemperie.Normale
        };
    }
    
    public static void AppliquerIntemperie(Terrain terrain, TypeIntemperie intemperie)
    {
        switch (intemperie)
        {
            case TypeIntemperie.Pluie:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("🌧️ Il pleut ! Toutes les plantes sont arrosées.");
                for (int i = 0; i < terrain.Largeur; i++)
                    for (int j = 0; j < terrain.Hauteur; j++)
                        terrain.Grille[i, j].Arroser();
                break;
                
            case TypeIntemperie.Secheresse:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("☀️ Sécheresse ! Les plantes perdent de la santé.");
                // Réduction de santé implémentée dans la progression
                break;
                
            case TypeIntemperie.Grele:
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("🧊 Grêle ! Dégâts sur certaines plantes.");
                for (int i = 0; i < terrain.Largeur; i++)
                {
                    for (int j = 0; j < terrain.Hauteur; j++)
                    {
                        if (!terrain.Grille[i, j].EstVide() && rnd.NextDouble() < 0.3)
                        {
                            // Simuler des dégâts de grêle
                            Console.WriteLine($"💥 Plante en ({i},{j}) endommagée par la grêle !");
                        }
                    }
                }
                break;
                
            case TypeIntemperie.Gel:
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("❄️ Gel ! Les plantes sensibles au froid souffrent.");
                break;
                
            case TypeIntemperie.Canicule:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("🔥 Canicule ! Stress hydrique pour les plantes.");
                break;
        }
        Console.ResetColor();
    }
}