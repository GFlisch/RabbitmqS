#!/bin/bash

IMAGE_NAME="rabbitmqssl"

docker build --no-cache=true -t ${IMAGE_NAME} .
