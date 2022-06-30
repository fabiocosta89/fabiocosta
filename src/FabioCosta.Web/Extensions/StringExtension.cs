namespace FabioCosta.Web.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Count the number of words existant in the string
        /// </summary>
        /// <param name="phrase">The string to count the words</param>
        /// <returns>The number of words in the string</returns>
        public static int WordCounter(this string phrase)
        {
            // if string is empty, return 0
            if (string.IsNullOrWhiteSpace(phrase)) return 0;

            int wordCounter = 1;

            for(int index = 0; index < phrase.Length; index++)
            {
                // check whether the current character is white space or new line or tab character
                if (phrase[index] == ' ' || phrase[index] == '\n' || phrase[index] == '\t')
                {
                    wordCounter++;
                }
            }

            return wordCounter;
        }
    }
}
