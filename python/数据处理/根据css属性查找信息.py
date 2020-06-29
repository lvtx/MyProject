from urllib.request import urlopen
from bs4 import BeautifulSoup
html = urlopen("http://www.pythonscraping.com/pages/warandpeace.html")
bsObj = BeautifulSoup(html,features="lxml")
nameList = bsObj.findAll("span", {"class":"green"})
for name in nameList:
    print(name.get_text())  
# "<span class="red">Heavens! what a virulent attack!</span>" replied 
# <span class="green">the prince</span>, not in the least disconcerted by this reception.