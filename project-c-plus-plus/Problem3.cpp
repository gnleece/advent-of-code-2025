#include <algorithm>
#include <fstream>
#include <iostream>
#include <string>

void Problem3Part1()
{
    std::string file_path = "Input/input-3.txt";
    std::ifstream file (file_path);
    if (!file)
    {
        std::cerr << "Could not open file " << file_path << '\n';
        return;
    }
    
    uint64_t total_joltage = 0;
    
    std::string line;
    while (std::getline(file, line))
    {
        int max_first_digit = 0;
        size_t max_first_digit_pos = -1;
        for (size_t i = 0; i < line.length() - 1; i++)
        {
            int current_digit = line[i] - '0';
            if (current_digit > max_first_digit)
            {
                max_first_digit = current_digit;
                max_first_digit_pos = i;
            }
        }
        
        int max_second_digit = 0;
        for (size_t i = max_first_digit_pos + 1; i < line.length(); i++)
        {
            int current_digit = line[i] - '0';
            max_second_digit = std::max(current_digit, max_second_digit);
        }
        
        int current_joltage = max_first_digit * 10 + max_second_digit;
        total_joltage += current_joltage;
    }
    
    std::cout << "Total joltage: " << total_joltage << "\n";
}