# A00474740_MCDA5510

MCDA5510 .NET Assignment 1

Project Description:

To write a C# program to traverse a directory structure (DirWalker.cs) of CSV files
that contain CSV files with customer info. 

Challenges:

• The program should use logging for both info and all possible checked exceptions.
• Some lines in the file will contain incomplete records and should be ignored (and logged) – Counted as skipped rows.
• The program must use the CSV library.
• In the end, the program should log – Total execution time– Total number of valid rows– Total number of skipped rows.   
• Data ColumnsFirst Name, Last Name, Street Number, Street, City, Province, Country, Postal Code, Phone Number, Email Address 
• Add the date to the defined in the directory structure as a date data column.


The Assignment is completed and the main C# file is DirWalker.cs

'Sample Data' folder is present in the solution folder. 
We can give the Sample data folder dynamically.
The Output file is stored in Output\FullData.csv
The Log is stored in Logs\LogFile.log

Conditions checked for good data:
* First name should not be empty.
* Last name can be empty as person can or cannot have a last name and remaining fields are checked that its not null.
* Eexecution time, valid and invalid rows are logged in the log file.