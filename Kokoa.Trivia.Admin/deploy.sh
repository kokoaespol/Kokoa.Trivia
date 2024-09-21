#!/bin/bash

set -e
docker build -t aloussase/kokoa-trivia-admin .
docker push aloussase/kokoa-trivia-admin
