using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class Text_Grid : MonoBehaviour {

    static int ROWS = 20;
    const int GROUND = 19;

    static int COLUMNS = 13;
    const int LEFT_WALL = 0;
    const int RIGHT_WALL = 12;

    public static string[,] grid = new string[COLUMNS, ROWS];

    static float DROP_EVERY_SECONDS = 0.25f;
    float time_last_move = 0;

    public enum Pieces:int {L= 1, J, I, O, S, Z, T};
    public int number_of_pieces = 7;
    Pieces[] piece_choice = new Pieces[2];
    
    int[,] Xcoords = new int[4, 2];
    Text grid_text;

    // Use this for initialization
    void Start () {
        grid_text = GetComponent<Text>();
        grid_text.text = "";
        for (int row = 0; row < ROWS; row++)
        {
            grid[LEFT_WALL, row] = "|";
            grid[RIGHT_WALL, row] = "|";

            for (int col = LEFT_WALL + 1; col < RIGHT_WALL; col++)
            {
                if (row < GROUND)
                {
                    grid[col, row] = " ";
                }
                else
                {
                    grid[col, row] = "G"; //Ground mark
                }
            }
        }
        grid_print();
        piece_choice[1] = (Pieces)Random.Range(1, number_of_pieces);
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotate_Clockwise();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rotate_AntiClockwise();
        }
        
        if (Time.time - time_last_move > DROP_EVERY_SECONDS)
        {
            move_all_one_down(GROUND);
            time_last_move = Time.time;
        }
        grid_print();
        if (!enabled)
            Game_Over();
    }
    
    public void where_X_are()
    {
        int point = 0;
        for (int col = LEFT_WALL + 1; col < RIGHT_WALL; col++)
        {
            for (int row = GROUND - 1; row >= 0; row--)
            {
                if (grid[col, row].Equals("X"))
                {
                    Debug.Log(point.ToString());
                    Xcoords[point, 0] = col;
                    Xcoords[point, 1] = row;
                    point++;
                }
            }
        }
    }

    //game controls
    public void rotate_Clockwise()
    {
        where_X_are();
        //find center of Xs
        int central_coordinate_for_rows = 0;
        int central_coordinate_for_cols = 0;
        for (int point = 0; point <4; point++)
        {
            central_coordinate_for_cols = central_coordinate_for_cols + Xcoords[point, 0];
            central_coordinate_for_rows = central_coordinate_for_rows + Xcoords[point, 1];
        }
        central_coordinate_for_cols = (int)Mathf.Floor(central_coordinate_for_cols / 4f);
        central_coordinate_for_rows = (int)Mathf.Floor(central_coordinate_for_rows / 4f);


        //move Xs to Zero and Rotate them
        int[,] rotation = { { 0, 1 }, { -1, 0 } };
        int[,] Trans_to_Zero_Xcoords = new int[4, 2];
        int[,] Rotated_Xcoords = new int[4, 2];

        for (int point = 0; point < 4; point++)
        {
            Trans_to_Zero_Xcoords[point, 0] = (Xcoords[point, 0] - central_coordinate_for_cols);
            Trans_to_Zero_Xcoords[point, 1] = (Xcoords[point, 1] - central_coordinate_for_rows);
        }
        for (int point = 0; point < 4; point++)
        {
            Rotated_Xcoords[point, 0] = (Trans_to_Zero_Xcoords[point, 0] * rotation[0, 0] + Trans_to_Zero_Xcoords[point, 1] * rotation[0, 1]) + central_coordinate_for_cols;
            Rotated_Xcoords[point, 1] = (Trans_to_Zero_Xcoords[point, 0] * rotation[1, 0] + Trans_to_Zero_Xcoords[point, 1] * rotation[1, 1]) + central_coordinate_for_rows;
        }
        //Xs rotated and moved back to where they were

        //check on validity of rotation
        for (int point = 0; point < 4; point++)
        {

            if ((Rotated_Xcoords[point, 0] < LEFT_WALL + 1) || (Rotated_Xcoords[point, 0] > RIGHT_WALL - 1))
            {
                Debug.Log("Too Right too Left");
                return;
            }
            if((Rotated_Xcoords[point, 1] > 18) || (Rotated_Xcoords[point, 1] < 0))
            {
                Debug.Log("Too High too Low");
                return;
            }
            if ((grid[Rotated_Xcoords[point, 0], Rotated_Xcoords[point, 1]].Equals("G")))
            {
                Debug.Log("Hit the ground turn to G");
                //MISSING
                return;
            }
        }
        //all good remove old Xs add new ones.
        for (int point = 0; point < 4; point++)
        {
            grid[Xcoords[point, 0], Xcoords[point, 1]] = " ";
        }
        for (int point = 0; point < 4; point++)
        {
            grid[Rotated_Xcoords[point, 0], Rotated_Xcoords[point, 1]] = "X";
        }
    }
    public void rotate_AntiClockwise()
    {
        where_X_are();
        //find center of Xs
        int central_coordinate_for_rows = 0;
        int central_coordinate_for_cols = 0;
        for (int point = 0; point < 4; point++)
        {
            central_coordinate_for_cols = central_coordinate_for_cols + Xcoords[point, 0];
            central_coordinate_for_rows = central_coordinate_for_rows + Xcoords[point, 1];
        }
        central_coordinate_for_cols = (int)Mathf.Floor(central_coordinate_for_cols / 4f);
        central_coordinate_for_rows = (int)Mathf.Floor(central_coordinate_for_rows / 4f);


        //move Xs to Zero and Rotate them
        int[,] rotation = { { 0, -1 }, { 1, 0 } };
        int[,] Trans_to_Zero_Xcoords = new int[4, 2];
        int[,] Rotated_Xcoords = new int[4, 2];

        for (int point = 0; point < 4; point++)
        {
            Trans_to_Zero_Xcoords[point, 0] = (Xcoords[point, 0] - central_coordinate_for_cols);
            Trans_to_Zero_Xcoords[point, 1] = (Xcoords[point, 1] - central_coordinate_for_rows);
        }
        for (int point = 0; point < 4; point++)
        {
            Rotated_Xcoords[point, 0] = (Trans_to_Zero_Xcoords[point, 0] * rotation[0, 0] + Trans_to_Zero_Xcoords[point, 1] * rotation[0, 1]) + central_coordinate_for_cols;
            Rotated_Xcoords[point, 1] = (Trans_to_Zero_Xcoords[point, 0] * rotation[1, 0] + Trans_to_Zero_Xcoords[point, 1] * rotation[1, 1]) + central_coordinate_for_rows;
        }
        //Xs rotated and moved back to where they were

        //check on validity of rotation
        for (int point = 0; point < 4; point++)
        {

            if ((Rotated_Xcoords[point, 0] < LEFT_WALL + 1) || (Rotated_Xcoords[point, 0] > RIGHT_WALL - 1))
            {
                Debug.Log("Too Right too Left");
                return;
            }
            if ((Rotated_Xcoords[point, 1] > 18) || (Rotated_Xcoords[point, 1] < 0))
            {
                Debug.Log("Too High too Low");
                return;
            }
            if ((grid[Rotated_Xcoords[point, 0], Rotated_Xcoords[point, 1]].Equals("G")))
            {
                Debug.Log("Hit the ground turn to G");
                //MISSING
                return;
            }
        }
        //all good remove old Xs add new ones.
        for (int point = 0; point < 4; point++)
        {
            grid[Xcoords[point, 0], Xcoords[point, 1]] = " ";
        }
        for (int point = 0; point < 4; point++)
        {
            grid[Rotated_Xcoords[point, 0], Rotated_Xcoords[point, 1]] = "X";
        }
    }

    public void move_left()
    {
        for (int row = GROUND - 1; row >= 0; row--)
        {
            if (grid[LEFT_WALL + 1, row].Equals("X"))
            {
                Debug.Log("X On edge");
                return;
            }
        }
        where_X_are();
        for (int point = 0; point < 4; point++)
        {
            if ((grid[Xcoords[point, 0] - 1, Xcoords[point, 1]].Equals("G")))
            {
                Debug.Log("No room to left");
                return;
            }
        }
        for (int point = 0; point < 4; point++)
        {
            grid[Xcoords[point, 0], Xcoords[point, 1]] = " ";
        }
        for (int point = 0; point < 4; point++)
        {
            grid[Xcoords[point, 0] - 1, Xcoords[point, 1]] = "X";
        }
    }
    public void move_right()
    {
        for (int row = GROUND - 1; row >= 0; row--)
        {
            if (grid[RIGHT_WALL - 1, row].Equals("X"))
            {
                Debug.Log("X On edge");
                return;
            }
        }
        where_X_are();
        for (int point = 0; point < 4; point++)
        {
            if ((grid[Xcoords[point, 0] + 1, Xcoords[point, 1]].Equals("G")))
            {
                Debug.Log("No room to left");
                return;
            }
        }
        for (int point = 0; point < 4; point++)
        {
            grid[Xcoords[point, 0], Xcoords[point, 1]] = " ";
        }
        for (int point = 0; point < 4; point++)
        {
            grid[Xcoords[point, 0] + 1, Xcoords[point, 1]] = "X";
        }
    }

    //Game rules
    public void move_all_one_down(int drop_above_rows)
    {
        where_X_are();
        bool groundhit = false;
        for (int point = 0; point < 4; point++)
        {
            if ((grid[Xcoords[point, 0], Xcoords[point, 1] + 1].Equals("G")))
            {
                groundhit = true;
            }
        }
        if (groundhit)
        {
            groundhit = false;
            for (int point = 0; point < 4; point++)
            {
                grid[Xcoords[point, 0], Xcoords[point, 1]] = "G";
                if (Xcoords[point, 1] == 0)
                {

                    enabled = false;
                }
            }
            for (int row = 0; row < GROUND; row++)
            {
                check_full_row(row);
            }
            new_piece();
            where_X_are();
        }
        else
        {
            for (int point = 0; point < 4; point++)
            {
                grid[Xcoords[point, 0], Xcoords[point, 1]] = " ";
            }
            for (int point = 0; point < 4; point++)
            {
                grid[Xcoords[point, 0] , Xcoords[point, 1] + 1] = "X";
            }
        }
    }               
    public void check_full_row(int row)
    {
        //Debug.Log("check_full_row: " + i.ToString());
        for (int col = LEFT_WALL + 1; col < RIGHT_WALL; col++)
        {
            if (!(grid[col, row].Equals("G")))
                return;
        }
        line_destroy_and_drop(row);
    }
    public void line_destroy_and_drop(int row_to_destroy)
    {
        //Debug.Log("line_destroy_and_drop");
        for (int col = LEFT_WALL + 1; col < RIGHT_WALL; col++)
        {
            grid[col, row_to_destroy] = " ";
        }

        for (int row = row_to_destroy - 1; row >= 0; row--)
        {
            for (int col = LEFT_WALL + 1; col < RIGHT_WALL; col++)
            {
                grid[col, row + 1] = grid[col, row];
            }
        }
    }

    //UI stuff
    public void Game_Over()
    {


        grid_text.text = "GAME OVER!";

        grid_text.fontSize = 16;
        EditorApplication.isPaused = true;
    }
    public void grid_print()
    {
        string to_print = piece_choice[0].ToString() + "\n";

        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLUMNS; col++)
            {
                to_print = to_print + grid[col, row];
            }
            to_print = to_print + "\n";
        }
        grid_text.text = to_print;
    }
    public void new_piece()
    {
        piece_choice[0] = (Pieces)Random.Range(1, number_of_pieces);
        switch (piece_choice[1])
        {
            case Pieces.L:
                grid[5, 0] = "X"; grid[5, 1] = "X"; grid[5, 2] = "X"; grid[6, 2] = "X";
                break; //L
            case Pieces.J:
                grid[5, 0] = "X"; grid[5, 1] = "X"; grid[5, 2] = "X"; grid[4, 2] = "X";
                break; //J
            case Pieces.I:
                grid[5, 0] = "X"; grid[5, 1] = "X"; grid[5, 2] = "X"; grid[5, 3] = "X";
                break; //I
            case Pieces.O:
                grid[5, 0] = "X"; grid[6, 0] = "X"; grid[5, 1] = "X"; grid[6, 1] = "X";
                break; //O
            case Pieces.Z:
                grid[4, 0] = "X"; grid[5, 0] = "X"; grid[5, 1] = "X"; grid[6, 1] = "X";
                break; //S
            case Pieces.S:
                grid[5, 0] = "X"; grid[6, 0] = "X"; grid[5, 1] = "X"; grid[4, 1] = "X";
                break; //Z
            case Pieces.T:
                grid[4, 0] = "X"; grid[5, 0] = "X"; grid[6, 0] = "X"; grid[5, 1] = "X";
                break; //T
            default:
                Debug.Log("New Piece Choice fail!");
                break;
        }
        piece_choice[1] = piece_choice[0];
    }
}
