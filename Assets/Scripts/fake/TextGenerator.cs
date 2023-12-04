using UnityEngine;

namespace fake
{
    public static class TextGenerator
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz";

        public static string GetName()
        {
            var numberOfWords = Random.Range(1, 3);
            var fakeName = "";

            for (int i = 0; i < numberOfWords; i++)
            {
                var wordLength = Random.Range(4, 8);
                var word = string.Empty;

                for (int j = 0; j < wordLength; j++)
                {
                    char randomChar = characters[Random.Range(0, characters.Length)];
                    word += randomChar;
                }

                fakeName += char.ToUpper(word[0]) + word.Substring(1);

                if (i < numberOfWords - 1) fakeName += " ";
            }

            return fakeName;
        }

        public static string GetParagraph()
        {
            var numberOfSentences = Random.Range(1, 4);
            int totalWords = Random.Range(10, 26);
            string fakeParagraph = "";

            for (int i = 0; i < numberOfSentences; i++)
            {
                int wordsInSentence = totalWords / numberOfSentences;
                if (i == numberOfSentences - 1)
                {
                    // Add the remaining words to the last sentence
                    wordsInSentence = totalWords - (numberOfSentences - 1) * wordsInSentence;
                }

                for (int j = 0; j < wordsInSentence; j++)
                {
                    int wordLength = Random.Range(4, 8);
                    string word = "";

                    for (int k = 0; k < wordLength; k++)
                    {
                        char randomChar = characters[Random.Range(0, characters.Length)];
                        word += randomChar;
                    }

                    word = char.ToUpper(word[0]) + word.Substring(1);

                    if (j > 0) fakeParagraph += " ";

                    fakeParagraph += word;
                }


                if (i < numberOfSentences - 1) fakeParagraph += ". ";
                else fakeParagraph += ".";
            }
            
            return fakeParagraph;
        }
    }
}