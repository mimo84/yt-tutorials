#!/bin/bash

display_usage() {
	echo "List functions in lambda"
	echo -e "\nUsage: $0 [endpoint-url] \n"
}

if [  $# -ne 1 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1

aws --endpoint-url="$endpoint_url" lambda list-functions
