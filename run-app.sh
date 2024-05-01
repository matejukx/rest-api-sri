#!/bin/bash

# Run the application
docker build -t rest-api-sri .

docker run -p 5207:8080 rest-api-sri