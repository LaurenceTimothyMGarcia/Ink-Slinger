using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{

    public int rows = 10;
    public int columns = 10;
    public float initialAliveProbability = .5f;
    public int birthThreshold = 5;
    public int survivalThreshold = 4;
    public int iterationCount = 5;


    public int[,] grid;
    // Start is called before the first frame update
    void Awake()
    {
        GenerateNewGrid();
    }

    void GenerateNewGrid() {
        grid = new int[rows,columns];
        RandomizeGrid();
        for(int i = 0; i < iterationCount; i++) {
            runIteration();
        }
        RemoveDisconnects();
    }

// populate the grid with random 1 or 0 values
    void RandomizeGrid() {
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                if(Random.value < initialAliveProbability) {
                    grid[i,j] = 1;
                }
            }
        }
    }

// run a modified iteration of Game of Life
    void runIteration() {
        // make a clone of the original grid
        int[,] oldGrid = (int[,]) grid.Clone();
        for(int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                bool CurrentCellAlive = grid[row,col] > 0;
                // count the number of neighbors
                int AliveNeighborCount = 0;
                // iterate through the nine cells 
                for (int i = row - 1; i <= row + 1; i++) {
                    for (int j = col - 1; j <= col + 1; j++) {
                        // exclude self
                        if(!(i == row && j == col)) {
                            // check if this neighbor is in bounds
                            if(CellInBounds(i,j)) {
                                // if it's in bounds, check if this neighbor is alive and increase the count if so
                                if(oldGrid[i,j] > 0) {
                                    AliveNeighborCount++;
                                }
                            }
                            // if the neighbor is out of bounds, just count it as alive
                            else {
                                AliveNeighborCount++;
                            }
                        }
                    }
                }
                // then depending on whether or not the current cell is alive or dead, determine if the cell will change
                // a living cell needs more than SurvivalThreshold living neighbors to continue surviving, otherwise it dies
                if(CurrentCellAlive) {
                    // kill the cell if it doesn't have enough neighbors
                    if(AliveNeighborCount < survivalThreshold) {
                        grid[row,col] = 0;
                    }
                }
                // if the cell is not alive, then it will be born if it has more than BirthThreshold neighbors
                else if(AliveNeighborCount > birthThreshold) {
                    grid[row,col] = 1;
                }
            }
        }
    }

// remove any disconnected parts by identifying all blobs and deleting all but the largest
    void RemoveDisconnects() {
        // for some reason just using 0 instead of 1 to identify empty cells is not working
        // so just invert every cell, I fucking guess
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                // fuck it. ternary operator, just for kicks
                grid[i,j] = grid[i,j] == 1 ? 0 : 1;
            }
        }

        // identify blobs
        int numberOfBlobs = 0;
        // iterate through the array and attempt to flood fill on any cells with a value of exactly 1
        // any cell with a value of 1 is part of a blob that has not been identified yet
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                if(grid[i,j] == 1) {
                    // the flood fill marks each cell with a unique value
                    // so we set this value to the number of identified blobs so far + 1
                    // to guarantee a mark unique from each blob, as well as unique from 1 (which is am unidentified cell)
                    numberOfBlobs++;
                    FloodFill(i, j, numberOfBlobs + 1);
                }
            }
        }

        // identify which blob is the largest
        int[] blobSizes = new int[numberOfBlobs];
        // iterate through the array again, incrementing the size of each
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                // the -2 offset is because of value 0 being a "full" cell and value 1 being an untested cell
                if(grid[i,j] > 1) blobSizes[grid[i,j] - 2]++;
            }
        }

        int largestBlob = 0;
        int currentLargest = -1;
        for(int i = 0; i < numberOfBlobs; i++) {
            if(blobSizes[i] > currentLargest) {
                largestBlob = i;
                currentLargest = blobSizes[i];
            }
        }
        
        // so we now know blob #largestBlob is the largest; any values in said largest blob have the value largestBlob + 2
        // now we will set all cells with value largestBlob + 2 to 1, and all others to 0, erasing any smaller blobs
        
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                // fuck it. ternary operator again
                grid[i,j] = (grid[i,j] == largestBlob + 2) ? 1 : 0;
            }
        }
    }

    void FloodFill(int x, int y, int value) {
        // if this cell is not an untested cell (aka has a value of 1), don't do anything
        if(!CellInBounds(x,y) || grid[x,y] != 1) {
            return;
        }
        // if it is an untested cell, fill it with the value, then attempt to flood fill each of this cell's Von Neumann neighbors
        else {
            grid[x,y] = value;
            // apparently, left > right > up > down is best for performance
            FloodFill(x - 1, y, value);
            FloodFill(x + 1, y, value);
            FloodFill(x, y + 1, value);
            FloodFill(x, y - 1, value);
        }
    }

// determines if a cell is "in bounds" in the grid
// potentially later I can change this to change the "in bounds" area from a rectangle to an oval for more natural looking "rooms"
    bool CellInBounds(int x, int y) {
        if( (0 <= x && x < rows) && (0 <= y && y < columns) ) {
            return true;
        }
        return false;
    }

    // the final grid should just be trues and falses because why the hell not. i dont feel like rewriting my code in the other scripts
    public bool[,] GetGrid() {
        bool[,] outGrid = new bool[rows,columns];

        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                if(grid[i,j] > 0) outGrid[i,j] = true;
            }
        }
        return outGrid;
    }

    void printGrid() {
        string outString = "";
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                outString += grid[i,j];
            }
            outString += "\n";
        }
        Debug.Log(outString);
    }
}
