using System.Collections;
using System;
using System.Collections.Generic;


class Program
{
    private static void Main(string[] args)
    {
        //Main function script:
        int[,] start_board = Gen_board();
        int h = conflicts(start_board);
        bool solution_found = false;
        
        while(!solution_found)
        {
            bool restart = false;
            while(restart != true)
            {
                restart = Display(start_board,h);
            }
            
            h = conflicts(start_board); // Recalculate the number of conflicts
            
            if(h == 0) // Break out of the loop if no conflicts found
            {
                solution_found = true;
            }
        }
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

    public static bool Display(int[,] board , int h)
    {
        bool restart = false;
        int neighbor_count = 0;
        int [,] next_state = board;

        //current Hueristic:
        Console.WriteLine("Currnet h:" + h);
        Console.WriteLine("Current State ");
        for(int x=0; x<8; x++)
        {
            for(int y=0; y<8; y++)
            {
                Console.Write(board[x,y] + " ");
            }
            Console.Write("\n");
        }

        (neighbor_count, next_state) =  check_states(board);

        if(next_state != board && conflicts(next_state) < conflicts(board) )
        {
            Console.WriteLine("RESTART");
            restart = true;
        }
        else
        {
            Console.WriteLine("Setting new current state");
        }

        Console.WriteLine("Neighbors found with lower h: "  + neighbor_count);
        return restart;
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

    public static (int, int[,]) check_states(int[,] board)
    {
    	HashSet<(int, int)> checkedPositions = new HashSet<(int, int)>();
    	int final_i;
    	int current_h = conflicts(board);
    	int[,] neighbor_state = board;
    	int neighbor_state_count = 0; // counter for states found with a lower h
    
    	for (int x = 0; x < board.GetLength(0); x++)
    	{
    		for (int y = 0; y < board.GetLength(1); y++)
    		{
    			if (board[x, y] == 1 && !checkedPositions.Contains((x, y)))
    			{
    				for (int i = 1; i < board.GetLength(0); i++)
    				{
    					if (x + i < board.GetLength(0) && !checkedPositions.Contains((x + i, y)) && board[x + i, y] == 0)
    					{
    						final_i = i;
    						neighbor_state[x, y] = 0;
    						neighbor_state[x + i, y] = 1;
    						int new_h = conflicts(neighbor_state);
    						if (new_h < current_h)
    						{
    							current_h = new_h;
    							board = neighbor_state;
    							neighbor_state_count = 1;
    						}
    						else
    						{
    							neighbor_state[x, y] = 1;
    							neighbor_state[x + i, y] = 0;
    
    						}
    						checkedPositions.Add((x + final_i, y));
    						break;
    
    					}
    					else if (x - i >= 0 && !checkedPositions.Contains((x - i, y)) && board[x - i, y] == 0)
    					{
    						final_i = i;
    						neighbor_state[x, y] = 0;
    						neighbor_state[x - i, y] = 1;
    						int new_h = conflicts(neighbor_state);
    						if (new_h < current_h)
    						{
    							current_h = new_h;
    							board = neighbor_state;
    							neighbor_state_count = 1;
    						}
    						else
    						{
    							neighbor_state[x, y] = 1;
    							neighbor_state[x - i, y] = 0;
    
    						}
    						checkedPositions.Add((x - final_i, y));
    						break;
    
    					}
    				}
    				checkedPositions.Add((x, y));
    			}
    		}
    	}
    
    	if (conflicts(board) <= conflicts(neighbor_state))
    	{
    		return (neighbor_state_count, board);
    	}
    	else
    	{
    		return (neighbor_state_count, neighbor_state);
    	}
    }

}
