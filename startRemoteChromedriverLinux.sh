#!/bin/bash

#jeigu script neveikia, gali reiketi pakeisti dos eiluciu pabaigas i unix eiluciu pabaigas (galima naudoti dos2unix)
#prie chromedriver gali jungtis tik irengianiai baltajame sarase, todel reikia prideti savo vietini ipv4/ipv6 adresa, 127.0.0.1 pridetas kaip numatytasis

chromedriver --port=9515 --whitelisted-ips=192.168.0.10
