#include <fstream>
#include <iostream>
#include <string>

void Problem1Part1()
{
    std::string file_path = "Input/input-1.txt";
    std::ifstream file (file_path);
    if (!file)
    {
        std::cerr << "Could not open file " << file_path << '\n';
        return;
    }
    
    int dial_position = 50;
    int dial_size = 100;
    int zero_counter = 0;
    
    std::string line;
    while (std::getline(file, line))
    {
        if (line.empty())
        {
            std::cerr << "Found empty line in" << file_path << ", skipping" << '\n';
            continue;
        }
        std::cout << line << '\n';
        
        // First character of the line indicates direction
        char direction = line[0];
        
        // Remaining characters are the rotation distance
        std::string distance_string = line.substr(1);
        int distance = stoi(distance_string);
        
        if (direction == 'L')
        {
            dial_position -= distance;
            while (dial_position < 0)
            {
                dial_position += dial_size;
            }
            dial_position = dial_position % dial_size;
        }
        else if (direction == 'R')
        {
            dial_position += distance;
            dial_position = dial_position % dial_size;
        }
        
        if (dial_position == 0)
        {
            zero_counter++;
        }
    }
    
    std::cout << "Zero count: " << zero_counter << '\n';
}

void Problem1Part2()
{
    std::string file_path = "Input/input-1.txt";
    std::ifstream file (file_path);
    if (!file)
    {
        std::cerr << "Could not open file " << file_path << '\n';
        return;
    }
    
    int dial_position = 50;
    int dial_size = 100;
    int zero_counter = 0;
    
    std::string line;
    while (std::getline(file, line))
    {
        if (line.empty())
        {
            std::cerr << "Found empty line in" << file_path << ", skipping" << '\n';
            continue;
        }
        std::cout << line << '\n';
        
        int previous_position = dial_position;
        
        // First character of the line indicates direction
        char direction = line[0];
        
        // Remaining characters are the rotation distance
        std::string distance_string = line.substr(1);
        int distance = stoi(distance_string);
        
        int full_rotations = distance / dial_size;
        int remainder_distance = distance % dial_size;
        
        if (direction == 'L')
        {
            dial_position -= remainder_distance;
            if (dial_position <= 0 &&
                previous_position > 0)  // Edge case where dial is already at 0 and moves left doesn't count as a click
            {
                zero_counter++;
            }
            
            while (dial_position < 0)
            {
                dial_position += dial_size;
            }
        }
        else if (direction == 'R')
        {
            dial_position += remainder_distance;
            if (dial_position >= dial_size)
            {
                zero_counter++;
            }
        }
        
        dial_position = dial_position % dial_size;
        
        zero_counter += full_rotations;
    }
    
    std::cout << "Zero count: " << zero_counter << '\n';
}