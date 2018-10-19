using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using galaxy_merchant_back.Controllers.DataType;
using Microsoft.AspNetCore.Http;

namespace galaxy_merchant_back.Controllers
{
    internal static class CoinEngine
    {        
        private const string not_identified = "I have no idea what you are talking about.";        
        private static int RomanValue(int index)
        {
            int basefactor = ((index % 2) * 4 + 1);
            return index > 1 ? (int)(basefactor * System.Math.Pow(10.0, index / 2)) : basefactor;
        }
        private static float FromRoman(string roman)
        {
            string literals = "MDCLXVI";
            int value = 0, index = 0;
            foreach (char literal in literals)
            {
                value = RomanValue(literals.Length - literals.IndexOf(literal) - 1);
                index = roman.IndexOf(literal);
                if (index > -1)
                    return FromRoman(roman.Substring(index + 1)) + (index > 0 ? value - FromRoman(roman.Substring(0, index)) : value);
            }
            return 0;
        }
        public static void TranslateValues(string GalaxyString, DB last_data)
        {
            GalaxyString = GalaxyString.ToUpper().Trim();
            string[] words = GalaxyString.Split(" IS ");                        
            //primary attribution  
            try
            {

                if(last_data.coin.Contains(words[0])){
                    if(last_data.galaxylang.ContainsKey(words[0]))
                        last_data.galaxylang[words[0]] = Enum.Parse<RomanVal>(words[1]);
                    else                        
                        last_data.galaxylang.Add(words[0], Enum.Parse<RomanVal>(words[1]));

                    return;
                }
                throw new UnrecognizeWordException(not_identified);                                                
            }
            catch (UnrecognizeWordException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new UnrecognizeWordException(not_identified);
            }
        }
        public static void ComputeValues(string GalaxyString, DB last_data)
        {
            var result_in_romans = string.Empty;
            GalaxyString = GalaxyString.ToUpper().Replace("CREDITS", string.Empty).Replace("IS ", string.Empty).Trim();

            float credits = 0f;

            string[] words = GalaxyString.Split(" ");
            string word_to_compute = string.Empty;
            foreach (var word in words)
            {
                if (last_data.coin.Contains(word))
                {
                    foreach (var value in last_data.galaxylang)
                        if (value.Key.Contains(word))
                            result_in_romans += Enum.GetName(typeof(RomanVal), value.Value);
                }
                else if (float.TryParse(word, out credits)) { }
                else word_to_compute = word;
            }

            if (!IsARomanNumber(result_in_romans))
                throw new UnrecognizeWordException(not_identified);

            var decimals = FromRoman(result_in_romans);
            var word_value = credits / decimals;

            last_data.words_values.Add(word_to_compute, word_value);            
        }
        public static string AnswerQuestion(string GalaxyString, DB last_data)
        {
            var result_in_romans = string.Empty;            
            GalaxyString = GalaxyString.ToUpper().Replace("HOW MUCH IS ", string.Empty)
                                    .Replace("HOW MANY CREDITS IS ", string.Empty)
                                    .Replace("?", string.Empty)
                                    .Trim();

            string[] words = GalaxyString.Split(" ");
            string word_to_compute = string.Empty;
            foreach (var word in words)
            {
                if (last_data.coin.Contains(word))
                {
                    foreach (var value in last_data.galaxylang)
                        if (value.Key.Contains(word))
                            result_in_romans += Enum.GetName(typeof(RomanVal), value.Value);
                }
                else word_to_compute = word;
            }

            if (IsARomanNumber(result_in_romans))
            {
                var cardinal = FromRoman(result_in_romans);
                float multiplier = 1f, result;
                if (word_to_compute.Equals("")) result = cardinal;
                else
                    if (last_data.words_values.TryGetValue(word_to_compute, out multiplier))
                    result = cardinal * multiplier;
                else throw new UnrecognizeWordException(not_identified);
                return GalaxyString = GalaxyString.ToLower() + " is " + (multiplier * cardinal) + " Credits";
            }
            throw new UnrecognizeWordException(not_identified);
        }
        private static bool IsARomanNumber(string result_in_romans)
            {
                if (new Regex(@"^(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})$", RegexOptions.None).IsMatch(result_in_romans))
                    return true;
                return false;
            }
        public static PhraseType Phrase(string GalaxyString, DB last_data)
          {              
                var input = GalaxyString.ToUpper().Trim();
                if ((input.StartsWith("HOW MUCH IS ") || (input.StartsWith("HOW MANY CREDITS IS ")) && input.EndsWith("?"))) return PhraseType.Query;
                else
                {
                    var is_a_valid_input = false;
                    foreach (var enum_name in last_data.coin)
                        if (input.StartsWith(enum_name)) { is_a_valid_input = true; break; }

                    if (is_a_valid_input)
                    {
                        if (input.Contains("CREDITS")) return PhraseType.Compute;
                        return PhraseType.Set;
                    }
                    //WRONG PHRASE
                    throw new UnrecognizeWordException(not_identified);
                }
            }
    }
}