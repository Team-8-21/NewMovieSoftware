namespace MovieSoftware.MVVM.Model.Classes
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            // Ingen mellemrum længere
            return $"{UserName},{Password}";
        }

        public static User FromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            //Split kun på det første komma
            string[] parts = input.Split(new[] { ',' }, 2);

            if (parts.Length < 2)
                throw new FormatException($"Ugyldig user-linje: '{input}'");

            //Trim mellemrum væk
            string username = parts[0].Trim();
            string password = parts[1].Trim();

            return new User
            {
                UserName = username,
                Password = password
            };
        }
    }
}
