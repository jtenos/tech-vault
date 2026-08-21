Go to Tools | Options | Source Control | Visual Studio Team Foundation Server, then click Configure User Tools and then Add. Enter the following:   
* Extension: \*  
* Operation: Compare  
* Command: \<Path to ExamDiff.exe\>   
* Arguments: `%1 %2 /dn1:%6 /dn2:%7 /nh`

Go to Tools | Options | Source Control | Visual Studio Team Foundation Server, then click Configure User Tools and then Add. Enter the following:

* Extension: \*  
* Operation: Merge  
* Command: \<Path to ExamDiff.exe\>  
* Arguments: `/merge %1 %3 %2 /o:%4 /dn1:%6 /dn2:%8 /dn3:%7 /dno:%9 /nh`
