#!/usr/bin/python3
import socket
import sys
import ipaddress
import traceback

all = []

def unroll_ips(ip_range):
    ip_network = ipaddress.ip_network(ip_range, strict=False)


    ret = [str(ip) for ip in ip_network.hosts()]
    ret.append(str(ip_network.network_address))
    ret.append(str(ip_network.broadcast_address))
    return ret

def get_all_ips(domain_name):
    try:
        ips = socket.getaddrinfo(domain_name, None)
        ip_list = [ip[4][0] for ip in ips]
        return ip_list
    except socket.gaierror as e:
#        print(f"Error: {e}")
        return []

with open(sys.argv[1]) as f:
        for line in f:
                line = line.rstrip()
                isDNS = False
#               print("Processing: " + line)
                try:
                        dns_ips = get_all_ips(line)
                        for ip in dns_ips:
                                isDNS = True
                                all.append(ip)
                except:
                        pass

                if isDNS == False:
                        try:
                                unrolled_ips = unroll_ips(line)
                                #print(unrolled_ips)
                                for ip in unrolled_ips:
                                        #print(ip)
                                        all.append(ip)
                        except:
                                print("The following line is not a network subnet or DNS entry: " + line)
                                print("\n\n\n")
                                print(traceback.format_exc())
                                exit()

all_deduped = list(set(all))
all_deduped = sorted(ipaddress.IPv4Address(ip) for ip in all_deduped)

for line in all_deduped:
        print(line)
