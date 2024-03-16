using System;
using System.Linq;

namespace Infrastructure.Shared.Services;

public static class PasswordGenerator
{

    public static string CreateRandomPassword(int length = 20)
    {
        string generatedPassword = string.Empty;
        while (!ValidatePassword(generatedPassword))
        {
            // Create a string of characters, numbers, and special characters that are allowed in the password
            string[] charArr = new string[] { "ABCDEFGHJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "0123456789", "!@#$%^&*?" };
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                string validChars = charArr[new Random().Next(0, charArr.Length)];
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            generatedPassword = new string(chars);
        }
        return generatedPassword;

    }

    private static bool ValidatePassword(string password)
    {
        bool result = false;
        string[] charArr = new string[] { "ABCDEFGHJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "0123456789", "!@#$%^&*?" };
        foreach (var item in charArr)
        {
            result = password.Any(e => item.Any(x => x == e));
            if (!result) break;
        }
        return result;

    }

}