using Microsoft.Maui.Controls;
using System;

namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private GameRules _gameRules; // Declare the GameRules object
        private string[] _wordList = { "CORRECT", "HANGMAN", "COMPUTER", "MAUI", "DEVELOPER" };
        private string _wordToGuess;

        public HangmanGamePage()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            // Pick a random word from the list
            Random rand = new Random();
            _wordToGuess = _wordList[rand.Next(_wordList.Length)];

            // Initialize the game with the chosen word and 7 lives
            _gameRules = new GameRules(_wordToGuess, 7);

            // Set the initial hangman image and masked word
            HangmanImage.Source = "hang1.png";
            MaskedWordLabel.Text = _gameRules.GetMaskedWord();
            RemainingLivesLabel.Text = $"Lives Remaining: {_gameRules.RemainingLives}";

            // Enable input controls (in case of restart)
            GuessEntry.IsEnabled = true;
            GuessButton.IsEnabled = true;
            ResultLabel.Text = "";
        }

        private void OnGuessButtonClicked(object sender, EventArgs e)
        {
            // Check if input is empty
            if (string.IsNullOrEmpty(GuessEntry.Text))
            {
                return;
            }

            // Ensure we get a single character from the input
            char guessedLetter = GuessEntry.Text.ToUpper()[0];

            // Now check if the letter is part of the word
            bool correctGuess = _gameRules.CheckLetterInWord(guessedLetter);

            // Clear the input after guess
            GuessEntry.Text = string.Empty;

            // Update the UI based on the guess result
            if (!correctGuess)
            {
                // Reduce lives if guess was incorrect
                if (_gameRules.RemainingLives <= 0)
                {
                    RemainingLivesLabel.Text = "Game Over!";
                    HangmanImage.Source = "hang8.png";
                    MaskedWordLabel.Text = $"The word was: {_wordToGuess}";
                    ResultLabel.Text = "You Died!";
                    DisableInput();
                }
                else
                {
                    // Update the hangman image based on remaining lives
                    HangmanImage.Source = $"hang{8 - _gameRules.RemainingLives}.png";
                    RemainingLivesLabel.Text = $"Lives Remaining: {_gameRules.RemainingLives}";
                }
            }
            else
            {
                // Update the masked word
                MaskedWordLabel.Text = _gameRules.GetMaskedWord();
            }

            // Check if the word is fully guessed (no more underscores)
            if (!_gameRules.GetMaskedWord().Contains("_"))
            {
                ResultLabel.Text = "Congratulations, you won!";
                MaskedWordLabel.Text = _wordToGuess; 
                DisableInput();
            }
        }

        // Disable input controls when the game is over
        private void DisableInput()
        {
            GuessEntry.IsEnabled = false;
            GuessButton.IsEnabled = false;
        }
    }

    // GameRules class for handling the game logic
    public class GameRules
    {
        private string _wordToGuess;
        private string _maskedWord;
        public int RemainingLives { get; private set; }

        public GameRules(string wordToGuess, int lives)
        {
            _wordToGuess = wordToGuess;

            // Initialize the masked word with underscores and spaces in between
            _maskedWord = string.Join(" ", new string('_', wordToGuess.Length).ToCharArray());

            RemainingLives = lives;
        }

        // This method checks if the guessed letter is in the word
        public bool CheckLetterInWord(char guessedLetter)
        {
            bool letterFound = false;
            char[] maskedWordArray = _maskedWord.Replace(" ", "").ToCharArray();

            for (int i = 0; i < _wordToGuess.Length; i++)
            {
                if (_wordToGuess[i] == guessedLetter && maskedWordArray[i] == '_')
                {
                    maskedWordArray[i] = guessedLetter;
                    letterFound = true;
                }
            }

            _maskedWord = string.Join(" ", maskedWordArray);

            // Decrease the lives if the letter is incorrect
            if (!letterFound)
            {
                RemainingLives--;
            }

            return letterFound;
        }

        // Get the masked word
        public string GetMaskedWord()
        {
            return _maskedWord;
        }
    }
}
