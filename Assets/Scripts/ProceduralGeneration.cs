using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public int rows = 100;
    public int columns = 100;

    public int roomSize = 16;
    public int numberOfRooms = 6;

    public int MaxRoomAttempts = 100;

    bool[,] grid;

    List<Room> rooms;
    List<Hallway> hallways;

    CellularAutomata CAGenerator;

    void Awake()
    {
        CAGenerator = GetComponent<CellularAutomata>();
        RunPCG();
    }

    void RunPCG()
    {
        // get an empty grid
        grid = new bool[rows, columns];

        rooms = new List<Room>();

        var counter = 0;
        while (rooms.Count < numberOfRooms && counter < MaxRoomAttempts)
        {
            AttemptAddRoom();
            counter++;
        }

        hallways = new List<Hallway>();
        RandomizedPrims();

        AddRoomsToGrid();
        AddHallwaysToGrid();
    }

    void AttemptAddRoom()
    {
        Room newRoom = new Room(Random.Range(0, rows), Random.Range(0, columns), roomSize, roomSize, CAGenerator);

        if (newRoom.inBounds(rows, columns))
        {
            bool newRoomValid = true;

            foreach (Room room in rooms)
            {
                if (newRoom.intersects(room, 1)) newRoomValid = false;
            }

            if (newRoomValid) rooms.Add(newRoom);
        }
    }

    void RandomizedPrims() {
        // each room is a node and each hallway is an edge
        // step zero: have a graph
        // we can just... connect every room, for now...
        List<Hallway> candidateHallways = new List<Hallway>();
        // add a candidate hallway from each room to every other room
        for(int i = 0; i < rooms.Count; i++) {
            for(int j = i + 1; j < rooms.Count; j++) {
                // the constructor for a hallway already gives each hallway a randomized weight
                candidateHallways.Add(new Hallway(rooms[i],rooms[j]));
            }
        }
        // step one: choose a random node to be the initial vertex in our tree
        List<Room> connectedRooms = new List<Room>();
        connectedRooms.Add(rooms[Random.Range(0,rooms.Count)]);
        
        // step 2 (repeat until done): add the lowest weight edge that connects a connected node to an unconnected node
        while(connectedRooms.Count < rooms.Count) {
            candidateHallways.Sort(compareHallways);
            candidateHallways.Reverse();

            for(int i = 0; i < candidateHallways.Count; i++) {
                Hallway candidateHallway = candidateHallways[i];
                // if exactly one of the two rooms this hallway connects is already connected...
                if(connectedRooms.Contains(candidateHallway.from) && !connectedRooms.Contains(candidateHallway.to) || !connectedRooms.Contains(candidateHallway.from) && connectedRooms.Contains(candidateHallway.to)) {
                    // add this hallway to the main map
                    hallways.Add(candidateHallway);
                    // then mark the previously unconnected room as connected
                    if(connectedRooms.Contains(candidateHallway.from)) {
                        connectedRooms.Add(candidateHallway.to);
                    }
                    else {
                        connectedRooms.Add(candidateHallway.from);
                    }
                    break;
                }
            }
        }

        // yippee!!! prim's is done!
    }

    int compareHallways(Hallway a, Hallway b) {
        return a.weight.CompareTo(b.weight);
    }

    void AddRoomsToGrid()
    {
        foreach (Room room in rooms)
        {
            for (int i = 0; i < room.rows; i++)
            {
                for (int j = 0; j < room.columns; j++)
                {
                    grid[i + room.topLeftPosition.x, j + room.topLeftPosition.y] = room.grid[i, j];
                }
            }
        }
    }

    void AddHallwaysToGrid()
    {
        foreach (Hallway hallway in hallways)
        {
            foreach (Vector2Int point in hallway.getPoints())
            {
                grid[point.x,point.y] = true;
            }
        }
    }

    public bool[,] GetGrid()
    {
        return grid;
    }

    class Room
    {
        public int rows;
        public int columns;
        public Vector2Int topLeftPosition;

        public bool[,] grid;

        public Room(int x, int y, int n_rows, int n_columns, CellularAutomata CAGenerator)
        {
            topLeftPosition = new Vector2Int(x, y);
            rows = n_rows;
            columns = n_columns;
            newGrid(CAGenerator);
        }

        void newGrid(CellularAutomata CAGenerator)
        {
            grid = CAGenerator.GetGrid();
        }

        bool[,] getGrid()
        {
            return grid;
        }

        public bool inBounds(int x, int y)
        {
            return ((topLeftPosition.x >= 0) && (topLeftPosition.x + rows < x) && (topLeftPosition.y >= 0) && (topLeftPosition.y + columns < y));
        }

        public bool intersects(Room other, int minGap)
        {
            bool intersecting = false;

            // two rectangular rooms are intersecting if any of one room's corners is inside the bounds of the other
            // if one of the left or right edges is inside the horizontal bounds of the other...
            if (((this.topLeftPosition.x >= other.topLeftPosition.x - minGap) && (this.topLeftPosition.x <= other.topLeftPosition.x + other.rows + minGap)) ||
               ((this.topLeftPosition.x + this.rows >= other.topLeftPosition.x - minGap) && (this.topLeftPosition.x + this.rows <= other.topLeftPosition.x + other.rows + minGap)))
            {
                // and one of the up or down edges is inside the vertical bounds of the other...
                if (((this.topLeftPosition.y >= other.topLeftPosition.y - minGap) && (this.topLeftPosition.y < other.topLeftPosition.y + other.columns + minGap)) ||
                   ((this.topLeftPosition.y + this.columns >= other.topLeftPosition.y - minGap) && (this.topLeftPosition.y + this.columns <= other.topLeftPosition.y + other.columns + minGap)))
                {
                    // then the rooms are intersecting
                    intersecting = true;
                }
            }

            return intersecting;
        }
        public Vector2Int getRandomPoint() {
            Vector2Int outVector = new Vector2Int(Random.Range(0,rows),Random.Range(0,columns));
            while(!grid[outVector.x,outVector.y]) {
                outVector = new Vector2Int(Random.Range(0,rows),Random.Range(0,columns));
            }
            return outVector;
        }
    }
    class Hallway
    {
        public Room from;
        public Room to;
        // used for Prim's algorithm
        public float weight;

        Vector2Int startPoint;
        Vector2Int endPoint;
        List<Vector2Int> points;

        public Hallway(Room a_from, Room a_to)
        {
            from = a_from;
            to = a_to;
            weight = Random.value;
            drawPoints();
        }

        void drawPoints()
        {
            points = new List<Vector2Int>();
            // todo: make startPoint and endPoint a random point in from and to rooms
            startPoint = from.topLeftPosition + from.getRandomPoint();
            endPoint = to.topLeftPosition + to.getRandomPoint();
            // coinflip to draw horizontal or vertical line first
            if (Random.value > .5)
            {
                for (int i = startPoint.x; i != endPoint.x; i += startPoint.x < endPoint.x ? 1 : -1)
                {
                    points.Add(new Vector2Int(i, startPoint.y));
                }
                for (int i = startPoint.y; i != endPoint.y; i += startPoint.y < endPoint.y ? 1 : -1)
                {
                    points.Add(new Vector2Int(endPoint.x, i));
                }
            }
            else
            {
                for (int i = startPoint.y; i != endPoint.y; i += startPoint.y < endPoint.y ? 1 : -1)
                {
                    points.Add(new Vector2Int(startPoint.x, i));
                }
                for (int i = startPoint.x; i != endPoint.x; i += startPoint.x < endPoint.x ? 1 : -1)
                {
                    points.Add(new Vector2Int(i, endPoint.y));
                }
            }

        }


        public List<Vector2Int> getPoints()
        {
            return points;
        }
    }
}

    
