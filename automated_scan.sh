#!/bin/bash

echo -e "\n\e[1;34m[-] This script will perform swift host discover with nmap, create a curated list of IP addresses, and then launch deeper scans against those live hosts.\e[0m\n"

# range="USER INPUT"
read -p "[+] Enter the IP range: " range

echo -e "\n\e[32m[-] Performing host discovery on $range. Note that this requries sudo due to the usage of raw sockets to discover Windows hosts using IP protocol ping.\e[0m"

sudo nmap -sn -PO $range | grep -Eo "([0-9]{1,3}[\.]){3}[0-9]{1,3}" > live_ips.txt

echo -e "\n\e[1;34m[-] Host discovery via IP protocol ping complete, starting intense scanning on live hosts.\e[0m"

sudo nmap -sSVC -iL live_ips.txt -oA completed_scan

echo -e "\n\e[1;32m[!] Done, results are in completed_scan.txt.\e[0m"
