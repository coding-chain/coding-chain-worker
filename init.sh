#!/bin/bash
template_path="Participations"
if [ -n "${AssetsSettings:ParticipationTemplatesPath}" ];
then
  template_path=${AssetsSettings:ParticipationTemplatesPath}
  fi;
mkdir  -p "${template_path}"
setfacl -m user:"$(whoami)":rwx "${template_path}"
dotnet  CodingChainWorker.WebApi.dll
