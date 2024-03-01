using System.Collections;
using System;
using System.Collections.Generic;

//Cameron Dennis 

class Program
{
    private static void Main(string[] args)
    {
        //Main function script:
        int[,] start_board = Gen_board();
        int h = conflicts(start_board);
        bool solution_found = false;
        int restarts =0;
        int total_states=0;
        int x=0;
        
        while(!solution_found)
        {
            bool restart = false;
            while(restart != true)
            {
                start_board = Gen_board();
                (restart,start_board,x) = Restart(start_board,h);
                total_states += x;
            }
            restarts++;
            
            h = conflicts(start_board); // Recalculate the number of conflicts
            
            if(h == 0) // Break out of the loop if no conflicts found. Game Over
            {
                solution_found = true;
            }
        }


        //Solution Found, print reults:
        Console.WriteLine("Solution Found!");
        Console.WriteLine("State Changes: " + total_states);
        Console.WriteLine("Restarts: " + restarts);
    }

    public static int[,] Gen_board()
    {
        int[,] new_board = new int[8, 8];
        
        Random random = new Random();int min = 0;int max = 7;
        for (int i = 0; i < 8; i++)
        {
            int x = random.Next(min, max + 1);
            int y = random.Next(min, max + 1);

            //If there is already a queen in the x,y coordinate, then randomize it again.
            if(new_board[x, y] == 1)
            {
                x = random.Next(min, max + 1);
                y = random.Next(min, max + 1);

                //If there is somehow another queen in the x,y coordinate, 
                //then keep randomizing it until we find a x,y coordinate without a queen in it .
                while(new_board[x,y] == 1)
                {
                    x = random.Next(min, max + 1);
                    y = random.Next(min, max + 1);
                }
                
                new_board[x,y] = 1;
            }
            else
            {
                new_board[x, y] = 1;
            }
        }
        return new_board;
    }


    //Simple Board Display function:
    public static void Display(int[,] board , int h)
    {
        //current Hueristic / State :
        Console.WriteLine("Currnet h: " + h);
        Console.WriteLine("Current State: ");

        //Prints enterned Board:
        for(int x=0; x<8; x++)
        {
            for(int y=0; y<8; y++)
            {
                if(y==7)
                {
                    Console.Write(board[x,y] + " ");
                }
                else
                {
                    Console.Write(board[x,y] + ",");
                }
            }
            Console.WriteLine();
        }
    }

    public static (bool,int[,],int change_State_count) Restart(int[,] board , int h)
    {
        bool restart = false;
        int neighbor_count = 0;
        int [,] next_state = board;
        int change_State_count =0;
        
        //Checks all neighbor states of entered board. Returns the Hueristic value, next state the algorithm has decided, 
        //and how many times it changed states
        (h, next_state,neighbor_count) =  check_states(board);

        //Restart check:
        if(next_state == board && h == 0)
        {
            restart = true;
        }
        else if(next_state == board)
        {
            restart = true;
        }
        else
        {
            //Last resort to break out of infanite loop if next_state is somehow diffrent than board.
            Console.WriteLine("Error: RESTART_2 exception Found");
            Console.Write("\n");
        }
        return (restart ,next_state,neighbor_count);
    }

