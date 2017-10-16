using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_Grid : MonoBehaviour {

    static int ROWS = 20;
    static int COLUMNS = 11;
    float time_last_move = 0;
    Text grid_text;
    public static string[,] grid = new string[ROWS,COLUMNS];
    
    // Use this for initialization
    void Start () {
        grid_text = GetComponent<Text>();
        grid_text.text = "";
        for (int i =0; i< ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                if (i < ROWS - 1)
                {
                    grid[i, j] = "O";
                }
                else
                {
                    grid[i, j] = "G";
                }
            }
        }
        grid_print();
        new_drop();
        
        for( int j = 0; j< COLUMNS; j++)
        {
            grid[17, j] = "X";
        }
        grid[17, 5] = "O"; 
    }
    // Update is called once per frame
    void Update() {
        if (Time.time - time_last_move > 0.5f)
        {
            move_one_down();
            time_last_move = Time.time;
            for (int i = 0; i < ROWS-1; i++)
            {
                check_full_row(i);
            }
        }
        grid_print();
    }

    public bool check_full_row(int i)
    {
        Debug.Log("check_full_row: " + i.ToString());
        for (int j = 0; j < COLUMNS; j++)
        {
            if (!(grid[i, j].Equals("X")))
                return false;
        }
        line_destroy_and_drop(i);
        return true;
    }

    public void line_destroy_and_drop(int i)
    {
        Debug.Log("line_destroy_and_drop");
        for (int j = 0; j < COLUMNS; j++)
        {
            grid[i, j] = "O";
        }
        move_one_down(i);
    }

    public void new_drop()
    {
        grid[0, 5] = "X";
    }
     
    public void move_one_down(int k = 19)
    {
        for (int i = k - 2; i >= 0; i--)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                if (grid[i + 1, j].Equals("O"))
                {
                    grid[i + 1, j] = grid[i, j];
                    grid[i, j] = "O";
                }
                else
                {
                    //new_drop();
                }
            }
        }
    }


    public void grid_print()
    {
        string to_print = "";
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                to_print = to_print + grid[i, j];
            }
            to_print = to_print + "\n";
        }
        grid_text.text = to_print;
    }
}
