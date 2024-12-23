# .NET Core Extensions

[![MIT License](https://img.shields.io/badge/license-MIT-red.svg)](https://github.com/sabatex/Extensions/blob/master/LICENSE.TXT)
[![NuGet Version](https://img.shields.io/nuget/v/Sabatex.Extensions?label=version&logo=nuget&style=social&label=)](https://www.nuget.org/packages/Sabatex.Extensions)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sabatex.Extensions?color=%232694F9&label=downloads&logo=nuget&style=social)](https://www.nuget.org/packages/Sabatex.Extensions)


 - #Observable object with INotifyPropertyChanged implemented

 - #Char extensions     UpperKeyToRus(),char.UpperKeyToUkr - recode englesh key to Russian or Ukrainian

 - #DateTime extensions: BeginOfDay(),EndOfDay(),BeginOfWeek(),EndOfWeek(),BeginOfMonth(),
                        EndOfMonth(),BeginOfYear(),EndOfYear(),Quarter(),BeginOfQuarter(),
                        EndOfQuarter(),WeekOfYear()
 
 - #Enum extensions:   Description() - get enum description attribute
                      GetEnumDisplayName()  - get array Tuple<name,description> 
                      GetEnumListWithDescription() - get array Tuple<enum,description>

 - #MemberInfo extensions: GetCustomAttribute(Type attributeType) 

 - #Period Represents a dates period (start - end)
 - 
 - #UInt128 Represents a 128-bit unsigned integer

 3.1.7
     - set readonly struct UInt128
 3.1.6 -
      - change Target Framework from 2.0 to 2.1
      - add MinValue to UInt128

