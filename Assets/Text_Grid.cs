using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_Grid : MonoBehaviour {

    Text grid_text;
    public static char[,] grid = new char[20,10];
    // Use this for initialization
    void Start () {

        grid_text = GetComponent<Text>();
        grid_text.text = "";
        for (int i =0; i<20; i++)
        {
            for (int j=0; j<10; j++)
            {
                grid[i, j] = 'O';
            }
        }

        string each_line = "";
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                each_line = each_line + grid[i, j].ToString();
            }
            grid_text.text = grid_text.text + each_line + "\n";
            each_line = "";
        }
    }
    
    // Update is called once per frame
    void Update() {

    }  
}
