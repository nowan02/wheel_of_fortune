using System;
using System.IO;

namespace wheel_of_fortune
{
    /// <summary>
    /// The game class initialises with the path of the proverbs (which it reads into an array) and the number of players, which is user given.
    /// The players' points will be stored in an array with the lenght of the number of players.
    /// All the fields within the class are private, as they will not be accessed outside of the class. 
    /// The prize pool and beginning prize range is predefined.
    /// </summary>
    class Game
    {
        readonly string _vowels;
        readonly string[] _proverbs;
        readonly int[] _prizes;
        string _chosenProverb;
        char[] _chosenProverbArray;
        int[] _playerScores;

        public Game(string ProverbsPath)
        {
            _vowels = "aeiou";
            _proverbs = File.ReadAllLines(ProverbsPath);
            _prizes = new int[] {500, 1000, 5000, 10000, 20000};  // The prizes are the current HUF banknotes in use :)
        }

        /// <summary>
        /// Prints out the letters that have been guessed by the current player (this is the input) with the help of the character array
        /// Created by the _init() method. It iterates through the original string, when it reaches a correctly guessed letter,
        /// The corresponding underline is replaced by the letter itself.
        /// Then it randomly chooses an amount from the prize pool and multiplies it by the number of letters guessed.
        /// This amount is returned as prize * multiplier and added to the player's prizes.
        /// </summary>
        int _outGuess(char Guess, int Player)
        {
        int _final = 0;
        int _multiplier = 0;
        int _prize = 0;
    
        _prize = _prizes[new Random().Next(0, _prizes.Length)];
        Console.WriteLine("Player {0} spinned value {1} HUF", Player, _prize);
        _multiplier = 0;
        for(int i = 0; i < _chosenProverbArray.Length; i++)
        {
            if(_chosenProverb[i] == Guess)
            {
                _chosenProverbArray[i] = Guess;
                _multiplier += 1;
                Console.Write($"{Guess} ");
            }
            else
            {
                Console.Write($"{_chosenProverbArray[i] }");
            }
        }
        Console.WriteLine("\nPlayer {0} earns {1}*{2} = {3} HUF", Player, _multiplier, _prize, _multiplier*_prize);
        _final += _prize*_multiplier;
        return _final;
        }

        int _outGuess(string Guess, int Player)
        {
            int _multiplier = 0;
            int _prize = _prizes[new Random().Next(0, _prizes.Length)];
            for(int i = 0; i < _chosenProverb.Length; i++)
            {
                if(_chosenProverb[i] == Guess[i]) _multiplier++;
            }

            if(_multiplier == _chosenProverb.Length)
            {
                for(int i = 0; i < _chosenProverbArray.Length; i++)
                {
                    _chosenProverbArray[i] = Guess[i];
                }
                Console.WriteLine("Player {0} guessed the sentence:\n{1}", Player, _chosenProverb);
                Console.WriteLine("Player {0} wins {1}*{2} HUF", Player, _multiplier, _prize);
                return _prize*_multiplier;
            }
            Console.WriteLine("Player {0}'s guess wasn't right.");
            return 0;
        }

        /// <summary>
        /// Initialises the game instance by choosing a random proverb and converting it to a character array.
        /// This array is only made up of underlines in the beginning, as no letters have been guessed so far.
        /// The user is given the option to choose the number of players, if left blank it defaults to 1 player.
        /// </summary>
        void _init()
        {
            Console.WriteLine("### Welcome to Wheel of Fortune! ###");
            Console.WriteLine("Firstly, please type in the number of players!\nIf you want to play by yourself, type 1!");
            int playerNum = 0;
            while(playerNum == 0)
            {
                try
                {
                    playerNum = int.Parse(Console.ReadLine());
                }
                catch(Exception)
                {
                    Console.WriteLine("The entered character was not a number, try again!");
                }
            }
            _playerScores = new int[playerNum];
            _chosenProverb = _proverbs[new Random().Next(0, _proverbs.Length)].ToLower();
            _chosenProverbArray = new char[_chosenProverb.Length];
            
            for(int i = 0; i < _chosenProverbArray.Length; i++)
            {
                if(_chosenProverb[i] != ' ') 
                {
                _chosenProverbArray[i] = '_';
                } else _chosenProverbArray[i] = ' ';
            }
        }

        /// <summary>
        /// Compares the current sentence to the proverb which currently needs to be guessed.
        /// </summary>
        bool _compare()
        {
            string _sentence = "";
            for(int i = 0; i < _chosenProverbArray.Length; i++)
            {
                _sentence += _chosenProverbArray[i];
            }

            return _sentence == _chosenProverb;
        }

        bool _checkForVowel(char Guess)
        {
            int i = 0;
            while(i < _vowels.Length && Guess != _vowels[i])
            {
                i++;
            }
            if(i < _vowels.Length) return true;
            return false;
        }

        /// <summary>
        /// The main game loop.
        /// </summary>
        public void GameLoop()
        {
            _init();
            int _round = 1;
            int _player = 0;
            int _win = 0;
            char _guess;
            do
            {
                Console.WriteLine("Round {0}:", _round);
                _guess = ' ';
                do
                {
                    try
                    {
                        Console.WriteLine("Player {0}, please write the letter you want to guess, or type '0' if you'd like to guess the entire sentence!", _player+1);
                        _guess = char.Parse(Console.ReadLine());
                        if(_guess == '0')
                        {
                            Console.WriteLine("Player {0} would like to guess the entire sentence.", _player+1);
                            _playerScores[_player] += _outGuess(Console.ReadLine().ToLower(), _player);
                        }
                        else
                        { 
                            if(_checkForVowel(_guess))
                            {
                                Console.WriteLine("Vowels can't be guessed, try again!");
                                _guess = ' ';
                                continue;
                            }
                        }
                        Console.WriteLine("Player {0} guessed \"{1}\"", _player+1, _guess);
                        _win = _outGuess(_guess, _player);
                        _playerScores[_player] += _win;
                        _guess = ' ';
                        if(_win != 0) Console.WriteLine("Player {0} takes another turn.", _player+1);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("The input wasn't a valid character, try again!");
                    }
                }
                while (_guess == ' ' && _win != 0);
                _round++;
                _win = 0;
            }
            while(_compare() == false);
            // End of game
            Console.WriteLine("######## GAME OVER ########");
            Console.WriteLine("Player {0} won the entire game and took home {1} HUF", _player, _playerScores[_player]);
            Console.WriteLine("Would you like to start over? Type 'yes' or 'no'");
            // Game restart option
            string _reset = "";
            while(_reset != "yes" || _reset != "no")
            {
                _reset = Console.ReadLine();
            }

            if(_reset == "yes")
            {
                GameLoop();
            }
            else
            {
                Console.WriteLine("Goodbye!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game1 = new Game("proverbs.txt");
            game1.GameLoop();
        }
    }
}
