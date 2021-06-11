using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services
{
    public class TokenService : ITokenService
    {
        public string GetToken(int length = 20)
        {
            Random random = new Random();

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            return string.Join(string.Empty, Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]));
        }
    }
}
