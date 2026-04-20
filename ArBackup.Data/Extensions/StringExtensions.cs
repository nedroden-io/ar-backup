using System.Text;

namespace ArBackup.Data.Extensions;

public static class StringExtensions
{
    extension(string source)
    {
        public string ToBase64()
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            
            return Convert.ToBase64String(bytes);
        }

        public string FromBase64()
        {
            var bytes = Convert.FromBase64String(source);
            
            return Encoding.UTF8.GetString(bytes);
        }
    }
}