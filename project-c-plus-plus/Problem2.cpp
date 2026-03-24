#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include <vector>

bool IsValidPart1(int64_t id)
{
    std::string id_string = std::to_string(id);
    
    // An id can't be invalid if it has odd length
    if (id_string.length() % 2 != 0)
    {
        return true;
    }
    
    std::string first_half_string = id_string.substr(0, id_string.length() / 2);
    std::string second_half_string = id_string.substr(id_string.length() / 2);
    
    return first_half_string != second_half_string;
}

bool IsValidPart2(int64_t id)
{
    std::string id_string = std::to_string(id);
    size_t id_string_length = id_string.length();
    size_t max_pattern_length = id_string_length / 2;
    
    for (size_t patternLength = 1; patternLength <= max_pattern_length; patternLength++)
    {
        if (id_string_length % patternLength != 0)
        {
            continue;
        }
        
        std::string pattern = id_string.substr(0, patternLength);
        size_t pattern_count = id_string_length / patternLength;
        bool all_chunks_match_pattern = true;
        for (size_t i = 1; i < pattern_count; i++)
        {
            std::string current_chunk = id_string.substr(i * patternLength, patternLength);
            if (current_chunk != pattern)
            {
                all_chunks_match_pattern = false;
                break;
            }
        }
        
        if (all_chunks_match_pattern)
        {
            return false;
        }
    }
    
    return true;
}

void Problem2(bool part2)
{
    std::string file_path = "Input/input-2.txt";
    std::ifstream file (file_path);
    if (!file)
    {
        std::cerr << "Could not open file " << file_path << '\n';
        return;
    }
    
    int64_t total = 0;
    
    std::string line;
    while (std::getline(file, line))
    {
        std::istringstream range_split_stream(line);
        std::string token;
        std::vector<std::string> range_tokens;
        
        // Get all the ranges
        while (std::getline(range_split_stream, token, ','))
        {
            range_tokens.push_back(token);
        }
        
        // Process each range
        for (std::string range_token : range_tokens)
        {
            size_t delim_index = range_token.find_first_of('-');
            if (delim_index == std::string::npos)
            {
                std::cerr << "Error parsing range: " << range_token << '\n';
                continue;
            }
            
            std::string range_start_string = range_token.substr(0, delim_index);
            std::string range_end_string = range_token.substr(delim_index + 1);
            
            int64_t range_start = std::stoll(range_start_string);
            int64_t range_end = std::stoll(range_end_string);
            
            std::cout << "Range: " << range_start << " - " << range_end << '\n';
            
            // Validate all the ids in the range
            for (int64_t id = range_start; id <= range_end; id++)
            {
                bool is_valid = part2 ? IsValidPart2(id) : IsValidPart1(id);
                if (!is_valid)
                {
                    total += id;
                }
            }
        }
    }
    
    std::cout << "Total: " << total << '\n';
}

void Problem2Part1()
{
    Problem2(false);
}

void Problem2Part2()
{
    Problem2(true);
}