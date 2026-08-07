```powershell
Write-Host "Please note: Lines in the files must be sorted for this to work"
Write-Host "Usage: HugeFileLineDiff.ps1 first_file.txt second_file.txt"
Write-Host "This will generate {first_file}.diff_{timestamp}.txt and {second_file}.diff_{timestamp}.txt, which you can compare with a regular diff tool"

$inputFile1 = Get-Item -Path $args[0]
$inputFile2 = Get-Item -Path $args[1]

$reader1 = [System.IO.File]::OpenText($inputFile1.FullName)
$reader2 = [System.IO.File]::OpenText($inputFile2.FullName)

$now = Get-Date -Format "yyyyMMddHHmmss"

$outputFile1 = "{0}.diff_{1}{2}" -f [System.IO.Path]::Combine($inputFile1.DirectoryName, [System.IO.Path]::GetFileNameWithoutExtension($inputFile1.FullName)), $now, [System.IO.Path]::GetExtension($inputFile1)
$outputFile2 = "{0}.diff_{1}{2}" -f [System.IO.Path]::Combine($inputFile2.DirectoryName, [System.IO.Path]::GetFileNameWithoutExtension($inputFile2.FullName)), $now, [System.IO.Path]::GetExtension($inputFile2)

$writer1 = New-Object System.IO.StreamWriter($outputFile1)
$writer2 = New-Object System.IO.StreamWriter($outputFile2)

$line1 = $reader1.ReadLine()
$line2 = $reader2.ReadLine()

while ($true)
{
    # Write-Host "$line1 $line2"
    if ($null -eq $line1 -and $null -eq $line2)
    {
        break
    }
    if ($null -eq $line1)
    {
        $writer2.WriteLine($line2)
        $line2 = $reader2.ReadLine()
        continue
    }
    if ($null -eq $line2)
    {
        $writer1.WriteLine($line1)
        $line1 = $reader1.ReadLine()
        continue
    }
    $cmp = $line1.CompareTo($line2)
    if ($cmp -eq 0)
    {
        $line1 = $reader1.ReadLine()
        $line2 = $reader2.ReadLine()
        continue
    }
    if ($cmp -lt 0)
    {
        $writer1.WriteLine($line1)
        $line1 = $reader1.ReadLine()
        continue
    }
    if ($cmp -gt 0)
    {
        $writer2.WriteLine($line2)
        $line2 = $reader2.ReadLine()
        continue
    }
}

$reader1.Close()
$reader2.Close()
$writer1.Close()
$writer2.Close()
```