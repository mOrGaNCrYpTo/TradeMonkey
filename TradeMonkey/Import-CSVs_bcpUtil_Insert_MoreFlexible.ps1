Param (
    [string]$folderPath = "<Path_to_your_CSV_Folder>",
    [string]$sqlServerInstance = "<Your_SQL_Server_Instance_Name>",
    [string]$databaseName = "<Your_Database_Name>",
    [string]$tableName = "<Your_Existing_Table_Name>"
)

$files = Get-ChildItem -Path $folderPath -Filter *.csv

foreach ($file in $files) {
    $fullPath = $file.FullName
    $tempFile = [System.IO.Path]::GetTempFileName()
    (Get-Content $fullPath | Select-Object -Skip 2) | Set-Content $tempFile

    $bcpCommand = @"
bcp $databaseName.dbo.$tableName IN "$tempFile" -S $sqlServerInstance -c -t, -T
"@

    Invoke-Expression $bcpCommand
    Remove-Item $tempFile
}
