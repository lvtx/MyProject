from urllib.request import urlopen
from bs4 import BeautifulSoup
html = urlopen("http://www.pythonscraping.com/pages/page3.html")
bsObj = BeautifulSoup(html)
for child in bsObj.find("table",{"id":"giftList"}).children:
    print(child)

# <tr><th>
# Item Title
# </th><th>
# Description
# </th><th>
# Cost
# </th><th>
# Image
# </th></tr>


# <tr class="gift" id="gift1"><td>
# Vegetable Basket
# </td><td>
# This vegetable basket is the perfect gift for your health conscious (or overweight) friends!
# <span class="excitingNote">Now with super-colorful bell peppers!</span>
# </td><td>
# $15.00
# </td><td>
# <img src="../img/gifts/img1.jpg"/>
# </td></tr>


# <tr class="gift" id="gift2"><td>
# Russian Nesting Dolls
# </td><td>
# Hand-painted by trained monkeys, these exquisite dolls are priceless! And by "priceless," we mean "extremely expensive"! <span class="excitingNote">8 entire dolls per set! Octuple the presents!</span>
# </td><td>
# $10,000.52
# </td><td>
# <img src="../img/gifts/img2.jpg"/>
# </td></tr>


# <tr class="gift" id="gift3"><td>
# Fish Painting
# </td><td>
# If something seems fishy about this painting, it's because it's a fish! <span class="excitingNote">Also hand-painted by trained monkeys!</span>
# </td><td>
# $10,005.00
# </td><td>
# <img src="../img/gifts/img3.jpg"/>
# </td></tr>


# <tr class="gift" id="gift4"><td>
# Dead Parrot
# </td><td>
# This is an ex-parrot! <span class="excitingNote">Or maybe he's only resting?</span>
# </td><td>
# $0.50
# </td><td>
# <img src="../img/gifts/img4.jpg"/>
# </td></tr>


# <tr class="gift" id="gift5"><td>
# Mystery Box
# </td><td>
# If you love suprises, this mystery box is for you! Do not place on light-colored surfaces. May cause oil staining. <span class="excitingNote">Keep your friends guessing!</span>
# </td><td>
# $1.50
# </td><td>
# <img src="../img/gifts/img6.jpg"/>
# </td></tr>