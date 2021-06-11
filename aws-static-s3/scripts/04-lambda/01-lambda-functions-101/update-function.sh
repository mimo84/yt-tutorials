#!/bin/bash

display_usage() {
	echo "Updates a lambda function"
	echo -e "\nUsage: $0 [endpoint-url] [zip-file] [function-name] \n"
  echo -e "\nZip the folder before running, e.g. zip -r function.zip ."
}

if [  $# -ne 3 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1
zip_file=$2
function_name=$3

aws --endpoint-url="$endpoint_url" lambda update-function-code --function-name $function_name --zip-file "fileb://$zip_file"

