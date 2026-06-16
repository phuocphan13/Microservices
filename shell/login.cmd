@echo off

set /p username=Enter SSH username: 

set host=nextcloudsg.ddns.net
set port=18890

echo.
echo Connecting to %host% as %username% ...
echo.

echo ================= PORT MAPPING ================
echo IdentityServer  -^> http://localhost:8081
echo PgAdmin         -^> http://localhost:8082
echo Portainer       -^> http://localhost:9000
echo.
echo Catalog API     -^> http://localhost:5001
echo Basket API      -^> http://localhost:5002
echo Discount API    -^> http://localhost:5003
echo Discount GRPC   -^> http://localhost:5004
echo Ordering API    -^> http://localhost:5005
echo.
echo MongoDB         -^> localhost:27017
echo Redis ^(basket^)  -^> localhost:6379
echo Redis ^(catalog^) -^> localhost:6380
echo PostgreSQL      -^> localhost:5432
echo MSSQL           -^> localhost:1433
echo RabbitMQ AMQP   -^> localhost:5672
echo RabbitMQ UI     -^> http://localhost:15672
echo ==============================================
echo.

echo First login: password only -> MFA setup required
echo Next logins: password + verification code
echo.

ssh -t -p %port% ^
-o PreferredAuthentications=password,keyboard-interactive ^
-o PubkeyAuthentication=no ^
%username%@%host% ^
-L 8081:localhost:80 ^
-L 8082:localhost:80 ^
-L 9000:localhost:9000 ^
-L 5001:localhost:5001 ^
-L 5002:localhost:5002 ^
-L 5003:localhost:5003 ^
-L 5004:localhost:5004 ^
-L 5005:localhost:5005 ^
-L 27017:localhost:27017 ^
-L 6379:localhost:6379 ^
-L 6380:localhost:6379 ^
-L 5432:localhost:5432 ^
-L 1433:localhost:1433 ^
-L 5672:localhost:5672 ^
-L 15672:localhost:15672
