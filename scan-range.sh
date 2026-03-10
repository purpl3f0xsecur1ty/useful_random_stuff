# For this script, make sure targets are in CIDR notation:
# 192.168.0.0/24

if [ $# -eq 0 ]; then
  echo "[+] Usage: $0 <path to target>"
  echo "[+] Example: $0 targets.txt"
  echo "\e[31m[!] Exiting...\e[0m"
  exit 1
fi

for cidr in `cat $1`; do
  dir=$(pwd)
  echo "[+] Found $cidr"
  netblock=`echo ${cidr} | cut -f 1 -d '/'`
  mkdir -p $dir/${netblock}
  cd $dir/${netblock}
  echo "\e[32m [+] Initiating Masscan for $netblock.\e[0m"
  masscan --rate=10000 -p1- -oG ${netblock}-masscan.grep ${cidr}
  cat ${netblock}-masscan.grep | grep "Host:" | grep Timestamp | cut -f 3 -d " " | sort -u > living-hosts
  cat ${netblock}-masscan.grep | grep "Host:" | grep Timestamp | cut -f 5 -d " " | cut -f 1 -d '/' | sort -u > portlist
  for i in `cat portlist`; do
          echo -n ${i},
  done > portlist-commas
  echo "\e[32m [+] Initiating nmap for $netblock.\e[0m"
  nmap -A -sS -sV -p $(cat portlist-commas | sed -e 's/,$//') -iL living-hosts -oA living-hosts -v -T3
  cd $dir
done
