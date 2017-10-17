using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_Grid : MonoBehaviour {

    static int ROWS = 20;
    const int GROUND = 19;

    static int COLUMNS = 13;
    const int LEFT_WALL = 0;
    const int RIGHT_WALL = 12;

    static float DROP_EVERY_SECONDS = 0.5f;
    int LEFT_OR_RIGHT = 0;
    
    public enum STRIKE:int { WALL_HIT, GROUND_HIT, CLOCK_ROTATION, ANTICLOCK_ROTATION};

    public enum Pieces:int {L= 1, J, I, O, S, Z, T};
    int number_of_pieces = 7;
    Pieces piece_choice;

    float time_last_move = 0;
    Text grid_text;

    public static string[,] grid = new string[ROWS, COLUMNS];

  
    // Use this for initialization
    void Start () {
        grid_text = GetComponent<Text>();
        grid_text.text = "";
        for (int i = 0; i < ROWS; i++)
        {
            grid[i, LEFT_WALL] = "|";
            grid[i, RIGHT_WALL] = "|";

            for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LEFT_OR_RIGHT = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LEFT_OR_RIGHT = 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        { }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        { }

        if (Time.time - time_last_move > DROP_EVERY_SECONDS)
        {
            move_one_down(LEFT_OR_RIGHT);
            LEFT_OR_RIGHT = 0;
            time_last_move = Time.time;
            for (int i = 0; i < GROUND; i++)
            {
                check_full_row(i);
            }
        }
        grid_print();

    }

    public void move_one_down(int Dj, int k = GROUND)
    {
      //  bool need_new_piece = false;
        for (int i = GROUND - 1; i >= 0; i--)
        {
            for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
            {
                if (grid[i, j].Equals("X"))
                {
                    if (grid[i + 1, j + Dj].Equals(" "))
                    {
                        grid[i + 1, j + Dj] = grid[i, j];
                        grid[i, j] = " ";
                    }
                    else if (grid[i + 1, j].Equals("G"))
                    {
                        piece_behavior(STRIKE.GROUND_HIT);
                        return;
                    }
                    else if (grid[i + 1, j + Dj].Equals("|"))
                    {
                        for (int m = 0; m < 5; m++)
                        {
                            move_one_down(0, i - 1);
                        }
                        return;
                    }
                }
            }
        }
        
    }

    public void piece_behavior(STRIKE what_hit, int i = GROUND - 1, int j = LEFT_WALL + 1)
    {
        if (what_hit == STRIKE.GROUND_HIT)
        {
            for (int k = i; k >= 0; k--)
            {
                for (int m = j; m < RIGHT_WALL; m++)
                {
                    if (grid[k, m].Equals("X"))
                    {
                        grid[k, m] = "G";
                    }
                }
            }
            new_drop();
        }
    }


    public void new_drop()
    {
        piece_choice = (Pieces) Random.Range(1, number_of_pieces);
        switch (piece_choice)
        {
            case Pieces.L:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[2, 6] = "X";
                break; //L
            case Pieces.J:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[2, 4] = "X";
                break; //J
            case Pieces.I:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[3, 5] = "X";
                break; //I
            case Pieces.O:
                grid[0, 5] = "X"; grid[0, 6] = "X"; grid[1, 5] = "X"; grid[1, 6] = "X";
                break; //O
            case Pieces.S:
                grid[0, 4] = "X"; grid[0, 5] = "X"; grid[1, 5] = "X"; grid[1, 6] = "X";
                break; //S
            case Pieces.Z:
                grid[0, 5] = "X"; grid[0, 6] = "X"; grid[1, 5] = "X"; grid[1, 4] = "X";
                break; //Z
            case Pieces.T:
                grid[0, 4] = "X"; grid[0, 5] = "X"; grid[0, 6] = "X"; grid[1, 5] = "X";
                break; //T
            default:
                Debug.Log("New Piece Choice fail!");
                break;
        }
    }

    public bool check_full_row(int i)
    {
        //Debug.Log("check_full_row: " + i.ToString());
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            if (!(grid[i, j].Equals("G")))
                return false;
        }
        line_destroy_and_drop(i);
        return true;
    }

    public void line_destroy_and_drop(int i)
    {
        //Debug.Log("line_destroy_and_drop");
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            grid[i, j] = " ";
        }
        move_one_down(0, i);
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
