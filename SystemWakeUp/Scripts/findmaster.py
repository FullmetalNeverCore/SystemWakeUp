
import sys
import netlookup
from scapy.all import ARP, Ether, srp



#trying to find master's device by scanning local network in specific ip range
def network_scan(rng)->list:
    arp_rqst = ARP(pdst=rng) #making ARP request to send in specific range of network

    broadcast = Ether(dst="ff:ff:ff:ff:ff:ff") #creating Ethernet Frame and broadcasting request to all devices in network,meaning of "ff:ff:ff:ff:ff:ff"

    packet = arp_rqst / broadcast #creating complete scapy packet

    responses = srp(packet,timeout=2,verbose=False)[0] #sending requests through the network and getting only responses

    return [{'ip':recieved.psrc,'mac':recieved.hwsrc} for sent,recieved in responses] #for device's info


#trying to find my device within network
def find_master(devices)->list:
    for subject in devices:
            if 'apple' in subject['mac'].lower():
                return subject
    return None

