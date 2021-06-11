#!/bin/bash

display_usage() {
	echo "Invokes a lambda function"
	echo -e "\nUsage: $0 [endpoint-url] [function-name] [output] \n"
}

if [  $# -ne 3 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1
function_name=$2
output=$3

aws --endpoint-url="$endpoint_url" lambda invoke --function-name $function_name $output --log-type Tail --query 'LogResult' --output text |  base64 -d

