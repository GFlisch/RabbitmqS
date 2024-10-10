FROM rabbitmq:management

RUN     apt-get update \
        && rabbitmq-plugins enable rabbitmq_auth_mechanism_ssl \
	&& apt-get install ca-certificates -y \
	&& apt-get install openssl -y  \ 
	&& mkdir -p /home/testca/certs \
	&& mkdir -p /home/testca/private \
	&& chmod 700 /home/testca/private \
	&& echo 01 > /home/testca/serial \
	&& touch /home/testca/index.txt

COPY rabbitmq.conf /etc/rabbitmq/rabbitmq.conf
COPY openssl.cnf /home/testca
COPY prepare-server.sh generate-client-keys.sh /home/

RUN mkdir -p /home/server \
	&& mkdir -p /home/client \
	&& chmod +x /home/generate-client-keys.sh /home/prepare-server.sh 

RUN /bin/bash /home/prepare-server.sh \
	&& cp /home/testca/cacert.pem /usr/local/share/ca-certificates/cacert.crt \
	&& update-ca-certificates

RUN /bin/bash /home/generate-client-keys.sh	

CMD rabbitmq-server
#sleep infinity
