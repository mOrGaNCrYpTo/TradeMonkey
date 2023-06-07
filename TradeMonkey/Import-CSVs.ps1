Param (
    [string]$folderPath = "<Path_to_your_CSV_Folder>",
    [string]$sqlServerInstance = "<Your_SQL_Server_Instance_Name>",
    [string]$databaseName = "<Your_Database_Name>",
    [string]$tableName = "<Your_Existing_Table_Name>"
)

$files = Get-ChildItem -Path $folderPath -Filter *.csv

foreach ($file in $files) {
    $fullPath = $file.FullName
    $sqlCommand = @"
BULK INSERT $tableName
FROM '$fullPath'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 3
)
"@

    Invoke-Sqlcmd -ServerInstance $sqlServerInstance -Database $databaseName -Query $sqlCommand
}
