#!/bin/bash

IMAGE_NAME="rabbitmqssl"

docker build --no-cache=true  --progress=plain  -t ${IMAGE_NAME} .
