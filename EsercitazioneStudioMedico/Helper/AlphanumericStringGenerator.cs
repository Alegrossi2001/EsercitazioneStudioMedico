using System.Text;

namespace EsercitazioneStudioMedico.Helper
{
    public static class AlphanumericStringGenerator
    {
        public static readonly Random random = new Random();
        private const string alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomString(int length = 16)
        {
            var stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(alphanumericCharacters[random.Next(alphanumericCharacters.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}
