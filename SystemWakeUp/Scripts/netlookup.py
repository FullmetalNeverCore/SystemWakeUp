
import sys
import getmac


def scan(mastermac):
    base = "192.168.8."
    for x in range(1,255):
        data = getmac.get_mac_address(ip=f"{base}{x}")
        if data == mastermac:
            return data
    return None


if __name__ == "__main__":
    print(scan(sys.argv[1]))
    
