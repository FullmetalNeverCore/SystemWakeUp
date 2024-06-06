
import sys
import getmac
import re



def validMac(mac_address)->bool:
    # Regular expression pattern for a MAC address
    mac_pattern = r'^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$'
    return bool(re.match(mac_pattern, mac_address))

def scan(mastermac)->str:
    base = "192.168.8."
    for x in range(1,255):
        data = getmac.get_mac_address(ip=f"{base}{x}")
        if data == mastermac:
            assert validMac(data)
            return data
    return None


if __name__ == "__main__":
    print(scan(sys.argv[1]))
    
