#!/usr/bin/env bash

set -ex

docker build . -t ghcr.io/mimo84/fitfood_fe:latest && docker push ghcr.io/mimo84/fitfood_fe:latest
