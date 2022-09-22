namespace Crossword
{
    enum Direction
    {
        Horizontally,
        Vertically
    }

    class PlacedWord
    {
        public string word;
        public int startPosX;
        public int startPosY;
        public Direction direction;

        public PlacedWord(string word, int startPosX, int startPosY, Direction direction)
        {
            this.word = word;
            this.startPosX = startPosX;
            this.startPosY = startPosY;
            this.direction = direction;
        }   
    }

    internal class Program
    {
        //static string[] words = { "downtime", "midtown", "mowed", "women", "demo" };
        static string[] words = { "legendary", "enlarged", "general", "greedy", "anger" };
        static char[,] grid = new char[20, 20];
        static List<PlacedWord> placedWords = new List<PlacedWord>();

        static void Main(string[] args)
        {
            GenerateCrossword();
            DisplayCrossword();
        }

        static void GenerateCrossword()
        {
            for(int i = 0; i < words.Length; i++)
            {
                PlaceWord(words[i]);
            }
        }

        static void PlaceWord(string word)
        {
            int startX = 0;
            int startY = 0;

            //Place the first word
            if (placedWords.Count == 0)
            {
                WriteWordToGrid(word, startX, startY, Direction.Horizontally);
                return;
            }

            bool wordPlaced = false;

            //check for possible position
            for (int i = placedWords.Count - 1; i >= 0; i--)
            {
                PlacedWord placedWord = placedWords[i];

                if (wordPlaced)
                    return;

                //find where the two words intersect
                var intersections = word.Intersect(placedWord.word);

                foreach (char intersection in intersections)
                {
                    int placedWordIntersectionIndex = placedWord.word.IndexOf(intersection);

                    //define start position and direction
                    if (placedWord.direction == Direction.Horizontally)
                    {
                        //Console.WriteLine("trying to place {0} to {1}", word, placedWord.word);

                        startX = placedWord.startPosX + placedWordIntersectionIndex;
                        startY = placedWord.startPosY;

                        //check if can be placed vertically
                        bool canBePlaced = CheckWordPlacement(word, startX, startY, Direction.Vertically);
                        if (canBePlaced)
                        {
                            WriteWordToGrid(word, startX, startY, Direction.Vertically);
                            wordPlaced = true;
                            return;
                        }
                        else
                        {
                            canBePlaced = CheckWordPlacementOverlap(word, startX, startY - 1, Direction.Vertically);
                            if (canBePlaced)
                            {
                                WriteWordToGrid(word, startX, startY - 1, Direction.Vertically);
                                wordPlaced = true;
                                return;
                            }
                            continue;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("trying to place {0} to {1}", word, placedWord.word);

                        startX = placedWord.startPosX;
                        startY = placedWord.startPosY + placedWordIntersectionIndex;

                        //check if can be placed vertically
                        bool canBePlaced = CheckWordPlacement(word, startX, startY, Direction.Horizontally);
                        if (canBePlaced)
                        {
                            WriteWordToGrid(word, startX, startY, Direction.Horizontally);
                            wordPlaced = true;
                            return;
                        }
                        else
                        {
                            canBePlaced = CheckWordPlacementOverlap(word, startX - 1, startY, Direction.Horizontally);
                            if (canBePlaced)
                            {
                                WriteWordToGrid(word, startX - 1, startY, Direction.Horizontally);
                                wordPlaced = true;
                                return;
                            }
                            continue;
                        }
                    }
                }
            }
        }

        static bool CheckWordPlacementOverlap(string word, int startX, int startY, Direction direction)
        {
            //Console.WriteLine("Checking {0}, x{1}, y{2}", word, startX, startY);
            if (direction == Direction.Horizontally)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (startX + i >= 0)
                    {
                        if (i == 0 && grid[startY, startX] != default(char))
                        {
                            //Console.WriteLine("initial cell not empty");
                            return false;
                        }
                        else if (i == 1)
                        {
                            if (grid[startY, startX + i] != word[i])
                            {
                                //Console.WriteLine("second cell not the right word {0}, {1}", grid[startY + i, startX], word[i]);
                                return false;
                            }
                        }
                        else
                        {
                            if (grid[startY, startX + i] != default(char))
                            {
                                //Console.WriteLine("cell x{0}, y{1} not empty", startX, startY + i);
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (startY + i >= 0)
                    {
                        if (i == 0 && grid[startY, startX] != default(char))
                        {
                            //Console.WriteLine("initial cell not empty");
                            return false;
                        }
                        else if (i == 1)
                        {
                            if (grid[startY + i, startX] != word[i])
                            {
                                //Console.WriteLine("second cell not the right word {0}, {1}", grid[startY + i, startX], word[i]);
                                return false;
                            }
                        }
                        else
                        {
                            if (grid[startY + i, startX] != default(char))
                            {
                                //Console.WriteLine("cell x{0}, y{1} not empty", startX, startY + i);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        static bool CheckWordPlacement(string word, int startX, int startY, Direction direction)
        {
            if (direction == Direction.Horizontally)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (i == 0)
                    {
                        if (grid[startY, startX] != word[i])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //check if there is adjacent word
                        if (startY > 0 && grid[startY - 1, startX + i] != default(char))
                        {
                            return false;
                        }
                        else if (startY < grid.GetLength(0) && grid[startY + 1, startX + i] != default(char))
                        {
                            return false;
                        }

                        //check if row/column is empty
                        if (grid[startY, startX + i] != default(char))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (i == 0)
                    {
                        if (grid[startY, startX] != word[i])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //check if there is adjacent word
                        if (startX > 0 && grid[startY + i, startX - 1] != default(char))
                        {
                            return false;
                        }
                        else if (startX < grid.GetLength(1) && grid[startY + i, startX + 1] != default(char))
                        {
                            return false;
                        }

                        //check if row/column is empty
                        if (grid[startY + i, startX] != default(char))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        static void WriteWordToGrid(string word, int posX, int posY, Direction direction)
        {
            if (direction == Direction.Horizontally)
            {
                for(int i = 0; i < word.Length; i++)
                {
                    grid[posY, posX + i] = word[i];
                }
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    grid[posY + i, posX] = word[i];
                }
            }

            placedWords.Add(new PlacedWord(word, posX, posY, direction));
        }

        static void DisplayCrossword()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == default(char))
                    {
                        grid[i, j] = "*".ToCharArray()[0];
                    }
                    Console.Write("{0} ", grid[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}