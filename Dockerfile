FROM lstrcontainers.azurecr.io/landstar.dockerbase:8.0

WORKDIR /home/app

ENV APPDYNAMICS_AGENT_TIER_NAME="IdentityServer" 

COPY ["_release/Landstar.Identity/","/home/app/."]

ENTRYPOINT ["dotnet", "Landstar.Identity.dll"]