using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace KeyboardChatterBlocker
{
    /// <summary>
    /// Shared CSV reading and field escaping.
    /// </summary>
    public static class CsvHelper
    {
        /// <summary>
        /// Reads CSV records, skipping blank and malformed records.
        /// </summary>
        public static IEnumerable<string[]> ReadFile(string path)
        {
            using (TextFieldParser parser = new TextFieldParser(path) { HasFieldsEnclosedInQuotes = true, TrimWhiteSpace = false })
            {
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields;
                    try
                    {
                        fields = parser.ReadFields();
                    }
                    catch (MalformedLineException)
                    {
                        continue;
                    }
                    yield return fields;
                }
            }
        }

        private static readonly char[] QuotedFieldCharacters = { ',', '"', '\r', '\n' };

        /// <summary>
        /// Creates a CSV record, quoting fields containing commas, quotes, or newlines.
        /// </summary>
        public static string CreateLine(params string[] fields)
        {
            return string.Join(",", fields.Select(field => field.IndexOfAny(QuotedFieldCharacters) >= 0 ? "\"" + field.Replace("\"", "\"\"") + "\"" : field));
        }
    }
}
