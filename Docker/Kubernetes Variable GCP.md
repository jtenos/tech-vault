# **Kubernetes edit variables in GCP**

Under **Workloads → {the workload} → {choose your item}**

**kubectl → Get YAML** will give you the YAML data for this item. Inside is a base-64 value. Copy that value, then in the terminal run:

**echo "paste-your-data-here" | base64 \-d**

You’ll get the contents in plain text.

To edit, add the content to a text file:

**echo "paste-your-data-here" | base64 \-d \> mytempfile.txt**

Edit it with vim, then in the terminal:

**base64 \-w 0 mytempfile.txt**

This will give you the base64. Copy this to the clipboard.

**kubectl → Edit** will open a vim editor, where you can replace the base64 contents.  
Get and decode the data again to confirm it saved correctly.