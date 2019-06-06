using System;

namespace SimpleMasterMind
{
	class Program
	{
		  	
		static void Main(string[] args)
		{
			
			var game = new Game(10, 4, 1, 6, true); //Spoiled answer. Set to 'false' to hide until the very end
			game.Play();            
			Console.ReadLine();
		}       
	}
}

