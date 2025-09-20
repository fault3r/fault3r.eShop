#!/bin/bash

solutions=(
  "CatalogService/CatalogService.sln"
  "CatalogManagementService/CatalogManagementService.sln"
)

for sln in "${solutions[@]}"; do
  if [ -f "$sln" ]; then
    echo "Cleaning $sln ..."
    dotnet clean "$sln"
  else
    echo "Solution file $sln not found!"
  fi
done