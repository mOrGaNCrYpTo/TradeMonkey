Param (
    [string]$folderPath = "C:\Users\Clint\Downloads\DATA",
    [string]$sqlServerInstance = "HP\MFSQL",
    [string]$databaseName = "TradeMonkey",
    [string]$tableName = "dbo.Kucoin_History"
)

$files = Get-ChildItem -Path $folderPath -Filter *.csv

foreach ($file in $files) {
    try {
        $fullPath = $file.FullName
        $sqlCommand = @"
BULK INSERT $tableName
FROM '$fullPath'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '0x0A',
    FIRSTROW = 3
)
"@

        Write-Host "Importing file: $fullPath"
        Invoke-Sqlcmd -ServerInstance $sqlServerInstance -Database $databaseName -Query $sqlCommand
        Write-Host "Successfully imported file: $fullPath"
    }
    catch {
        Write-Host "Error importing file: $fullPath" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
}
