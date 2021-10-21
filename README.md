 ## REST API WITH ASP NET, MS SQL & DOCKER COMPOSE 


 # 1. Prerequisites
 Make sure that you have: 
 - Dot net 6.0 runtime
 - Docker and docker compose 
 - *An IDE to open the solution file

 # 2. Opening the solution 
  It is located at /MySolution.sln

  # 3. Setting up the credentials and API Key
 My default API Key value is located in appsettings.json file. And it is: 
```
 "x-api-key" : "qweasdzxc"
```

Open your Postman and follow this link: 
https://www.postman.com/savonarolla/workspace/forgreenm/overview

This will take you to my API project. Where everything is already set.

Or, you can create your own in Postman project. 
Just select as Authorization type - API Key.
With it's Key parameter as "x-api-key"
And value as "qweasdzxc" 
*both without quotation marks..*


 # 4. Compiling the binaries
1) Compile Server Project 
2) Compile DbUp Project 
 
 
# 5. Running Docker Compose


Now that we have the binaries of this project. 
We can put them inside our container. Alongside with our already setup and test data scripted MS SQL database. 
Run in /Server folder following commands in terminal.

```
docker-compose build
```

```
docker-compose up
```
*please wait at least 40sec

# 6. Running tests 
 Now our application should be online and running at 
 ``` 
 http://localhost:8000 
 ```

--------------------
### Future To Do List 

 - Mock DB
 - Implement HTTPS protocol
 - Implement secure and personal API Keys  
 - Deploy on AWS
 - Configure CD pipeline

https://hub.docker.com/repository/docker/savonarolla/my-test-repo

:D
