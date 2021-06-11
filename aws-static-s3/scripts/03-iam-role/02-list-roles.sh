#!/bin/bash

display_usage() {
	echo "Lists AWS roles"
	echo -e "\nUsage: $0 [endpoint-url] \n"
}

if [  $# -ne 1 ]
then
  display_usage
  exit 1
fi

endpoint_url=$1


aws iam list-roles --endpoint-url=$endpoint_url