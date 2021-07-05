using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Application.Contracts.IService;
using Domain.Contracts;

namespace CodingChainApi.Infrastructure.Services
{
    public  class HashService : IHashService
    {
        public string GetHash(string text)
        {
            byte[] data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString(""));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}