using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int totalNodes = 0;
        DateTime start;
        DateTime end;




        Boolean debug = false;
        string[][] arr=new string[9][];// array of remaining possibilities 9x9
        TextBox[][] comb=new TextBox[9][];// array of the textboxes
        int[][] situation = new int[9][];// array of the situation of solution: found-2/given-1/guessed-3
       //for the ai part
       // packet pbb;
        Stack<packet> stk=new Stack<packet>();
        List<int[][]> trash = new List<int[][]>();
        
        //for keeping track of which grids were already searched 
       

        //[ROW],[COLUMN]
        public MainWindow()
        {
            InitializeComponent();
           
            addItems();//adds the numbers in the comboBoxes
            
        }

       
        private void comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void fillStrings() {

           //initialising the 2D array
            for (int a = 0; a < 9; a++) { arr[a]=new String[9];situation[a]=new int[9];}

        for(int a=0;a<9;a++){
        for(int b=0;b<9;b++){
        arr[a][b]="123456789";
        }
        }
        }
        private void addItems() {
            for (int a = 0; a < 9;a++ ) {
            comb[a]=new TextBox[9];
            }


            comb[0][0] = comb00; comb[0][1] = comb01; comb[0][2] = comb02;
            comb[1][0] = comb10; comb[1][1] = comb11; comb[1][2] = comb12;
            comb[2][0] = comb20; comb[2][1] = comb21; comb[2][2] = comb22;

            comb[0][3] = comb03; comb[0][4] = comb04; comb[0][5] = comb05;
            comb[1][3] = comb13; comb[1][4] = comb14; comb[1][5] = comb15;
            comb[2][3] = comb23; comb[2][4] = comb24; comb[2][5] = comb25;

            comb[0][6] = comb06; comb[0][7] = comb07; comb[0][8] = comb08;
            comb[1][6] = comb16; comb[1][7] = comb17; comb[1][8] = comb18;
            comb[2][6] = comb26; comb[2][7] = comb27; comb[2][8] = comb28;





            comb[3][0] = comb30; comb[3][1] = comb31; comb[3][2] = comb32;
            comb[4][0] = comb40; comb[4][1] = comb41; comb[4][2] = comb42;
            comb[5][0] = comb50; comb[5][1] = comb51; comb[5][2] = comb52;

            comb[3][3] = comb33; comb[3][4] = comb34; comb[3][5] = comb35;
            comb[4][3] = comb43; comb[4][4] = comb44; comb[4][5] = comb45;
            comb[5][3] = comb53; comb[5][4] = comb54; comb[5][5] = comb55;

            comb[3][6] = comb36; comb[3][7] = comb37; comb[3][8] = comb38;
            comb[4][6] = comb46; comb[4][7] = comb47; comb[4][8] = comb48;
            comb[5][6] = comb56; comb[5][7] = comb57; comb[5][8] = comb58;



            comb[6][0] = comb60; comb[6][1] = comb61; comb[6][2] = comb62;
            comb[7][0] = comb70; comb[7][1] = comb71; comb[7][2] = comb72;
            comb[8][0] = comb80; comb[8][1] = comb81; comb[8][2] = comb82;

            comb[6][3] = comb63; comb[6][4] = comb64; comb[6][5] = comb65;
            comb[7][3] = comb73; comb[7][4] = comb74; comb[7][5] = comb75;
            comb[8][3] = comb83; comb[8][4] = comb84; comb[8][5] = comb85;

            comb[6][6] = comb66; comb[6][7] = comb67; comb[6][8] = comb68;
            comb[7][6] = comb76; comb[7][7] = comb77; comb[7][8] = comb78;
            comb[8][6] = comb86; comb[8][7] = comb87; comb[8][8] = comb88;

            

             

          
        }
        

        //for checking that if a number can be fixed in a spot
        private Boolean checkNarrowed() {
            Boolean reDo = false;
         
            // If a number has only one spot in a block
            
            for (int counter = 1; counter < 10;counter++ ) {
                int[] flag=new int[9];
               
                for (int row = 0; row < 9;row++ ) {
                    for (int column = 0; column < 9;column++ ) {
                    if(arr[row][column].Contains(counter+"")){
                        if (row < 3  && column < 3                            ){ flag[0]++; }
                        if (row < 3  && column < 6 && column > 2              ){ flag[1]++; }
                        if (row < 3  && column > 5                            ){ flag[2]++; }


                        if (row < 6 && row > 2 &&   column < 3                ){ flag[3]++; }
                        if (row < 6 && row > 2 &&   column > 2 && column < 6  ){ flag[4]++; }
                        if (row < 6 && row > 2 &&   column > 5                ){ flag[5]++; }


                        if (row > 5 &&    column < 3                          ){ flag[6]++; }
                        if (row > 5 &&    column < 6 && column > 2            ){ flag[7]++; }
                        if (row > 5 &&    column > 5                          ){ flag[8]++; }




                    }
                    }
                }

                for (int a = 0; a < 9;a++ ) {
                if(flag[a]==1){
                    int rPatch = 0; int cPatch = 0;
                    if (a == 0) {   }
                    if (a == 1) { cPatch = 3; }
                    if (a == 2) { cPatch = 6; }

                    if (a == 3) {             rPatch = 3; }
                    if (a == 4) { cPatch = 3; rPatch = 3; }
                    if (a == 5) { cPatch = 6; rPatch = 3; }

                    if (a == 6) {             rPatch = 6; }
                    if (a == 7) { cPatch = 3; rPatch = 6; }
                    if (a == 8) { cPatch = 6; rPatch = 6; }
                    for (int r = 0; r < 3; r++)
                    {
                        for (int c = 0;  c < 3; c++)
                        {
                            if (arr[r + rPatch][c + cPatch].Contains(counter + "")) {
                                selector(r + rPatch, c + cPatch,counter);
                                reDo = true;
                            }
                        }
                    }

                    



                }
                }
            
            }


            // if a spot has only one number


            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (arr[r][c].Length == 1) { selector(r, c, Convert.ToInt32(arr[r][c])); }
                }
            }
        
            //return type says if the method did some narrowing or not
          
            return reDo;
        }
        private Boolean checkNarrowed(packet P)
        {
            Boolean reDo = false;

            // If a number has only one spot in a block

            for (int counter = 1; counter < 10; counter++)
            {
                int[] flag = new int[9];

                for (int row = 0; row < 9; row++)
                {
                    for (int column = 0; column < 9; column++)
                    {
                        if (P.arr[row][column].Contains(counter + ""))
                        {
                            if (row < 3 && column < 3) { flag[0]++; }
                            if (row < 3 && column < 6 && column > 2) { flag[1]++; }
                            if (row < 3 && column > 5) { flag[2]++; }


                            if (row < 6 && row > 2 && column < 3) { flag[3]++; }
                            if (row < 6 && row > 2 && column > 2 && column < 6) { flag[4]++; }
                            if (row < 6 && row > 2 && column > 5) { flag[5]++; }


                            if (row > 5 && column < 3) { flag[6]++; }
                            if (row > 5 && column < 6 && column > 2) { flag[7]++; }
                            if (row > 5 && column > 5) { flag[8]++; }




                        }
                    }
                }

                for (int a = 0; a < 9; a++)
                {
                    if (flag[a] == 1)
                    {
                        int rPatch = 0; int cPatch = 0;
                        if (a == 0) { }
                        if (a == 1) { cPatch = 3; }
                        if (a == 2) { cPatch = 6; }

                        if (a == 3) { rPatch = 3; }
                        if (a == 4) { cPatch = 3; rPatch = 3; }
                        if (a == 5) { cPatch = 6; rPatch = 3; }

                        if (a == 6) { rPatch = 6; }
                        if (a == 7) { cPatch = 3; rPatch = 6; }
                        if (a == 8) { cPatch = 6; rPatch = 6; }
                        for (int r = 0; r < 3; r++)
                        {
                            for (int c = 0; c < 3; c++)
                            {
                                if (P.arr[r + rPatch][c + cPatch].Contains(counter + ""))
                                {
                                    P.grid[r + rPatch][c + cPatch] = counter;
                                    P.arr[r + rPatch][c + cPatch] = "";
                                    
                                    reDo = true;
                                }
                            }
                        }





                    }
                }

            }


            // if a spot has only one number


            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (P.arr[r][c].Length == 1)
                    {
                        P.grid[r][c] = Convert.ToInt32(P.arr[r][c][0]+"");
                        P.arr[r][c] = "";

                        reDo = true;
                    }
                }
            }
            //return type says if the method did some narrowing or not
            return reDo;
        }

        //name explains it all
        private void removePossiblities() {

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (!comb[r][c].Text.Equals("")) {
                        arr[r][c] = "";
                        int i = Convert.ToInt32(comb[r][c].Text); 
                        remover(comb[r][c], r, c, i); }
                }
            }
            
        
           
        }
        private void removePossiblities(packet p)
        {

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (p.grid[r][c]!=0)
                    {
                        p.arr[r][c] = "";

                        remover(  r, c, p.grid[r][c], p);
                    }
                }
            }



        }
     
        //helper method of remove possibility
        private void remover(TextBox combo,int row,int column,int i) {
            blk.Text = ""+row+"--"+column;
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (row == r || column == c) { arr[r][c]= arr[r][c].Replace(i+"", ""); }
                }
            }

            int rPathc = 0;
            int cPatch = 0;


            if (row < 6 && row > 2) { rPathc = 3; }
            if (row > 5) { rPathc = 6; }

            if (column < 6 && column > 2) { cPatch = 3; }
            if (column > 5) { cPatch = 6; }


            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                { arr[r+rPathc][c+cPatch] = arr[r+rPathc][c+cPatch].Replace(i + "", "");   }
            }



        }
        private void remover( int row, int column, int i,packet p)
        {



            blk.Text = "" + row + "--" + column;
           
            
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (row == r || column == c) { p.arr[r][c] = p.arr[r][c].Replace(i + "", ""); }
                }
            }

            int rPathc = 0;
            int cPatch = 0;


            if (row < 6 && row > 2) { rPathc = 3; }
            if (row > 5) { rPathc = 6; }

            if (column < 6 && column > 2) { cPatch = 3; }
            if (column > 5) { cPatch = 6; }


            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                { p.arr[r + rPathc][c + cPatch] = p.arr[r + rPathc][c + cPatch].Replace(i + "", ""); }
            }



        }
        

        //selects a number to be inserted into the grid
        private void selector(int row,int column,int value) {
             
            comb[row][column].Background = Brushes.LawnGreen;
            arr[row][column] = "";
            situation[row][column] = 2;
            comb[row][column].Text = (value )+"";
           
        }
       

        //checking if two positions have exactly same possibilities
        private Boolean coupleChecker() {
            Boolean ans = false;
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (arr[r][c].Length == 2)
                    {
                       
                        for (int rr = r; rr < 9; rr++)
                        {
                            for (int cc = c; cc < 9; cc++)
                            {

                              // 
                                 if(arr[r][c].Equals(arr[rr][cc]) && (r!=rr || c!=cc)){
                                char[] asd=arr[r][c].ToCharArray();

                              
                                     
                                     if(r==rr){ //if row is same
                                           
                                     for (int z = 0; z < 9;z++ ) {
                                         
                                       if(z!=c && z!=cc){
                                     
                                           int temp = arr[r][z].Length;
                                           arr[r][z] = arr[r][z].Replace(asd[0]+"","");
                                          arr[r][z] = arr[r][z].Replace(asd[1] + "", "");
                                          // ans flag is to see if the method reduced any possibilities
                                          if (temp != arr[r][z].Length) { ans = true; }
                                       }
                                     }
                                 }
                                    

                                     if (c == cc)//if column is same
                                     {

                                         // ans flag is to see if the method reduced any possibilities
                                         for (int z = 0; z < 9; z++)
                                         {
                                             if (z != r && z != rr)
                                             {
                                                 int temp = arr[z][c].Length;
                                                 arr[z][c] = arr[z][c].Replace(asd[0] + "", "");
                                                 arr[z][c] = arr[z][c].Replace(asd[1] + "", "");
                                                 if (temp != arr[z][c].Length) { ans = true; }
                                             }
                                         }
                                     }


                                     //if block is same
                                     Boolean b = false;//to see if the method reduced any possibilities
                                     int rPathc = 0;
                                     int cPatch = 0;
                                     if (r < 3 && c < 3 && rr < 3 && cc < 3) { b = true; }
                                     if (r < 3 && c < 6 && c > 2 && rr < 3 && cc < 6 && cc > 2) { cPatch = 3; b = true; }
                                     if (r < 3 && c > 5 && rr < 3 && cc > 5) { cPatch = 6; b = true; }


                                     if (r < 6 && r > 2 && c < 3 && rr < 6 && rr > 2 && cc < 3) { rPathc = 3; b = true; b = true; }
                                     if (r < 6 && r > 2 && c > 2 && c < 6 && rr < 6 && rr > 2 && cc > 2 && cc < 6) { cPatch = 3; rPathc = 3; b = true; }
                                     if (r < 6 && r > 2 && c > 5 && rr < 6 && rr > 2 && cc > 5) { cPatch = 6; rPathc = 3; b = true; }


                                     if (r > 5 && c < 3 && rr > 5 && cc < 3) { rPathc = 6; b = true; }
                                     if (r > 5 && c < 6 && c > 2 && rr > 5 && cc < 6 && cc > 2) { cPatch = 3; rPathc = 6; b = true; }
                                     if (r > 5 && c > 5 && rr > 5 && cc > 5) { cPatch = 6; rPathc = 6; b = true; }




                                     if (b)
                                     {
                                         
                                         for (int d = 0; d < 3; d++)
                                         {
                                             for (int dd = 0; dd < 3; dd++)
                                             {
                                               if ((d + rPathc != r || dd + cPatch != c) && (d + rPathc != rr || dd + cPatch != cc)){
                                                   int temp=arr[d+rPathc][dd+cPatch].Length;
                                                   arr[d + rPathc][dd + cPatch] = arr[d + rPathc][dd + cPatch].Replace(asd[0]+"","");
                                                   arr[d + rPathc][dd + cPatch] = arr[d + rPathc][dd + cPatch].Replace(asd[1] + "", "");
                                                   if (arr[d + rPathc][dd + cPatch].Length != temp) { ans = true; }
                                               }
                                             }


                                         }

                                     }
                                 }
                            }

                        }
                    }
                }
            }
            return ans;
        }
        private Boolean coupleChecker(packet p)
        {
            Boolean ans = false;
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (p.arr[r][c].Length == 2)
                    {

                        for (int rr = r; rr < 9; rr++)
                        {
                            for (int cc = c; cc < 9; cc++)
                            {

                                // 
                                if (p.arr[r][c].Equals(p.arr[rr][cc]) && (r != rr || c != cc))
                                {
                                    char[] asd = p.arr[r][c].ToCharArray();



                                    if (r == rr)
                                    { //if row is same

                                        for (int z = 0; z < 9; z++)
                                        {

                                            if (z != c && z != cc)
                                            {

                                                int temp = p.arr[r][z].Length;
                                                p.arr[r][z] = p.arr[r][z].Replace(asd[0] + "", "");
                                                p.arr[r][z] = p.arr[r][z].Replace(asd[1] + "", "");
                                                // ans flag is to see if the method reduced any possibilities
                                                if (temp != p.arr[r][z].Length) { ans = true; }
                                            }
                                        }
                                    }
                                   

                                    if (c == cc)//if column is same
                                    {

                                        // ans flag is to see if the method reduced any possibilities
                                        for (int z = 0; z < 9; z++)
                                        {
                                            if (z != r && z != rr)
                                            {
                                                int temp = p.arr[z][c].Length;
                                                p.arr[z][c] = p.arr[z][c].Replace(asd[0] + "", "");
                                                p.arr[z][c] = p.arr[z][c].Replace(asd[1] + "", "");
                                                if (temp != p.arr[z][c].Length) { ans = true; }
                                            }
                                        }
                                    }


                                    //if block is same
                                    Boolean b = false;//to see if the method reduced any possibilities
                                    int rPathc = 0;
                                    int cPatch = 0;
                                    if (r < 3 && c < 3 && rr < 3 && cc < 3) { b = true; }
                                    if (r < 3 && c < 6 && c > 2 && rr < 3 && cc < 6 && cc > 2) { cPatch = 3; b = true; }
                                    if (r < 3 && c > 5 && rr < 3 && cc > 5) { cPatch = 6; b = true; }


                                    if (r < 6 && r > 2 && c < 3 && rr < 6 && rr > 2 && cc < 3) { rPathc = 3; b = true; b = true; }
                                    if (r < 6 && r > 2 && c > 2 && c < 6 && rr < 6 && rr > 2 && cc > 2 && cc < 6) { cPatch = 3; rPathc = 3; b = true; }
                                    if (r < 6 && r > 2 && c > 5 && rr < 6 && rr > 2 && cc > 5) { cPatch = 6; rPathc = 3; b = true; }


                                    if (r > 5 && c < 3 && rr > 5 && cc < 3) { rPathc = 6; b = true; }
                                    if (r > 5 && c < 6 && c > 2 && rr > 5 && cc < 6 && cc > 2) { cPatch = 3; rPathc = 6; b = true; }
                                    if (r > 5 && c > 5 && rr > 5 && cc > 5) { cPatch = 6; rPathc = 6; b = true; }


                                    if (b)
                                    {

                                        for (int d = 0; d < 3; d++)
                                        {
                                            for (int dd = 0; dd < 3; dd++)
                                            {
                                                if ((d + rPathc != r || dd + cPatch != c) && (d + rPathc != rr || dd + cPatch != cc))
                                                {
                                                    int temp = p.arr[d + rPathc][dd + cPatch].Length;
                                                    p.arr[d + rPathc][dd + cPatch] = p.arr[d + rPathc][dd + cPatch].Replace(asd[0] + "", "");
                                                    p.arr[d + rPathc][dd + cPatch] = p.arr[d + rPathc][dd + cPatch].Replace(asd[1] + "", "");
                                                    if (p.arr[d + rPathc][dd + cPatch].Length != temp) { ans = true; }
                                                }
                                            }


                                        }

                                    }
                                }
                            }

                        }
                    }
                }
            }
            return ans;
        }

        //checking if the grid is complete
        private Boolean checkIfDone() {

            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++) {

                    if (situation[a][b] != 1 && situation[a][b] != 2) {
                        
                        return false; }
                
                }
            }

            return true;
        }
        private Boolean checkIfDone(packet p)
        {

            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {

                    if (p.grid[a][b]==0)
                    {
                       return false;
                    }

                }
            }

            return true;
        }
       
        //fills the possibilits(Initialy)
        private void fillPossibilities() {
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {

                    if (situation[a][b] != 1 && situation[a][b] != 2) { 
                        
                        comb[a][b].Text = arr[a][b];

                        comb[a][b].Background = Brushes.Cyan;
                        comb[a][b].Foreground = Brushes.White;

                    }

                }
            }
        }
        private void fillGivens(){
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (!comb[r][c].Text.Equals(""))
                    {
                        comb[r][c].IsEnabled = false;
                        comb[r][c].Background = Brushes.DarkViolet;
                        comb[r][c].Foreground = Brushes.White;
                        situation[r][c] = 1;
                    }

                }
            }
        }
        
        //returns the current grid
        private int[][] getGrid()
        {
            int[][] ans = new int[9][];
            for (int a = 0; a < 9; a++) { ans[a] = new int[9]; }

            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                     
                    if (comb[a][b].Text.Length > 0) { ans[a][b] = Convert.ToInt32(comb[a][b].Text);
                     
                    }
                }
            }
            return ans;

        }
        
         //searches if a packet is already checkd / in trash
        private Boolean alreadyInTrash(packet p) {

           

            for(int counter=0;counter<trash.Count;counter++){
                Boolean flag = false;
              

                int[][] asd=trash[counter];
                for (int r = 0; r < 9;r++ ) {
                for (int c = 0; c < 9;c++ ) {
                

                    if (p.grid[r][c] != asd[r][c]) { flag = true; break; }
                    if (r == 8 && c == 8) { return true; }
                }
                if (flag) { break; }
            }
        }
          

            return false;
        }

        //makes and returns a packet from a give 2D grid
        private packet makePacket(int[][] grid) {
   
            //imports a grid generates the other arttribute to make the packet.. returns the packet
            int[][] sit = new int[9][];
            string[][] pos = new string[9][];


            for (int a = 0; a < 9;a++ ) {
                sit[a]=new int[9];pos[a]=new string[9];
            }
            //generating situation
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    pos[a][b] = "123456789";
                    if (grid[a][b] != 0) {
                        sit[a][b] = 3;
                        if (situation[a][b] == 1) { sit[a][b] = 1; }
                        if(situation[a][b]==2){ sit[a][b] = 2; }

                    }
                     
                }
            }
           
            //generating and removing possibilities
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (grid[row][column] != 0)
                    {
                        pos[row][column] = "";
                        int i = grid[row][column];

                        //row and column wise

                        for (int r = 0; r < 9; r++)
                        {
                            for (int c = 0; c < 9; c++)
                            {
                                if (row == r || column == c) { arr[r][c] = arr[r][c].Replace(i + "", ""); }
                            }
                        }
                        //detecting block
                        int rPathc = 0;
                        int cPatch = 0;


                        if (row < 6 && row > 2) { rPathc = 3; }
                        if (row > 5) { rPathc = 6; }

                        if (column < 6 && column > 2) { cPatch = 3; }
                        if (column > 5) { cPatch = 6; }


                       //removing block wise

                        for (int r = 0; r < 3; r++)
                        {
                            for (int c = 0; c < 3; c++)
                            { 
                                pos[r + rPathc][c + cPatch] = pos[r + rPathc][c + cPatch].Replace(i + "", ""); }
                        }






                    }
                }
            }
       


           
            return new packet(grid,pos,sit);
        
        
        
        }


        //tries to narrow down the grid using Logic
        private void tryLogic() {
            fillStrings();//fills the array with possibilities
            fillGivens();//fills the givens with voilet color
            removePossiblities();
            blk.Text = arr[0][0] + "";
            Boolean flag = checkNarrowed();

            while (flag)
            {
                removePossiblities(); flag = checkNarrowed();
            }
           


            flag = coupleChecker();
            while (flag)
            {
                removePossiblities();
                if (checkNarrowed()) { continue; }
                flag = coupleChecker();
                // MessageBox.Show(arr[0][0]);
            }
        
        }
        private void tryLogic(packet p)
        {
            
            removePossiblities(p);
            //blk.Text = arr[0][0] + "";
            Boolean flag = checkNarrowed(p);

            while (flag)
            {
                removePossiblities(p); flag = checkNarrowed(p);
            }
        


            flag = coupleChecker(p);
            while (flag)
            {
                removePossiblities(p);
                if (checkNarrowed(p)) { continue; }
                flag = coupleChecker(p);
              

            }

        }


        private int[] getMinimumLength(packet p) {
            int ans = 10;
            int c = -1, r = -1;
            
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {

                    if (p.arr[a][b].Length < ans && p.arr[a][b].Length!=0)
                    {
                        ans = p.arr[a][b].Length;
                        r = a; c = b;
                    }
                }
            }

            int[] an = {r, c ,ans};
            return an;
        }
        


        //duplicates and returns the given packet
        private packet makeDuplicate(packet p) {
            
            int[][] grid=new int[9][];
            string[][] arr = new string[9][];
            int[][] sit=new int[9][];




            for (int r = 0; r < 9;r++ ) {
                grid[r] = new int[9];
                arr[r] = new string[9];
                sit[r] = new int[9];

                for (int c = 0; c < 9;c++ ) {


                    grid[r][c] = p.grid[r][c];
                    sit[r][c] = p.grid[r][c];
                    arr[r][c] = p.arr[r][c];

                }
            
            
            
            }


            packet pp = new packet(grid,arr,sit);
            return pp;
        
        
        }



        //breaks down a grid when logic cant solve further (Makes a guess)
        private packet[] breakDown(packet P) {
           
            //breaks down a packet by guessing into 2 or 3 more packets
            int[] targetCell = getMinimumLength(P);


            if (targetCell[2] == 10) { packet[] tt = { P }; return tt; }

            if (debug)
            {
                MessageBox.Show("ROW: " + targetCell[0] + "--Column:" + targetCell[1] + "---Length: " + targetCell[2] + "--Pos:" + P.arr[targetCell[0]][targetCell[1]]);
            }

              int numOfPackets = targetCell[2];

            packet[] temps = new packet[numOfPackets];

            for(int i=0;i<numOfPackets && numOfPackets<=3;i++){
                temps[i] = makeDuplicate(P);
            }


            if (numOfPackets >= 2) {
                temps[0].grid[targetCell[0]][targetCell[1]] = Convert.ToInt32(P.arr[targetCell[0]][targetCell[1]][0] + ""); removePossiblities(temps[0]);
                temps[1].grid[targetCell[0]][targetCell[1]] = Convert.ToInt32(P.arr[targetCell[0]][targetCell[1]][1] + ""); removePossiblities(temps[1]);
             }
            if (numOfPackets >= 3)
            {
                temps[2].grid[targetCell[0]][targetCell[1]] = Convert.ToInt32(P.arr[targetCell[0]][targetCell[1]][2] + ""); removePossiblities(temps[2]);
            }


            if (numOfPackets >= 2)
            {
                while (checkNarrowed(temps[0])) { removePossiblities(temps[0]); }
                while (checkNarrowed(temps[1])) { removePossiblities(temps[1]); }
            }
            
            if (numOfPackets >= 3)
            {
                while (checkNarrowed(temps[2])) { removePossiblities(temps[2]); }
            }






        return temps;
        }


        //adds the array to the stack from where nodes are poped and read
        private void addToStack(packet[] p) {
            //adds the array of packet to the stack to use by poping later

            if (p.Length == 1) { trash.Add(p[0].grid); }

            for (int i = 0; i < p.Length;i++ ) {

                if (!alreadyInTrash(p[i]))
                {

                    totalNodes = totalNodes + p.Length;

                    stk.Push(p[i]);
                }
                else {
            
                
                }
            }
        
        
        }

        private void fillTheGrid(packet p) {
            //fills the grid and colrs them after solving by using AI

            for (int r = 0; r < 9;r++ ) {
                for (int c = 0; c < 9;c++ ) {
                    if (comb[r][c].Text.Equals("")) { 
                        comb[r][c].Text = p.grid[r][c]+"";
                        comb[r][c].Background = Brushes.Aqua;
                    }
                }
            }
        
        }

        private Boolean checkForErrors(packet p) {

            //checks only ROW and Column wise
            // as it works by naroowing down possibilities.. checking isnt necessary


            for (int counter = 1; counter < 10;counter++ ) {
                int flag = 0;
                for (int r = 0; r < 9;r++ ) {
                    for (int c = 0; c < 9;c++ ) {
                        if (p.grid[r][c] == counter) { flag++; }
                    }
                    if (flag > 1) { return false; }
                    else { flag = 0; }
                }
                int flag2 = 0;
                for (int c = 0; c < 9; c++)
                {
                    for (int r = 0; r < 9; r++)
                    {
                        if (p.grid[r][c] == counter) { flag2++; }
                    }
                    if (flag2 > 1) { return false; }
                    else { flag2 = 0; }
                }

                 
            }
            return true;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            start = DateTime.Now;
            tryLogic();
            // if (checkIfDone()) { return; }//return if the job is done
            if (checkIfDone()) { return; }  
            //at this point the AI part comes in
            //because simple logic failed to solve the puzzle




            if (debug)
            { MessageBox.Show("AI Starts"); }
            packet starter = new packet(getGrid(),arr,situation);
            
 
            packet[] temp= breakDown(starter);
            
            trash.Add(starter.grid);
        
            addToStack(temp);
             
            while(stk.Count!=0){

                packet p = stk.Pop();
                trash.Add(p.grid);
                tryLogic(p);
                if (checkIfDone(p)) {

                    if (checkForErrors(p))
                    {

                        fillTheGrid(p);
                        end = DateTime.Now;
                      

                        
                        int sm = start.Millisecond;
                       
                        int em = end.Millisecond;
                     
                        int time = em-sm;

                         if (time < 0) { time = time + 1000; }


                        MessageBox.Show("Completed.\nTotal Nodes: "+totalNodes+"         Time Required: "+time+" Miliseconds");
                    return;} }
                else { addToStack(breakDown(p)); }
                
            
            }
 

 
        }

        
    }
}
