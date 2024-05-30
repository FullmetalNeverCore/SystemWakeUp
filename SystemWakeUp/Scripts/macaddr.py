
import sys
import getmac


def get_mac(ip):
    print(getmac.get_mac_address(ip=ip))

if __name__ == "__main__":
    get_mac(sys.argv[1]) #easy way to get mac addr