# WeatherSample
My sample weather project using dot net core and clean architecture

This project is coded in Clean Architecture pattern. 
ScheduleWeatherUpdate Command will be handle by ScheduleWeatherUpdateHandler, which schedule the WeatherUpdateService with Hangfire scheduler.
Also there is a Mediator in Controller which handle any Request automatically. Using mediator pattern for Controller actions is so usefull because we can easily measure and log our commands, we can schedule our IRequest , etc.

Also as a sample of CQRS pattern, I provide Commands in Application Layer, and I always segregate Commands and Queries.
