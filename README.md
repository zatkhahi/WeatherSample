# WeatherSample
My sample weather project is using dot net core 5 and clean architecture

This project is using Clean Architecture. (Core Domain and Application, Infrastructure and Presentation layers)  

ScheduleWeatherUpdate Command will be handle by ScheduleWeatherUpdateHandler, which schedules the WeatherUpdateService with Hangfire scheduler.

Also there is a Mediator in Controller which handle any Request automatically. Using mediator pattern for Controller actions is so usefull because we can easily measure and log our commands, we can schedule our IRequest , etc.

Also as a sample of CQRS pattern, I provide Commands in Application Layer, and I always segregate Commands and Queries.

## Rest tests
There are some REST API tests in "Rest Tests" directory. Use 'REST Client' extension in VSCode to run these examples.

I use to write all REST api tests with .rest format in most of my projects, It is better than Postman, because it is so easy to copy, share and version them.

## Migrations and Default User
Migration is used to generate the database, and User 'admin' with password 123456 will be create as administrator user at first start up.

## RabbitMQ
As test job request, I used RabbitMQ to send a simple serialized JSON message to logs channel, as well as saving the recieved weather data to DBContext.
