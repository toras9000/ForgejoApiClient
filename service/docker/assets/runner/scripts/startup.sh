#!/usr/bin/env bash

template_config=/assets/configs/config.yml
service_config=/assets/services/service.yml
runner_config=/data/config.yml

set -eu

echo "Prepare config"
cp  "$template_config" "$runner_config"

if [ ! -f "$service_config" ]; then
    echo "Waiting for runner setup ..."
    while [ ! -f "$service_config" ]
    do
        sleep 1s
    done

    echo "Waiting for setup complete ..."
    sleep 5s
fi

echo "Apply service config"
sed '1s/^\xef\xbb\xbf//' "$service_config" >> "$runner_config"

echo "Start runner daemon"
forgejo-runner daemon --config "$runner_config"
