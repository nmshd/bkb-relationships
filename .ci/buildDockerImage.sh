#!/bin/bash
set -e
set -u
set -x

docker build --file ./Relationships.API/Dockerfile --tag ghcr.io/nmshd/bkb-relationships:${TAG-temp} .
