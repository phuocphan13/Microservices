# Basic Ecommmerce project is built based on Microservices architecture.

This project is my self-learning project. 
I based on [this course](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/) in Udemy of [Mehmet Ozkaya](https://github.com/mehmetozkaya).
And I'm still enhancing it with:
- Implement new business.
- Apply new structural.
- Apply unit test.
- Apply integration test.

## Introduction

My idea of this project is about applying some technologies that I want to learn. So I'm using lot of technologies but they are reasonable to apply.

This project is running in .NET 6.

Here is the structure of project that I capture from the course (I mentioned above).
![image](https://github.com/phuocphan13/Microservices/assets/44283172/54edd605-8afc-44f5-a665-4db7d1ea1bf0)

I applied Unit Test with xUnit (Catalog.API) and NUnit (Will implement soon).

And I also apply Integration test with [testcontainers](https://testcontainers.com) framework.


## Getting Started

Firstly, you have to clone this project in your local environment.
And install the Docker Desktop.

Access in `./docker-compose.dcproj` by terminal and run the command

```bash
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
```

After running the app, if you want to stop, you can use the command

```bash
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down
```

## Support

If you are having problems, please let me know by contacting me in [Linkedin](https://www.linkedin.com/in/phuoc-phan-47a3ab138/).

## License

This project is licensed with the [MIT license](LICENSE.txt).