    public static int conflicts(int[,] board)
    {
        int h_count = 0;
        // new hashset to remember the places we've checked and made note of queens. 
        HashSet<(int, int)> checkedPositions = new HashSet<(int, int)>();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] == 1 && !checkedPositions.Contains((x, y)))
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        // Vertical / Horizontal checks:
                        if (y + i < 8 && !checkedPositions.Contains((x, y + i)) && board[x, y + i] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x, y + i));
                        }

                        if (x + i < 8 && !checkedPositions.Contains((x + i, y)) && board[x + i, y] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x + i, y));
                        }

                        if (y - i >= 0 && !checkedPositions.Contains((x, y - i)) && board[x, y - i] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x, y - i));
                        }

                        if (x - i >= 0 && !checkedPositions.Contains((x - i, y)) && board[x - i, y] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x - i, y));
                        }

                        // Diagonal checks:
                        if (x + i < 8 && y + i < 8 && !checkedPositions.Contains((x + i, y + i)) && board[x + i, y + i] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x + i, y + i));
                        }

                        if (x - i >= 0 && y + i < 8 && !checkedPositions.Contains((x - i, y + i)) && board[x - i, y + i] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x - i, y + i));
                        }

                        if (x - i >= 0 && y - i >= 0 && !checkedPositions.Contains((x - i, y - i)) && board[x - i, y - i] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x - i, y - i));
                        }

                        if (x + i < 8 && y - i >= 0 && !checkedPositions.Contains((x + i, y - i)) && board[x + i, y - i] == 1)
                        {
                            h_count++;
                            checkedPositions.Add((x + i, y - i));
                        }
                    } 
                    checkedPositions.Add((x, y));
                }
            }
        }

        return h_count;
    }

    public static (int, int[,], int) check_states(int[,] board)
    {
        bool new_check = true;
    	HashSet<(int, int)> checkedPositions = new HashSet<(int, int)>();
    	int final_i;
        int new_h;
    	int current_h = conflicts(board);
    	int[,] neighbor_state = board;
    	int neighbor_state_count = 0; // counter for states found with a lower h
    
    	for (int x = 0; x < board.GetLength(0); x++)
    	{
    		for (int y = 0; y < board.GetLength(1); y++)
    		{
                neighbor_state_count = 0;

    			if (board[x, y] == 1 && !checkedPositions.Contains((x, y))) //Queen found 
    			{
                    current_h = conflicts(board); // makes sure current is up to date before using it in the next for loop

    				for (int i = 1; i < board.GetLength(0); i++)
    				{
                        
    					if (x + i < board.GetLength(0) && !checkedPositions.Contains((x + i, y)) && board[x + i, y] == 0)
    					{
    						final_i = i;
                            neighbor_state_count ++;
    						neighbor_state[x, y] = 0;
    						neighbor_state[x + i, y] = 1;
    						new_h = conflicts(neighbor_state);
                            
    						if (new_h < current_h)
    						{
    							current_h = new_h;
    							board = neighbor_state;
                                if(!new_check)
                                {
                                    Console.WriteLine("Neighbors found with lower h: "  + neighbor_state_count);
                                    Console.WriteLine("Setting new current state:");
                                    Console.Write("\n");
                                }
                                else
                                {
                                    Console.WriteLine("RESTART");
                                    Console.WriteLine("Neighbors found with lower h: "  + 0);
                                    Console.Write("\n");
                                }
                               
                                checkedPositions.Add((x + final_i, y));

                                //Display the board we move to:
                                Display(board,current_h);
                                new_check = false;
                                break;
    						}
    						else
    						{
    							neighbor_state[x, y] = 1;
    							neighbor_state[x + i, y] = 0;
                                checkedPositions.Add((x, y));

    						}
    					}

    					if (x - i >= 0 && !checkedPositions.Contains((x - i, y)) && board[x - i, y] == 0)
    					{
    						final_i = i;
                            neighbor_state_count ++;
    						neighbor_state[x, y] = 0;
    						neighbor_state[x - i, y] = 1;
    						new_h = conflicts(neighbor_state);

    						if (new_h < current_h)
    						{
    							current_h = new_h;
    							board = neighbor_state;
                                if(!new_check)
                                {
                                    Console.WriteLine("Neighbors found with lower h: "  + neighbor_state_count);
                                    Console.WriteLine("Setting new current state:");
                                    Console.Write("\n");
                                }
                                else
                                {
                                    Console.WriteLine("RESTART");
                                    Console.WriteLine("Neighbors found with lower h: "  + 0);
                                    Console.Write("\n");
                                }

    							neighbor_state_count ++;
                                checkedPositions.Add((x - final_i, y));

                                //Display the board we move to:
                                Display(board,current_h);
                                new_check = false;
                                break;
    						}
    						else
    						{
    							neighbor_state[x, y] = 1;
    							neighbor_state[x - i, y] = 0;
                                checkedPositions.Add((x , y));
    						}
    					}
    				}
    				checkedPositions.Add((x, y));
    			}
    		}
    	}

        //return if: return the next desired state.
    	if (conflicts(board) <= conflicts(neighbor_state))
    	{
    		return (neighbor_state_count, board,neighbor_state_count);
    	}
    	else
    	{
    		return (neighbor_state_count, neighbor_state,neighbor_state_count);
    	}
    }
}
