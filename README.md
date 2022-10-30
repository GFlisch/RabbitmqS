# RabbitMQ with SSL Configuration in Docker

> RabbitMQ and SSL step by step.

This repository aims to build a Rabbitmq image with SSL to access the node and authenticate via a certificate on the port 5671.

The base image used is the Rabbitmq:management. So for development the management web site is available on the port 15672 (non ssl).

The image is creating the necessary certificates.
- Certificate authorithy.
- Server certificate used by Rabbitmq to configure amqps.
- Client certificate to be used to authenticate a client via it. 

It is recommended to mount a volume so that the client certificate can be reached from the
host system. Client certificates are generated under the **/home/client** directory.

The repo has been inspired by the [roboconf](https://github.com/roboconf/rabbitmq-with-ssl-in-docker) repo. <br>

The repo contains also a console app in .NET to test the connection...

The configuration for the openssl.conf and the rabbitmq.conf are a copy of the definition available on the [Rabbitmq-tls](https://www.rabbitmq.com/ssl.html).

The Dockerfile install the latest version of Rabbitmq with management, the certificate and openssl binaries.

## To build this image

Open a linux distro. I am using wsl from windows 11 => ubuntu image.
git clone the repo.

```
./build.sh
```

The generated image contains SSL certificates for the server side.

To run the image run this command:

```
docker run  -d --name rbmq-ssl -p 5671:5671 -p 15672:15672 -v /tmp/rabbitmq-ssl:/tmp/rabbitmq-ssl rabbitmq-with-ssl
```

The mapping used here is to transfer the client certificates in /tmp/client.

## Configure Rabbit.

We have to create a user in Rabbitmq based on the certificate.</br>
1. Start the management portal and log with the user guest and password guest. 
2. Go to the Admin part of the site.
3. Create a new user with no password and named: buildkitsandbox
4. Give to this user full access to the vhost /

## Windows client.

1. Copy the client certificates.
2. Register the certificate authority: trust-store.p12 in the trusted store.
3. Register the client certificate: key-store.p12 in the personal store.

Trying to read the p12 via the file in code doesn't work. Via the Windows certificate store, it works.

The certificates generated are valid for 1 year!

You will be able to test via the console app.
