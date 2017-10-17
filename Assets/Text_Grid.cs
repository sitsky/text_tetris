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
   
    
    public enum STRIKE:int { WALL_HIT, GROUND_HIT, CLOCK_ROTATION, ANTICLOCK_ROTATION};

    public enum Pieces:int {L= 1, J, I, O, S, Z, T};
    int number_of_pieces = 7;
    Pieces piece_choice;
    bool need_new_piece;

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
        need_new_piece = true;
        new_piece();
    }
    // Update is called once per frame
    void Update() {

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                move_left();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                move_right();
            }
        
        if (Time.time - time_last_move > DROP_EVERY_SECONDS)
        {
            move_all_one_down(GROUND);
            for (int i = 0; i < GROUND; i++)
            {
                check_full_row(i);
            }
            time_last_move = Time.time;
            new_piece();
        }
        grid_print();
    }


    public void move_left()
    {
        for (int i = GROUND - 1; i >= 0; i--)
        {
            if (grid[i, LEFT_WALL + 1].Equals("X"))
            {
                Debug.Log("X On edge");
                return;
            }
        }

        int[,] Xcoords = new int[4, 2];
        int k = 0;
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            for (int i = GROUND - 1; i >= 0; i--)
            {
                if (grid[i, j].Equals("X"))
                {
                    Xcoords[k, 0] = i;
                    Xcoords[k, 1] = j;
                    k++;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if ((grid[Xcoords[i, 0], Xcoords[i, 1] - 1].Equals("G")))
            {
                Debug.Log(grid[Xcoords[i, 0], Xcoords[i, 1] - 1] + "  " + Xcoords.ToString());
                Debug.Log("No room to left");
                return;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            grid[Xcoords[i, 0], Xcoords[i, 1]] = " ";
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(grid[Xcoords[i, 0], Xcoords[i, 1] - 1]);
            grid[Xcoords[i, 0], Xcoords[i, 1] - 1] = "X";
            Debug.Log(grid[Xcoords[i, 0], Xcoords[i, 1] - 1]);
        }
    }

    public void move_right()
    {
        for (int i = GROUND - 1; i >= 0; i--)
        {
            if (grid[i, RIGHT_WALL - 1].Equals("X"))
            {
                Debug.Log("X On edge");
                return;
            }
        }
        int[,] Xcoords = new int[4, 2];
        int k = 0;
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL - 1; j++)
        {
            for (int i = GROUND - 1; i >= 0; i--)
            {
                if (grid[i, j].Equals("X"))
                {
                    Xcoords[k, 0] = i;
                    Xcoords[k, 1] = j;
                    Debug.Log("X COORDS: " + Xcoords[k, 0].ToString() + "  " + Xcoords[k, 1].ToString());
                    k++;
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if ((grid[Xcoords[i, 0], Xcoords[i, 1] + 1].Equals("G")))
            {
                Debug.Log(grid[Xcoords[i, 0], Xcoords[i, 1] + 1] + "  " + Xcoords.ToString());
                Debug.Log("No room to left");
                return;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            grid[Xcoords[i, 0], Xcoords[i, 1]] = " ";
        }
        for (int i = 0; i < 4; i++)
        { 
            grid[Xcoords[i, 0], Xcoords[i, 1] + 1] = "X";
        }
    }


    public void move_all_one_down(int drop_above_rows)
    {
        int[,] Xcoords = new int[4, 2];
        int k = 0;

        for (int i = drop_above_rows - 1; i >= 0; i--)
        {
            for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
            {
                if (grid[i, j].Equals("X"))
                {
                    Xcoords[k, 0] = i;
                    Xcoords[k, 1] = j;
                    Debug.Log("X COORDS: " + Xcoords[k, 0].ToString() + "  " + Xcoords[k, 1].ToString());
                    k++;
                }
            }
        }

        bool groundhit = false;
        for (int i = 0; i < 4; i++)
        {
            if ((grid[Xcoords[i, 0] + 1, Xcoords[i, 1]].Equals("G")))
            {
                groundhit = true;
                need_new_piece = true;
            }
        }
        if (groundhit)
        {
            for (int i = 0; i < 4; i++)
            {
                grid[Xcoords[i, 0], Xcoords[i, 1]] = "G";
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                grid[Xcoords[i, 0], Xcoords[i, 1]] = " ";
            }
            for (int i = 0; i < 4; i++)
            {
                grid[Xcoords[i, 0] + 1, Xcoords[i, 1]] = "X";
            }
        }
    }
            
        
    

    public void new_piece()
    {
        if (need_new_piece )
        {
            need_new_piece = false;
            piece_choice = (Pieces)Random.Range(1, number_of_pieces);
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
    }

    public bool check_full_row(int row)
    {
        //Debug.Log("check_full_row: " + i.ToString());
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            if (!(grid[row, j].Equals("G")))
                return false;
        }
        line_destroy_and_drop(row);
        return true;
    }

    public void line_destroy_and_drop(int row)
    {
        //Debug.Log("line_destroy_and_drop");
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            grid[row, j] = " ";
        }
        move_all_one_down(row);
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
