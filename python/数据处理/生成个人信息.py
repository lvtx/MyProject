import sys
import time
import csv
import random
from faker import Faker

def GetPersonInfo():
    i = 0
    fake=Faker(locale='zh_CN')
    while i < 400:
        profile = fake.simple_profile()
        TeacherName = profile['name']
        if profile['sex'] == 'M':
            Sex = '男'
        else :
            Sex = '女'
        Address = profile['address']
        Email = profile['mail']
        DepartmentId = random.randint(1,15)
        Birthdate = profile['birthdate']
        i = i + 1
        TeacherId = i
        personInfo = [TeacherId,TeacherName,Sex,Address,Email,DepartmentId,Birthdate]
        Path = "D:\\数据库\\ssn.csv\\PersonInfo.csv"
        SaveInCsv(personInfo,Path)

def GetIdCardNum():
    i = 0
    fake=Faker(locale='zh_CN')
    while i < 6178:
        profile = fake.ssn()
        ssn=[profile]
        Path = "D:\\数据库\\ssn.csv"
        i = i + 1
        SaveInCsv(ssn,Path)

def SaveInCsv(Info,Path):
    csvFile = open(Path,'a',newline="",encoding="utf-8")
    try:
        writer=csv.writer(csvFile)
        writer.writerow(Info)
    finally:
        csvFile.close()

if __name__=='__main__':
    #GetPersonInfo()
    GetIdCardNum()