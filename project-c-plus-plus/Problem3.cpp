#include <algorithm>
#include <fstream>
#include <iostream>
#include <queue>
#include <stack>
#include <string>
#include <vector>

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

uint64_t IntegerPow(uint64_t base, unsigned int exponent)
{
    uint64_t result = 1;
    for (size_t i = 0; i < exponent; i++)
    {
        result *= base;
    }
    
    return result;
}

void Problem3Part2()
{
    std::string file_path = "Input/input-3.txt";
    std::ifstream file (file_path);
    if (!file)
    {
        std::cerr << "Could not open file " << file_path << '\n';
        return;
    }
    
    uint64_t total_joltage = 0;
    size_t num_batteries = 12;
    
    std::string line;
    while (std::getline(file, line))
    {
        std::cout << line << "\n";
        
        std::stack<int> digit_stack;
        size_t previous_max_digit_position = -1;
        for (size_t i = num_batteries; i > 0; i--)
        {
            int current_max_digit = -1;
            for (size_t j = previous_max_digit_position + 1; j < line.length() - i + 1; j++)
            {
                int current_digit = line[j] - '0';
                if (current_digit > current_max_digit)
                {
                    current_max_digit = current_digit;
                    previous_max_digit_position = j;
                }
            }
            
            digit_stack.push(current_max_digit);
            current_max_digit = -1;
        }
        
        uint64_t joltage = 0;
        int current_battery_index = 0;
        while (!digit_stack.empty())
        {
            int current_digit = digit_stack.top();
            digit_stack.pop();
            
            std::cout << "\t" << current_digit << ", " <<  IntegerPow(10, current_battery_index);
            
            joltage += (current_digit * IntegerPow(10, current_battery_index));
            current_battery_index++;
        }
        
        total_joltage += joltage;
        
        std::cout << "\n\tAdded " << joltage << "\n\n";
    }
    
    std::cout << "Total joltage: " << total_joltage << "\n";
}