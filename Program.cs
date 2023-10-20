int size = 50; /*the size of the field, field is square*/
int[,] screen = new int[size, size]; /*the (for now) empty playfield (referd to as "gameboard" at some points later)*/
int steps = 10; /*number of steps for the simulation to be run*/



static int[,] randomBoard(int size){ /*returns a array same size as the "gameboard" where very pixel is randmly chosen to be either alive or dead*/
    Random rnd = new Random();
    int[,] randomisedBoard = new int[size, size];

    foreach (int y in Enumerable.Range( 0, size)){ /*for each pixel*/
        foreach (int x in Enumerable.Range( 0, size)){
            randomisedBoard[x, y] = rnd.Next(0, 2); /*randomly set to 1 or 0*/
        }
    }

    return randomisedBoard;
}

static void clearScreen(){ /*Writes a screen's worth of empty rows :P*/
    foreach (int i in Enumerable.Range(0, 60)){
        Console.WriteLine(" ");
    }
}

static void drawScreen(int[,] screen, int size, bool skipClear){ /*prints out the current state of the screen*/
    string symbol = "  ";

    if (skipClear == false){ /*if skipClear is true it skips clearing the screan before drawing the board*/
        clearScreen(); /*makes console blank (in a janky way)*/
    }

    foreach (int y in Enumerable.Range( 0, size)){ /*for each pixel*/
        foreach (int x in Enumerable.Range( 0, size)){

            if (screen[x, y] == 1){ /*sets correct symbol depending if pixel is alive or not*/
                symbol = "■ ";
            }
            else{
                symbol = "  ";
            }

            if (x == size-1){ /*Write the symbol for the given pixel, if last pixel of the row WriteLine to create new row afterwards*/
                Console.WriteLine(symbol);
            }
            else{
                Console.Write(symbol);
            }
        }
    }
}

static int[,] updateBoard(int[,] screen, int size){ /*Updates the "gameboard" to the next tick of the simulation*/

    int[,] updatedScreen = new int[size, size]; /*Create new array to collect values to return*/

    foreach (int y in Enumerable.Range( 0, size)){ /*for every pixel*/
        foreach (int x in Enumerable.Range( 0, size)){

            int total = 0; /*Collects how many alive neighbors*/

            int tempX = 0;
            int tempY = 0;

            foreach (int dy in Enumerable.Range( -1, 3)){ /*for all relevent combinations of relative pixels*/
                foreach (int dx in Enumerable.Range( -1, 3)){

                    tempX = x + dx; /*x screenwrap*/
                    if (tempX < 0){
                        tempX = tempX + size;
                    }
                    else if (tempX >= size){
                        tempX = tempX - size;
                    }

                    tempY = y + dy; /*y screenwrap*/
                    if (tempY < 0){
                        tempY = tempY + size;
                    }
                    else if (tempY >= size){
                        tempY = tempY - size;
                    }

                    if (dx != 0 || dy != 0){ /*dont count courself as a neighbor*/
                        total = total + screen[tempX, tempY];
                    }
                }
            }

            if (screen[x, y] == 1){ /*if pixel is alive*/
                if ((total == 2) || (total == 3)){ /*and has either 2 or 3 alive neighbors*/
                    updatedScreen[x, y] = 1; /*then continue being alive for next tick*/
                }
            }
            else{ /*if pixel is dead*/
                if (total == 3){ /*and has 3 alive neighbors*/
                    updatedScreen[x, y] = 1; /*then make alive for next tick*/
                }
            }
        }
    }
    return updatedScreen; /*passes the pdated screen back out of the function*/
}



bool validAnswer = false;

while (validAnswer == false){ /*ask user how large the "gameboard" should be and if invalid answer asks again*/
    Console.WriteLine("\nWhat size of board?");
    try{
        size = int.Parse(Console.ReadLine());
        validAnswer = true;
    }
    catch{
        Console.WriteLine("\nNot a number\n");
    }
}

validAnswer = false;

while (validAnswer == false){ /*ask user how many steps the simulation should run for and if invalid answer asks again*/
    Console.WriteLine("\nSimulate how many steps?");
    try{
        steps = int.Parse(Console.ReadLine());
        validAnswer = true;
    }
    catch{
        Console.WriteLine("\nNot a number\n");
    }
}

screen = randomBoard(size); /*generates random board and copie sit to "screen"*/

clearScreen(); /*clears screen of previous 2 questions*/

Console.WriteLine("Press enter to start simulation:\n"); /*Draws inital state of "gameboard" and waits for user to start simulation*/
drawScreen(screen, size, true);
Console.ReadLine();

while (steps >= 0){ /*for the number of ticks given in question two it updates the board before drawing it with a brief wait between ticks*/
    screen = updateBoard(screen, size);
    drawScreen(screen, size, false);
    steps -= 1;
    Thread.Sleep(200);
}