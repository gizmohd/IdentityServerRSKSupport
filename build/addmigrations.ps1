param([string] $migration = 'DbInit', [string] $targetContext = 'All')
$currentPath = Get-Location
Set-Location "../Landstar.Identity"
 

#Initialze db context and define the target directory
$targetContexts = @{ 
    ApplicationDbContext           = "Data\Migrations\ApplicationDb";    
    ConfigurationDbContext         = "Data\Migrations\IdentityServer\ConfigurationDb";
    IdentityExpressDbContext       = "Data\Migrations\IdentityServer\IdentityExpressDb";
    PersistedGrantDbContext        = "Data\Migrations\IdentityServer\PersistedGrantDb";
    SamlConfigurationDbContext     = "Data\Migrations\IdentityServer\SamlConfigurationDb";
    SamlArtifactDbContext          = "Data\Migrations\IdentityServer\SamlArtifactDb";
  
    
}
$migration = "Initial_Testing"
 

#Fix issue when the tools is not installed and the nuget package does not work see https://github.com/MicrosoftDocs/azure-docs/issues/40048
#Write-Host "Updating donet ef tools"
$env:Path += "	% USERPROFILE % \.dotnet\tools";
#dotnet tool update --global dotnet-ef

Write-Host "Start migrate projects"

foreach ($context in $targetContexts.Keys) {
                               
        $migrationPath = $targetContexts[$context];
            dotnet ef migrations add  $migration -c $context -o $migrationPath
                 
} 
 
Set-Location $currentPath