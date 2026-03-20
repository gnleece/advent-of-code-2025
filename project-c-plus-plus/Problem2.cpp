#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include <vector>

bool IsValidPart1(int64_t id)
{
    std::string idString = std::to_string(id);
    
    // An id can't be invalid if it has odd length
    if (idString.length() % 2 != 0)
    {
        return true;
    }
    
    std::string firstHalfString = idString.substr(0, idString.length() / 2);
    std::string secondHalfString = idString.substr(idString.length() / 2);
    
    return firstHalfString != secondHalfString;
}

bool IsValidPart2(int64_t id)
{
    std::string idString = std::to_string(id);
    size_t idStringLength = idString.length();
    size_t maxPatternLength = idStringLength / 2;
    
    for (size_t patternLength = 1; patternLength <= maxPatternLength; patternLength++)
    {
        if (idStringLength % patternLength != 0)
        {
            continue;
        }
        
        std::string pattern = idString.substr(0, patternLength);
        size_t patternCount = idStringLength / patternLength;
        bool allChunksMatchPattern = true;
        for (size_t i = 1; i < patternCount; i++)
        {
            std::string currentChunk = idString.substr(i * patternLength, patternLength);
            if (currentChunk != pattern)
            {
                allChunksMatchPattern = false;
                break;
            }
        }
        
        if (allChunksMatchPattern)
        {
            return false;
        }
    }
    
    return true;
}

void Problem2(bool part2)
{
    std::string filePath = "Input/input-2.txt";
    std::ifstream file (filePath);
    if (!file)
    {
        std::cerr << "Could not open file " << filePath << '\n';
        return;
    }
    
    int64_t total = 0;
    
    std::string line;
    while (std::getline(file, line))
    {
        std::istringstream rangeSplitStream(line);
        std::string token;
        std::vector<std::string> rangeTokens;
        
        // Get all the ranges
        while (std::getline(rangeSplitStream, token, ','))
        {
            rangeTokens.push_back(token);
        }
        
        // Process each range
        for (std::string rangeToken : rangeTokens)
        {
            size_t delimIndex = rangeToken.find_first_of('-');
            if (delimIndex == std::string::npos)
            {
                std::cerr << "Error parsing range: " << rangeToken << '\n';
                continue;
            }
            
            std::string rangeStartString = rangeToken.substr(0, delimIndex);
            std::string rangeEndString = rangeToken.substr(delimIndex + 1);
            
            int64_t rangeStart = std::stoll(rangeStartString);
            int64_t rangeEnd = std::stoll(rangeEndString);
            
            std::cout << "Range: " << rangeStart << " - " << rangeEnd << '\n';
            
            // Validate all the ids in the range
            for (int64_t id = rangeStart; id <= rangeEnd; id++)
            {
                bool isValid = part2 ? IsValidPart2(id) : IsValidPart1(id);
                if (!isValid)
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