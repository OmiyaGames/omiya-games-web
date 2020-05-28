using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace OmiyaGames.Web
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="UrlHelpers.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.2.0-preview.1<br/>
    /// <strong>Date:</strong> 5/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial version.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A series of utilities used throughout the <see cref="OmiyaGames.Web"/> namespace.
    /// </summary>
    public static class UrlHelpers
    {
        /// <summary>
        /// The maximum WebGL build name
        /// </summary>
        public const int MaxSlugLength = 45;
        /// <summary>
        /// Set of invalid folder chars: "/, \, :, *, ?, ", <, >, and |."
        /// </summary>
        public static readonly ISet<char> InvalidFileNameCharactersSet = new HashSet<char>()
        {
            '\\',
            '/',
            ':',
            '*',
            '?',
            '"',
            '<',
            '>',
            '|'
        };

        /// <summary>
        /// Common strings that appears at the start of web URLs.
        /// </summary>
        public static readonly string[] stripStartOfUrl = new string[]
        {
            "https://www.",
            "http://www.",
            "https://",
            "http://"
        };

        /// <summary>
        /// Removes the https:// and the last / from a web URL.
        /// </summary>
        /// <param name="url">Full web URL.</param>
        /// <returns>Shortened web URL.</returns>
        public static string ShortenUrl(string url)
        {
            foreach (string stripFromStart in stripStartOfUrl)
            {
                if (url.StartsWith(stripFromStart) == true)
                {
                    url = url.Remove(0, stripFromStart.Length);
                    break;
                }
            }
            url = url.TrimEnd('/');
            return url;
        }

        /// <summary>
        /// Removes any invalid characters for building a file name.
        /// </summary>
        /// <param name="text">Text to remove diacritics from.</param>
        /// <param name="stringBuilder">
        /// An optional <see cref="StringBuilder"/> this method will use to
        /// generate the returned string. Good for performance.
        /// </param>
        /// <returns>
        /// <paramref name="text"/> with invalid file characters removed.
        /// </returns>
        /// <remarks>
        /// Taken from http://archives.miloush.net/michkap/archive/2007/05/14/2629747.html
        /// </remarks>
        public static string RemoveDiacritics(string text, StringBuilder stringBuilder = null)
        {
            // Setup StringBuilder
            if (stringBuilder == null)
            {
                stringBuilder = new StringBuilder(text.Length);
            }
            else
            {
                stringBuilder.Clear();
            }

            // Go through each character in the string.
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            foreach (char c in normalizedString)
            {
                // Check if this character is valid
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if ((unicodeCategory != UnicodeCategory.NonSpacingMark) && (InvalidFileNameCharactersSet.Contains(c) == false))
                {
                    // If so, append to the String Builder.
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Converts a filename to a web-friendly string.
        /// </summary>
        /// <param name="originalString">Original file name.</param>
        /// <returns>A web-friendly slug.</returns>
        /// <remarks>
        /// Taken from http://predicatet.blogspot.com/2009/04/improved-c-slug-generator-or-how-to.html
        /// </remarks>
        public static string GenerateSlug(string originalString)
        {
            // Remove invalid chars
            string returnSlug = Regex.Replace(originalString.ToLower(), @"[^a-z0-9\s-.]", "");

            // Convert multiple spaces into one space
            returnSlug = Regex.Replace(returnSlug, @"\s+", " ").Trim();

            // Trim the length of the slug down to MaxSlugLength characters
            if (returnSlug.Length > MaxSlugLength)
            {
                returnSlug = returnSlug.Substring(0, MaxSlugLength).Trim();
            }

            // Replace spaces with hyphens
            returnSlug = Regex.Replace(returnSlug, @"\s", "-");

            return returnSlug;
        }
    }
}
