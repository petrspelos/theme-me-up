namespace ThemeMeUp.ApiWrapper.Entities.Api
{
    public class Category
    {
        public bool General { get; private set; }
        public bool Anime { get; private set; }
        public bool People { get; private set; }

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
    }
}
