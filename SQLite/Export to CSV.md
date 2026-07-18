```shell
#!/bin/bash

# --- Configuration ---
DB_NAME="my_db_filename.sqlite3"
OUTPUT_DIR="exports_$(date +%Y%m%d_%H%M%S)"

# Check if the database file exists
if [ ! -f "$DB_NAME" ]; then
    echo "Error: Database file '$DB_NAME' not found."
    exit 1
fi

# Create output directory
mkdir -p "$OUTPUT_DIR"
echo "--- Starting export to directory: $OUTPUT_DIR ---"

# Get a list of all table names, excluding system tables
TABLES=$(sqlite3 "$DB_NAME" ".tables" | tr ' ' '\n' | grep -v '^$' | grep -v '^sqlite_')

# Loop through each table and export it
for TABLE in $TABLES; do
    OUTPUT_FILE="${OUTPUT_DIR}/${TABLE}.csv"
    
    echo "Exporting table: **$TABLE** -> $OUTPUT_FILE"
    
    # Use sqlite3 commands:
    # .mode csv       : Sets output to CSV format
    # .header on      : Includes column names as the first row
    # .once <file>    : Directs the output of the next query to the specified file
    # SELECT * ...    : The query to retrieve all data
    
    sqlite3 "$DB_NAME" <<EOF
.mode csv
.header on
.once "$OUTPUT_FILE"
SELECT * FROM "$TABLE";
EOF
    
    if [ $? -eq 0 ]; then
        echo "✅ Success: $TABLE exported."
    else
        echo "❌ Failure: Could not export $TABLE."
    fi
    
done

echo "--- Export complete. ---"
```