// project-c-plus-plus.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <fstream>
#include <iostream>
#include <string>

void Problem1Part1()
{
    std::string filePath = "Input/input-1.txt";
    std::ifstream file (filePath);
    if (!file)
    {
        std::cerr << "Could not open file " << filePath << '\n';
        return;
    }
    
    int dialPosition = 50;
    int dialSize = 100;
    int zeroCounter = 0;
    
    std::string line;
    while (std::getline(file, line))
    {
        if (line.empty())
        {
            std::cerr << "Found empty line in" << filePath << ", skipping" << '\n';
            continue;
        }
        std::cout << line << '\n';
        
        // First character of the line indicates direction
        char direction = line[0];
        
        // Remaining characters are the rotation distance
        std::string distanceString = line.substr(1);
        int distance = atoi(distanceString.c_str());
        
        if (direction == 'L')
        {
            dialPosition -= distance;
            while (dialPosition < 0)
            {
                dialPosition += dialSize;
            }
            dialPosition = dialPosition % dialSize;
        }
        else if (direction == 'R')
        {
            dialPosition += distance;
            dialPosition = dialPosition % dialSize;
        }
        
        if (dialPosition == 0)
        {
            zeroCounter++;
        }
    }
    
    std::cout << "Zero count: " << zeroCounter << '\n';
}

int main()
{
    std::cout << "Hello World!\n";
    
    Problem1Part1();
}