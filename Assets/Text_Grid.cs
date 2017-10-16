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
            for (int j=0; j< COLUMNS; j++)
            {
                grid[i, j] = "O";
            }
        }
        grid_print();

        grid[0, 5] = "X";
    }
    // Update is called once per frame
    void Update() {
        if (Time.time - time_last_move > 1)
        {
            for (int i = 18; i >= 0; i--)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    grid[i + 1, j] = grid[i, j];
                    grid[i, j] = "O";
                }
            }
            time_last_move = Time.time;
        }
        grid_print();
    } 
    
    public void grid_print()
    {
        string to_print = "";
        for (int i = 0; i < ROWS; i++)
        {
            for (int j= 0; j < COLUMNS; j++)
            {
                to_print = to_print + grid[i, j];
            }
            to_print = to_print + "\n";
        }
        grid_text.text = to_print;
    } 
}
