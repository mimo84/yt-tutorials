#!/bin/bash


display_usage() {
	echo "Creates a lambda function with node"
	echo -e "\nUsage: $0 [endpoint-url] [zip-file-full-path] [function-name] [handler] [role] \n"
}

if [  $# -ne 5 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1
zip_file_path=$2
function_name=$3
handler=$4
role=$5

aws --endpoint-url=$endpoint_url lambda create-function --function-name $function_name --zip-file "fileb://$zip_file_path" --handler $handler --runtime nodejs12.x --role $role

