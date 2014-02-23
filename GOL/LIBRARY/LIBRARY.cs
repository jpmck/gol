/*-----------------------------------------------------------------------------------------------*/
/*--------------------------------------------HEADER---------------------------------------------*/
/*-----------------------------------------------------------------------------------------------

Program by James McKenna

PURPOSE:
 * Create the Game of Life.
 
ALGORITHM:
 * Use do loop to handle menu options (loop on bool for parsing user entry)
     * If user entry is valid, select appropriate entry with if statements
     * If user entry is invalid, do loop repeats

 * Random numbers method
     * Using 2 for loops, loop through array and fill each position with a random number

 * No repeating rows method
     * Assign values to first row, by looping through first row and filling each position with a
       random number.
     * Using 2 for loops, loop through the rest of the rows and utilize if statements to check for
       repeats, trying over as necessary.

 * Latin squares method
     * Similar to no repeating rows, but instead do no pre-population of the 1st row, and check for
       both row and column repeats, trying over as necessary.

BONUS:
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LIBRARY
{
    struct cells
    {
        public bool live;
        public bool liveNext;
        public decimal age;
        public byte liveNeighbors;
    }
    
    public class LIFE
    {
        private cells[,] PetriDish = new cells[42,62];
        private int generation, generations, menuNum;
        private bool menuOK;
        string type;
        
        // Life constructor
        public LIFE()
        {
            Console.SetWindowSize(64, 52);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "GAME OF LIFE";
            Console.CursorVisible = false;
            Welcome();
        }

        // Welcome the user the the GOL...
        public void Welcome()
        {
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | Welcome to the                                             |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" |  ##   ##   # #   ##     ##   ##    #    #  ##   ##         |");
            Console.WriteLine(" | #  # #  # # # # #  #   #  # #  #   #    # #  # #  #        |");
            Console.WriteLine(" | #    #  # # # # #  #   #  # #      #    # #    #           |");
            Console.WriteLine(" | # ## #### #   # ###    #  # ###    #    # ###  ###         |");
            Console.WriteLine(" | #  # #  # #   # #      #  # #      #    # #    #           |");
            Console.WriteLine(" | #  # #  # #   # #  #   #  # #      #    # #    #  #        |");
            Console.WriteLine(" |  ##  #  # #   #  ##     ##  #       ### # #     ##   # # # |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" |                                            Game of Life... |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" |                             (c) 2012 James P. McKenna, Jr. |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine("");
            Console.Write("                  { Press ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ANY KEY");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" to begin. }");
            Console.ReadKey();
        }

        // main menu starter
        public void StartMenuGo()
        {
            do
            {
                StartMenuShow();

                menuOK = int.TryParse(Console.ReadLine(), out menuNum);

                //If menu selection parses as int
                if (menuOK)
                {
                    /* If quit option (0) is selected -------------------------------------------*/
                    if (menuNum == 0)
                    {
                        Console.WriteLine();
                        menuOK = true;
                    }

                    /* If random (1) is selected ------------------------------------------------*/
                    else if (menuNum == 1)
                    {
                        type = "Random    ";
                        ConsolePrint(type);
                        SubMenu(type);
                    }

                    /* If glider grid 2 option (2) is selected ----------------------------------*/
                    else if (menuNum == 2)
                    {
                        type = "Glider    ";
                        ConsolePrint(type);
                        SubMenu(type);
                    }

                    /* If Latin Square option (3) is selected -----------------------------------*/
                    else if (menuNum == 3)
                    {
                        ConsolePrint("Pulsar    ");
                        SubMenu("Pulsar    ");
                    }

                    /* If sudoku option (4) is selected -----------------------------------------*/
                    else if (menuNum == 4)
                    {
                        ConsolePrint("Glider Gun");
                        SubMenu("Glider Gun");
                    }

                    /* If more info (5) is selected ---------------------------------------------*/
                    else if (menuNum == 5)
                    {
                        Info();
                    }

                    /* If instructions (5) is selected ---------------------------------------------*/
                    else if (menuNum == 6)
                    {
                        Instructions();
                    }

                    /* If they decided inputting another number was a good idea for some reason. */
                    else if (menuNum > 6 || menuNum < 0)
                    {
                        menuOK = false;
                    }
                }
            }//...While the menu selection is bad, do the loop again...
            while (!menuOK);
        }

        // does the submenu, after generations complete...
        private void SubMenu(string type)
        {
            do
            {
                // show the submenu
                SubMenuGenQuery();
                
                // try to parse user input
                menuOK = int.TryParse(Console.ReadLine(), out generations);

                // num too high or low, menu not ok
                if (generations > 500 || generations < 1)
                    menuOK = false;
                
                //input good?...
                if (menuOK)
                {
                    // print the initial generation
                    GenerationPrint(generations, type);
                                        
                    do
                    {
                        // show menu asking for number of generations
                        SubMenuGenContinue();

                        // input good?
                        menuOK = int.TryParse(Console.ReadLine(), out menuNum);

                        if (!menuOK)
                        {
                            menuOK = false;
                            break;
                        }
                        // if 0, quit
                        if (menuNum == 0)
                        {
                            menuOK = true;
                        }
                        else if (menuNum == 1)
                        {
                            menuOK = false;
                            break;
                        }
                        else if (menuNum == 2)
                        {
                            Stats();
                            menuOK = false;
                            break;
                        }
                        else if (menuNum > 2 || menuNum < 0)
                            break;
                    } while (!menuOK);

                }

            } while (!menuOK);

            menuOK = false;
        }

        // display the sub menu for how many generations to go through
        private void SubMenuGenQuery()
        {
            Console.SetCursorPosition(0, 46);
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("NUMBER OF GENERATIONS TO GENERATE? (1-500)                 ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine("   # of Generations:                                     ");
            Console.SetCursorPosition(21, 49);
        }

        // display the sub menu after generation asking to continue...
        private void SubMenuGenContinue()
        {
            Console.SetCursorPosition(0, 46);
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("CONTINUE?  1: YES | 2: YES (W/STATS) | 0: NO (MAIN MENU)   ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine("   Make a Selection:      ");
            Console.SetCursorPosition(21, 49);
        }

        // displays the main menu...
        private void StartMenuShow()
        {
            Console.Clear();
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("THE GAME OF LIFE - MAIN MENU                               ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | 1: Generate a random \"petri dish\"...                       |");
            Console.WriteLine(" | 2: Generate a grid with a \"glider\" in it...                |");
            Console.WriteLine(" | 3: Generate a grid with a \"pulsar\" in it...                |");
            Console.WriteLine(" | 4: Generate a grid with a \"glider gun\" in it...            |");
            Console.WriteLine(" |============================================================|");
            Console.WriteLine(" | 5: Get more info on the Game of Life                       |");
            Console.WriteLine(" | 6: Get instructions on how to use the Game of Life         |");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | 0: Quit                                                    |");
            Console.WriteLine(" ==============================================================");
            Console.Write("   Make a Selection: ");
        }

        // displays info to the console about the GOL...
        public void Info()
        {
            Console.Clear();
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ABOUT THE GAME OF LIFE                                     ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | Conway's Game of Life, also known as the Game of Life or   |");
            Console.WriteLine(" | simply Life, is a cellular autonmaton devised by the       |");
            Console.WriteLine(" | British mathematician John Horton Conway in 1970.  It is   |");
            Console.WriteLine(" | the best-known example of a cellular automaton.            |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | The \"game\" is actually a zero-player game, meaning that    |");
            Console.WriteLine(" | its evolution is determined by its initial state, needing  |");
            Console.WriteLine(" | no input from human players.  One interacts with the Game  |");
            Console.WriteLine(" | of Life by creating an initial configuration and observing |");
            Console.WriteLine(" | how it evolves.                                            |");
            Console.WriteLine(" ==============================================================\n");
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("THE RULES OF THE GAME OF LIFE                              ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | The universe of the Game of Life is an infinite two-       |");
            Console.WriteLine(" | dimensional orthogonal grid of square cells, each of which |");
            Console.WriteLine(" | is in one of two possible states, live or dead.  Every     |");
            Console.WriteLine(" | cell interacts with its eight neighbours, which are the    |");
            Console.WriteLine(" | cells that are directly horizontally, vertically, or       |");
            Console.WriteLine(" | diagonally adjacent. At each step in time, the following   |");
            Console.WriteLine(" | transitions occur:                                         |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | 1.) Any live cell with fewer than two live neighbours      |");
            Console.WriteLine(" |     dies, as if by needs caused by underpopulation.        |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | 2.) Any live cell with more than three live neighbours     |");
            Console.WriteLine(" |     dies, as if by overcrowding.                           |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | 3.) Any live cell with two or three live neighbours lives, |");
            Console.WriteLine(" |     unchanged, to the next generation.                     |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | 4.) Any dead cell with exactly three live neighbours cells |");
            Console.WriteLine(" |     will come to life.                                     |");
            Console.WriteLine(" ==============================================================\n");
            Console.Write("                 { Press ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ANY KEY");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" continue. }");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("WHAT IS THE MEANING OF LIFE...?                            ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | \"The Answer to the Great Question...                       |");
            Console.WriteLine(" | \"of LIFE, the Universe and Everything...                   |");
            Console.WriteLine(" | \"is...                                                     |");
            Console.WriteLine(" | \"forty-two...\"                                             |");
            Console.WriteLine(" |                       - Deep Thought,                      |");
            Console.WriteLine(" |                       The Hitchhiker's Guide to the Galaxy |");
            Console.WriteLine(" ==============================================================");
            Console.Write("          { Press ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ANY KEY");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" go back to the main menu. }");
            Console.ReadKey();
            StartMenuGo();
        }

        // displays instructions to the console
        public void Instructions()
        {
            Console.Clear();
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("INSTRUCTIONS                                               ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | From the main menu, select a starting arrangement to begin |");
            Console.WriteLine(" | the game with.  (1) gives a grid of squares, (2-4) place a |");
            Console.WriteLine(" | known pattern on the grid of squares.                      |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | After selecting a starting pattern, it will be displayed   |");
            Console.WriteLine(" | and you will be prompted for the number of generations to  |");
            Console.WriteLine(" | cycle through.  After pressing enter, the Game of Life     |");
            Console.WriteLine(" | will run through the number of selected generations.       |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" | At this point you can press (1) to continue, (2) to dis-   |");
            Console.WriteLine(" | play statistics about the grid, or (3) to return to the    |");
            Console.WriteLine(" | main menu.  (Information on statistics is shown below.)    |");
            Console.WriteLine(" ==============================================================\n");
            Console.WriteLine(" ==============================================================");
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("STATISTICS OPTION                                          ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | If Stats are requested, they will be displayed at the top  |");
            Console.WriteLine(" | of the screen.  Below is a mock-up of the stats display.   |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | GOL: Example        0000 | 0000 1111 2222 3333 4444 | 1000 |");
            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | # live cells ==========^      ^    ^    ^    ^    ^     ^  |");
            Console.WriteLine(" |                               |    |    |    |    |     |  |");
            Console.WriteLine(" | # cells one generation old ===|    |    |    |    |     |  |");
            Console.WriteLine(" | # cells two generations old =======|    |    |    |     |  |");
            Console.WriteLine(" | # cells three generation old ===========|    |    |     |  |");
            Console.WriteLine(" | # cells four generation old =================|    |     |  |");
            Console.WriteLine(" | # cells over four generations old ================|     |  |");
            Console.WriteLine(" |                                                         |  |");
            Console.WriteLine(" | # of generations =======================================|  |");
            Console.WriteLine(" |                                                            |");
            Console.WriteLine(" ==============================================================\n");
            Console.Write("          { Press ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ANY KEY");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" go back to the main menu. }");
            Console.ReadKey();
            StartMenuGo();
        }

        // generate random cells and print to console.
        public void ConsolePrint(string type)
        {
            if (type == "Random    ")
            {
                RandomCellsGeneration();
                Print(type);
            }
            if (type == "Glider    ")
            {
                GliderGeneration();
                Print(type);
            }
            if (type == "Pulsar    ")
            {
                PulsarGeneration();
                Print(type);
            }
            if (type == "Glider Gun")
            {
                GliderGunGeneration();
                Print(type);
            }

            
            //Console.ReadKey();
        }

        // Goes through x generations while displying to the console...
        public void GenerationPrint(int generations, string type)
        {
            do
            {
                CycleLife();
                Print(type);
                System.Threading.Thread.Sleep(25); // pause n milliseconds
                generations --;
            } while (generations > 0);
        }

        // Randomly places cells on the PetriDish array
        private void RandomCellsGeneration()
        {
            Random rand = new Random();

            generation = 1;

            for (int i = 1; i < 42; i++)
            {
                for (int j = 1; j < 62; j++)
                {
                    if (rand.Next(0, 2) == 1)
                    {
                        PetriDish[i, j].live = true;
                        PetriDish[i, j].age = 0;
                    }

                    else
                    {
                        PetriDish[i, j].live = false;
                        PetriDish[i, j].age = -1;
                    }

                    PetriDish[i, j].liveNeighbors = 0;
                    PetriDish[i, j].liveNext = false;
                }
            }
        }

        // places a glider on the array
        private void GliderGeneration()
        {
            generation = 1;
            
            // new agar for our petri dish!
            for (int i = 0; i < 42; i++)
            {
                for (int j = 0; j < 62; j++)
                {
                    PetriDish[i, j].live = false;
                    PetriDish[i, j].age = -1;
                    PetriDish[i, j].liveNeighbors = 0;
                    PetriDish[i, j].liveNext = false;
                }
            }

            //insert the glider
            PetriDish[1, 2].live = true;
            PetriDish[1, 2].age = 0;

            PetriDish[2, 3].live = true;
            PetriDish[2, 3].age = 0;

            PetriDish[3, 1].live = true;
            PetriDish[3, 1].age = 0;
            PetriDish[3, 2].live = true;
            PetriDish[3, 2].age = 0;
            PetriDish[3, 3].live = true;
            PetriDish[1, 3].age = 0;
        }

        // places a pulsar on the array
        private void PulsarGeneration()
        {
            generation = 1;

            // new agar for our petri dish!
            for (int i = 1; i < 42; i++)
            {
                for (int j = 1; j < 62; j++)
                {
                    PetriDish[i, j].live = false;
                    PetriDish[i, j].age = -1;
                    PetriDish[i, j].liveNeighbors = 0;
                    PetriDish[i, j].liveNext = false;
                }
            }

            //insert the pulsar
            PetriDish[13, 18].live = true;
            PetriDish[13, 18].age = 0;
            PetriDish[13, 19].live = true;
            PetriDish[13, 19].age = 0;
            PetriDish[13, 20].live = true;
            PetriDish[13, 20].age = 0;
            PetriDish[13, 24].live = true;
            PetriDish[13, 24].age = 0;
            PetriDish[13, 25].live = true;
            PetriDish[13, 25].age = 0;
            PetriDish[13, 26].live = true;
            PetriDish[13, 26].age = 0;

            PetriDish[18, 18].live = true;
            PetriDish[18, 18].age = 0;
            PetriDish[18, 19].live = true;
            PetriDish[18, 19].age = 0;
            PetriDish[18, 20].live = true;
            PetriDish[18, 20].age = 0;
            PetriDish[18, 24].live = true;
            PetriDish[18, 24].age = 0;
            PetriDish[18, 25].live = true;
            PetriDish[18, 25].age = 0;
            PetriDish[18, 26].live = true;
            PetriDish[18, 26].age = 0;

            PetriDish[20, 18].live = true;
            PetriDish[20, 18].age = 0;
            PetriDish[20, 19].live = true;
            PetriDish[20, 19].age = 0;
            PetriDish[20, 20].live = true;
            PetriDish[20, 20].age = 0;
            PetriDish[20, 24].live = true;
            PetriDish[20, 24].age = 0;
            PetriDish[20, 25].live = true;
            PetriDish[20, 25].age = 0;
            PetriDish[20, 26].live = true;
            PetriDish[20, 26].age = 0;

            PetriDish[25, 18].live = true;
            PetriDish[25, 18].age = 0;
            PetriDish[25, 19].live = true;
            PetriDish[25, 19].age = 0;
            PetriDish[25, 20].live = true;
            PetriDish[25, 20].age = 0;
            PetriDish[25, 24].live = true;
            PetriDish[25, 24].age = 0;
            PetriDish[25, 25].live = true;
            PetriDish[25, 25].age = 0;
            PetriDish[25, 26].live = true;
            PetriDish[25, 26].age = 0;

            //
            PetriDish[15, 16].live = true;
            PetriDish[15, 16].age = 0;
            PetriDish[16, 16].live = true;
            PetriDish[16, 16].age = 0;
            PetriDish[17, 16].live = true;
            PetriDish[17, 16].age = 0;
            PetriDish[21, 16].live = true;
            PetriDish[21, 16].age = 0;
            PetriDish[22, 16].live = true;
            PetriDish[22, 16].age = 0;
            PetriDish[23, 16].live = true;
            PetriDish[23, 16].age = 0;

            PetriDish[15, 21].live = true;
            PetriDish[15, 21].age = 0;
            PetriDish[16, 21].live = true;
            PetriDish[16, 21].age = 0;
            PetriDish[17, 21].live = true;
            PetriDish[17, 21].age = 0;
            PetriDish[21, 21].live = true;
            PetriDish[21, 21].age = 0;
            PetriDish[22, 21].live = true;
            PetriDish[22, 21].age = 0;
            PetriDish[23, 21].live = true;
            PetriDish[23, 21].age = 0;

            PetriDish[15, 23].live = true;
            PetriDish[15, 23].age = 0;
            PetriDish[16, 23].live = true;
            PetriDish[16, 23].age = 0;
            PetriDish[17, 23].live = true;
            PetriDish[17, 23].age = 0;
            PetriDish[21, 23].live = true;
            PetriDish[21, 23].age = 0;
            PetriDish[22, 23].live = true;
            PetriDish[22, 23].age = 0;
            PetriDish[23, 23].live = true;
            PetriDish[23, 23].age = 0;

            PetriDish[15, 28].live = true;
            PetriDish[15, 28].age = 0;
            PetriDish[16, 28].live = true;
            PetriDish[16, 28].age = 0;
            PetriDish[17, 28].live = true;
            PetriDish[17, 28].age = 0;
            PetriDish[21, 28].live = true;
            PetriDish[21, 28].age = 0;
            PetriDish[22, 28].live = true;
            PetriDish[22, 28].age = 0;
            PetriDish[23, 28].live = true;
            PetriDish[23, 28].age = 0;
        }

        // places a glider gun on the array
        private void GliderGunGeneration()
        {
            generation = 1;

            // new agar for our petri dish!
            for (int i = 1; i < 42; i++)
            {
                for (int j = 1; j < 62; j++)
                {
                    PetriDish[i, j].live = false;
                    PetriDish[i, j].age = -1;
                    PetriDish[i, j].liveNeighbors = 0;
                    PetriDish[i, j].liveNext = false;
                }
            }

            //insert the glider gun
            PetriDish[1, 25].live = true;
            PetriDish[1, 25].age = 0;
            
            PetriDish[2, 23].live = true;
            PetriDish[2, 23].age = 0;
            PetriDish[2, 25].live = true;
            PetriDish[2, 25].age = 0;

            PetriDish[3, 13].live = true;
            PetriDish[3, 13].age = 0;
            PetriDish[3, 14].live = true;
            PetriDish[3, 14].age = 0;
            PetriDish[3, 21].live = true;
            PetriDish[3, 21].age = 0;
            PetriDish[3, 22].live = true;
            PetriDish[3, 22].age = 0;
            PetriDish[3, 35].live = true;
            PetriDish[3, 35].age = 0;
            PetriDish[3, 36].live = true;
            PetriDish[3, 36].age = 0;

            PetriDish[4, 12].live = true;
            PetriDish[4, 12].age = 0;
            PetriDish[4, 16].live = true;
            PetriDish[4, 16].age = 0;
            PetriDish[4, 21].live = true;
            PetriDish[4, 21].age = 0;
            PetriDish[4, 22].live = true;
            PetriDish[4, 22].age = 0;
            PetriDish[4, 35].live = true;
            PetriDish[4, 35].age = 0;
            PetriDish[4, 36].live = true;
            PetriDish[4, 36].age = 0;

            PetriDish[5, 1].live = true;
            PetriDish[5, 1].age = 0;
            PetriDish[5, 2].live = true;
            PetriDish[5, 2].age = 0;
            PetriDish[5, 11].live = true;
            PetriDish[5, 11].age = 0;
            PetriDish[5, 17].live = true;
            PetriDish[5, 17].age = 0;
            PetriDish[5, 21].live = true;
            PetriDish[5, 21].age = 0;
            PetriDish[5, 22].live = true;
            PetriDish[5, 22].age = 0;

            PetriDish[6, 1].live = true;
            PetriDish[6, 1].age = 0;
            PetriDish[6, 2].live = true;
            PetriDish[6, 2].age = 0;
            PetriDish[6, 11].live = true;
            PetriDish[6, 11].age = 0;
            PetriDish[6, 15].live = true;
            PetriDish[6, 15].age = 0;
            PetriDish[6, 17].live = true;
            PetriDish[6, 17].age = 0;
            PetriDish[6, 18].live = true;
            PetriDish[6, 18].age = 0;
            PetriDish[6, 23].live = true;
            PetriDish[6, 23].age = 0;
            PetriDish[6, 25].live = true;
            PetriDish[6, 25].age = 0;

            PetriDish[7, 11].live = true;
            PetriDish[7, 11].age = 0;
            PetriDish[7, 17].live = true;
            PetriDish[7, 17].age = 0;
            PetriDish[7, 25].live = true;
            PetriDish[7, 25].age = 0;

            PetriDish[8, 12].live = true;
            PetriDish[8, 12].age = 0;
            PetriDish[8, 16].live = true;
            PetriDish[8, 16].age = 0;
            
            PetriDish[9, 13].live = true;
            PetriDish[9, 13].age = 0;
            PetriDish[9, 14].live = true;
            PetriDish[9, 14].age = 0;
        }

        //Prints the current PetriDish array to the console...
        private void Print(string type)
        {
            Console.Clear();

            Console.WriteLine(" ==============================================================");
            Console.WriteLine(" | GOL: "+ type +"                            Generation: " + generation.ToString("000") + " |");
            Console.WriteLine(" ==============================================================");


            for (int i = 0; i < 42; i++)
            {
                for (int j = 0; j < 62; j++)
                {
                    if (PetriDish[i, j].live == true)
                    {
                        if (PetriDish[i, j].age == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("#");
                        }

                        if (PetriDish[i, j].age == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("#");
                        }

                        if (PetriDish[i, j].age == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("#");
                        }

                        if (PetriDish[i, j].age == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("#");
                        }

                        if (PetriDish[i, j].age >= 4)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("#");
                        }

                        //Console.Write("#");
                    }
                    else
                        Console.Write(" ");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();

            }
        }

        // Stats generation
        // Goes through one cycle of life in the dish...
        private void Stats()
        {
            int numAlive = 0, num0 = 0, num1 = 0, num2 = 0, num3 = 0, num4plus = 0;

            for (int i = 1; i < 41; i++)
            {
                for (int j = 1; j < 61; j++)
                {
                    if (PetriDish[i, j].live == true)
                        numAlive++;
                    if (PetriDish[i, j].age == 0)
                        num0++;
                    if (PetriDish[i, j].age == 1)
                        num1++;
                    if (PetriDish[i, j].age == 2)
                        num2++;
                    if (PetriDish[i, j].age == 3)
                        num3++;
                    if (PetriDish[i, j].age > 4)
                        num4plus++;
                }
            }

            Console.SetCursorPosition(24, 1);
            Console.Write(numAlive.ToString("0000") + " | " + num0.ToString("0000") + " ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(num1.ToString("0000") + " ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(num2.ToString("0000") + " ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(num3.ToString("0000") + " ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(num4plus.ToString("0000") + " ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("|");

        }

        // Goes through one cycle of life in the dish...
        private void CycleLife()
        {
            for (int i = 1; i < 41; i++)
            {
                for (int j = 1; j < 61; j++)
                {
                    //Add up (bool to byte) total number of neighbors...
                    PetriDish[i, j].liveNeighbors = (byte)(Convert.ToByte(PetriDish[i - 1, j - 1].live) + Convert.ToByte(PetriDish[i - 1, j].live) + Convert.ToByte(PetriDish[i - 1, j + 1].live) +
                        Convert.ToByte(PetriDish[i, j - 1].live) + Convert.ToByte(PetriDish[i, j + 1].live) +
                        Convert.ToByte(PetriDish[i + 1, j - 1].live) + Convert.ToByte(PetriDish[i + 1, j].live) + Convert.ToByte(PetriDish[i + 1, j + 1].live));

                    //if cell is alive...
                    if (PetriDish[i, j].live == true)
                    {
                        //if cell has too few / many neighbors...
                        if (PetriDish[i, j].liveNeighbors < 2 || PetriDish[i, j].liveNeighbors > 3)
                        {
                            //it will die
                            PetriDish[i, j].liveNext = false;
                            PetriDish[i, j].age = -1;
                        }

                        //if cell has right number of neighbors
                        else if (PetriDish[i, j].liveNeighbors == 2 || PetriDish[i, j].liveNeighbors == 3)
                        {
                            //it will live
                            PetriDish[i, j].liveNext = true;
                            PetriDish[i, j].age++;
                        }
                    }

                    //if cell is dead
                    if (PetriDish[i, j].live == false)
                    {
                        //if cell has right number of neighbors
                        if (PetriDish[i, j].liveNeighbors == 3)
                        {
                            //it will live
                            PetriDish[i, j].liveNext = true;
                            PetriDish[i, j].age = 0;
                        }
                    }
                }

            }

            //go through and change values of next to now
            for (int i = 0; i < 42; i++)
            {
                for (int j = 0; j < 62; j++)
                {
                    PetriDish[i, j].live = PetriDish[i, j].liveNext;
                }
            }

            generation++;
        }
    }
}