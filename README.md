# Pokedex API

## Getting Started

### Prerequisites
* Dotnet 5
* VS Code
* Docker

### Running
#### VS Code
##### Extensions
##### Debugging


#### Docker
##### Building the image
##### Running the container

### Notes on Testing
#### dotnet watch test --no-restore --collect:"XPlat Code Coverage" 
#### TestResults dir
#### Coverage Gutters


## Improvements
* Add integration tests to run during deployment
* Distributed caching for translations
* Deploy behind a load balancer to improve scalability and availability, possible ssl offloading
* Include infrastructure as code
* Implement some form of authentication (OAuth/OIDC, API Keys/Headers)
* Implement retries for external API calls using a library like Polly
* Possibly make the FunTranslation api endpoint configurable
* More logging for easier correlation
* Health check endpoint
* Rate limiting
* Dev Ops
* Allow locale to be passed via query string param