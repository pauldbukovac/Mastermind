using System;
using System.Text;

namespace Mastermind
{
	class Program
	{
		// The length of the secret code.
		private const int CodeLength = 4;

		// The lowest digit that can be generated when creating a code.
		private const int MinDigit = 1;

		// The highest digit that can be generated when creating a code.
		private const int MaxDigit = 6;

		// Used to randomly generate a secret code.
		private static Random rand = new Random();

		static void Main(string[] args)
		{
			// Ask the user for how many chances they'd like to try to guess the code.
			int chances = GetChances();

			// Get a new secret code.
			int[] code = GetRandomCode();

			// Keeps track of whether the user has run out of chances.
			bool gameIsOver = false, userDidWin = false;

			CompareResult result = new CompareResult();

			// Run until the user guesses the code or runs out of chances.
			while (!gameIsOver && !userDidWin) 
			{
				// Prompt the user to enter a guess, then validate that guess.
				int[] guess = GetGuess();

				// Compare the guess to the randomly generated secret code.
				result = CheckGuess(code, guess);

				// Show the user their current score.
				Console.WriteLine(result.GetScore());

				// If the user got 0 wrong, then they won.
				userDidWin = result.Wrong == 0;

				// Update how many chances the user has and check to see if the game is over.
				chances--;
				gameIsOver = chances == 0;
			}

			if (userDidWin)
			{
				Console.WriteLine("You solved it!");
			}
			else
			{
				Console.WriteLine("You lose :(");
			}

			// This is here just to let the user see the end-game messages without the console closing.
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}

		// Prompt the user for how many chances they'd like to try to guess the code.
		private static int GetChances()
		{
			string input = "";

			int chances = 0;

			// Loop until the user enters a number greater than 0.
			while (chances <= 0)
			{
				Console.WriteLine("How many chances to guess would you like?");

				input = Console.ReadLine();

				int.TryParse(input, out chances);
			}

			return chances;
		}

		// Prompt the user for their guess at the code, then turn that guess (a string) into an int[].
		private static int[] GetGuess()
		{
			string guess = "";

			// Loop until the user enters a code that contains only numbers and is also the same length as the
			// secret code.
			while (!Validate(guess))
			{
				Console.WriteLine("Please enter your guess.");

				guess = Console.ReadLine();
			}

			// Turn a string of digits into an int[].
			return ParseGuess(guess);
		}

		// Randomly generate a secret code.
		private static int[] GetRandomCode()
		{
			int[] code = new int[CodeLength];

			for (int i = 0; i < CodeLength; i++)
			{
				code[i] = rand.Next(MinDigit, MaxDigit + 1);
			}

			return code;
		}

		// Turn a string of digits into an int[].
		private static int[] ParseGuess(string guess)
		{
			int[] ret = new int[CodeLength];

			for (int i = 0; i < CodeLength; i++)
			{
				ret[i] = (int)char.GetNumericValue(guess[i]);
			}

			return ret;
		}

		// Check that the user's input is both an integer and the same length as the secret code.
		private static bool Validate(string str)
		{
			return int.TryParse(str, out int unused) && str.Length == CodeLength;
		}

		// Compare the user's guess to the secret code.
		private static CompareResult CheckGuess(int[] code, int[] guess)
		{
			CompareResult result = new CompareResult();

			for (int i = 0; i < CodeLength; i++)
			{
				if (code[i] == guess[i])
				{
					result.Right++;
				}
				else
				{
					result.Wrong++;
				}
			}

			return result;
		}

		// The results of comparing the user's guess to the secret code.
		private class CompareResult
		{
			// The number of digits the user guessed correctly.
			public int Right { get; set; }

			// The number of digits the user guessed incorrectly.
			public int Wrong { get; set; }

			// For each digit the user has guessed right, a '+' is printed. A '-' is printed for every incorrect guess.
			public string GetScore()
			{
				StringBuilder sbOut = new StringBuilder();

				for (int i = 0; i < Right; i++)
				{
					sbOut.Append('+');
				}

				for (int i = 0; i < Wrong; i++)
				{
					sbOut.Append('-');
				}

				return sbOut.ToString();
			}
		}
	}
}
