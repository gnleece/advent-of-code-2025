#include <fstream>
#include <iostream>
#include <string>
#include <vector>

bool IsLocationOccupied(std::vector<std::string>& grid, size_t row, size_t col)
{
    if (row < 0 || row >= grid.size())
    {
        return false;
    }

    const std::string& row_string = grid[row];
    
    if (col < 0 || col >= row_string.length())
    {
        return false;
    }
    
    char location_char = row_string[col];
    return location_char == '@';    
}

bool IsLocationAccessible(std::vector<std::string>& grid, int row, int col)
{
    if (row < 0 || row > grid.size())
    {
        return false;
    }

    const std::string& row_string = grid[row];
    
    if (col < 0 || col >= row_string.length())
    {
        return false;
    }

    int neighbor_count = 0;
    for (int y = row - 1; y <= row + 1; y++)
    {
        for (int x = col - 1; x <= col + 1; x++)
        {
            if (x == col && y == row)
            {
                continue;
            }
            
            if (IsLocationOccupied(grid, y, x))
            {
                neighbor_count++;
            }
        }
    }
    
    return neighbor_count < 4;
}

void Problem4Part1()
{
    std::string file_path = "Input/input-4.txt";
    std::ifstream file(file_path);
    
    if (!file)
    {
        std::cerr << "Could not open file: " << file_path << "\n";
        return;
    }
    
    std::vector<std::string> grid; 
    
    std::string line;
    while (std::getline(file, line))
    {
        grid.push_back(line);
    }
    
    if (grid.empty())
    {
        std::cerr << "File is empty: " << file_path << "\n";
        return;
    }
    
    size_t num_rows = grid.size();
    size_t num_columns = grid[0].length();
    
    int num_accessible_locations = 0;
    
    for (size_t y = 0; y < num_rows; y++)
    {
        for (size_t x = 0; x < num_columns; x++)
        {
            if (IsLocationOccupied(grid, y, x) && IsLocationAccessible(grid, y, x))
            {
                num_accessible_locations++;
            }
        }
    }
    
    std::cout << "Num accessible locations: " << num_accessible_locations << "\n";
}
