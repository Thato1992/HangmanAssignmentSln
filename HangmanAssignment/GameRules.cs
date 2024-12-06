public class GameRules
{
    public string Word { get; private set; } = "CROSSWORD";
    public string MaskedWord { get; private set; }
    public int RemainingLives { get; private set; } = 5;

    public GameRules()
    {
        MaskedWord = new string('_', Word.Length); // Mask the word initially
    }

    public void ProcessGuess(string guess)
    {
        if (string.IsNullOrEmpty(guess) || guess.Length > 1)
            return;

        char letter = guess[0];
        bool guessedCorrectly = false;

        // Check if the guessed letter is in the word
        var maskedWordArray = MaskedWord.ToCharArray();
        for (int i = 0; i < Word.Length; i++)
        {
            if (Word[i] == letter && maskedWordArray[i] == '_')
            {
                maskedWordArray[i] = letter;
                guessedCorrectly = true;
            }
        }

        MaskedWord = new string(maskedWordArray);

        if (!guessedCorrectly)
        {
            RemainingLives--;
        }
    }

    public string GetMaskedWord()
    {
        return MaskedWord;
    }
}
