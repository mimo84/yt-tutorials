#!/bin/bash


display_usage() {
	echo "Inserts items in a dynamod db table"
	echo -e "\nUsage: $0 [endpoint-url] [json-file-full-path] \n"
}

if [  $# -le 1 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1
json_full_path=$2

aws --endpoint-url="$endpoint_url" dynamodb batch-write-item --request-items "file://$json_full_path"