#!/bin/bash

display_usage() {
	echo "Creates a new AWS role"
	echo -e "\nUsage: $0 [endpoint-url] [json-file-full-path] [role-name] \n"
}

if [  $# -le 1 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1
json_full_path=$2
role_name=$3

aws --endpoint-url="$endpoint_url" iam create-role --role-name "$role_name" --assume-role-policy-document file://"$json_full_path"