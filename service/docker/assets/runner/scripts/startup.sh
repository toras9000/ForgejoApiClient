#!/usr/bin/env bash

if [ ! -f /data/.runner ]; then
    echo "Waiting for runner setup ..."
    while [ ! -f /data/.runner ]
    do
        sleep 1s
    done

    echo "Waiting for setup complete ..."
    sleep 5s
fi

echo "Start runner daemon"
forgejo-runner daemon "$@"
