import sys
import time
import urllib3
import requests
import numpy as np
import re
import csv
from bs4 import BeautifulSoup
from urllib import parse

headers=[{'User-Agent':'Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6'},\
{'User-Agent':'Mozilla/5.0 (Windows NT 6.2) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.12 Safari/535.11'},\
{'User-Agent': 'Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)'}]

def do_spider(book_tag_lists,max_page):
    for book_tag in book_tag_lists:
        bookLink_spider(book_tag,max_page)

def bookLink_spider(book_tag,max_page):
    #当前爬取的页数-1
    page_num = 0
    #每页有20本书
    page_size = 20
    while(1):
        if page_num >= max_page:
            break
        url = "https://book.douban.com/tag/"+parse.quote(book_tag)+"?start="+str(page_num * page_size)+"&type=T"
        print(url)
        http = urllib3.PoolManager()
        time.sleep(np.random.rand() * 5)
        print('第' + str(page_num + 1) +'页')
        try:
            r = http.request("GET", url, headers=headers[page_num % len(headers)])
            plain_text = r.data.decode()
            # print(plain_text)
        except Exception as e:
            print(e)
            continue
 
        bsObj = BeautifulSoup(plain_text, features="lxml")
        # for link in bsObj.find("li", {"class","subject-item"}).find("a",href=re.compile("^(https://book.douban.com/subject/)*$")):
        #     try:
        #         if 'href' in link.attrs:
        #             print(link.attrs['href'])
        ligroup = bsObj.find_all("li", class_="subject-item")    
        for item in ligroup:
            try:
                links_by_searched = item.find("a",href=re.compile
                                ("^(https://book.douban.com/subject/)+[0-9+]"))
                book_link = links_by_searched.attrs["href"]
                book_info = item.find("div",{"class":"pub"}).get_text().split("/")
                try:
                    book_remark = item.find("p").get_text()
                except:
                    print(str(e))
                book_spider(book_link,book_info,book_remark,book_tag)
            except Exception as e:
                print(str(e))
                continue
        page_num += 1

def book_spider(book_link,book_info,book_remark,book_type):
    url = book_link
    http = urllib3.PoolManager()
    ran = np.random.rand()
    time.sleep(ran * 5)
    try:
        print(url)
        author = book_info[0]
        translator = book_info[1]
        number = book_info[2].strip()
        r = http.request("GET", url, headers=headers[int(ran) % len(headers)])
        plain_text = r.data.decode()
            # print(plain_text)
    except Exception as e:
        print("获取图书信息时捕获错误:" + str(e))
    
    bsObj = BeautifulSoup(plain_text, features="lxml")
    try:
        bookInfo_Group = bsObj.find("div",id = "info")
        # print(bookInfo_Group)
        # bookInfos = bookInfo_Group.findAll("span",{"class":"pl"})
        # for bookInfo in bookInfos:
        try:
            BookName = bsObj.find("span",property = "v:itemreviewed").get_text().strip()
            if(len(BookName) >= 50):
                print("书名太长已取消录入")
                return
        except:
            BookName = "暂无"
        BookType = book_type
        TimeIn = time.strftime('%Y-%m-%d %H:%M:%S',time.localtime(time.time()))
        BookTypeId = ""
        try:
            Author = author.strip()
        except:
            Author = "暂无"

        if(is_number(number[0])):
            Translator = "无"
        else:
            try:
                Translator = translator.strip()
            except:
                Translator = "无"
        try:
            PageNumber = bookInfo_Group.find(text="页数:").next_element.lstrip().rstrip()
        except:
            PageNumber = "暂无"
        try:
            Price = bookInfo_Group.find(text="定价:").next_element.lstrip().rstrip()
        except:
            Price = "暂无"
        try:
            Layout = bookInfo_Group.find(text="装帧:").next_element.lstrip().rstrip()
        except:
            Layout = "暂无"
        try:
            Press = bookInfo_Group.find(text="出版社:").next_element.lstrip().rstrip()
        except:
            Press = "暂无"
        try:
            ISBN = bookInfo_Group.find(text="ISBN:").next_element.lstrip().rstrip()
        except:
            ISBN = "暂无"
        try:
            PubDate = bookInfo_Group.find(text="出版年:").next_element.lstrip().rstrip()
        except:
            PubDate = "暂无"
        try:
            BookRemark = re.findall('[\u4e00-\u9fa5a-zA-Z0-9]+',book_remark,re.S)
            BookRemark = "".join(BookRemark)
            if BookRemark == "":
                BookRemark = "暂无"
        except:
            BookRemark = "暂无"
        book_list = [BookName, TimeIn, BookTypeId, BookType ,Author, Translator ,PageNumber 
                            ,Price,Layout,Press,ISBN,PubDate,BookRemark]
        csvFile = open("D:\\数据库\\bookInfo.csv",'a',newline="",encoding="utf-8")
        try:
            writer=csv.writer(csvFile)
            writer.writerow(book_list)
        finally:
            csvFile.close()
        # for sibling in bsObj.find("div",id = "info").span.next_siblings:
        #     print(sibling.find("#text"))
        
    except Exception as e:
        print(e)

def is_number(s):
    try:
        float(s)
        return True
    except ValueError:
        pass
    try:
        import unicodedata
        unicodedata.numeric(s)
        return True
    except (TypeError, ValueError):
        pass
    return False

def add_record(BookName, TimeIn,BookType, Author, Translator ,PageNumber 
                            ,Price ,Layout,ISBS,PubDate,BookRemark):
    csvFile = open("D:/数据库/BookInfo.csv",'w+')
    try:
        writer = csv.writer(csvFile)
        writer.writerow(('BookName', 'TimeIn','BookType', 'Author', 'Translator' ,'PageNumber' 
                            ,'Price' ,'Layout','Press','ISBN','PubDate','BookRemark'))
    finally:
        csvFile.close()                    


if __name__=='__main__':
    # book_tag_lists=['小说','散文','武侠','历史','心理学','科学','经济学','科幻','数学','人物传记','音乐']
    book_tag_lists=['音乐']
    do_spider(book_tag_lists,50)