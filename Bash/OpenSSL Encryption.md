# Encrypt
```bash
#!/bin/bash

if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <input_file>"
    exit 1
fi

input_file=$1
output_file="$1.encrypted"

password='my-password'

openssl enc -aes-256-ecb -salt -pass pass:$password -in "$input_file" -out "$output_file"
```


# Decrypt
```bash
#!/bin/bash

if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <input_file>"
    exit 1
fi

input_file=$1
input_file_remove_extension="${input_file%.*}"
output_file="$input_file_remove_extension.decrypted"

password='my-password'

openssl enc -d -aes-256-ecb -salt -pass pass:$password -in "$input_file" -out "$output_file"
```