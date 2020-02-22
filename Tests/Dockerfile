FROM ubuntu:16.04
RUN apt update && apt upgrade && apt install  wget apt-transport-https -y
RUN wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt update -y
RUN apt install dotnet-sdk-3.1 -y