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


    public bool[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        GenerateNewGrid();
    }

    void GenerateNewGrid() {
        grid = new bool[rows,columns];
        RandomizeGrid();
        for(int i = 0; i < iterationCount; i++) {
            runIteration();
        }
    }

// populate the grid with random true or false values
    void RandomizeGrid() {
        for(int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                if(Random.value < initialAliveProbability) {
                    grid[i,j] = true;
                }
            }
        }
    }

// run a modified iteration of Game of Life
    void runIteration() {
        // make a clone of the original grid
        bool[,] oldGrid = (bool[,]) grid.Clone();
        for(int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                bool CurrentCellAlive = grid[row,col];
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
                                if(oldGrid[i,j]) {
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
                        grid[row,col] = false;
                    }
                }
                // if the cell is not alive, then it will be born if it has more than BirthThreshold neighbors
                else if(AliveNeighborCount > birthThreshold) {
                    grid[row,col] = true;
                }
            }
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

    public bool[,] GetGrid() {
        return grid;
    }

}
