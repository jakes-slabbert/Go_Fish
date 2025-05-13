namespace GoFish.Services
{
    public static class GameNameGenerator
    {
        private static readonly string[] Adjectives = { "Epic", "Fiery", "Frozen", "Mystic", "Ancient", "Savage", "Shadow", "Titanic", "Wicked", "Infernal" };
        private static readonly string[] Nouns = { "Battle", "Clash", "Showdown", "Quest", "Duel", "Skirmish", "Saga", "Crusade", "Tournament", "War" };
        private static readonly string[] Powers = { "Kings", "Titans", "Legends", "Warlords", "Champions", "Gods", "Shadows", "Dragons", "Realms", "Fates" };

        public static string Generate()
        {
            var random = new Random();
            var adjective = Adjectives[random.Next(Adjectives.Length)];
            var noun = Nouns[random.Next(Nouns.Length)];
            var power = Powers[random.Next(Powers.Length)];

            return $"{adjective} {noun} of {power}";
        }
    }
}
