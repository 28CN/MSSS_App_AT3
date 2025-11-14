# MSSS Staff Management Application

This project contains two simple WPF (Windows Presentation Foundation) applications for managing a staff list for "Malin Space Science Systems" (MSSS).

Both applications perform the same functions:
* Loads staff data from `MalinStaffNamesV3.csv`.
* Displays the full staff list.
* Allows real-time filtering by Staff Name and Staff ID.
* Allows users to Add, Update, and Delete staff records in a separate admin window.

The only difference is the data structure used for comparison:

1.  **MSSS_App_Dictionary**: Uses a `Dictionary<int, string>`.
2.  **MSSS_App_SortedDictionary**: Uses a `SortedDictionary<int, string>`.

## How to Run

1.  Open either the `MSSS_App_Dictionary.sln` or `MSSS_App_SortedDictionary.sln` file in Visual Studio.
2.  Build and run the project (F5 or `dotnet run`).
