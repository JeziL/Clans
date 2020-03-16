import os
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
    os.rename("clash-windows-amd64.exe", "clash.exe")
    copy("clash.exe", "../Clans/Resources/clash.exe")
