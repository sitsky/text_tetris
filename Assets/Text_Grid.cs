using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_Grid : MonoBehaviour {

    static int ROWS = 20;
    const int GROUND = 19;

    static int COLUMNS = 13;
    const int LEFT_WALL = 0;
    const int RIGHT_WALL = 12;

    public static string[,] grid = new string[COLUMNS, ROWS];

    static float DROP_EVERY_SECONDS = 0.5f;
    float time_last_move = 0;

    public enum Pieces:int {L= 1, J, I, O, S, Z, T};
    public int number_of_pieces = 7;

    Pieces[] piece_choice = new Pieces[2];
    bool need_new_piece;
    //static string next_piece;

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

        if (Time.time - time_last_move > DROP_EVERY_SECONDS)
        {
            move_all_one_down(GROUND);
            for (int row = 0; row < GROUND; row++)
            {
                check_full_row(row);
            }
            time_last_move = Time.time;
        }
        grid_print();
    }

    public void where_X_are()
    {
        int point = 0;
        for (int col = LEFT_WALL + 1; col < RIGHT_WALL; col++)
        {
            for (int row = GROUND - 1; row >= 0; row--)
            {
                if (grid[row, col].Equals("X"))
                {
                    Xcoords[point, 0] = col;
                    Xcoords[point, 1] = row;
                    point++;
                }
            }
        }
    }

    public void rotate_Clockwise()
    {
        where_X_are();
        int central_coordinate_for_rows = 0;
        int central_coordinate_for_cols = 0;
        for (int point = 0; point <4; point++)
        {
            central_coordinate_for_cols = central_coordinate_for_cols + Xcoords[point, 0];
            central_coordinate_for_rows = central_coordinate_for_rows + Xcoords[point, 1];
        }
        central_coordinate_for_cols = (int)Mathf.Floor(central_coordinate_for_cols / 4f);
        central_coordinate_for_rows = (int)Mathf.Floor(central_coordinate_for_rows / 4f);

        for (int point = 0; point < 4; point++)
        {
            Xcoords[point, 0] = (Xcoords[point, 0] - central_coordinate_for_cols);
            Xcoords[point, 1] = (Xcoords[point, 1] - central_coordinate_for_rows);
        }

        int[,] rotation = { { 0, 1 }, { -1, 0 } };
        int[,] Rotated_Xcoords = new int[4, 2];
        for (int point = 0; point < 4; point++)
        {
            Rotated_Xcoords[point, 0] = (Xcoords[point, 0] * rotation[0, 0] + Xcoords[point, 1] * rotation[0, 1]) + central_coordinate_for_cols;
            Rotated_Xcoords[point, 1] = (Xcoords[point, 0] * rotation[1, 0] + Xcoords[point, 1] * rotation[1, 1]) + central_coordinate_for_rows;
        }
        for (int point = 0; point < 4; point++)
        {
            if ((grid[Rotated_Xcoords[point, 0], Rotated_Xcoords[point, 1] ].Equals("G")))
            {
                Debug.Log("No room to rotate turn to G");
                return;
            }
            if ((Rotated_Xcoords[point, 0] < LEFT_WALL + 1) || (Rotated_Xcoords[point, 1] < 0))
            {
                return;
            }
            if(Rotated_Xcoords[point, 0] > RIGHT_WALL)
            {
                return;
            }
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
            if (grid[row, LEFT_WALL + 1].Equals("X"))
            {
                Debug.Log("X On edge");
                return;
            }
        }
        where_X_are();
        for (int point = 0; point < 4; point++)
        {
            if ((grid[Xcoords[point, 0], Xcoords[point, 1] - 1].Equals("G")))
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
            Debug.Log(grid[Xcoords[point, 0], Xcoords[point, 1] - 1]);
            grid[Xcoords[point, 0], Xcoords[point, 1] - 1] = "X";
            Debug.Log(grid[Xcoords[point, 0], Xcoords[point, 1] - 1]);
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
        where_X_are();
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
        where_X_are();
        bool groundhit = false;
        for (int i = 0; i < 4; i++)
        {
            if ((grid[Xcoords[i, 0] + 1, Xcoords[i, 1]].Equals("G")))
            {
                groundhit = true;
            }
        }
        if (groundhit)
        {
            for (int i = 0; i < 4; i++)
            {
                grid[Xcoords[i, 0], Xcoords[i, 1]] = "G";
            }
            new_piece();
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
            
    /*public static class pieces
    {
        public static void L() { grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[2, 6] = "X"; next_piece = "XXX\n X"; }

    }
    */
    public void new_piece()
    {
        piece_choice[0] = (Pieces)Random.Range(1, number_of_pieces);
        switch (piece_choice[1])
        {
            case Pieces.L:
                grid[0, 5] = "X"; grid[1, 5] = "X"; grid[2, 5] = "X"; grid[2, 6] = "X"; //pieces.L();
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
        piece_choice[1] = piece_choice[0];
    }

    public void check_full_row(int row)
    {
        //Debug.Log("check_full_row: " + i.ToString());
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            if (!(grid[row, j].Equals("G")))
                return;
        }
        line_destroy_and_drop(row);
    }

    public void line_destroy_and_drop(int row)
    {
        //Debug.Log("line_destroy_and_drop");
        for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
        {
            grid[row, j] = " ";
        }

        for (int i = row; i >= 0; i--)
        {
            for (int j = LEFT_WALL + 1; j < RIGHT_WALL; j++)
            {
                grid[i, j] = grid[i + 1, j];
            }
        }
    }

    public void grid_print()
    {
        string to_print = "";
        //to_print = next_piece + "\n";
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
