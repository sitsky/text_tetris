using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_Grid : MonoBehaviour {

    static int ROWS = 20;
    static int COLUMNS = 11;
    static float DROP_EVERY_SECONDS = 0.5f;
    const int GROUND = 19;

    enum Pieces : int {L= 1, J, I, O, S, Z, T};
    int number_of_pieces = 7;

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
                if (i < GROUND)
                {
                    grid[i, j] = " ";
                }
                else
                {
                    grid[i, j] = "G"; //Ground mark
                }
            }
        }
        grid_print();
        new_drop();
        
    }
    // Update is called once per frame
    void Update() {
        if (Time.time - time_last_move > DROP_EVERY_SECONDS)
        {
            move_one_down();
            time_last_move = Time.time;
            for (int i = 0; i < GROUND; i++)
            {
                check_full_row(i);
            }
        }
        grid_print();
    }

    public void new_drop()
    {
        int piece_choise = Random.Range(1, number_of_pieces);
        switch (piece_choise)
        {
            case 1:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[2, 6] = "X";
                break; //L
            case 2:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[2, 4] = "X";
                break; //J
            case 3:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[3, 5] = "X";
                break; //I
            case 4:
                grid[0, 5] = "X"; grid[0, 6] = "X"; grid[1, 5] = "X"; grid[1, 6] = "X";
                break; //O
            case 5:
                grid[0, 4] = "X"; grid[0, 5] = "X"; grid[1, 5] = "X"; grid[1, 6] = "X";
                break; //S
            case 6:
                grid[0, 5] = "X"; grid[0, 6] = "X"; grid[1, 5] = "X"; grid[1, 4] = "X";
                break; //Z
            case 7:
                grid[0, 4] = "X"; grid[0, 5] = "X"; grid[0, 6] = "X"; grid[1, 5] = "X";
                break; //T
            default:
                Debug.Log("New Piece Choice fail!");
                break;
        }
    }

    public void move_one_down(int k = GROUND)
    {
        bool need_new_piece = false;
        for (int i = GROUND - 1; i >= 0; i--)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                if (grid[i, j].Equals("X"))
                {
                    if (grid[i + 1, j].Equals(" "))
                    {
                        grid[i + 1, j] = grid[i, j];
                        grid[i, j] = " ";
                    }
                    else if (grid[i + 1, j].Equals("G"))
                    {
                        grid[i, j] = "G";
                        need_new_piece = true;
                    }
                }
            }
        }
        if (need_new_piece)
        {
            new_drop();
        }
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
            grid[i, j] = " ";
        }
        move_one_down(i);
    }

    public void grid_print()
    {
        string to_print = "|";
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                to_print = to_print + grid[i, j];
            }
            to_print = to_print + "|\n|";
        }
        grid_text.text = to_print;
    }
}
