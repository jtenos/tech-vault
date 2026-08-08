Basic usage of robocopy – there are a lot more options, but this is what I find useful:

robocopy c:\input\ c:\output\ /E /COPY:DAT /DCOPY:DAT /XF excl1.* /XF excl2.* /XD excl* /MIR

* /E will include subdirectories
* /COPY uses DAT by default (Data, Attributes, Timestamps)
* /DCOPY uses DA by default
* /MIR will delete items that are in the target but not the source
