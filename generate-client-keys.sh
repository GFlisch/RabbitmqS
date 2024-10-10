#!/bin/bash

set -eu

#
# Prepare the client's stuff.
#
cd /home/client

echo "generate RSA KEY" 

# Generate a private RSA key.
openssl genrsa -out key.pem 2048

echo "RSA KEY generated" 
echo "$(cat key.pem)"

# Generate a certificate from our private key.
openssl req -new -key key.pem -out req.pem -outform PEM -subj /CN=svcdemo/O=client/ -nodes 
echo "$(cat req.pem)"

# Sign the certificate with our CA.
cd /home/testca
openssl ca -config openssl.cnf -in /home/client/req.pem -out /home/client/cert.pem -notext -batch -extensions client_ca_extensions
echo "$(cat /home/client/cert.pem)"

# Create a key store that will contain our certificate.
cd /home/client
cat cert.pem /home/testca/cacert.pem > cert-chain.pem
echo "$(cat cert-chain.pem)"
openssl pkcs12 -export -out key-store.pfx -in cert-chain.pem -inkey key.pem -passout pass:P@ssw0rd

# Create a trust store that will contain the certificate of our CA.
openssl pkcs12 -export -out trust-store.pfx -in /home/testca/cacert.pem -inkey /home/testca/private/cakey.pem -passout pass:P@ssw0rd 

chmod 666 *.* 
echo "$(ls -l)"