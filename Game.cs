using System;
using System.Text;

namespace SimpleMasterMind
{
	public class Game
	{
		private readonly int NumberOfAttempts;
		private readonly int NumberOfDigits;
		private readonly int MinDigitValue;
		private readonly int MaxDigitValue;
		private readonly bool SpoilAnswer;
		
		public Game(int numberOfAttempts, int numberOfDigits, int minDigitValue, int maxDigitValue, bool spoilAnswer = false)
		{
			NumberOfAttempts = numberOfAttempts;
			NumberOfDigits = numberOfDigits;
			MinDigitValue = minDigitValue;
			MaxDigitValue = maxDigitValue;
			SpoilAnswer = spoilAnswer;
		}
		
		public void Play() {
			var iGuess = GetRandomFourDigitArray();   

			if(SpoilAnswer) {
				Console.WriteLine("Spoiler: The correct combination is: " + ReturnPrintableArray(iGuess));
			}
           
			Console.WriteLine(string.Format("Enter {0} digits between and including 1 and 6 (comma separated). You have 10 attempts remaining.", NumberOfDigits));
			Console.WriteLine(string.Format("You have {0} attempts remaining.", NumberOfAttempts));

			var sb = new StringBuilder();
			var correctCount = CompareArrays(iGuess);
			
			if (correctCount < NumberOfDigits) {
				Console.WriteLine("You lost");
				Console.WriteLine("The correct combination was: " + ReturnPrintableArray(iGuess));
			}
		}
		
		int[] GetRandomFourDigitArray()
		{
			var inputArray = new int[NumberOfDigits];
			var rnd = new Random();
			for (int i = 0; i < NumberOfDigits; i++) {
				inputArray[i] = rnd.Next(MinDigitValue, MaxDigitValue + 1);
			}
			return inputArray;
		}
        
		string ReturnPrintableArray(int[] array)
		{
			var sb = new StringBuilder();
			sb.Append('[');
			for (int i = 0; i < array.Length; i++) {
				sb.Append(array[i]);
				sb.Append(',');
			}
			sb = sb.Remove(sb.Length - 1, 1);
			sb.Append(']');
			return sb.ToString();
		}
		
		int[] ReadInputArray()
		{
			var returnArray = new int[NumberOfDigits];
			
			for (; ;) {
				var inputString = Console.ReadLine();
				if (inputString.Contains(",")) {
					if (inputString.StartsWith(",", StringComparison.InvariantCulture)) {
						inputString = inputString.Substring(1);
					}
                    	
					if (inputString.EndsWith(",", StringComparison.InvariantCulture)) {
						inputString = inputString.Substring(0, inputString.Length - 1);
					}

					var inputArray = inputString.Split(',');
					if (inputArray.Length != NumberOfDigits) {
						Console.WriteLine(string.Format("Game only accepts {0} digits. Please try again", NumberOfDigits));
						continue;
					}

					int inputDigit = 0;
					for (int i = 0; i < inputArray.Length; i++) {

						int.TryParse(inputArray[i], out inputDigit);
						if (inputDigit <= 0) {
							Console.WriteLine("Invalid input. Only numbers are allowed. Please try again");
							break;
						} 
                            
						if (inputDigit > MaxDigitValue) {
							Console.WriteLine(string.Format("Invalid input. Only numbers from {0} to {1} are allowed. Please try again", MinDigitValue, MaxDigitValue));
							break;
						}

						returnArray[i] = inputDigit;
					}

					if (inputDigit < MinDigitValue || inputDigit > MaxDigitValue) {
						continue;
					}

					break;
				}
                    
				Console.WriteLine("Invalid input. Please try again");
				continue;
			}
			
			return returnArray;
		}
		
		int CompareArrays(int[] expectedArray) {
			int correctCount = 0;
			var sb = new StringBuilder();
			for (int t = 0; t < NumberOfAttempts; t++) {
				
				var uGuess = ReadInputArray();                                              
                
				sb.Clear();
				sb.Append('[');
				for (int i = 0; i < expectedArray.Length; i++) {
					if (uGuess[i] == expectedArray[i]) {
						sb.Append('+');
						correctCount++;
					} else if (Array.IndexOf(expectedArray, uGuess[i]) > -1) {
						sb.Append('-');
					} else {
						sb.Append(' ');
					}
					sb.Append(',');
				}

				sb = sb.Remove(sb.Length - 1, 1);
				sb.Append(']');

				if (correctCount >= NumberOfDigits) {
					Console.WriteLine("Congratulations. You won!");
					break;
				} else if (t <= NumberOfAttempts - 2) {
					correctCount = 0;
					Console.WriteLine(sb.ToString());
					Console.WriteLine(string.Format("Nice try. But guess again. You have {0} attempts remaining.", NumberOfAttempts - t - 1));
					continue;
				}
			}
			
			return correctCount;
		}
	}
}
