import sys
import glob
import tarfile
import zipfile
import requests
from shutil import copy

if __name__ == "__main__":
    resp = requests.get("https://api.github.com/repos/Dreamacro/clash/tags").json()
    url = "https://github.com/Dreamacro/clash/releases/download/{0}/clash-windows-amd64-{0}.zip".format(resp[0]["name"])
    r = requests.get(url, allow_redirects=True, stream=True)
    with open("clash.zip", 'wb') as fd:
        for chunk in r.iter_content(chunk_size=128):
            fd.write(chunk)
    with zipfile.ZipFile("clash.zip","r") as zip_ref:
        zip_ref.extractall()
    for file in glob.glob("clash*.exe"):
        copy(file, "../Clans/Resources/clash.exe")

    url = "https://download.maxmind.com/app/geoip_download?edition_id=GeoLite2-Country&license_key={0}&suffix=tar.gz".format(sys.argv[1])
    r = requests.get(url, allow_redirects=True, stream=True)
    with open("Country.tar.gz", 'wb') as fd:
        for chunk in r.iter_content(chunk_size=128):
            fd.write(chunk)
    tar = tarfile.open("Country.tar.gz", "r:gz")
    tar.extractall()
    tar.close()
    for file in glob.glob("GeoLite*/*.mmdb"):
        copy(file, "../Clans/Resources/Country.mmdb")
