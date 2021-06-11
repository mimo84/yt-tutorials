#!/bin/bash

display_usage() {
	echo "Creates a s3 bucket with web access with LocalStack"
	echo -e "\nUsage: $0 [bucket-name] [endpoint-url] [folder-path] \n"
}

if [  $# -le 2 ]
then
  display_usage
  exit 1
fi

if [[ ( $# == "--help") ||  $# == "-h" ]]
then
  display_usage
  exit 0
fi

bucket_name=$1
endpoint_url=$2
folder_path=$3


aws s3 mb --endpoint-url="$endpoint_url" --region us-east-1 "s3://static-s3-bucket"

aws s3api --endpoint-url="http://localhost:4566" put-public-access-block \
    --bucket static-s3-bucket \
    --public-access-block-configuration "BlockPublicAcls=false,IgnorePublicAcls=false,BlockPublicPolicy=false,RestrictPublicBuckets=false"

aws s3api --endpoint-url="http://localhost:4566/" put-bucket-policy --bucket static-s3-bucket --policy "{
  \"Version\": \"2012-10-17\",
  \"Statement\": [
      {
          \"Sid\": \"PublicReadGetObject\",
          \"Effect\": \"Allow\",
          \"Principal\": \"*\",
          \"Action\": \"s3:GetObject\",
          \"Resource\": \"arn:aws:s3:::static-s3-bucket/*\"
      }
  ]
}"

aws s3 --endpoint-url="http://localhost:4566" website "s3://static-s3-bucket" --index-document index.html --error-document index.html

aws s3 --endpoint-url="$endpoint_url" sync $folder_path "s3://static-s3-bucket/"

echo "You can now access your static site here: $endpoint_url/static-s3-bucket/index.html"
echo "Done"