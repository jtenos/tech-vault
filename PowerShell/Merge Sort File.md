```powershell
$inputFile = Get-Item -Path $args[0]
if (-not (Test-Path -Path $inputFile.FullName)) {
    throw "File not found"
}

$tempDir = New-Item -Path ([System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), ("MergeSortFile-{0}" -f [guid]::NewGuid().ToString('N')))) -ItemType Directory
if (-not (Test-Path -Path $tempDir.FullName)) {
    $tempDir = New-Item -Path $tempDir.FullName -ItemType Directory
}

$NUM_LINES_PER_TEMP_FILE = 1000

# Split Input File:
$numOutputFiles = 0
$reader = [System.IO.File]::OpenText($inputFile.FullName)
$line = $null
$outputFileCount = 0
$writer = $null
while ($null -ne ($line = $reader.ReadLine())) {
    if ($null -eq $writer) {
        $fullFileName = ([System.IO.Path]::Combine($tempDir.FullName, ("{0}.{1}.input" -f [DateTime]::Now.Ticks, [guid]::NewGuid().ToString('N'))))
        $writer = New-Item -Path $fullFileName -ItemType File
        $writer = [System.IO.File]::AppendText($writer.FullName)
        $numOutputFiles++
        Write-Output "$numOutputFiles | Writing to $fullFileName"
    }
    $writer.WriteLine($line)
    $outputFileCount++
    if ($outputFileCount -eq $NUM_LINES_PER_TEMP_FILE) {
        # Write-Output "Closing $($fullFileName)"
        $writer.Close()
        $writer = $null
        $outputFileCount = 0
    }
}
$reader.Close()
$reader = $null

if ($null -ne $writer) {
    # Write-Output "Closing $fullFileName"
    $writer.Close()
    $writer = $null
}

# Sort Temporary Files:
$counter = $numOutputFiles
Get-ChildItem -Path $tempDir.FullName | ForEach-Object {
    $counter--
    Write-Output "$counter | Sorting $($_.FullName)"
    $lines = Get-Content -Path $_.FullName
    $sortedLines = $lines | Sort-Object
    Set-Content -Path $_.FullName -Value $sortedLines
}

# Merge Temporary Files
$inputFiles = Get-ChildItem -Path $tempDir.FullName -Filter "*.input"
while ($inputFiles.Count -gt 1) {
    $file1 = $inputFiles[0]
    $file2 = $inputFiles[1]

    Write-Output "$($inputFiles.Count) | Merging $($file1.Name) and $($file2.Name)"

    $outputFile = New-Item -Path ([System.IO.Path]::Combine($tempDir.FullName, ("{0}.{1}.input" -f [DateTime]::Now.Ticks, [guid]::NewGuid().ToString('N')))) -ItemType File
    $writer = [System.IO.File]::AppendText($outputFile.FullName)
    $reader1 = [System.IO.File]::OpenText($file1.FullName)
    $reader2 = [System.IO.File]::OpenText($file2.FullName)
    $line1 = $reader1.ReadLine()
    $line2 = $reader2.ReadLine()
    while ($true) {
        if ($null -eq $line1 -and $null -eq $line2) { break }

        if ($null -eq $line1) {
            $writer.WriteLine($line2)
            $line2 = $reader2.ReadLine()
            continue
        }

        if ($null -eq $line2) {
            $writer.WriteLine($line1)
            $line1 = $reader1.ReadLine()
            continue
        }

        $compare = [string]::Compare($line1, $line2)
        switch ($compare) {
            -1 {
                $writer.WriteLine($line1)
                $line1 = $reader1.ReadLine()
            }
            0 {
                $writer.WriteLine($line1)
                $line1 = $reader1.ReadLine()
                $writer.WriteLine($line2)
                $line2 = $reader2.ReadLine()
            }
            1 {
                $writer.WriteLine($line2)
                $line2 = $reader2.ReadLine()
            }
        }
    }
    $writer.Close()
    $reader1.Close()
    $reader2.Close()

    Remove-Item -Path $file1.FullName
    Remove-Item -Path $file2.FullName

    $inputFiles = Get-ChildItem -Path $tempDir.FullName -Filter "*.input"
}

$outputFile = New-Item -Path ("{0}-sorted{1}" -f [System.IO.Path]::GetFileNameWithoutExtension($inputFile.FullName), $inputFile.Extension) -ItemType File -ErrorAction SilentlyContinue

Write-Output "Moving $($inputFiles[0].FullName) to $($outputFile.FullName)"
Move-Item -Path $inputFiles[0].FullName -Destination $outputFile.FullName -Force

Remove-Item -Path $tempDir.FullName -Recurse -Force

```