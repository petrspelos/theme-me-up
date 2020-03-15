namespace ThemeMeUp.ApiWrapper.Entities.Api
{
    public class Category
    {
        public bool General { get; set; }
        public bool Anime { get; set; }
        public bool People { get; set; }

        public Category() { }

        public Category(byte raw)
        {
            General = raw.IsBitSet(3);
            Anime = raw.IsBitSet(2);
            People = raw.IsBitSet(1);
        }

        public Category(string input)
        {
            input = input.Trim().ToLower();

            if(input == "general")
            {
                General = true;
            }
            else if(input == "anime")
            {
                Anime = true;
            }
            else if(input == "people")
            {
                People = true;
            }
        }

        public string ToQueryParameter()
        {
            if(IsDefault()) { return "categories=111"; }

            return $"categories={General.ToBinaryString()}{Anime.ToBinaryString()}{People.ToBinaryString()}";
        }

        private bool IsDefault() => !General && !Anime && !People;
    }
}
